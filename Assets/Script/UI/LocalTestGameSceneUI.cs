using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class LocalController : MonoBehaviour, IPointerClickHandler
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

    void IPointerClickHandler.OnPointerClick(UnityEngine.EventSystems.PointerEventData eventData)
    {
        if (whiteController.enabled)
        {
            spriteRenderer.sprite = blackButton;

            whiteController.enabled = false;
            blackController.enabled = true;

            whiteController.TurnEnd();
            blackController.OpponentTurnEnd();

            blackController.TurnStart();
            whiteController.OpponentTurnStart();

            blackController.Draw();
            whiteController.OpponentDraw();
        }
        else
        {
            spriteRenderer.sprite = whiteButton;

            blackController.enabled = false;
            whiteController.enabled = true;

            blackController.TurnEnd();
            whiteController.OpponentTurnEnd();

            whiteController.TurnStart();
            blackController.OpponentTurnStart();

            whiteController.Draw();
            blackController.OpponentDraw();
        }
    }
}
