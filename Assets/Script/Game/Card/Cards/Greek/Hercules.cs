using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hercules : SoulCard
{
    protected override int CardID => Card.cardIdDict["헤라클레스"];
    [HideInInspector] public PlayerController playercontroller = null;
    private int multipleAD = 2;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt);
        //버프 관련 변경 머지 후 추가예정
        InfusedPiece.buff.AddBuffByKeyword(cardName, Buff.BuffType.Taunt);

        if (playercontroller == GameBoard.instance.CurrentPlayerController())
        {
            ADmultiply();
        }
        
        playercontroller.OnMyTurnStart += ADmultiply;
        playercontroller.OnMyTurnEnd += ADoriginate;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt, 0);
        //버프 관련 변경 머지 후 추가예정
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Taunt);

        if (playercontroller == GameBoard.instance.CurrentPlayerController())
        {
            ADoriginate();
        }

        playercontroller.OnMyTurnStart -= ADmultiply;
        playercontroller.OnMyTurnEnd -= ADoriginate;
    }

    public void ADmultiply()
    {
        InfusedPiece.AD *= multipleAD;
    }

    public void ADoriginate()
    {
        InfusedPiece.AD /= multipleAD;
    }
}