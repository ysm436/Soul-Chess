using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Frigg : SoulCard
{
    public int decreaseAmount = 20;

    private Dictionary<ChessPiece, int> decreaseAmountDictionary;

    protected override void Awake()
    {
        base.Awake();

        decreaseAmountDictionary = new();
        OnInfuse += (ChessPiece chessPiece) => DecreaseEnemyPiecesAD();
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnStart += DecreaseEnemyPiecesAD;
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnEnd += IncreaseEnemyPiecesAD;
        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnSoulRemoved += RemoveEffect;
    }

    private void IncreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

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
        }
    }

    private void DecreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        decreaseAmountDictionary.Clear();
        foreach (ChessPiece piece in enemyPieceList)
        {
            if (piece.AD < decreaseAmount)
            {
                decreaseAmountDictionary.TryAdd(piece, piece.AD);
            }

            piece.AD -= decreaseAmount;
        }
    }

    public override void RemoveEffect()
    {
        IncreaseEnemyPiecesAD();
        GameBoard.instance.myController.OnMyTurnStart -= DecreaseEnemyPiecesAD;
        GameBoard.instance.myController.OnMyTurnEnd -= IncreaseEnemyPiecesAD;
        decreaseAmountDictionary.Clear();
    }
}
