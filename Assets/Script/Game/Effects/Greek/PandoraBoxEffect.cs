using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PandoraBoxEffect : Effect
{
    [SerializeField] private int changeADorHP = 10;

    public override void EffectAction(PlayerController player)
    {
        List<ChessPiece> pieces = GameBoard.instance.gameData.pieceObjects.Where(piece =>
            piece.pieceColor == player.playerColor && piece.soul != null).ToList();

        foreach (ChessPiece piece in pieces)
        {
            int temp = SynchronizedRandom.Range(0,2);
            if (temp == 0)
            {
                Debug.Log("PandoraBox: Spell Effect AD");
                GameBoard.instance.chessBoard.TileEffect(effectPrefab, piece);
                piece.AD += changeADorHP;
                piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.AD, changeADorHP, true);
            }
            else
            {
                Debug.Log("PandoraBox: Spell Effect HP");
                GameBoard.instance.chessBoard.TileEffect(effectPrefab, piece);
                piece.maxHP += changeADorHP;
                piece.buff.AddBuffByValue("판도라의 상자", Buff.BuffType.HP, changeADorHP, true);
            }
        }
    }

}
