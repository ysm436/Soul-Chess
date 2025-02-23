using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Tyr : SoulCard
{
    protected override int CardID => Card.cardIdDict["티르"];
    private PlayerController playerController;
    protected override void Awake()
    {
        base.Awake();
    }
    
    private Tween StunRandomEnemyPiece()
    {
        Debug.Log("Tyr: Soul Effect");
        return GameBoard.instance.chessBoard.StunByTyrEffect(GetComponent<TyrEffect>().effectPrefab, InfusedPiece);
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEndAnimation += StunRandomEnemyPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "티르: 내 턴이 끝날 때, 무작위 적 기물 하나를 기절시킵니다.", true);
    }

    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEndAnimation -= StunRandomEnemyPiece;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}