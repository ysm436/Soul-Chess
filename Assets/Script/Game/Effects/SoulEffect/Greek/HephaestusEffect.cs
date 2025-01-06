using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HephaestusEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hephaestus HephaestusComponent = gameObject.GetComponent<Hephaestus>();

        HephaestusComponent.AddEffect();

        HephaestusComponent.InfusedPiece.buff.AddBuffByDescription(HephaestusComponent.cardName, Buff.BuffType.Description, "헤파이스토스: 이동 후 주위의 모든 영혼이 깃든 기물에게 " + HephaestusComponent.rangeDamage + " 피해", true);
        HephaestusComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description);
    }
}
