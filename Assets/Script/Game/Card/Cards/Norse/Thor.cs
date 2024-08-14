using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thor : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += (ChessPiece chessPiece) => GameBoard.instance.myController.OnMyTurnEnd += AttackRandomEnemyPiece;
        OnInfuse += (ChessPiece chessPiece) => chessPiece.OnSoulRemoved += RemoveEffect;
    }

    private void AttackRandomEnemyPiece()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        if (enemyPieceList.Count == 0)
            return;

        enemyPieceList[Random.Range(0, enemyPieceList.Count)].HP -= InfusedPiece.AD;
    }

    public override void AddEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd += AttackRandomEnemyPiece;
    }

    public override void RemoveEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd -= AttackRandomEnemyPiece;
    }
}
