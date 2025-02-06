using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Thor : SoulCard
{
    protected override int CardID => Card.cardIdDict["토르"];

    [HideInInspector] public PlayerController player = null;

    protected override void Awake()
    {
        base.Awake();
    }

    private void AttackRandomEnemyPiece()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor != player.playerColor && piece.soul != null).ToList(); //영혼 부여된 기물만 제거

        if (enemyPieceList.Count == 0)
            return;

        ChessPiece objPiece = enemyPieceList[SynchronizedRandom.Range(0, enemyPieceList.Count)];
        
        GameBoard.instance.chessBoard.DamageByCardEffect(GetComponent<ThorEffect>().effectPrefab, InfusedPiece, objPiece, InfusedPiece.AD);
    }

    public override void AddEffect()
    {
        if (player != null) player.OnMyTurnEnd += AttackRandomEnemyPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (player != null) player.OnMyTurnEnd -= AttackRandomEnemyPiece;
    }
}
