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
    [SerializeField] private Button saveButton;
    [SerializeField] public Button deckloadButton;
    [SerializeField] public GameObject deckListPanel;
    [SerializeField] public GameObject newDeckPanel;
    public DeckManager deckinfo;

    private void Awake()
    {
        enableDeckListPanel();
        quitButton.onClick.AddListener(LoadMainScene);
        newDeckButton.onClick.AddListener(enableNewDeckPanel);
        cancelButton.onClick.AddListener(enableDeckListPanel);
        saveButton.onClick.AddListener(saveDeck);

        deckinfo = newDeckPanel.GetComponent<DeckManager>();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void enableDeckListPanel()
    {
        deckListPanel.SetActive(true);
        newDeckPanel.SetActive(false);
    }

    private void enableNewDeckPanel()
    {
        deckinfo.deck_index = deckinfo.DeckList.Count;

        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }

    private void saveDeck()
    {
        deckinfo.DeckSave(deckinfo.deck_index);
        deckinfo.DeckReset();

        deckListPanel.SetActive(true);
        newDeckPanel.SetActive(false);
    }

    public void LoadDeck(int deck_index)
    {
        deckinfo.DeckLoad(deck_index);

        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }
}