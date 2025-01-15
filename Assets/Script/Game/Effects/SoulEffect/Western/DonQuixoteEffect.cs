using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DonQuixoteEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        DonQuixote donQuixoteComponent = gameObject.GetComponent<DonQuixote>();

        donQuixoteComponent.AddEffect();
        donQuixoteComponent.InfusedPiece.OnSoulRemoved += donQuixoteComponent.RemoveEffect;
    }
}
