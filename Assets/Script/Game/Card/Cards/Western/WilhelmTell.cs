using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WilhelmTell : SoulCard
{
    protected override int CardID => cardIdDict["빌헬름 텔"];

    private void OnInfused(ChessPiece piece)
    {
        GetComponent<WilhelmTellEffect>().targetTypes[0].tileCoordinate = piece.coordinate;
    }


    protected override void Awake()
    {
        base.Awake();
    }
    private void Start()
    {
        OnInfuse += OnInfused;
    }
    public override void AddEffect()
    {

    }
    public override void RemoveEffect()
    {

    }
}
