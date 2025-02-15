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
    [SerializeField] public Button cancelQuitPanelButton;
    [SerializeField] public Button quitInPanelButton;
    [SerializeField] private GameObject deckListPanel;
    [SerializeField] private GameObject createDeckPanel;
    [SerializeField] private GameObject quitCautionPanel;
    public DeckManager deckManager;
    public bool debug = false;

    private void Awake()
    {
        deckManager = createDeckPanel.GetComponent<DeckManager>();

        CancelButtonFunction();
        quitButton.onClick.AddListener(LoadMainSceneFunc);
        newDeckButton.onClick.AddListener(NewDeckButtonFunction);
        cancelButton.onClick.AddListener(CancelButtonFunction);
        saveButton.onClick.AddListener(SaveButtonFunction);
        cancelQuitPanelButton.onClick.AddListener(CancelQuitPanel);
        quitInPanelButton.onClick.AddListener(QuitInPanelButton);
    }

    private void LoadMainSceneFunc()
    {
        if (createDeckPanel.activeSelf)
        {
            quitCautionPanel.SetActive(true);
        }
        else
        {
            GameManager.instance.LoadMainScene();
        }
    }

    private void CancelQuitPanel()
    {
        quitCautionPanel.SetActive(false);
    }

    private void QuitInPanelButton()
    {
        GameManager.instance.LoadMainScene();
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