using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public float bgmVolume { get; set; }

    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private int num = 1;

    void Awake()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmVolume = 1f;

        bgmDictionary.Add("MainScene", Resources.Load<AudioClip>("BGM/Teller of the Tales"));
        bgmDictionary.Add("LobbyScene", Resources.Load<AudioClip>("BGM/Midnight Tale"));
        bgmDictionary.Add("DeckBuildingScene", Resources.Load<AudioClip>("BGM/Folk Round"));
        bgmDictionary.Add("GameScene1", Resources.Load<AudioClip>("BGM/Red Castle"));
        bgmDictionary.Add("GameScene2", Resources.Load<AudioClip>("BGM/Into The Forest"));
        bgmDictionary.Add("GameScene3", Resources.Load<AudioClip>("BGM/Master of the Feast"));
    }

    public void PlayBgm(string name)
    {
        if(name.Equals("GameScene"))
        {
            StartCoroutine("PlayBgmList");
        }
        else
        {
            bgmPlayer.loop = true;
            bgmPlayer.volume = bgmVolume;
            bgmPlayer.clip = bgmDictionary[name];
            bgmPlayer.Play();
        }
    }

    IEnumerator PlayBgmList()
    {
        bgmPlayer.loop = false;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmDictionary["GameScene3"];
        bgmPlayer.Play();
        while(true)
        {
            yield return new WaitForSeconds(1.0f);
            if(!bgmPlayer.isPlaying)
            {
                switch(num % 3)
                {
                    case 1:
                        bgmPlayer.clip = bgmDictionary["GameScene1"];
                        bgmPlayer.Play();
                        num++;
                        break;
                    case 2:
                        bgmPlayer.clip = bgmDictionary["GameScene2"];
                        bgmPlayer.Play();
                        num++;
                        break;
                    case 3:
                        bgmPlayer.clip = bgmDictionary["GameScene3"];
                        bgmPlayer.Play();
                        num++;
                        break;
                }
            }
        }
    }

    public void StopBgm()
    {
        bgmPlayer.clip = null;
        bgmPlayer.Stop();
    }
}
