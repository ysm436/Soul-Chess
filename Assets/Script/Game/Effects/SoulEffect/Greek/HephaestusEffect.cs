using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HephaestusEffect : Effect
{
    public override void EffectAction()
    {
        string cardName = gameObject.GetComponent<SoulCard>().cardName;

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "헤파이스토스: 이동 후 주위의 모든 기물에게 20 피해", true);

        gameObject.GetComponent<SoulCard>().InfusedPiece.OnMove += gameObject.GetComponent<Hephaestus>().SoulEffect;
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<SoulCard>().RemoveEffect;
    }
}
