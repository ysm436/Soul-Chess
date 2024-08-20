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
            if (pieceList[i] != gameObject.GetComponent<SoulCard>().InfusedPiece)
            {
                pieceList[i].MinusHP(25);
            }
        }
    }
}
