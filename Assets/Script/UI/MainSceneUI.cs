using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MainSceneUI : MonoBehaviour
{
    [SerializeField] private Button matchingButton;
    [SerializeField] private Button deckBuildingButton;
    [SerializeField] private Button quitButton;
    [SerializeField] private Button pveButton;
    [SerializeField] private Button creditButton;

    private void Start()
    {
        matchingButton.onClick.AddListener(GameManager.instance.LoadMatchingScene);
        deckBuildingButton.onClick.AddListener(GameManager.instance.LoadDeckBuildingScene);
        quitButton.onClick.AddListener(QuitGame);
        pveButton.onClick.AddListener(GameManager.instance.LoadPvELobbyScene);
        creditButton.onClick.AddListener(GameManager.instance.LoadCreditScene);
    }

    private void QuitGame()
    {
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        Application.Quit();
#endif
    }
}
