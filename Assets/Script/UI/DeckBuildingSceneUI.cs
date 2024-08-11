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
    [SerializeField] private Button saveButton;
    [SerializeField] public Button deckloadButton;
    [SerializeField] public GameObject deckListPanel;
    [SerializeField] public GameObject newDeckPanel;
    public DeckManager deckinfo;

    private void Awake()
    {
        CancelButtonFunction();
        quitButton.onClick.AddListener(GameManager.instance.LoadMainScene);
        newDeckButton.onClick.AddListener(NewDeckButtonFunction);
        cancelButton.onClick.AddListener(CancelButtonFunction);
        saveButton.onClick.AddListener(saveButtonFunction);

        deckinfo = newDeckPanel.GetComponent<DeckManager>();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void CancelButtonFunction()
    {
        deckinfo.DeckCancel();
        deckinfo.TempDeckReset();
        GetComponent<DeckBuildingManager>().ReloadDisplayCard();

        deckListPanel.SetActive(true);
        newDeckPanel.SetActive(false);
    }

    private void NewDeckButtonFunction()
    {
        deckinfo.newDeckSignal = true;

        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }

    private void saveButtonFunction()
    {
        deckinfo.DeckSave(deckinfo.loaded_deck_index);
        deckinfo.TempDeckReset();
        GetComponent<DeckBuildingManager>().ReloadDisplayCard();

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