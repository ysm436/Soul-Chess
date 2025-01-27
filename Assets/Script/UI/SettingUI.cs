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
    public Slider bgmVolume;
    private SoundManager soundManager;

    void Awake()
    {
        soundManager = GameObject.Find("SoundManager").GetComponent<SoundManager>();

        giveUpButton.gameObject.SetActive(false);
        if (SceneManager.GetActiveScene().name == "GameScene" ||
            SceneManager.GetActiveScene().name == "PvEGameScene" ||
            SceneManager.GetActiveScene().name == "GameScene_KDH" ||
            SceneManager.GetActiveScene().name == "GameScene_PJH" ||
            SceneManager.GetActiveScene().name == "GameScene_WKH" ||
            SceneManager.GetActiveScene().name == "GameScene_YSM" ||
            SceneManager.GetActiveScene().name == "LocalTestGameScene" ||
            SceneManager.GetActiveScene().name == "TutorialScene")
        {
            giveUpButton.gameObject.SetActive(true);
        }

        QuitSettingUI();
        bgmVolume.value = soundManager.bgmVolume;
    }

    void Update()
    {
        UpdateBgmVolume();
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

    public void UpdateBgmVolume()
    {
        soundManager.bgmVolume = bgmVolume.value;
        soundManager.bgmPlayer.volume = bgmVolume.value;
    }
}
