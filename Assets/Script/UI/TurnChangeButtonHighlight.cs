using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class TurnChangeButtonHighlight : MonoBehaviour
{
    [HideInInspector] public SpriteRenderer spriteRenderer;
    PlayerController player;
    private bool isVisible = false;

    WaitForSeconds Wait = new WaitForSeconds(0.5f);

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.enabled = isVisible = false;
        player = GameBoard.instance.myController;
        StartCoroutine(EnableHighlight());
    }

    public IEnumerator EnableHighlight()
    {
        while(true)
        {
            if (!isVisible && player.TurnEndPossible && GameBoard.instance.isActivePlayer) spriteRenderer.enabled = isVisible = true;
            else spriteRenderer.enabled = isVisible = false;

            yield return Wait;
        }
    }
}
