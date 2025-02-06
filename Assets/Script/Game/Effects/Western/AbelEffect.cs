using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AbelEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Abel abelComponent = gameObject.GetComponent<Abel>();
        abelComponent.player = player;

        abelComponent.InfusedPiece.buff.AddBuffByDescription(abelComponent.cardName, Buff.BuffType.Description, "아벨: [유언] 무작위 적 기물 처치", true);
        abelComponent.AddEffect();
        abelComponent.InfusedPiece.OnSoulRemoved += abelComponent.RemoveEffect;
        abelComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Abel abelComponent = gameObject.GetComponent<Abel>();
        abelComponent.InfusedPiece.buff.TryRemoveSpecificBuff(abelComponent.cardName, Buff.BuffType.Description);
    }
}