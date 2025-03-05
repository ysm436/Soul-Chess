using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hydra : SoulCard
{
    protected override int CardID => cardIdDict["히드라"];

    private string baseDescription = "";

    [SerializeField] private int revivalCount = 2;

    [Header("Head Accessory")]
    [SerializeField] private Sprite twoHeadAccessory;
    [SerializeField] private Sprite oneHeadAccessory;
    [SerializeField] private Sprite noHeadAccessory;

    protected override void Awake()
    {
        base.Awake();

        baseDescription = description;
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

            if (revivalCount == 1)
            {
                InfusedPiece.SetAccessory(twoHeadAccessory);

            }
            else if (revivalCount == 0)
            {
                InfusedPiece.SetAccessory(oneHeadAccessory);
            }
            description = $"{baseDescription}\n({revivalCount}번 남음)";
        }
    }

    public override void AddEffect()
    {
        for(int i = InfusedPiece.buff.buffList.Count - 1; i >= 0; i--)
        {
            Buff.BuffInfo buffInfo = InfusedPiece.buff.buffList[i];

            if (buffInfo.sourceName == cardName && buffInfo.buffType == Buff.BuffType.Description) return;
        }
        InfusedPiece.buff.AddBuffByDescription(cardName, Buff.BuffType.Description, "히드라: 남은 부활 횟수 "+ revivalCount.ToString() +"번", true);
        description = $"{baseDescription}\n({revivalCount}번 남음)";
        InfusedPiece.OnKilled += Revival;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }
    public override void RemoveEffect()
    {
        InfusedPiece.buff.TryRemoveSpecificBuff(cardName, Buff.BuffType.Description);
        InfusedPiece.OnKilled -= Revival;
    }
}
