using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class KrakenEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Kraken krakenComponent = gameObject.GetComponent<Kraken>();
        krakenComponent.InfusedPiece.buff.AddBuffByDescription(krakenComponent.cardName, Buff.BuffType.Description, "크라켄: [유언] " + krakenComponent.repeat + "번만큼 무작위 적 기물에게 " + krakenComponent.damage + " 피해로 공격", true);
        krakenComponent.AddEffect();
        krakenComponent.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Kraken krakenComponent = gameObject.GetComponent<Kraken>();
        krakenComponent.InfusedPiece.buff.TryRemoveSpecificBuff(krakenComponent.cardName, Buff.BuffType.Description);
    }
}
