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
                ChessPiece objPiece = pieceList[i];

                objPiece.MinusHP(poseidonComponent.damageAmount);
                if (objPiece.isAlive)
                {
                    GameBoard.instance.chessBoard.AttackedAnimation(objPiece);
                }
                else
                {
                    objPiece.GetComponent<Animator>().SetTrigger("killedTrigger");
                    objPiece.MakeAttackedEffect();
                }
            }
        }
    }
}
