using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Athena : SoulCard
{
    protected override int CardID => Card.cardIdDict["아테나"];

    public int increasedAD = 10;
    public int increasedHP = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();

        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            target.maxHP += increasedHP;
            target.AD += increasedAD;

            target.buff.AddBuffByValue("아테나", Buff.BuffType.AD, increasedAD, false);
            target.buff.AddBuffByValue("아테나", Buff.BuffType.HP, increasedHP, false);
        }

        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();
        
        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            target.maxHP -= increasedHP;
            target.AD -= increasedAD;

            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.AD);
            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.HP);
        }
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }
}
