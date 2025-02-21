using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class SettingUI : MonoBehaviour
{
    [SerializeField] private Image blocker; //UI 뒤 Raycast 안되게
    [SerializeField] private Button giveUpButton;
    private bool isEnabledSetting = false;
    [SerializeField] private Slider bgmVolume;
    [SerializeField] private Slider sfxVolume;
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
        sfxVolume.value = soundManager.sfxVolume;
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

        if (isEnabledSetting && SceneManager.GetActiveScene().name == "PvEGameScene")
            Time.timeScale = 0;
        else if (!isEnabledSetting && SceneManager.GetActiveScene().name == "PvEGameScene")
            Time.timeScale = 1;
    }

    public void QuitSettingUI()
    {
        Time.timeScale = 1;
        isEnabledSetting = false;
        blocker.gameObject.SetActive(isEnabledSetting);
        gameObject.SetActive(isEnabledSetting);
        return;
    }

    public void GiveUp()
    {
        gameObject.SetActive(false);
        Debug.Log("항복");
    }

    public void UpdateBgmVolume()
    {
        soundManager.bgmVolume = bgmVolume.value;
        soundManager.bgmPlayer.volume = bgmVolume.value * 0.6f;
        soundManager.sfxVolume = sfxVolume.value;
    }
}
