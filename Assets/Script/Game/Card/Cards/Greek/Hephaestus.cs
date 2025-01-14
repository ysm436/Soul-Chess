using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hephaestus : SoulCard
{
    protected override int CardID => cardIdDict["헤파이스토스"];
    private PlayerController playerController;
    private int soulDamage = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public void AttackSoulPiece()
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.soul != null).ToList();

        if (enemyPieceList.Count == 0)
            return;
        
        foreach (var objPiece in enemyPieceList)
        {
            objPiece.MinusHP(soulDamage);
        }
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd += AttackSoulPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "헤파이스토스: 내 턴이 끝날 때, 영혼이 부여된 모든 기물에게 "+ soulDamage +" 피해를 줍니다.", true);
    }
    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd -= AttackSoulPiece;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}
