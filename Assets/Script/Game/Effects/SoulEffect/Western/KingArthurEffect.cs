using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingArthurEffect : Effect
{
    public override void EffectAction()
    {
        KingArthur kingarthur_component = gameObject.GetComponent<KingArthur>();
        bool pawn_allsoul = true;

        List<ChessPiece> friendlypiece = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == kingarthur_component.InfusedPiece.pieceColor).ToList();
        List<ChessPiece> friendlypawns = friendlypiece.Where(obj => obj.pieceType == ChessPiece.PieceType.Pawn).ToList();
        foreach (var pawn in friendlypawns)
        {
            if (pawn.soul == null)
            {
                pawn_allsoul = false;
            }
        }

        if (pawn_allsoul)
        {
            foreach (var piece in friendlypiece)
            {
                piece.AD *= 2;
            }
        }
    }
}