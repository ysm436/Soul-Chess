using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Typhon : SoulCard
{
    protected override int CardID => cardIdDict["튀폰"];
    private PlayerController playerController;
    [SerializeField] private int countdown = 3;

    protected override void Awake()
    {
        base.Awake();
    }

    private void KillCountDown()
    {
        countdown--;
        Debug.Log(countdown);

        if (countdown == 0)
        {
            InfusedPiece.Kill();
        }
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnStart += KillCountDown;
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "튀폰: 3턴 뒤, 이 기물은 파괴됩니다.", true);
    }

    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playerController = GameBoard.instance.whiteController;
        else
            playerController = GameBoard.instance.blackController;

        playerController.OnMyTurnStart -= KillCountDown;
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
    }
}
