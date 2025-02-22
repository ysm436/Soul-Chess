using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cerberus : SoulCard
{
    protected override int CardID => Card.cardIdDict["케르베로스"];

    public int extraAttack = 2;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        InfusedPiece.OnEndAttack += AttackExtra;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnEndAttack -= AttackExtra;
    }

    public void AttackExtra(ChessPiece targetPiece)
    {
        for (int i = 0; i < extraAttack; i++)
        {
            if (targetPiece.isAlive)
            {
                Debug.Log("Cerberus: Attack Target");
                targetPiece.Attacked(InfusedPiece, InfusedPiece.AD);
            }
            else
            {
                Debug.Log("Cerberus: Target is Die");
                break;
            }

        }
    }
}
