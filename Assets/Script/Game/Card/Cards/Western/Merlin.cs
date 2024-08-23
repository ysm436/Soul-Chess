using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Merlin : SoulCard
{
    protected override int CardID => Card.cardIdDict["멀린"];

    [HideInInspector] PlayerData player = null;

    Dictionary<string, int> spellCostDict = new Dictionary<string, int>() {
        {"처형", 4}, {"피의 독수리", 6}, {"판도라의 상자", 5}, {"라그나로크", 10},
        {"천둥번개", 3}, {"궁니르", 5}, {"드라우프노르", 0}, {"감반테인", 8},
    };
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            player = GameBoard.instance.gameData.playerWhite;
        else
            player = GameBoard.instance.gameData.playerBlack;      

        List<Card> ourSpellCards = player.hand.Where(card => card.GetComponent<SpellCard>() != null).ToList();

        foreach (var card in ourSpellCards)
        {
            card.cost = 0;
        }
        player.OnGetCard += DecreaseCost;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (player == null) return; 

        List<Card> ourSpellCards = player.hand.Where(card => card.GetComponent<SpellCard>() != null).ToList();

        int ourMiMirCount = 0;
        foreach (var piece in GameBoard.instance.gameData.pieceObjects)
        {
            if (piece.pieceColor == InfusedPiece.pieceColor && piece.soul != null)
            {
                if (!(piece.soul.cardName == "미미르" && //구속/침묵이 없는 아군 미미르 개수
                    (piece.GetKeyword(Keyword.Type.Restraint) != 0 || piece.GetKeyword(Keyword.Type.Silence) != 0)))
                    ourMiMirCount++;
            }
        }

        foreach (var card in ourSpellCards)
        {
            if (spellCostDict.ContainsKey(card.cardName))
            {
                card.cost = spellCostDict[card.cardName] - ourMiMirCount > 0 ? spellCostDict[card.cardName] - ourMiMirCount : 0;
            }
        }
        player.OnGetCard -= DecreaseCost;
        InfusedPiece.OnSoulRemoved -= RemoveEffect;
    }

    public void DecreaseCost(Card card)
    {
        if (card.GetComponent<SpellCard>() != null) card.cost = 0;
    }
}
