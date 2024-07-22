using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MatchingSceneUI : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button enterButton;

    private void Awake()
    {
        quitButton.onClick.AddListener(LoadMainScene);
        enterButton.onClick.AddListener(LoadLobbyScene);
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
}