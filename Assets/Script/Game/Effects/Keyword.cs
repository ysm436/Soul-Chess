using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Keyword : Effect
{
    public enum type
    {
        Defense,        // 방어력
        Immunity,       // 면역
        Taunt,          // 도발
        Shield,         // 보호막
        Stun,           // 기절
        Restraint,      // 구속
        Stealth,        // 은신
        Silence,        // 침묵
        Rush            // 돌진
    }

    public override void EffectAction()
    {

    }
}
