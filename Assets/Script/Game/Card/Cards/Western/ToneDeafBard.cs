using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneDeafBard : SoulCard
{
    protected override int CardID => Card.cardIdDict["음치 음유시인"];

    public ChessPiece buffedPiece = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        
    }
    public override void RemoveEffect()
    {
        if (buffedPiece != null && buffedPiece != gameObject.GetComponent<SoulCard>().InfusedPiece)
        {
            buffedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD);
            buffedPiece.buff.TryRemoveSpecificBuff(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP);
            buffedPiece.AD += 20;
            buffedPiece.maxHP = (buffedPiece.maxHP-20) > 0 ? buffedPiece.maxHP-20 : 1;
        }
    }
}
