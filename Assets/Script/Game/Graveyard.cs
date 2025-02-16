using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Graveyard : MonoBehaviour
{
    private GameBoard.PlayerColor _playercolor;
    public GameBoard.PlayerColor playerColor
    {
        get { return _playercolor; }

        set
        {
            if (value == GameBoard.PlayerColor.White)
            {
                graveyardBG.sprite = grvateyardBGList[0];
                graveyardCountText.color = Color.black;
            }
            else
            {
                graveyardBG.sprite = grvateyardBGList[1];
                graveyardCountText.color = Color.white;
            }
        }
    }

    [SerializeField] private SpriteRenderer graveyardBG;
    [SerializeField] private List<Sprite> grvateyardBGList;
    [SerializeField] private TextMeshPro graveyardCountText;
    [SerializeField] private int graveyardCount = 0;

    public void IncreaseGraveyard()
    {
        graveyardCount++;
        graveyardCountText.text = graveyardCount.ToString();
    }
}
