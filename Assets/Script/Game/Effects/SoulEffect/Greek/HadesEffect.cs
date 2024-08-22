using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hades hades_component = gameObject.GetComponent<Hades>();

        hades_component.InfusedPiece.buff.AddBuffByDescription(hades_component.cardName, Buff.BuffType.Description, "하데스: 자신 턴 동안 아군 HP가 1 미만으로 내려가지 않음", true);
        hades_component.AddEffect();
        hades_component.InfusedPiece.OnSoulRemoved += hades_component.RemoveEffect;
        hades_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Hades hades_component = gameObject.GetComponent<Hades>();
        hades_component.InfusedPiece.buff.TryRemoveSpecificBuff(hades_component.cardName, Buff.BuffType.Description);
    }
}