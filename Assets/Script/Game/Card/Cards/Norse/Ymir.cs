using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Ymir : SoulCard
{
    protected override int CardID => Card.cardIdDict["이미르"];
    private PlayerController playerController;
    private PlayerController opponentController;
    private PlayerData playerData;


    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
        {
            playerData = GameBoard.instance.gameData.playerWhite;
            playerController = GameBoard.instance.whiteController;
            opponentController = GameBoard.instance.blackController;
        }
        else
        {
            playerData = GameBoard.instance.gameData.playerBlack;
            playerController = GameBoard.instance.blackController;
            opponentController = GameBoard.instance.whiteController;
        }
        InfusedPiece.OnKilled += DrawUntilFull;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= DrawUntilFull;
    }

    public void DrawUntilFull(ChessPiece chessPiece)
    {
        int count = playerData.maxHandCardCount - playerData.hand.Count;
        playerController.MultipleDraw(count, opponentController);
    }
}