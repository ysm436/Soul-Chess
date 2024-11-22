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
    public bool debug = false;

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
        int SoulCardQuantity = deckmanager.local_chesspieces.Sum();

        if (debug)
            return true;

        if (deckmanager.local_card_count < 30)
        {            
            available = false;
            deckmanager.CautionText.text = "덱에 30장의 카드가 들어가야 합니다.";
        }
        else if (SoulCardQuantity < 16)
        {
            available = false;
            deckmanager.CautionText.text = "덱에 적어도 16개의 기물카드는 들어가야 합니다.";
        }
        /* if (deckmanager.local_chesspieces[0] < 8) available = false;
        else if (deckmanager.local_chesspieces[1] < 2) available = false;
        else if (deckmanager.local_chesspieces[2] < 2) available = false;
        else if (deckmanager.local_chesspieces[3] < 2) available = false;
        else if (deckmanager.local_chesspieces[4] < 1) available = false;
        else if (deckmanager.local_chesspieces[5] < 1) available = false; */

        return available;
    }

    public void debugToggle(bool debugtoggle)
    {
        if (debugtoggle)
        {
            deckmanager.debug_button = true;
            debug = true;
        }
        else
        {
            deckmanager.debug_button = false;
            debug = false;
        }
    }
}