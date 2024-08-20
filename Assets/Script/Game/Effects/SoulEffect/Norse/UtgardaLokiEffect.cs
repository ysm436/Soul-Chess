using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UtgardaLokiEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> enemyPieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor != player.playerColor).ToList();

        foreach (ChessPiece piece in enemyPieceList)
        {
            piece.SetKeyword(Keyword.Type.Silence);
            piece.buff.AddBuffByDescription(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.Description, "우트가르다 로키: 침묵 부여", false);
        }
    }
}
