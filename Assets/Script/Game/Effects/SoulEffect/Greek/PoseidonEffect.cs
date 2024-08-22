using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoseidonEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i] != gameObject.GetComponent<SoulCard>().InfusedPiece && pieceList[i].soul != null) //영혼 부여된 기물만 공격
            {
                pieceList[i].MinusHP(25);
            }
        }
    }
}
