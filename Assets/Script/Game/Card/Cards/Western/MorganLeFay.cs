using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorganLeFay : SoulCard
{
    protected override void Awake()
    {
        base.Awake();
        gameObject.GetComponent<SoulCard>().OnInfuse += SoulEffect;
    }

    public void SoulEffect(ChessPiece chessPiece)
    {
        //강림 시 OnMyTurnEnd에 함수 추가
        GameManager.instance.whiteController.OnMyTurnEnd += () => SoulEffect2(InfusedPiece);
    }

    public void SoulEffect2(ChessPiece piece) //턴 종료 시마다 호출되는 함수인데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameManager.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameManager.instance.gameData.pieceObjects[i].pieceColor == piece.pieceColor)
            {
                //체력이 깎여있는 아군만 pieceList에 추가해서 그 중에서 무작위 회복
                if (GameManager.instance.gameData.pieceObjects[i].HP != GameManager.instance.gameData.pieceObjects[i].maxHP)
                    pieceList.Add(GameManager.instance.gameData.pieceObjects[i]);
            }
        }
        if (pieceList.Count > 0)
        {
            int temp = Random.Range(0, pieceList.Count);
            pieceList[temp].HP = pieceList[temp].maxHP;
        }
    }
}
