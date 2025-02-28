using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Athena : SoulCard
{
    protected override int CardID => Card.cardIdDict["아테나"];

    public int increasedAD = 1;
    public int increasedHP = 1;

    private List<ChessPiece> buffedPieces;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        buffedPieces = new List<ChessPiece>();
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor && obj.soul != null).ToList();

        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            BuffPiece(target);
        }

        List<ChessPiece> myPieces = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();

        foreach (var myPiece in myPieces)
        {
            myPiece.OnSetSoul += BuffPiece;
        }

        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    private void BuffPiece(ChessPiece piece)
    {
        if (piece.buff.FindBuff("아테나", Buff.BuffType.AD, out int index))
        {
            // 이미 버프됨
            return;
        }

        GameObject effectPrefab = GetComponent<AthenaEffect>().effectPrefab;

        GameBoard.instance.chessBoard.TileEffect(effectPrefab, piece);

        piece.maxHP += increasedHP;
        piece.AD += increasedAD;

        piece.buff.AddBuffByValue("아테나", Buff.BuffType.AD, increasedAD, false);
        piece.buff.AddBuffByValue("아테나", Buff.BuffType.HP, increasedHP, false);

        buffedPieces.Add(piece);
    }

    public override void RemoveEffect()
    {
        foreach (var piece in buffedPieces)
        {
            piece.maxHP -= increasedHP;
            piece.AD -= increasedAD;

            piece.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.AD);
            piece.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.HP);
        }

        List<ChessPiece> myPieces = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();

        foreach (var myPiece in myPieces)
        {
            myPiece.OnSetSoul -= BuffPiece;
        }

        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }
}
