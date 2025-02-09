using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditSceneUI : MonoBehaviour
{
    [SerializeField] private Button backButton;
    void Start()
    {
        backButton.onClick.AddListener(GameManager.instance.LoadMainScene);
    }

}
