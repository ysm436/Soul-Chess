using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 기본 AD, HP, MoveCount 등은 저장 X, 버프된 값만 저장
// 구속 당할 때 제거되지 않고 침묵 당할 때 제거되는 변수들 저장하는 클래스
public class Buff : Effect
{
    public int buffedHP;                    //음수 = 디버프, 양수 = 버프
    public int buffedAD;
    public int buffedMoveCount;
    public bool canPassThroughPieces;

    public Buff()
    {
        buffedHP = 0;
        buffedAD = 0;
        buffedMoveCount = 0;
        canPassThroughPieces = false;
    }

    public void Clear()
    {
        buffedHP = 0;
        buffedAD = 0;
        buffedMoveCount = 0;
        canPassThroughPieces = false;
    }

    public override void EffectAction()
    {

    }
}
