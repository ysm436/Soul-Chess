using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Poseidon poseidonComponent = gameObject.GetComponent<Poseidon>();

        List<ChessPiece> targetList = new List<ChessPiece>();

        foreach (var target in GameBoard.instance.gameData.pieceObjects)
        {
            if (target != poseidonComponent.InfusedPiece && target.soul != null) //영혼 부여된 기물만 공격
            {
                targetList.Add(target);
            }
        }

        Debug.Log("Poseidon: Soul Effect");
        GameBoard.instance.chessBoard.DamageByPoseidonEffect(effectPrefab, poseidonComponent.InfusedPiece, targetList, poseidonComponent.damageAmount);
    }
}
