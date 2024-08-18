using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;

public class DeckBuildingSceneUI : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button newDeckButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button saveButton;
    [SerializeField] public Button deckloadButton;
    [SerializeField] public GameObject deckListPanel;
    [SerializeField] public GameObject newDeckPanel;
    public DeckManager deckmanager;

    private void Awake()
    {
        CancelButtonFunction();
        quitButton.onClick.AddListener(GameManager.instance.LoadMainScene);
        newDeckButton.onClick.AddListener(NewDeckButtonFunction);
        cancelButton.onClick.AddListener(CancelButtonFunction);
        saveButton.onClick.AddListener(saveButtonFunction);

        deckmanager = newDeckPanel.GetComponent<DeckManager>();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void CancelButtonFunction()
    {
        deckmanager.DeckCancel();
        deckmanager.CardSlotReset();
        GetComponent<DeckBuildingManager>().ReloadDisplayCard();

        deckListPanel.SetActive(true);
        newDeckPanel.SetActive(false);
    }

    private void NewDeckButtonFunction()
    {
        deckmanager.newDeckSignal = true;

        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }

    private void saveButtonFunction()
    {
        if (deckavailable())
        {
            deckmanager.DeckSave(deckmanager.loaded_deck_index);
            deckmanager.CardSlotReset();
            GetComponent<DeckBuildingManager>().ReloadDisplayCard();
            GameManager.instance.SaveDeckData();

            deckListPanel.SetActive(true);
            newDeckPanel.SetActive(false);
        }
        else
        {
            deckmanager.CautionPanel.SetActive(true);
        }
    }

    public void LoadDeck(int deck_index)
    {
        deckmanager.DeckLoad(deck_index);

        deckListPanel.SetActive(false);
        newDeckPanel.SetActive(true);
    }

    private bool deckavailable()
    {
        bool available = true;

        if (deckmanager.local_chesspieces[0] < 8) available = false;
        else if (deckmanager.local_chesspieces[1] < 2) available = false;
        else if (deckmanager.local_chesspieces[2] < 2) available = false;
        else if (deckmanager.local_chesspieces[3] < 2) available = false;
        else if (deckmanager.local_chesspieces[4] < 1) available = false;
        else if (deckmanager.local_chesspieces[5] < 1) available = false;

        if (available == false)
        {
            deckmanager.CautionText.text = "기물 당 하나의 카드는 존재해야 합니다.";
        }

        return available;
    }
}