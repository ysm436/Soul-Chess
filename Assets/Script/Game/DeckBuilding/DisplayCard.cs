using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayCard : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int cardindex;
    public int quantity;
    private string _CardName;
    public string CardName
    {
        get { return _CardName; }
        set
        {
            _CardName = value;
            CardNameTMP.text = value;
        }
    }
    private int _Cost;
    public int Cost
    {
        get { return _Cost; }
        set
        {
            _Cost = value;
            CardCostTMP.text = value.ToString();
        }
    }
    private int _HP;
    public int HP
    {
        get { return _HP; }
        set
        {
            _HP = value;
            CardHPTMP.text = value.ToString();
        }
    }
    private int _AD;
    public int AD
    {
        get { return _AD; }
        set
        {
            _AD = value;
            CardADTMP.text = value.ToString();
        }
    }
    public Card.Type CardType;
    public string Reigon;
    public string ChessPiece;
    public string Rarity;
    public string Description
    {
        set { CardDescriptionTMP.text = value; }
    }

    public TextMeshProUGUI CardNameTMP;
    public TextMeshProUGUI CardCostTMP;
    public TextMeshProUGUI CardDescriptionTMP;
    public GameObject SoulElements;
    public TextMeshProUGUI CardHPTMP;
    public TextMeshProUGUI CardADTMP;


    private Transform canvas;
    private CanvasGroup canvasGroup;
    private Vector3 Originposition;

    private Transform previousParent;
    private int previousDisplayIndex;
    private GameObject newDeckPanel;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start()
    {
        switch (CardType)
        {
            case Card.Type.Spell:
                SoulElements.SetActive(false);
                break;
            case Card.Type.Soul:
                break;
            default:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        newDeckPanel = GetComponentInParent<DeckBuildingSceneUI>().newDeckPanel;
        Originposition = transform.position;

        if (newDeckPanel.activeSelf) // 새로운 덱을 만들 때에만 드래그 가능
        {
            previousParent = transform.parent;
            previousDisplayIndex = transform.GetSiblingIndex();

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
                transform.SetSiblingIndex(previousDisplayIndex);
            }
        }
        else
        {
            transform.position = Originposition;
        }

        canvasGroup.blocksRaycasts = true;
    }

}
