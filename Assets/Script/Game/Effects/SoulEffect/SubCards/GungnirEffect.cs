using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GungnirEffect : TargetingEffect
{
    [HideInInspector] public int dmg = 50;
    [SerializeField] private Gungnir gungnir;
    public override void EffectAction(PlayerController player)
    {
        foreach (var target in targets)
        {
            (target as ChessPiece).SpellAttacked(dmg);
            Gungnir spellCard = Instantiate(gameObject.GetComponent<Gungnir>());
            spellCard.player = player;
            spellCard.isMine = true;
            spellCard.ReadyToGetGungnir();
        }
    }
}
