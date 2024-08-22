using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class KrakenEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Kraken kraken_component = gameObject.GetComponent<Kraken>();
        kraken_component.InfusedPiece.buff.AddBuffByDescription(kraken_component.cardName, Buff.BuffType.Description, "크라켄: [유언] 8번만큼 무작위 적 기물에게 8 피해로 공격", true);
        kraken_component.AddEffect();
        kraken_component.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        Kraken kraken_component = gameObject.GetComponent<Kraken>();
        kraken_component.InfusedPiece.buff.TryRemoveSpecificBuff(kraken_component.cardName, Buff.BuffType.Description);
    }
}
