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

    private void Start()
    {
        matchingButton.onClick.AddListener(GameManager.instance.LoadMatchingScene);
        deckBuildingButton.onClick.AddListener(GameManager.instance.LoadDeckBuildingScene);
        quitButton.onClick.AddListener(QuitGame);
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
