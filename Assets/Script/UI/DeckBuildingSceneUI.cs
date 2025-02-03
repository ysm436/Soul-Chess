using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Linq;
using System.Data.Common;

public class DeckBuildingSceneUI : MonoBehaviour
{
    [SerializeField] private Button quitButton;
    [SerializeField] private Button newDeckButton;
    [SerializeField] private Button cancelButton;
    [SerializeField] private Button saveButton;
    [SerializeField] public Button deckLoadButton;
    [SerializeField] public GameObject deckListPanel;
    [SerializeField] public GameObject createDeckPanel;
    public DeckManager deckManager;
    public bool debug = false;

    private void Awake()
    {
        CancelButtonFunction();
        quitButton.onClick.AddListener(GameManager.instance.LoadMainScene);
        newDeckButton.onClick.AddListener(NewDeckButtonFunction);
        cancelButton.onClick.AddListener(CancelButtonFunction);
        saveButton.onClick.AddListener(SaveButtonFunction);

        deckManager = createDeckPanel.GetComponent<DeckManager>();
    }

    private void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }

    private void CancelButtonFunction()
    {
        deckManager.DeckCancel();
        deckManager.CardSlotReset();
        GetComponent<DeckBuildingManager>().ReloadDisplayCard();

        deckListPanel.SetActive(true);
        createDeckPanel.SetActive(false);
    }

    private void NewDeckButtonFunction()
    {
        deckManager.newDeckSignal = true;

        deckListPanel.SetActive(false);
        createDeckPanel.SetActive(true);
    }

    private void SaveButtonFunction()
    {
        if (deckAvailable())
        {
            deckManager.DeckSave(deckManager.loadedDeckIndex);
            deckManager.CardSlotReset();
            GetComponent<DeckBuildingManager>().ReloadDisplayCard();
            GameManager.instance.SaveDeckData();

            deckListPanel.SetActive(true);
            createDeckPanel.SetActive(false);
        }
        else
        {
            deckManager.cautionPanel.SetActive(true);
        }
    }

    public void LoadDeckButtonFunction(int deckIndex)
    {
        deckManager.DeckLoad(deckIndex);

        deckListPanel.SetActive(false);
        createDeckPanel.SetActive(true);
    }

    private bool deckAvailable()
    {
        bool available = true;

        if (debug)
            return true;

        if (deckManager.loadedDeckCardCount < 24)
        {            
            available = false;
            deckManager.cautionText.text = "덱에 24장의 카드가 들어가야 합니다.";
        }

        return available;
    }

    public void debugToggle(bool debugtoggle)
    {
        if (debugtoggle)
        {
            deckManager.debugButton = true;
            debug = true;
        }
        else
        {
            deckManager.debugButton = false;
            debug = false;
        }
    }
}