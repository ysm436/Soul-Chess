using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class DisplayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int cardindex;
    public int quantity;
    public string CardName;
    public int Cost;
    public string CardType;
    public string Reigon;
    public string ChessPiece;
    public string Rarity;
    
    private Transform canvas;
    private CanvasGroup canvasGroup;
    private Vector3 Originposition;

    private Transform previousParent;
    private GameObject newDeckPanel;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        newDeckPanel = this.GetComponentInParent<DeckBuildingSceneUI>().newDeckPanel;
        Originposition = transform.position;

        if (newDeckPanel.activeSelf) // 새로운 덱을 만들 때
        {
            previousParent = transform.parent;

            transform.SetParent(canvas);
            transform.SetAsLastSibling();

            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (newDeckPanel.activeSelf)
        {
            if (transform.parent == canvas)
            {
                transform.position = Originposition;
                transform.SetParent(previousParent);
            }
        }
        else
        {
            transform.position = Originposition;
        }

        canvasGroup.blocksRaycasts = true;
    }

}
