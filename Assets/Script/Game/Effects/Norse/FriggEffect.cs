using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FriggEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Frigg friggComponent = gameObject.GetComponent<Frigg>();

        friggComponent.InfusedPiece.buff.AddBuffByDescription(friggComponent.cardName, Buff.BuffType.Description, "프리그: 내 턴 동안 상대 기물 공격력 "+ friggComponent.decreaseAmount +" 감소", true);

        friggComponent.AddEffect();
        friggComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
