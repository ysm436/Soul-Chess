using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganLeFay : SoulCard
{
    protected override int CardID => Card.cardIdDict["모르건 르 페이"];

    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<SoulCard>().OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        //강림 시 OnMyTurnEnd에 함수 추가
        GameBoard.instance.whiteController.OnMyTurnEnd += () => SoulEffect2(InfusedPiece);
    }

    public void SoulEffect2(ChessPiece piece) //턴 종료 시마다 호출되는 함수인데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameBoard.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameBoard.instance.gameData.pieceObjects[i].pieceColor == piece.pieceColor)
            {
                //체력이 깎여있는 아군만 pieceList에 추가해서 그 중에서 무작위 회복
                if (GameBoard.instance.gameData.pieceObjects[i].HP != GameBoard.instance.gameData.pieceObjects[i].maxHP)
                    pieceList.Add(GameBoard.instance.gameData.pieceObjects[i]);
            }
        }
        if (pieceList.Count > 0)
        {
            int temp = Random.Range(0, pieceList.Count);
            pieceList[temp].HP = pieceList[temp].maxHP;
        }
    }
}
