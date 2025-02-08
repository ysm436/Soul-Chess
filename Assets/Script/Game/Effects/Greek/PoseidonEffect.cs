using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Poseidon poseidonComponent = gameObject.GetComponent<Poseidon>();

        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i] != poseidonComponent.InfusedPiece && pieceList[i].soul != null) //영혼 부여된 기물만 공격
            {
                Debug.Log("Poseidon: Soul Effect");
                GameBoard.instance.chessBoard.DamageByCardEffect(effectPrefab, poseidonComponent.InfusedPiece, pieceList[i], poseidonComponent.damageAmount);
            }
        }
    }
}
