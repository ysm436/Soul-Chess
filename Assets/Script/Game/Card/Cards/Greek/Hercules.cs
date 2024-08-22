using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hercules : SoulCard
{
    protected override int CardID => Card.cardIdDict["헤라클레스"];
    private PlayerController playercontroller;
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

        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercontroller = GameBoard.instance.whiteController;
        else
            playercontroller = GameBoard.instance.blackController;
        
        playercontroller.OnMyTurnStart += ADmultiply;
        playercontroller.OnMyTurnEnd += ADoriginate;
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

        playercontroller.OnMyTurnStart -= ADmultiply;
        playercontroller.OnMyTurnEnd -= ADoriginate;
    }

    public void ADmultiply()
    {
        if (playercontroller.playerColor == InfusedPiece.pieceColor) //playercontroller가 piececolor인지 확인
        {
            InfusedPiece.AD *= multipleAD;
        }
    }

    public void ADoriginate()
    {
        if (playercontroller.playerColor == InfusedPiece.pieceColor) //playercontroller가 piececolor인지 확인
        {
            InfusedPiece.AD /= multipleAD;
        }
    }
}