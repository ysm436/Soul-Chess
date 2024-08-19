using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ymir : SoulCard
{
    protected override int CardID => Card.cardIdDict["이미르"];
    private PlayerData playercolor;


    protected override void Awake()
    {
        base.Awake();

        if (GameBoard.instance.myController.playerColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;
    }

    public override void AddEffect()
    {
        InfusedPiece.OnKilled += DrawUntilFull;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= DrawUntilFull;
    }

    public void DrawUntilFull(ChessPiece chessPiece)
    {
        while (true)
        {
            if (playercolor.hand.Count < playercolor.maxHandCardCount && playercolor.deck.Count > 0)
            {
                playercolor.DrawCard();
            }
            else
            {
                break;
            }
        }
    }
    
}