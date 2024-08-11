using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LadyOfTheLake : SoulCard
{
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

    public void SoulEffect2(ChessPiece piece) //턴 종료 시마다 호출될 텐데
    {
        List<ChessPiece> pieceList = new List<ChessPiece>();
        for (int i = GameBoard.instance.gameData.pieceObjects.Count - 1; i >= 0; i--)
        {
            if (GameBoard.instance.gameData.pieceObjects[i].pieceColor == piece.pieceColor)
                pieceList.Add(GameBoard.instance.gameData.pieceObjects[i]);
        }

        int temp = Random.Range(0, pieceList.Count);
        pieceList[temp].maxHP += 20;
        pieceList[temp].AD += 20;
    }
}
