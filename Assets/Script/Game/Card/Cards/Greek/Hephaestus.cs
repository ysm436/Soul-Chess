using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Hephaestus : SoulCard
{
    protected override int CardID => cardIdDict["고독한 대장장이"];
    private PlayerController playerController;
    private int increaseAD = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    /* private Tween AttackSoulPiece()
    {
        return GameBoard.instance.chessBoard.DamageByHephaestusEffect(GetComponent<HephaestusEffect>().effectPrefab, InfusedPiece, soulDamage);
    } */

    private void IncreaseADRandomPiece()
    {
        List<ChessPiece> pieces = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor == InfusedPiece.pieceColor).ToList();

        if (pieces.Count != 0)
        {
            ChessPiece targetPiece = pieces[SynchronizedRandom.Range(0, pieces.Count())];
            GameBoard.instance.chessBoard.TileEffect(GetComponent<HephaestusEffect>().effectPrefab, targetPiece);
            targetPiece.AD += 1;
        }
        else
            Debug.Log("고독한 대장장이: No Target");
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd += IncreaseADRandomPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "고독한 대장장이: 내 턴이 끝날 때, 무작위 아군 기물의 공격력을 "+ increaseAD +" 증가시킵니다.", true);

        /* playerController.OnMyTurnEndAnimation += AttackSoulPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "헤파이스토스: 내 턴이 끝날 때, 영혼이 부여된 모든 기물에게 "+ soulDamage +" 피해를 줍니다.", true); */
    }
    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnEnd -= IncreaseADRandomPiece;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);

        /* playerController.OnMyTurnEndAnimation -= AttackSoulPiece;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description); */
    }
}
