using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

public class Mimir : SoulCard
{
    protected override int CardID => Card.cardIdDict["미미르"];
    private PlayerData playercolor;
    private int reduction = 1;

    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercolor = GameBoard.instance.gameData.playerWhite;
        else
            playercolor = GameBoard.instance.gameData.playerBlack;

        foreach (var card in playercolor.hand)
        {
            card.cost = (card.cost - reduction) > 0 ? (card.cost - reduction) : 0;
        }
        playercolor.OnGetCard += CardCostReduction;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        bool isExistOurMerlin = false;
        foreach (var piece in GameBoard.instance.gameData.pieceObjects)
        {
            if (piece.pieceColor == InfusedPiece.pieceColor && piece.soul != null)
            {
                if (!(piece.soul.cardName == "멀린" &&
                    (piece.soul.InfusedPiece.GetKeyword(Keyword.Type.Restraint) != 0 || //구속이나 침묵이 되어있지 않은 아군 멀린이 없다면
                     piece.soul.InfusedPiece.GetKeyword(Keyword.Type.Silence) != 0)))
                    isExistOurMerlin = true; break;
            }
        }
        foreach (var card in playercolor.hand)
        {
            if (card.GetComponent<SpellCard>() == null)
                card.cost += reduction;
            else { //SpellCard면
                if (!isExistOurMerlin) card.cost += reduction;
            }
        }
        playercolor.OnGetCard -= CardCostReduction;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void CardCostReduction(Card card)
    {
        card.cost -= reduction;
        if (card.cost < 0)
        {
            card.cost = 0;
        }
    }
}