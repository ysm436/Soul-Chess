using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavyJones : SoulCard
{
    protected override int CardID => Card.cardIdDict["데비 존스"];

    public int increasedAD = 1;
    public int increasedHP = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public void IncreaseStat(ChessPiece chessPiece)
    {
        InfusedPiece.AD += increasedAD;
        InfusedPiece.maxHP += increasedHP;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, increasedAD, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, increasedHP, true);
    }

    public override void AddEffect()
    {
        RemoveEffect();        

        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor == InfusedPiece.pieceColor).ToList();
        int deadMyPieceCount = 16 - pieceList.Count;

        InfusedPiece.AD += deadMyPieceCount * increasedAD;
        InfusedPiece.maxHP += deadMyPieceCount * increasedHP;

        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, deadMyPieceCount * increasedAD, true);
        InfusedPiece.buff.AddBuffByValue(cardName, Buff.BuffType.HP, deadMyPieceCount * increasedHP, true);

        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled += IncreaseStat;
        }
        
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        for(int i = InfusedPiece.buff.buffList.Count-1; i >= 0; i--)
        {
            Buff.BuffInfo buffInfo = InfusedPiece.buff.buffList[i];
            if (buffInfo.sourceName == cardName)
            {
                if (buffInfo.buffType == Buff.BuffType.AD)
                {
                    InfusedPiece.AD -= buffInfo.value;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.AD);
                }
                else if (buffInfo.buffType == Buff.BuffType.HP)
                {
                    InfusedPiece.maxHP = (InfusedPiece.maxHP-buffInfo.value) > 0 ? InfusedPiece.maxHP-buffInfo.value : 1;
                    InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.HP);
                }
            }
        }
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor == InfusedPiece.pieceColor).ToList();
        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled -= IncreaseStat;
        }
    }
}
