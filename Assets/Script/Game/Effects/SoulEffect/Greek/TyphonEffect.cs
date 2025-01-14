using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TyphonEffect : Effect
{
    public override void EffectAction(PlayerController player) //자신의 핸드와 덱의 모든 카드 파괴
    {
        GameBoard.PlayerColor myColor = player.playerColor;

        if (myColor == GameBoard.PlayerColor.White)
        {
            GameBoard.instance.gameData.playerWhite.RemoveHandCards();
            GameBoard.instance.gameData.playerWhite.RemoveDeckCards();
        }
        else 
        {
            GameBoard.instance.gameData.playerBlack.RemoveHandCards();
            GameBoard.instance.gameData.playerBlack.RemoveDeckCards();
        }
        
        gameObject.GetComponent<Typhon>().AddEffect();
    }
}
