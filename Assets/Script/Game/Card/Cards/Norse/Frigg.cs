using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Frigg : SoulCard
{
    //protected override int CardID => Card.cardIdDict["프리그"];

    public int decreaseAmount = 20;


    protected override void Awake()
    {
        base.Awake();
        OnInfuse += (ChessPiece chessPiece) => DecreaseEnemyPiecesAD();
        OnInfuse += (ChessPiece chessPiece) => GameManager.instance.whiteController.OnOpponentTurnEnd += DecreaseEnemyPiecesAD;
        OnInfuse += (ChessPiece chessPiece) => GameManager.instance.whiteController.OnMyTurnEnd += IncreaseEnemyPiecesAD;
    }

    private void IncreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameManager.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameManager.instance.whiteController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.AD += decreaseAmount;
        }
    }

    private void DecreaseEnemyPiecesAD()
    {
        List<ChessPiece> enemyPieceList = GameManager.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameManager.instance.whiteController.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.AD -= decreaseAmount;
        }
    }
}
