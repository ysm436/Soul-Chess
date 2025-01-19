using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;

public class CheckCardClicked : MonoBehaviour
{
    public TutorialManager tutorialManager;

    private bool isCardClicked;

    private void Start()
    {
        isCardClicked = false;
    }

    private void OnMouseDown()
    {
        if (!isCardClicked)
        {
            tutorialManager.CardClicked();
            isCardClicked = true;
        }
    }
}
