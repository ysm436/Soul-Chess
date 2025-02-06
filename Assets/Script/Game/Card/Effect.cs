using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: targettype이 card인 경우의 gettargetlist 구현

public abstract class Effect : MonoBehaviour
{
    public GameObject effectPrefab;
    public abstract void EffectAction(PlayerController player);
}
