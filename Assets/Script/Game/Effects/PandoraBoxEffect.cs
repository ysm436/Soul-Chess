using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PandoraBoxEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieces = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor == player.playerColor && piece.soul != null).ToList();
        int change = 10;

        foreach (ChessPiece piece in pieces)
        {
            int temp = SynchronizedRandom.Range(0,2);
            if (temp == 0)
            {
                piece.AD += change;
                piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.AD, change, true);
            }
            else
            {
                piece.maxHP += change;
                piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.HP, change, true);
            }
        }
    }

}
