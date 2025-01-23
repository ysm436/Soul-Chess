using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TutorialGameOverUI : MonoBehaviour
{
    public GameObject WinAnnounce;
    public GameObject DefeatedAnnounce;
    public GameObject SurrenderText;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnWin()
    {
        ShowWin(false);
    }
    public void OnDefeated()
    {
        ShowDefeated();
    }
    public void OnSurrender()
    {
        ShowDefeated();
        ShowWin(true);
    }
    public void ShowWin(bool isSurrender)
    {
        gameObject.SetActive(true);
        WinAnnounce.SetActive(true);
        if (isSurrender)
            SurrenderText.SetActive(true);
    }
    public void ShowDefeated()
    {
        gameObject.SetActive(true);
        DefeatedAnnounce.SetActive(true);
    }
    public void OnExit()
    {
        Destroy(GameBoard.instance.gameObject);
        SceneManager.LoadScene("MainScene");
    }
}
