using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GambanteinEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Debug.Log("상대의 패를 볼 수 있게 됩니다");
    }
}
