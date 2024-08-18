using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AresEffect : Effect
{
    public override void EffectAction()
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        foreach (ChessPiece piece in pieceList)
        {
            piece.OnKilled += gameObject.GetComponent<Ares>().IncreaseStat;
        }
        gameObject.GetComponent<Ares>().InfusedPiece.buff.AddBuffByDescription(gameObject.GetComponent<Ares>().cardName, Buff.BuffType.Description, "아레스: 적 기물 사망 시 +15/+15 부여", true);
        gameObject.GetComponent<Ares>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<Ares>().RemoveEffect;
    }
}
