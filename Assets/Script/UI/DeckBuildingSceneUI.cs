using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class DeckBuildingSceneUI : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button newDeckButton;
    [SerializeField] private Button cancelButton;
    //덱 구성 후 저장 버튼 (추후 기능 추가 대비)
    //[SerializeField] private Button saveButton;

    [SerializeField] private GameObject deckListPanel;
    [SerializeField] private GameObject newDeckPanel;

    private void Awake()
    {
        enableDeckListPanel();
        quitButton.onClick.AddListener(GameManager.instance.LoadMainScene);
        newDeckButton.onClick.AddListener(enableNewDeckPanel);
        cancelButton.onClick.AddListener(enableDeckListPanel);
    }

    private void enableDeckListPanel()
    {
        deckListPanel.SetActive(true);
        newDeckPanel.SetActive(false);
    }

    private void enableNewDeckPanel()
    {
        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }
}