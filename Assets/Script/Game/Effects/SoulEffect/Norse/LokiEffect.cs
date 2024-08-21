using System;
using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class LokiEffect : TargetingEffect
{
    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    SoulCard soul = null;

    void Awake()
    {
        if (soul == null) soul = gameObject.GetComponent<SoulCard>();
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece != soul.InfusedPiece;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction , false, true, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            gameObject.GetComponent<Loki>().targetPieceAD = (target as ChessPiece).AD;
            gameObject.GetComponent<Loki>().targetPieceHP = (target as ChessPiece).maxHP;

            if ((target as ChessPiece).soul != null && (target as ChessPiece).soul.cardName != "로키") //로키 카드는 복제 안됨
            {
                gameObject.GetComponent<Loki>().targetSoul = Instantiate((target as ChessPiece).soul);
                gameObject.GetComponent<Loki>().targetSoul.InfusedPiece = gameObject.GetComponent<Loki>().InfusedPiece;
                gameObject.GetComponent<Loki>().targetSoul.gameObject.SetActive(false);
                gameObject.GetComponent<Loki>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<Loki>().cardName, Buff.BuffType.Description,
                 $"로키 : [{gameObject.GetComponent<Loki>().targetSoul.cardName}] 복제", true);
                
                gameObject.GetComponent<Loki>().InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
            }
        }
        gameObject.GetComponent<Loki>().AddEffect();
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<Loki>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<Loki>().cardName, Buff.BuffType.Description);
    }
}
