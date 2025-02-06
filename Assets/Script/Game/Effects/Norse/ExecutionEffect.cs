using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExecutionEffect : TargetingEffect
{
    [SerializeField] public int basicAD;
    [SerializeField] public int insteadAD;
    [SerializeField] public int standardHP;

    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    SoulCard soul = null;

    void Awake()
    {
        if (soul == null) soul = gameObject.GetComponent<SoulCard>();
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.soul != null;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, condition);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects;
        ChessPiece kingPiece = null;

        foreach (var chessPiece in pieceList)
        {
            if (chessPiece.pieceType == ChessPiece.PieceType.King && chessPiece.pieceColor == player.playerColor)
                kingPiece = chessPiece;
        }

        if (kingPiece == null)
            Debug.Log("Execution: No King Piece");

        foreach (var target in targets)
        {
            if ((target as ChessPiece).GetHP <= standardHP)
            {
                Debug.Log("Execution: insteadAD Attack");
                GameBoard.instance.chessBoard.DamageByCardEffect(effectPrefab, kingPiece, target as ChessPiece, insteadAD);
            }
            else
            {
                Debug.Log("Execution: basicAD Attack");
                GameBoard.instance.chessBoard.DamageByCardEffect(effectPrefab, kingPiece, target as ChessPiece, basicAD);
            }
        }
    }
}
