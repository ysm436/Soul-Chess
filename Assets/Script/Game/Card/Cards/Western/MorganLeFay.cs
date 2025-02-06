using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganLeFay : SoulCard
{
    protected override int CardID => Card.cardIdDict["모르건 르 페이"];

    [HideInInspector] public PlayerController player = null;

    protected override void Awake()
    {
        base.Awake();
    }

    public void SoulEffect() //턴 종료 시마다 호출되는 함수인데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameBoard.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameBoard.instance.gameData.pieceObjects[i].pieceColor == player.playerColor)
            {
                //체력이 깎여있는 아군만 pieceList에 추가해서 그 중에서 무작위 회복
                if (GameBoard.instance.gameData.pieceObjects[i].GetHP != GameBoard.instance.gameData.pieceObjects[i].maxHP)
                    pieceList.Add(GameBoard.instance.gameData.pieceObjects[i]);
            }
        }
        if (pieceList.Count > 0)
        {
            int temp = SynchronizedRandom.Range(0, pieceList.Count);

            ChessPiece target = pieceList[temp];
            Debug.Log("MorganLeFay: Soul Effect");
            GameBoard.instance.chessBoard.TileEffect(gameObject.GetComponent<MorganLeFayEffect>().effectPrefab, target);
            target.AddHP(target.maxHP - target.GetHP);
        }
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
