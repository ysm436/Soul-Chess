using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    public Image blocker; //UI 뒤 Raycast 안되게
    public Button giveUpButton;
    bool isEnabledSetting = false;

    void Awake()
    {
        giveUpButton.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "GameScene" ||
            SceneManager.GetActiveScene().name == "GameScene_KDH" ||
            SceneManager.GetActiveScene().name == "GameScene_PJH" ||
            SceneManager.GetActiveScene().name == "GameScene_WKH" ||
            SceneManager.GetActiveScene().name == "GameScene_YSM" ||
            SceneManager.GetActiveScene().name == "LocalTestGameScene")
        {
            giveUpButton.gameObject.SetActive(true);
        }

        QuitSettingUI();
    }

    public void ToggleSettingUI()
    {
        if (isEnabledSetting) isEnabledSetting = false;
        else isEnabledSetting = true;

        blocker.gameObject.SetActive(isEnabledSetting);
        gameObject.SetActive(isEnabledSetting);
    }

    public void QuitSettingUI()
    {
        isEnabledSetting = false;
        blocker.gameObject.SetActive(isEnabledSetting);
        gameObject.SetActive(isEnabledSetting);
        return;
    }

    public void GiveUp()
    {
        //항복 기능
        Debug.Log("항복");
    }
}
