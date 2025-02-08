using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoulCost : MonoBehaviour
{
    public GameObject[] costIcons;
    public Sprite blackIcon;
    public Sprite whiteIcon;
    public GameBoard.PlayerColor playerColor;

    private void Start()
    {
        if (playerColor == GameBoard.PlayerColor.White)
        {
            foreach (var costIcon in costIcons)
            {
                costIcon.GetComponent<SpriteRenderer>().sprite = whiteIcon;
                costIcon.gameObject.SetActive(false);
            }
        }
        else
        {
            foreach (var costIcon in costIcons)
            {
                costIcon.GetComponent<SpriteRenderer>().sprite = blackIcon;
                costIcon.gameObject.SetActive(false);
            }
        }
    }

    public void Update() //최적화하려면 더 나은 방식으로 바꿀 수 있을 것 같음
    {
        if (playerColor == GameBoard.PlayerColor.White)
        {
            PlayerData white_data = GameBoard.instance.gameData.playerWhite;
            for (int i = 0; i < white_data.soulOrbs; i++)
            {
                costIcons[i].SetActive(true);
                costIcons[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            }
            for(int i = 0;i<white_data.soulEssence;i++)
                costIcons[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
        else
        {
            PlayerData black_data = GameBoard.instance.gameData.playerBlack;
            for (int i = 0; i < black_data.soulOrbs; i++)
            {
                costIcons[i].SetActive(true);
                costIcons[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 0.2f);
            }
            for (int i = 0; i < black_data.soulEssence; i++)
                costIcons[i].GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, 1);
        }
    }
}
