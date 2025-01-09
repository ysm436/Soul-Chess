using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class KingArthurEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        KingArthur kingArthurComponent = gameObject.GetComponent<KingArthur>();
        bool pawn_allsoul = true;

        List<ChessPiece> friendlypiece = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == kingArthurComponent.InfusedPiece.pieceColor).ToList();
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
                piece.buff.AddBuffByDescription(kingArthurComponent.cardName, Buff.BuffType.Description, "아서왕: 공격력 " + kingArthurComponent.multipleAmount + "배 적용됨", false);
                piece.AD *= kingArthurComponent.multipleAmount;
            }
        }
    }
}