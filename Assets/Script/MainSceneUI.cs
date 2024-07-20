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

    private void Awake()
    {
        matchingButton.onClick.AddListener(LoadMatchingScene);
        deckBuildingButton.onClick.AddListener(LoadDeckBuildingScene);
        quitButton.onClick.AddListener(QuitGame);
    }

    private void LoadMatchingScene()
    {
        SceneManager.LoadScene("MatchingScene");
    }

    private void LoadDeckBuildingScene()
    {
        SceneManager.LoadScene("DeckBuildingScene");
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
