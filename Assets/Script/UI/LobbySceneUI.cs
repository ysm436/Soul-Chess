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
    private List<List<int>> decklist;
    private List<string> decknamelist;
    
    public void ExitButton()
    {
        SceneManager.LoadScene("MatchingScene");
    }

    public void StartButton()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void OpenDeckButton()
    {
        decklist = DeckData.instance.DeckList;
        decknamelist = DeckData.instance.DeckNameList;

        if (DeckSelectPanel != null)
        {
            DeckSelectPanel.SetActive(true);

            if (DeckData.instance)
            {
                for (int i = 0; i < decklist.Count; i++)
                {
                    if(decklist[i] != null)
                    {
                        GameObject deckselectbutton = Instantiate(DeckSelectButton, DeckDisplay);
                        DeckSelectButton buttoninfo = deckselectbutton.GetComponent<DeckSelectButton>();
                        buttoninfo.deckname.text = decknamelist[i];
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

        SelectedDeckInfo.text = "Selected Deck Name : " + decknamelist[SelectedDeckIndex];
    }

}
