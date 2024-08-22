using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : SoulCard
{
    protected override int CardID => cardIdDict["히드라"];

    [HideInInspector] public int revivalCount = 8;

    protected override void Awake()
    {
        base.Awake();
    }

    public void Revival(ChessPiece chessPiece)
    {
        if (revivalCount > 0)
        {
            revivalCount--; Debug.Log("revivalCount: " + revivalCount.ToString());
            InfusedPiece.RemoveBuff();
            RemoveEffect();
            InfusedPiece.AddHP(9999); //currentHP가 음수여도 maxHP로 회복하도록 이상치 투입
            AddEffect();
            InfusedPiece.isRevivable = true;
            Debug.Log("부활");
        }
    }

    public override void AddEffect()
    {
        for(int i = InfusedPiece.buff.buffList.Count - 1; i >= 0; i--)
        {
            Buff.BuffInfo buffInfo = InfusedPiece.buff.buffList[i];

            if (buffInfo.sourceName == cardName && buffInfo.buffType == Buff.BuffType.Description) return;
        }
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "히드라: 남은 부활 횟수 "+revivalCount.ToString()+"번", true);
        InfusedPiece.OnKilled += Revival;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }
    public override void RemoveEffect()
    {
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
        InfusedPiece.OnKilled -= Revival;
    }
}
