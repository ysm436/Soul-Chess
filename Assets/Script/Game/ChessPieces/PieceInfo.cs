using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PieceInfo : MonoBehaviour
{
    [SerializeField]
    private TextMeshPro infoText;
    
    public void EditDescription(ChessPiece chessPiece)
    {
        string pieceTypeText = "없음";
        //string pieceBuffText = "없음";
        //string pieceDebuffText = "없음";
        string temp = "";

        switch(chessPiece.pieceType)
        {
            case ChessPiece.PieceType.Pawn : pieceTypeText = "폰"; break;
            case ChessPiece.PieceType.Knight : pieceTypeText = "나이트"; break;
            case ChessPiece.PieceType.Bishop : pieceTypeText = "비숍"; break;
            case ChessPiece.PieceType.Rook : pieceTypeText = "룩"; break;
            case ChessPiece.PieceType.Quene : pieceTypeText = "퀸"; break;
            case ChessPiece.PieceType.King : pieceTypeText = "킹"; break;
            default : pieceTypeText = "오류"; break;
        }

        temp = "종류: " + pieceTypeText + "\nHP: " + chessPiece.GetHP + " / " + chessPiece.maxHP;
        if (chessPiece.soul != null) temp += "(+" + chessPiece.soul.HP + ")";
        temp += "\nAD: " + chessPiece.AD;
        if (chessPiece.soul != null) temp += "(+" + chessPiece.soul.AD + ")";
        if (chessPiece.soul != null) temp += "\n영혼: " + chessPiece.soul.cardName;

        //Buff Text
        temp += "\n[적용 효과 목록]\n";
        foreach (Buff.BuffInfo buffInfo in chessPiece.buff.buffList)
        {
            string buffText;
            string currentSourceName = buffInfo.sourceName;
            string currentValue = buffInfo.value > 0 ? "+" + buffInfo.value.ToString() : buffInfo.value.ToString();
            string currentDescription = buffInfo.description;
            switch (buffInfo.buffType)
            {
                case Buff.BuffType.HP:
                    buffText = $"{currentSourceName}: HP {currentValue}";
                    break;
                case Buff.BuffType.AD:
                    buffText = $"{currentSourceName}: AD {currentValue}";
                    break;
                case Buff.BuffType.MoveCount:
                    buffText = $"{currentSourceName}: 이동 횟수 {currentValue}";
                    break;
                case Buff.BuffType.Defense: // 방어도 키워드 버프
                    buffText = $"{currentSourceName}: 방어도 {currentValue}";
                    break;
                case Buff.BuffType.Description:
                    buffText = currentDescription;
                    break;
                default: // 방어도 제외 키워드 버프 공통
                    buffText = $"{currentDescription}"; //출처 표기 시 $"'{currentSourceName}: {currentDescription}";
                    break;
            }
            buffText += "\n";

            temp += buffText;
        }

        infoText.text = temp;
    }
}
