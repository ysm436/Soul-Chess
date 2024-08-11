using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Kraken : SoulCard
{
    public int repeat;
    public int damage;

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<SoulCard>().OnInfuse += InfuseEffect;
    }

    public void InfuseEffect(ChessPiece chessPiece)
    {
        chessPiece.OnKilled += OnkilledEffect;
    }

    public void OnkilledEffect(ChessPiece chessPiece)
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor != GameBoard.instance.myController.playerColor).ToList();

        for (int i = 0; i < repeat; i++)
        {
            int ran = Random.Range(0, targets.Count);
            targets[ran].HP -= damage;

            if (!targets[ran].isAlive)
                targets.Remove(targets[ran]);
        }
    }

}
