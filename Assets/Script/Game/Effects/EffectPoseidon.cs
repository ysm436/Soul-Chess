using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPoseidon : TargetingEffect
{
    public Action<ChessPiece> infuse;

    public override void EffectAction()
    {
        infuse.Invoke(targets[0] as ChessPiece);

        List<ChessPiece> pieceList = GameManager.instance.gameData.pieceObjects;
        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i] != targets[0])
            {
                pieceList[i].HP -= 25;
                /*if (!pieceList[i].isAlive) {
                    GameManager.instance.GetBoardSquare(pieceList[i].coordinate).isTargetable = false;
                    GameManager.instance.KillPiece(pieceList[i]);
                }*/
            }
        }
    }
}
