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

        temp = "종류 : " + pieceTypeText + "\nHP : " + chessPiece.GetHP + " / " + chessPiece.maxHP;
        if (chessPiece.soul != null) temp += "(+" + chessPiece.soul.HP + ")";
        temp += "\nAD : " + chessPiece.AD;
        if (chessPiece.soul != null) temp += "(+" + chessPiece.soul.AD + ")";
        if (chessPiece.soul != null) temp += "\n \n[부여된 영혼]\n" + chessPiece.soul.cardName;
        //TODO : 버프/디버프 기능 완성 후 BuffText/DebuffText 조합 필요

        infoText.text = temp;
    }
}
