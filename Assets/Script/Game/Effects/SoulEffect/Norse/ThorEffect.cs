using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThorEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Thor thor_component = gameObject.GetComponent<Thor>();
        thor_component.player = player;

        thor_component.InfusedPiece.buff.AddBuffByDescription(thor_component.cardName, Buff.BuffType.Description, "토르: 턴 종료 시 무작위 적 기물에게 자신 공격력만큼 피해", true);
        thor_component.AddEffect();
        thor_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Thor thor_component = gameObject.GetComponent<Thor>();
        thor_component.InfusedPiece.buff.TryRemoveSpecificBuff(thor_component.cardName, Buff.BuffType.Description);
    }
}
