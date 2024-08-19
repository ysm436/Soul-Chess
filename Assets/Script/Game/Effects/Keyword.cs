using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Keyword : Effect
{
    public enum Type
    {
        Defense,        // ����
        Immunity,       // �鿪
        Taunt,          // ����
        Shield,         // ��ȣ��
        Stun,           // ����
        Restraint,      // ����
        Stealth,        // ����
        Silence,        // ħ��
        Rush            // ����
    }

    public static Type[] AllKeywords { get => Enum.GetValues(typeof(Type)).Cast<Type>().ToArray(); }

    public override void EffectAction(PlayerController player)
    {

    }
}
