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
            soulEssenceText.text = GameBoard.instance.gameData.playerWhite.soulEssence.ToString();
        }
        else
        {
            soulEssenceText.text = GameBoard.instance.gameData.playerBlack.soulEssence.ToString();
        }
    }
}
