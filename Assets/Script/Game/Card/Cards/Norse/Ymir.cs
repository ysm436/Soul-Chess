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
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;
        
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