using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using UnityEngine;
using UnityEngine.EventSystems;

public class CheckCardClicked : MonoBehaviour, IEndDragHandler
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
            //tutorialManager.CardClicked();
            isCardClicked = true;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (GameBoard.instance.isCardUsed(transform.position))
        {
            tutorialManager.CardClicked();
        }
    }
}
