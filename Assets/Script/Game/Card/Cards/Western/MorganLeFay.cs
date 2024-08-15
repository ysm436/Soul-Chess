using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganLeFay : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        //강림 시 OnMyTurnEnd에 함수 추가
        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;

        chessPiece.OnSoulRemoved += RemoveEffect;
    }

    public void SoulEffect2() //턴 종료 시마다 호출되는 함수인데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameBoard.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameBoard.instance.gameData.pieceObjects[i].pieceColor == GameBoard.instance.myController.playerColor)
            {
                //체력이 깎여있는 아군만 pieceList에 추가해서 그 중에서 무작위 회복
                if (GameBoard.instance.gameData.pieceObjects[i].GetHP != GameBoard.instance.gameData.pieceObjects[i].maxHP)
                    pieceList.Add(GameBoard.instance.gameData.pieceObjects[i]);
            }
        }
        if (pieceList.Count > 0)
        {
            int temp = Random.Range(0, pieceList.Count);
            pieceList[temp].AddHP(pieceList[temp].maxHP - pieceList[temp].GetHP);
        }
    }

    public override void AddEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd += SoulEffect2;
    }
    public override void RemoveEffect()
    {
        GameBoard.instance.myController.OnMyTurnEnd -= SoulEffect2;
    }
}
