using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SurtrEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Surtr surtr_component = gameObject.GetComponent<Surtr>();
        GameBoard.instance.whiteController.OnMyTurnEnd -= surtr_component.DecreaseCost;
        GameBoard.instance.blackController.OnMyTurnEnd -= surtr_component.DecreaseCost;

        GameBoard.instance.gameData.playerWhite.RemoveHandCards();
        GameBoard.instance.gameData.playerWhite.RemoveDeckCards();
        GameBoard.instance.gameData.playerBlack.RemoveHandCards();
        GameBoard.instance.gameData.playerBlack.RemoveDeckCards();

        surtr_component.player = player;
        surtr_component.AddEffect();

        surtr_component.InfusedPiece.buff.AddBuffByDescription(surtr_component.cardName, Buff.BuffType.Description, "수르트: 턴 종료 시 이 기물 파괴", true);
        surtr_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Surtr surtr_component = gameObject.GetComponent<Surtr>();
        surtr_component.InfusedPiece.buff.TryRemoveSpecificBuff(surtr_component.cardName, Buff.BuffType.Description);
    }
}
