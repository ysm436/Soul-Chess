using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thor : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += (ChessPiece chessPiece) => GameManager.instance.whiteController.OnMyTurnEnd += AttackRandomEnemyPiece;
    }

    private void AttackRandomEnemyPiece()
    {
        List<ChessPiece> enemyPieceList = GameManager.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameManager.instance.whiteController.playerColor).ToList();

        if (enemyPieceList.Count == 0)
            return;

        enemyPieceList[Random.Range(0, enemyPieceList.Count)].HP -= InfusedPiece.AD;
    }
}
