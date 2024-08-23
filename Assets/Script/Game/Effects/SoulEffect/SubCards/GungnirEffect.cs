using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GungnirEffect : TargetingEffect
{
    [HideInInspector] public int dmg = 50;
    [SerializeField] private Gungnir gungnir;
    [SerializeField] private GameObject gungnir_prefab;

    public override void EffectAction(PlayerController player)
    {


        foreach (var target in targets)
        {
            (target as ChessPiece).SpellAttacked(dmg);
            /* Gungnir spellCard = Instantiate(gameObject.GetComponent<Gungnir>());
            spellCard.player = player;
            spellCard.owner = GetComponent<Card>().owner;
            spellCard.ReadyToGetGungnir(); */

            GameObject gungnir_card = Instantiate(gungnir_prefab);
            //Card gungnir_cardcomponent = gungnir_card.GetComponent<Card>();
            //gungnir_cardcomponent.isInSelection = false;
            //gungnir_cardcomponent.owner = gungnir.owner;

            //gungnir_card.GetComponent<Gungnir>().ReadyToGetGungnir(player);
        }
    }
}
