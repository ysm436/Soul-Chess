using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hades : SoulCard
{
    protected override int CardID => Card.cardIdDict["하데스"];

    private PlayerController playercontroller;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercontroller = GameBoard.instance.whiteController;
        else
            playercontroller = GameBoard.instance.blackController;

        //playercontroller.OnMyTurnStart +=
    }

    public override void RemoveEffect()
    {
    }


}
