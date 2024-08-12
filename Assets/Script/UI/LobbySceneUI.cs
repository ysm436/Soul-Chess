using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{
    public int SelectedDeckIndex = 0;

    [SerializeField] private GameObject DeckSelectPanel;
    [SerializeField] private GameObject DeckSelectButton;
    [SerializeField] private Transform DeckDisplay;
    [SerializeField] private Transform TrashCan;
    [SerializeField] private TextMeshProUGUI SelectedDeckInfo;

    public void ExitButton()
    {
        GameManager.instance.LoadMatchingScene();
    }

    public void StartButton()
    {
        GameManager.instance.LoadGameScene();
    }

    public void OpenDeckButton()
    {
        if (DeckSelectPanel != null)
        {
            DeckSelectPanel.SetActive(true);

            if (GameManager.instance.deckList.Count > 0)
            {
                for (int i = 0; i < GameManager.instance.deckList.Count; i++)
                {
                    if(GameManager.instance.deckList[i] != null)
                    {
                        GameObject deckselectbutton = Instantiate(DeckSelectButton, DeckDisplay);
                        DeckSelectButton buttoninfo = deckselectbutton.GetComponent<DeckSelectButton>();
                        buttoninfo.deckname.text = GameManager.instance.deckList[i].deckname;
                        buttoninfo.deck_index = i;
                    }
                }
            }
        }
    }

    public void CloseDeckButton()
    {
        if (DeckSelectPanel != null)
        {
            DeckSelectPanel.SetActive(false);
        }
        
        for(int i = DeckDisplay.childCount; i > 0; i--)
        {
            Transform deck = DeckDisplay.GetChild(i - 1);
            Destroy(deck.gameObject);
        }

        SelectedDeckInfo.text = "Selected Deck Name : " + GameManager.instance.deckList[SelectedDeckIndex].deckname;
    }

}
