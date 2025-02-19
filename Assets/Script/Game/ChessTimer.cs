using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ChessTimer : MonoBehaviour
{
    private int _minute;
    private int minute
    {
        get { return _minute; }

        set {
            _minute = value;
            timerMText.text = _minute.ToString();
        }
    }
    private int _second;
    private int second
    {
        get { return _second; }

        set {
            _second = value;
            timerSText.text = _second.ToString("D2");
        }
    }

    private GameBoard.PlayerColor _playercolor;
    public GameBoard.PlayerColor playerColor
    {
        get { return _playercolor; }

        set {
            if (value == GameBoard.PlayerColor.White)
            {
                timerBG.sprite = timerBGList[0];
                foreach (var timerText in gameObject.GetComponentsInChildren<TextMeshPro>())
                {
                    timerText.color = Color.black;
                }
            }
            else
            {
                timerBG.sprite = timerBGList[1];
                foreach (var timerText in gameObject.GetComponentsInChildren<TextMeshPro>())
                {
                    timerText.color = Color.white;
                }
            }

            _playercolor = value;
        }
    }

    [SerializeField] private SpriteRenderer timerBG;
    [SerializeField] private List<Sprite> timerBGList;
    [SerializeField] private TextMeshPro timerMText;
    [SerializeField] private TextMeshPro timerSText;

    private Coroutine timerCoroutine;

    private void Awake()
    {
        minute = 2;
        second = 0;
    }

    public void StartTimer()
    {
        minute = 2;
        second = 0;
        timerCoroutine = StartCoroutine(TimerCoroutine());
    }

    public void StopTimer()
    {
        StopCoroutine(timerCoroutine);
        timerCoroutine = null;
    }

    private IEnumerator TimerCoroutine()
    {
        while (true)
        {
            if (second == 0)
            {
                if (minute == 0)
                {
                    if (SceneManager.GetActiveScene().name == "PvEGameScene")
                    {
                        PvELocalController localControllerComponent = GetComponentInParent<PvELocalController>();
                        PvEPlayerController playerController;
                        if (playerColor == GameBoard.PlayerColor.White)
                            playerController = localControllerComponent.whiteController as PvEPlayerController;
                        else
                            playerController = localControllerComponent.blackController as PvEPlayerController;

                        if (playerController == GameBoard.instance.myController)
                        {
                            playerController.PvERandomMovePiece();
                        }
                    }
                    else
                    {
                        LocalController localControllerComponent = GetComponentInParent<LocalController>();
                        PlayerController playerController;
                        if (playerColor == GameBoard.PlayerColor.White)
                            playerController = localControllerComponent.whiteController;
                        else
                            playerController = localControllerComponent.blackController;
                        
                        if (playerController == GameBoard.instance.myController)
                        {
                            playerController.RandomMovePiece();
                        }
                    }
                }
                else
                {
                    minute -= 1;
                    second = 59;
                }
            }
            else
            {
                second -= 1;
            }

            yield return new WaitForSeconds(1f);
        }
    }
}
