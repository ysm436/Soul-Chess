using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SoulOrb : MonoBehaviour
{
    public TextMeshPro soulEssenceText;
    public GameBoard.PlayerColor playerColor;

    public void Update() //최적화하려면 더 나은 방식으로 바꿀 수 있을 것 같음
    {
        if (playerColor == GameBoard.PlayerColor.White)
        {
            PlayerData white_data = GameBoard.instance.gameData.playerWhite;
            soulEssenceText.text = white_data.soulEssence.ToString() + " / " + white_data.soulOrbs.ToString();
        }
        else
        {
            PlayerData black_data = GameBoard.instance.gameData.playerBlack;
            soulEssenceText.text = black_data.soulEssence.ToString() + " / " + black_data.soulOrbs.ToString();
        }
    }
}
