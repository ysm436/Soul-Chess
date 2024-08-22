using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Hades : SoulCard
{
    protected override int CardID => Card.cardIdDict["하데스"];

    private PlayerController playercontroller;
    protected override void Awake()
    {
        base.Awake();
    }

    public override void AddEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercontroller = GameBoard.instance.whiteController;
        else
            playercontroller = GameBoard.instance.blackController;

        MakeHadesAffect();

        playercontroller.OnMyTurnStart += MakeHadesAffect;
        playercontroller.OnMyTurnEnd += RemoveHadesAffect;
    }

    public override void RemoveEffect()
    {
        if (InfusedPiece.pieceColor == GameBoard.PlayerColor.White)
            playercontroller = GameBoard.instance.whiteController;
        else
            playercontroller = GameBoard.instance.blackController;

        RemoveHadesAffect();

        playercontroller.OnMyTurnStart -= MakeHadesAffect;
        playercontroller.OnMyTurnEnd -= RemoveHadesAffect;
    }

    private void MakeHadesAffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();
        targets.Remove(InfusedPiece);
        
        foreach (var target in targets)
        {
            target.AffectByHades = true;
        }
    }

    private void RemoveHadesAffect()
    {
        List<ChessPiece> targets = GameBoard.instance.gameData.pieceObjects.Where(obj => obj.pieceColor == InfusedPiece.pieceColor).ToList();
        targets.Remove(InfusedPiece);
        
        foreach (var target in targets)
        {
            target.AffectByHades = false;
        }
    }
}
