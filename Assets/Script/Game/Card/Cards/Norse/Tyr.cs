using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tyr : SoulCard
{
    protected override int CardID => Card.cardIdDict["티르"];
    private PlayerController playerController;
    protected override void Awake()
    {
        base.Awake();
    }

    private void StunRandomPiece()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != InfusedPiece.pieceColor).ToList();

        if (enemyPieceList.Count == 0)
            return;

        ChessPiece objectPiece = enemyPieceList[SynchronizedRandom.Range(0, enemyPieceList.Count)];

        objectPiece.SetKeyword(Keyword.Type.Stun);
        objectPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Stun);
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd += StunRandomPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "티르: 내 턴이 끝날 때, 무작위 적 기물 하나를 기절시킵니다.", true);
    }

    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd -= StunRandomPiece;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}