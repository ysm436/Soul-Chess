using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{

    [SerializeField] private GameObject DeckSelectPanel;
    [SerializeField] private GameObject DeckSelectButton;

    [SerializeField] private Transform DeckDisplay;

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
        List<List<int>> decklist = DeckData.instance.DeckList;
        List<string> decknamelist = DeckData.instance.DeckNameList;

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
    }

}
