using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Athena : SoulCard
{
    protected override int CardID => Card.cardIdDict["아테나"];

    private int IncreaseAmountAD = 10;
    private int IncreaseAmountHP = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == GameBoard.instance.myController.playerColor).ToList();

        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            target.maxHP += IncreaseAmountHP;
            target.AD += IncreaseAmountAD;

            target.buff.AddBuffByValue("아테나", Buff.BuffType.AD, 10, true);
            target.buff.AddBuffByValue("아테나", Buff.BuffType.HP, 10, true);
        }
    }

    public override void RemoveEffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == GameBoard.instance.myController.playerColor).ToList();
        
        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            target.maxHP -= IncreaseAmountHP;
            target.AD -= IncreaseAmountAD;

            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.AD);
            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.HP);
        }
    }
}
