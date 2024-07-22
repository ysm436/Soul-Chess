using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbySceneUI : MonoBehaviour
{

    [SerializeField] private GameObject PanelName;

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
