using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{

    [SerializeField] private GameObject PanelName;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

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
        if (PanelName != null)
        {
            PanelName.SetActive(true);
        }
    }

    public void CloseDeckButton()
    {
        if (PanelName != null)
        {
            PanelName.SetActive(false);
        }
    }

}
