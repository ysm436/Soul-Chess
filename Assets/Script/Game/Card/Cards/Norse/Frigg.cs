using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Frigg : SoulCard
{
    public int decreaseAmount = 20;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += (ChessPiece chessPiece) => DecreaseEnemyPiecesAD();
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnOpponentTurnEnd += DecreaseEnemyPiecesAD;
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnEnd += IncreaseEnemyPiecesAD;
    }

    private void IncreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.AD += decreaseAmount;
        }
    }

    private void DecreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.AD -= decreaseAmount;
        }
    }
}
