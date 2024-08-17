using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DavyJonesEffect : Effect
{
    int change = 10;
    public override void EffectAction()
    {
        //강림 시점에서 죽은 기물들 개수 (16-현재 기물 수) 만큼 스탯 증가 적용 후 남아있는 기물의 Onkilled 시 스탯 10씩 추가
        List<ChessPiece> pieceList = GameBoard.instance.gameData.pieceObjects.Where(piece => piece.pieceColor == GameBoard.instance.myController.playerColor).ToList();
        int deadMyPieceCount = 16 - pieceList.Count;

        gameObject.GetComponent<SoulCard>().InfusedPiece.AD += deadMyPieceCount * change;
        gameObject.GetComponent<SoulCard>().InfusedPiece.maxHP += deadMyPieceCount * change;

        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.AD, deadMyPieceCount * change, true);
        gameObject.GetComponent<SoulCard>().InfusedPiece.buff.AddBuffByValue(gameObject.GetComponent<SoulCard>().cardName, Buff.BuffType.HP, deadMyPieceCount * change, true);

        foreach (var piece in pieceList)
        {
            piece.OnKilled += gameObject.GetComponent<DavyJones>().IncreaseStat;
        }

        gameObject.GetComponent<SoulCard>().InfusedPiece.OnSoulRemoved += gameObject.GetComponent<SoulCard>().RemoveEffect;
    }
}
