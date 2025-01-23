using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class HeavyArmoredInfantryEffect : Effect
{
    public override void EffectAction(PlayerController player)
    {
        HeavyArmoredInfantry heavyArmoredInfantryComponent = gameObject.GetComponent<HeavyArmoredInfantry>();
        Vector2Int StandardPosition = heavyArmoredInfantryComponent.InfusedPiece.coordinate;

        Vector2Int tempPosition = StandardPosition;
        tempPosition += Vector2Int.left;
        if (GameBoard.instance.gameData.IsValidCoordinate(tempPosition))
        {
            ChessPiece objPiece = GameBoard.instance.gameData.GetPiece(tempPosition);
            if (objPiece != null && objPiece.pieceColor == player.playerColor)
            {
                objPiece.maxHP += heavyArmoredInfantryComponent.increasedHP;
                objPiece.buff.AddBuffByValue(heavyArmoredInfantryComponent.cardName, Buff.BuffType.HP, heavyArmoredInfantryComponent.increasedHP, false);
            }
        }

        tempPosition = StandardPosition;
        tempPosition += Vector2Int.right;
        if (GameBoard.instance.gameData.IsValidCoordinate(tempPosition))
        {
            ChessPiece objPiece = GameBoard.instance.gameData.GetPiece(tempPosition);
            if (objPiece != null && objPiece.pieceColor == player.playerColor)
            {
                objPiece.maxHP += heavyArmoredInfantryComponent.increasedHP;
                objPiece.buff.AddBuffByValue(heavyArmoredInfantryComponent.cardName, Buff.BuffType.HP, heavyArmoredInfantryComponent.increasedHP, false);
            }
        }
    }
}
