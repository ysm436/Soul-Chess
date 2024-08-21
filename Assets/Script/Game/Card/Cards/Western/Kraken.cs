using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Kraken : SoulCard
{
    protected override int CardID => Card.cardIdDict["크라켄"];

    public int repeat;
    public int damage;

    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InfuseEffect;
    }

    public void InfuseEffect(ChessPiece chessPiece)
    {
        chessPiece.OnKilled += OnKilledEffect;

        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public void OnKilledEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor != InfusedPiece.pieceColor).ToList();

        for (int i = 0; i < repeat; i++)
        {
            int ran = Random.Range(0, targets.Count);
            targets[ran].MinusHP(damage);

            if (!targets[ran].isAlive)
                targets.Remove(targets[ran]);
        }
    }

    public override void AddEffect()
    {
        InfusedPiece.OnKilled += OnKilledEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= OnKilledEffect;
    }
}
