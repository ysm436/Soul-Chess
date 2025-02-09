using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DG.Tweening;

public class Thor : SoulCard
{
    protected override int CardID => Card.cardIdDict["토르"];

    [HideInInspector] public PlayerController player = null;

    protected override void Awake()
    {
        base.Awake();
    }

    private Tween AttackRandomEnemyPiece()
    {
        int attackDamage = InfusedPiece.AD;

        Debug.Log("Thor: Soul Effect");
        return GameBoard.instance.chessBoard.DamageByThorEffect(GetComponent<ThorEffect>().effectPrefab, InfusedPiece, attackDamage);
    }

    public override void AddEffect()
    {
        if (player != null) player.OnMyTurnEndAnimation += AttackRandomEnemyPiece;
        InfusedPiece.OnSoulRemoved += RemoveEffect;
    }

    public override void RemoveEffect()
    {
        if (player != null) player.OnMyTurnEndAnimation -= AttackRandomEnemyPiece;
    }
}
