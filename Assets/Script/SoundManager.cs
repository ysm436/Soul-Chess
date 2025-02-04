using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public AudioSource bgmPlayer;
    public AudioSource sfxPlayer;
    public float bgmVolume { get; set; }

    private Dictionary<string, AudioClip> bgmDictionary = new Dictionary<string, AudioClip>();
    private int num = 1;

    private Dictionary<string, AudioClip> sfxDictionary = new Dictionary<string, AudioClip>();

    private readonly Dictionary<string, string> elementSFXDictionary = new Dictionary<string, string>()
    {
        { "수르트", "Fire" },
        { "제우스", "Electricity" },
        { "포세이돈", "Water" },
        { "약탈선", "Water" },
    };

    void Awake()
    {
        bgmPlayer = gameObject.AddComponent<AudioSource>();
        bgmVolume = 0.7f;

        sfxPlayer = gameObject.AddComponent<AudioSource>();

        InitializeBGM();
        InitializeSFX();
    }

    private void InitializeBGM()
    {
        bgmDictionary.Add("MainScene", Resources.Load<AudioClip>("BGM/Teller of the Tales"));
        bgmDictionary.Add("LobbyScene", Resources.Load<AudioClip>("BGM/Midnight Tale"));
        bgmDictionary.Add("DeckBuildingScene", Resources.Load<AudioClip>("BGM/Folk Round"));
        bgmDictionary.Add("GameScene1", Resources.Load<AudioClip>("BGM/Red Castle"));
        bgmDictionary.Add("GameScene2", Resources.Load<AudioClip>("BGM/Into The Forest"));
        bgmDictionary.Add("GameScene3", Resources.Load<AudioClip>("BGM/Master of the Feast"));
        bgmDictionary.Add("GameScene4", Resources.Load<AudioClip>("BGM/NewGameBGM"));
    }

    private void InitializeSFX()
    {
        sfxDictionary.Add("Attack", Resources.Load<AudioClip>("SFX/Attack"));
        sfxDictionary.Add("Destroy", Resources.Load<AudioClip>("SFX/Destroy"));
        sfxDictionary.Add("Draw", Resources.Load<AudioClip>("SFX/Draw"));
        sfxDictionary.Add("Move", Resources.Load<AudioClip>("SFX/Move"));
        sfxDictionary.Add("SetSoul", Resources.Load<AudioClip>("SFX/SetSoul"));
        sfxDictionary.Add("Turn", Resources.Load<AudioClip>("SFX/Turn"));
        sfxDictionary.Add("UseCard", Resources.Load<AudioClip>("SFX/UseCard"));
        sfxDictionary.Add("Fire", Resources.Load<AudioClip>("SFX/Fire"));
        sfxDictionary.Add("Electricity", Resources.Load<AudioClip>("SFX/Electricity"));
        sfxDictionary.Add("Water", Resources.Load<AudioClip>("SFX/Water"));

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
        //bgmPlayer.loop = false;
        bgmPlayer.loop = true;
        bgmPlayer.volume = bgmVolume;
        bgmPlayer.clip = bgmDictionary["GameScene4"];
        bgmPlayer.Play();

        yield return new WaitForFixedUpdate();
        /*while(true)
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
        }*/
    }

    public void StopBgm()
    {
        bgmPlayer.clip = null;
        bgmPlayer.Stop();
    }

    public void PlaySFX(string name, int id = -1, float volume = 1.0f, float pitch = 1.0f, float startTime = 0f)
    {
        if (!sfxDictionary.ContainsKey(name))
        {
            Debug.Log("ERROR: SoundManager doesn't have SFX " + name);
            return;
        }

        if (name == "SetSoul" && id >= 0)
        {
            string cardName = Card.cardIdDict.FirstOrDefault(x => x.Value == id).Key;
            if (elementSFXDictionary.ContainsKey(cardName))
            {
                name = elementSFXDictionary[cardName];
            }
        }
        
        sfxPlayer.pitch = pitch;
        sfxPlayer.time = startTime;
        sfxPlayer.PlayOneShot(sfxDictionary[name], volume);
    }
}
