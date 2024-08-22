using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hercules : SoulCard
{
    protected override int CardID => Card.cardIdDict["헤라클레스"];
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

        if (GameBoard.instance.CurrentPlayerController().isMyTurn) //내 턴에 카드가 나왔거나, 구속등이 풀렸을 때
        {
            ADmultiply();
        }
        
        GameBoard.instance.myController.OnMyTurnStart += ADmultiply;
        GameBoard.instance.myController.OnMyTurnEnd += ADoriginate;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Taunt, 0);
        //버프 관련 변경 머지 후 추가예정
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Taunt);

        if (GameBoard.instance.CurrentPlayerController().isMyTurn) //내 턴에 침묵등이 걸렸을 때
        {
            ADoriginate();
        }

        GameBoard.instance.myController.OnMyTurnStart -= ADmultiply;
        GameBoard.instance.myController.OnMyTurnEnd -= ADoriginate;
    }

    public void ADmultiply()
    {
        if (GameBoard.instance.myController.playerColor == InfusedPiece.pieceColor) //mycontroller가 piececolor인지 확인
        {
            InfusedPiece.AD *= multipleAD;
        }
    }

    public void ADoriginate()
    {
        if (GameBoard.instance.myController.playerColor == InfusedPiece.pieceColor) //mycontroller가 piececolor인지 확인
        {
            InfusedPiece.AD /= multipleAD;
        }
    }
}