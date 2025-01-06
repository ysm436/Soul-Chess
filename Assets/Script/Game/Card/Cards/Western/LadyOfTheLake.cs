using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyOfTheLake : SoulCard
{
    protected override int CardID => Card.cardIdDict["호수의 여인"];

    [HideInInspector] public PlayerController player = null;
    public int increasedAD = 10;
    public int increasedHP = 10;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SoulEffect() //턴 종료 시마다 호출될 텐데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameBoard.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameBoard.instance.gameData.pieceObjects[i].pieceColor == InfusedPiece.pieceColor)
                pieceList.Add(GameBoard.instance.gameData.pieceObjects[i]);
        }

        int temp = SynchronizedRandom.Range(0, pieceList.Count);
        pieceList[temp].AD += increasedAD;
        pieceList[temp].maxHP += increasedHP;

        pieceList[temp].buff.AddBuffByValue(cardName, Buff.BuffType.AD, increasedAD, true);
        pieceList[temp].buff.AddBuffByValue(cardName, Buff.BuffType.HP, increasedHP, true);
    }

    public override void AddEffect()
    {
        player.OnMyTurnEnd += SoulEffect;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        player.OnMyTurnEnd -= SoulEffect;
    }
}
