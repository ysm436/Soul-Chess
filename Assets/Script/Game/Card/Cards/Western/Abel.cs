using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

// TODO 이벤트 시스템 수정 후 재수정

public class Abel : SoulCard
{
    protected override int CardID => Card.cardIdDict["아벨"];
    [HideInInspector] public PlayerController player = null;

    protected override void Awake()
    {
        base.Awake();
    }

    private void OnKilledEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
           piece.pieceColor != player.playerColor).ToList();

        if (enemyPieceList.Count == 0) return;

        int randomIndex = SynchronizedRandom.Range(0, enemyPieceList.Count);
        ChessPiece targetPiece = enemyPieceList[randomIndex];
        Debug.Log("Abel: Soul Effect");
        GameBoard.instance.chessBoard.KillByCardEffect(GetComponent<AbelEffect>().effectPrefab, InfusedPiece, targetPiece);
    }


    public override void AddEffect()
    {
        InfusedPiece.OnKilled += OnKilledEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= OnKilledEffect;
    }
}
