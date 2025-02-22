using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Kraken : SoulCard
{
    protected override int CardID => Card.cardIdDict["심해의 바다괴물"];

    public int repeat;
    public int damage;

    protected override void Awake()
    {
        base.Awake();
    }

    public void OnKilledEffect(ChessPiece chessPiece)
    {
        GameBoard.instance.chessBoard.DamageByKrakenEffect(GetComponent<KrakenEffect>().effectPrefab, InfusedPiece, repeat, damage);
    }

    public override void AddEffect()
    {
        InfusedPiece.OnKilled += OnKilledEffect;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        InfusedPiece.OnKilled -= OnKilledEffect;
    }
}
