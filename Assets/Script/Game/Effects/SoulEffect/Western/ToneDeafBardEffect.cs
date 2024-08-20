using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ToneDeafBardEffect : TargetingEffect
{
    public override void EffectAction()
    {
        int ADchange = -20;
        int maxHPchange = 20;

        foreach (var target in targets)
        {
            //원래 효과
            /*
            (target as ChessPiece).AD -= 20;
            (target as ChessPiece).maxHP += 20;

            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, ADchange, true);
            (target as ChessPiece).buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, maxHPchange, true);
            */
            //아군 구속 테스트용
            (target as ChessPiece).SetKeyword(Keyword.Type.Restraint);
            (target as ChessPiece).buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Restraint);

            //아군 침묵 테스트용
            //(target as ChessPiece).SetKeyword(Keyword.Type.Silence);
            //(target as ChessPiece).buff.AddBuffByKeyword(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Silence);
            gameObject.GetComponent<ToneDeafBard>().buffedPiece = target as ChessPiece;
        }
        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<SoulCard>().RemoveEffect;
    }
}
