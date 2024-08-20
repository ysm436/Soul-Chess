using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Apollon : SoulCard
{
    protected override int CardID => Card.cardIdDict["아폴론"];
    private int shield_exist = 1;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.SetKeyword(Keyword.Type.Shield, shield_exist);
    }

    public override void RemoveEffect()
    {
        shield_exist = InfusedPiece.GetKeyword(Keyword.Type.Shield); //보호막이 깨져 있는지 확인
        InfusedPiece.SetKeyword(Keyword.Type.Shield, 0);
    }
}