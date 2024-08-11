using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckData : MonoBehaviour
{
    public static DeckData instance;
    public List<List<int>> DeckList;
    public List<string> DeckNameList;

    private void Awake()
    {
        var obj = FindObjectsOfType<DeckData>();
        if (obj.Length == 1)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
}