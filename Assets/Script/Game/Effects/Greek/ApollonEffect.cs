using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ApollonEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        Apollon apollonComponent = gameObject.GetComponent<Apollon>();
        Vector2Int StandardPosition = apollonComponent.InfusedPiece.coordinate;

        Vector2Int tempPosition = StandardPosition;
        tempPosition += Vector2Int.left;
        if (GameBoard.instance.gameData.IsValidCoordinate(tempPosition))
        {
            ChessPiece objPiece = GameBoard.instance.gameData.GetPiece(tempPosition);
            if (objPiece != null && objPiece.pieceColor == player.playerColor)
            {
                objPiece.SetKeyword(Keyword.Type.Shield);
                objPiece.buff.AddBuffByKeyword(apollonComponent.cardName, Buff.BuffType.Shield);
            }
        }

        tempPosition = StandardPosition;
        tempPosition += Vector2Int.right;
        if (GameBoard.instance.gameData.IsValidCoordinate(tempPosition))
        {
            ChessPiece objPiece = GameBoard.instance.gameData.GetPiece(tempPosition);
            if (objPiece != null && objPiece.pieceColor == player.playerColor)
            {
                objPiece.SetKeyword(Keyword.Type.Shield);
                objPiece.buff.AddBuffByKeyword(apollonComponent.cardName, Buff.BuffType.Shield);
            }
        }

        apollonComponent.InfusedPiece.SetKeyword(Keyword.Type.Shield); //자신에게 보호막
        apollonComponent.InfusedPiece.buff.AddBuffByKeyword(apollonComponent.cardName, Buff.BuffType.Shield);
    }
}
