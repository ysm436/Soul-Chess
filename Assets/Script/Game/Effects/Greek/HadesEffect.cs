using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HadesEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Hades hadesComponent = gameObject.GetComponent<Hades>();

        hadesComponent.InfusedPiece.buff.AddBuffByDescription(hadesComponent.cardName, Buff.BuffType.Description, "하데스: 자신 턴 동안 아군 HP가 1 미만으로 내려가지 않음", true);
        hadesComponent.AddEffect();
        hadesComponent.InfusedPiece.OnSoulRemoved += hadesComponent.RemoveEffect;
        hadesComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Hades hadesComponent = gameObject.GetComponent<Hades>();
        hadesComponent.InfusedPiece.buff.TryRemoveSpecificBuff(hadesComponent.cardName, Buff.BuffType.Description);
    }
}