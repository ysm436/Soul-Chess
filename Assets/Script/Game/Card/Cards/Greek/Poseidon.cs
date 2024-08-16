using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Poseidon : SoulCard
{
    protected override int CardID => Card.cardIdDict["포세이돈"];
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        for (int i = pieceList.Count - 1; i >= 0; i--)
        {
            if (pieceList[i] != gameObject.GetComponent<SoulCard>().InfusedPiece)
            {
                pieceList[i].MinusHP(25);
            }
        }
    }

    public override void AddEffect()
    {

    }

    public override void RemoveEffect()
    {

    }
}
