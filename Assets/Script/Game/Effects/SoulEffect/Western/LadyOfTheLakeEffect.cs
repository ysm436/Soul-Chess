using System.Collections;
using System.Collections.Generic;
using ExitGames.Client.Photon.StructWrapping;
using UnityEngine;

public class LadyOfTheLakeEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        LadyOfTheLake ladyOfTheLake = gameObject.GetComponent<LadyOfTheLake>();

        ladyOfTheLake.player = player;

        ladyOfTheLake.InfusedPiece.buff.AddBuffByDescription(ladyOfTheLake.cardName, Buff.BuffType.Description, "호수의 여인: 자신 턴 종료 시 무작위 아군 기물에게 +" + ladyOfTheLake.increasedAD + "/+" + ladyOfTheLake.increasedHP + " 부여", true);
        ladyOfTheLake.AddEffect();
        ladyOfTheLake.InfusedPiece.OnSoulRemoved += RemoveBuffInfo;
    }

    public void RemoveBuffInfo()
    {
        LadyOfTheLake ladyOfTheLake = gameObject.GetComponent<LadyOfTheLake>();
        ladyOfTheLake.InfusedPiece.buff.TryRemoveSpecificBuff(ladyOfTheLake.cardName, Buff.BuffType.Description);
    }
}
