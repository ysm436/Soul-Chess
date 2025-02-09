using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GraceOfKingArthurEffect : Effect
{
    [SerializeField] private int multiplier;

    private List<(ChessPiece, int)> increasedADList;
    private int turnCount;

    // 공격력 2배 만드는 함수
    private void MultiplyAD(List<ChessPiece> pieces)
    {
        increasedADList = new List<(ChessPiece, int)>();

        foreach (ChessPiece piece in pieces)
        {
            int increasedAD = piece.AD * (multiplier - 1);
            piece.AD += increasedAD;
            piece.buff.AddBuffByValue("아서왕의 가호", Buff.BuffType.AD, increasedAD, true);

            increasedADList.Add((piece, increasedAD));
        }
    }

    // 공격력 원상복구하는 함수
    private void DecreaseAD()
    {
        turnCount++;

        // 내 다음 턴이 종료될 때만 실행
        if (turnCount != 2) return;

        for (int i = increasedADList.Count - 1; i >= 0; i--)
        {
            ChessPiece piece = increasedADList[i].Item1;
            int increasedAD = increasedADList[i].Item2;

            piece.AD -= increasedAD;
            piece.buff.TryRemoveSpecificBuff("아서왕의 가호", Buff.BuffType.AD);

            increasedADList.RemoveAt(i);
        }
    }

    // 카드 사용 시 호출
    public override void EffectAction(PlayerController player)
    {
        turnCount = 0;

        List<ChessPiece> myPieceList = GameBoard.instance.gameData.pieceObjects
            .Where(piece => piece.pieceColor == player.playerColor).ToList();

        MultiplyAD(myPieceList);

        player.OnMyTurnEnd += DecreaseAD;
    }


}
