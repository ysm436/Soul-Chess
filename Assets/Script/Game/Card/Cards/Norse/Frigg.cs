using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Frigg : SoulCard
{
    protected override int CardID => Card.cardIdDict["프리그"];

    public int decreaseAmount = 1;

    private Dictionary<ChessPiece, int> decreaseAmountDictionary;

    protected override void Awake()
    {
        base.Awake();

        decreaseAmountDictionary = new();
    }

    public void IncreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != InfusedPiece.pieceColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            if (decreaseAmountDictionary.ContainsKey(piece))
            {
                piece.AD += decreaseAmountDictionary[piece];
            }
            else
            {
                piece.AD += decreaseAmount;
            }
            piece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.AD);
        }
    }

    public void DecreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != InfusedPiece.pieceColor).ToList();

        decreaseAmountDictionary.Clear();
        foreach (ChessPiece piece in enemyPieceList)
        {
            if (piece.AD < decreaseAmount)
            {
                decreaseAmountDictionary.TryAdd(piece, piece.AD);
            }

            piece.AD -= decreaseAmount;
            piece.buff.AddBuffByValue(cardName, Buff.BuffType.AD, -decreaseAmount, false);
        }
    }

    public override void AddEffect()
    {
        
        DecreaseEnemyPiecesAD();
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        IncreaseEnemyPiecesAD();
        decreaseAmountDictionary.Clear();
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }
}
