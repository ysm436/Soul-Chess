using System.Collections;
using System.Collections.Generic;
using Photon.Pun;
using Unity.VisualScripting;
using UnityEngine;

public class GameOverUI : MonoBehaviour
{
    public GameObject WinAnnounce;
    public GameObject DefeatedAnnounce;

    private void Awake()
    {
        gameObject.SetActive(false);
    }
    public void OnWin()
    {
        ShowWin();
    }
    public void OnDefeated()
    {
        ShowDefeated();
    }
    public void ShowWin()
    {
        gameObject.SetActive(true);
        WinAnnounce.SetActive(true);
    }
    public void ShowDefeated()
    {
        gameObject.SetActive(true);
        DefeatedAnnounce.SetActive(true);
    }
    public void OnExit()
    {
        GameManager.instance.LoadMainSceneFromGameScene();
    }
}
