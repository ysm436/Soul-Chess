using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
//TODO: targettype이 card인 경우의 gettargetlist 구현

public abstract class TargetingEffect : Effect
{
    /// <summary>
    ///     효과의 대상으로 지정가능한 조건, 여러개의 대상을 지정하는 것을 고려해 만들었으며 각 카드 사용시 targetTypes 조건에 해당되는 오브젝트들이 targets에 저장됨
    /// </summary>
    [SerializeField]
    public List<EffectTarget> targetTypes = new List<EffectTarget>();
    public Vector3[] serializedTargetData
    {
        get
        {
            Vector3[] targetData = new Vector3[targets.Count];
            for (int i = 0; i < targets.Count; i++)
            {
                targetData[i] = new Vector3(targets[i].coordinate.x, targets[i].coordinate.y, (float)targetTypes[i].targetType);
            }
            return targetData;
        }
    }
    protected List<TargetableObject> targets = new List<TargetableObject>();

    [SerializeField] private bool isPositiveEffect;
    public bool IsPositiveEffect { get => isPositiveEffect; }

    [SerializeField] private bool isNegativeEffect;
    public bool IsNegativeEffect { get => isNegativeEffect; }

    public EffectTarget GetTargetType()
    {
        return targetTypes[targets.Count];
    }
    public bool isAvailable(GameBoard.PlayerColor playerColor)
    {
        foreach (var t in targetTypes)
        {
            if (t.GetTargetList(playerColor).Count == 0)
                return false;
        }
        return true;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="target"></param>
    /// <returns>
    ///     targets가 모두 결정된 경우
    /// </returns>
    public bool SetTarget(TargetableObject target)
    {
        targets.Add(target);
        return targets.Count >= targetTypes.Count;
    }
    [Serializable]
    public class EffectTarget
    {
        public EffectTarget(TargetType targetType, ChessPiece.PieceType targetPieceType, bool isOpponent, bool isFriendly, Predicate<ChessPiece> condition = null)
        {
            this.targetType = targetType;
            this.targetPieceType = targetPieceType;
            this.isOpponent = isOpponent;
            this.isFriendly = isFriendly;
            this.condition = condition;
        }
        public EffectTarget(TargetType targetType, TargetTiles targetTiles, Vector2Int tileCoordinate)
        {
            this.targetType = targetType;
            this.targetTiles = targetTiles;
            this.tileCoordinate = tileCoordinate;
        }
        public TargetType targetType;
        public ChessPiece.PieceType targetPieceType;
        public bool isOpponent;
        public bool isFriendly;
        Predicate<ChessPiece> condition;

        public TargetTiles targetTiles;
        [HideInInspector] public Vector2Int tileCoordinate;
        public enum TargetTiles
        {
            Around,
            Row,
            Colom
        }

        public List<TargetableObject> GetTargetList(GameBoard.PlayerColor playerColor)
        {
            switch (targetType)
            {
                case TargetType.Piece:
                    return GameBoard.instance.gameData.pieceObjects.Where<ChessPiece>(
                        obj => (
                            (obj.pieceType & targetPieceType) != 0)
                            && (obj.pieceColor == playerColor ? isFriendly : isOpponent)
                            && (obj.pieceColor == playerColor || (obj.pieceType != ChessPiece.PieceType.King && obj.GetKeyword(Keyword.Type.Stealth) != 1))
                            && (condition == null ? true : condition(obj)
                        )).Cast<TargetableObject>().ToList();
                case TargetType.Tile:
                    List<TargetableObject> targetList = new();
                    switch (targetTiles)
                    {
                        case TargetTiles.Row:
                            foreach (TargetableObject t in GameBoard.instance.gameData.boardSquares)
                                if (t.coordinate.y == tileCoordinate.y) targetList.Add(t);
                            break;
                        case TargetTiles.Colom:
                            foreach (TargetableObject t in GameBoard.instance.gameData.boardSquares)
                                if (t.coordinate.x == tileCoordinate.x) targetList.Add(t);
                            break;
                        case TargetTiles.Around:
                            int distance_x;
                            int distance_y;
                            foreach (TargetableObject t in GameBoard.instance.gameData.boardSquares)
                            {
                                distance_x = t.coordinate.x - tileCoordinate.x;
                                distance_y = t.coordinate.y - tileCoordinate.y;
                                if (distance_x >= -1 && distance_x <= 1 && distance_y >= -1 && distance_y <= 1 && !(distance_x == 0 && distance_y == 0))
                                    if (GameBoard.instance.gameData.IsValidCoordinate(t.coordinate))
                                        targetList.Add(t);
                            }
                            break;
                    }
                    return targetList;
                default:
                    return new List<TargetableObject>();
            }
        }
    }
    public void SetTargetsByCoordinate(Vector2Int[] targetCoordinateArray, TargetingEffect.TargetType[] targetTypes)
    {
        TargetableObject target;
        for (int i = 0; i < targetCoordinateArray.Length; i++)
        {
            UnityEngine.Debug.Log(targetCoordinateArray[i]);
            switch (targetTypes[i])
            {
                case TargetingEffect.TargetType.Piece:
                    target = GameBoard.instance.gameData.GetPiece(targetCoordinateArray[i]);
                    UnityEngine.Debug.Log("TargetType is piece");
                    break;
                case TargetingEffect.TargetType.Tile:
                    target = GameBoard.instance.gameData.GetBoardSquare(targetCoordinateArray[i]);
                    UnityEngine.Debug.Log("TargetType is tile");
                    break;
                default:
                    target = null;
                    UnityEngine.Debug.LogError("TargetType is null");
                    break;
            }
            targets.Add(target);
        }
    }
    public enum TargetType
    {
        Piece,
        Tile
    }
}
