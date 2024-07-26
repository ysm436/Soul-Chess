using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Poseidon : SoulCard
{
    override protected void Awake()
    {
        base.Awake();

        if (effect is EffectPoseidon)
        {
            (effect as EffectPoseidon).infuse += Infuse;
            (effect as EffectPoseidon).targetTypes[0].targetPieceType = pieceRestriction;
        }
    }
}
