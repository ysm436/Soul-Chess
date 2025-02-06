using System;
using UnityEngine;

public class DavidEffect : TargetingEffect
{
    [SerializeField] private int standardAD = 7;
    ChessPiece.PieceType targetPieceRestriction =
        ChessPiece.PieceType.Pawn |
        ChessPiece.PieceType.Knight |
        ChessPiece.PieceType.Bishop |
        ChessPiece.PieceType.Rook |
        ChessPiece.PieceType.Quene |
        ChessPiece.PieceType.King;

    void Awake()
    {
        Predicate<ChessPiece> condition = (ChessPiece piece) => piece.AD >= standardAD;
        EffectTarget effectTarget = new EffectTarget(TargetType.Piece, targetPieceRestriction, true, false, null);
        targetTypes.Add(effectTarget);
    }

    public override void EffectAction(PlayerController player)
    {
        David davidComponent = GetComponent<David>();

        foreach (var target in targets)
        {
            Debug.Log("David Effect");
            GameBoard.instance.chessBoard.KillByCardEffect(effectPrefab, davidComponent.InfusedPiece, target as ChessPiece);
        }
    }
}
