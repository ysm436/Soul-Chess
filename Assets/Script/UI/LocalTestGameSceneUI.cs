using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LocalController : MonoBehaviour
{
    SpriteRenderer spriteRenderer;
    public Sprite whiteButton;
    public Sprite blackButton;
    public PlayerController whiteController;
    public PlayerController blackController;

    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = whiteButton;
    }
    private void Start()
    {
        whiteController.enabled = true;
        blackController.enabled = false;
    }

    private void OnMouseUp()
    {
        if (whiteController.enabled)
        {
            spriteRenderer.sprite = blackButton;

            whiteController.enabled = false;
            blackController.enabled = true;

            whiteController.TurnEnd();
            blackController.OnOpponentTurnEnd?.Invoke();
        }
        else
        {
            spriteRenderer.sprite = whiteButton;

            blackController.enabled = false;
            whiteController.enabled = true;

            blackController.TurnEnd();
            whiteController.OnOpponentTurnEnd?.Invoke();
        }
    }

}
