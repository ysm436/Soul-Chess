using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerseusEffect : Effect
{


    public override void EffectAction(PlayerController player)
    {
        Perseus perseus_component = gameObject.GetComponent<Perseus>();
        perseus_component.InstantiateSelectionCard(gameObject.GetComponent<SoulCard>().InfusedPiece);
    }


}