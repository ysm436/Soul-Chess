using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingArthurEffect : Effect
{
    public override void EffectAction(PlayerController player)
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
                piece.buff.AddBuffByDescription(kingarthur_component.cardName, Buff.BuffType.Description, "아서왕: 공격력 2배 적용됨", false);
                piece.AD *= 2;
            }
        }
    }
}