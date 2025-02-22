using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class GazeOfMedusaEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieces = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor != player.playerColor && piece.soul != null).ToList();

        foreach (ChessPiece piece in pieces)
        {
            piece.SetKeyword(Keyword.Type.Stun);
            piece.buff.AddBuffByKeyword("메두사의 시선", Buff.BuffType.Stun);
        }
    }
}
