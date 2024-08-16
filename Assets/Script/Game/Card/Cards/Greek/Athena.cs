using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Athena : SoulCard
{
    protected override int CardID => Card.cardIdDict["아테나"];

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InfusedEffect;
    }

    public void InfusedEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == GameBoard.instance.myController.playerColor).ToList();
        targets.Remove(chessPiece);
        foreach (var target in targets)
        {
            target.maxHP += 10;
            target.AD += 10;

            target.buff.AddBuffByValue("아테나", Buff.BuffType.AD, 10, true);
            target.buff.AddBuffByValue("아테나", Buff.BuffType.HP, 10, true);
        }
        
        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void AddEffect()
    {
        InfusedEffect(InfusedPiece);
    }

    public override void RemoveEffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == GameBoard.instance.myController.playerColor).ToList();
        targets.Remove(InfusedPiece);
        foreach (var target in targets)
        {
            target.maxHP -= 10;
            target.AD -= 10;

            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.AD);
            target.buff.TryRemoveSpecificBuff("아테나", Buff.BuffType.HP);
        }
    }
}
