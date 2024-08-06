using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

//TODO: targettype이 card인 경우의 gettargetlist 구현

public abstract class TargetingEffect : Effect
{
    /// <summary>
    ///     효과의 대상으로 지정가능한 조건, 여러개의 대상을 지정하는 것을 고려해 만들었으며 각 카드 사용시 targetTypes 조건에 해당되는 오브젝트들이 targets에 저장됨
    /// </summary>
    [SerializeField]
    public List<EffectTarget> targetTypes = new List<EffectTarget>();
    protected List<TargetableObject> targets = new List<TargetableObject>();

    public EffectTarget GetTargetType()
    {
        return targetTypes[targets.Count];
    }
    public bool isAvailable(GameManager.PlayerColor playerColor)
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
        public EffectTarget(TargetType targetType, ChessPiece.PieceType targetPieceType, bool isOpponent, bool isFriendly)
        {
            this.targetType = targetType;
            this.targetPieceType = targetPieceType;
            this.isOpponent = isOpponent;
            this.isFriendly = isFriendly;
        }
        public TargetType targetType;
        public ChessPiece.PieceType targetPieceType;
        public bool isOpponent;
        public bool isFriendly;
        public List<TargetableObject> GetTargetList(GameManager.PlayerColor playerColor)
        {
            switch (targetType)
            {
                case TargetType.Piece:
                    return GameManager.instance.gameData.pieceObjects.Where<ChessPiece>(
                        obj => (
                            (obj.pieceType & targetPieceType) != 0)
                            && (obj.pieceColor == playerColor ? isFriendly : isOpponent)
                        // 여기에 조건 추가하시면 될 것 같습니다.
                        ).Cast<TargetableObject>().ToList();
                default:
                    return new List<TargetableObject>();
            }
        }
    }
    public enum TargetType
    {
        Piece,
        Card
    }
}
