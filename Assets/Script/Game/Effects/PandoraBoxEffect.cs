using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PandoraBoxEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieces = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor == player.playerColor).ToList();
        int temp = Random.Range(0, 2);
        int change = 20;
        if (temp == 0) change *= -1;

        foreach (ChessPiece piece in pieces)
        {
            piece.AD += change;
            piece.maxHP = (piece.maxHP + change > 0) ? piece.maxHP + change : 1;

            piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.AD, change, true);
            piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.HP, change, true);
        }
    }

}
