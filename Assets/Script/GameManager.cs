using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    static public GameManager instance;

    public bool isHost;
    public GameBoard gameBoard; //Used only in GameScene

    public List<Card> currentDeck;

    //private Deck deckList;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }


    // Update is called once per frame
    void Update()
    {

    }

    public void LoadMainScene()
    {
        SceneManager.LoadScene("MainScene");
    }
    public void LoadDeckBuildingScene()
    {
        SceneManager.LoadScene("DeckBuildingScene");
    }
    public void LoadMatchingScene()
    {
        SceneManager.LoadScene("MatchingScene");
    }
    public void LoadLobbyScene()
    {
        SceneManager.LoadScene("LobbyScene");
    }
    public void LoadGameScene()
    {
        SceneManager.LoadScene("GameScene");
    }
}
