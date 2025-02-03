using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using TMPro;

public class DisplayInfo : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int cardDisplayIndex;
    public int cardIndex;
    private int _quantity;
    public int Quantity
    {
        get { return _quantity; }
        set
        {
            _quantity = value;
            cardQunatityTMP.text = "x" + value.ToString();

            if (value == 0)
            {
                DisplayUnAvailable();
            }
            else
            {
                DisplayAvailable();
            }
        }
    }
    private string _cardName;
    public string CardName
    {
        get { return _cardName; }
        set
        {
            _cardName = value;
            cardNameTMP.text = value;
        }
    }
    private int _cost;
    public int Cost
    {
        get { return _cost; }
        set
        {
            _cost = value;
            cardCostTMP.text = value.ToString();
        }
    }
    private int _HP;
    public int HP
    {
        get { return _HP; }
        set
        {
            _HP = value;
            cardHPTMP.text = value.ToString();
        }
    }
    private int _AD;
    public int AD
    {
        get { return _AD; }
        set
        {
            _AD = value;
            cardADTMP.text = value.ToString();
        }
    }
    public Card.Type cardType;
    public Card.Reigon reigon;
    public ChessPiece.PieceType chessPieceType;

    private Card.Rarity _rarity;
    public Card.Rarity Rarity
    {
        get { return _rarity; }
        set
        {
            if (value == Card.Rarity.Common)
                cardFrameImage.sprite = cardFrameList[0];
            else if (value == Card.Rarity.Legendary)
                cardFrameImage.sprite = cardFrameList[1];
            else
                cardFrameImage.sprite = cardFrameList[2];

            _rarity = value;
        }
    }
    public string description
    {
        set { cardDescriptionTMP.text = value; }
    }

    [SerializeField] private TextMeshProUGUI cardQunatityTMP;
    [SerializeField] private TextMeshProUGUI cardNameTMP;
    [SerializeField] private TextMeshProUGUI cardCostTMP;
    [SerializeField] private TextMeshProUGUI cardTypeTMP;
    [SerializeField] private TextMeshProUGUI cardDescriptionTMP;
    [SerializeField] private GameObject soulElementPrefab;
    [SerializeField] private TextMeshProUGUI cardHPTMP;
    [SerializeField] private TextMeshProUGUI cardADTMP;

    [SerializeField] private List<Sprite> cardFrameList;
    [SerializeField] private Image cardFrameImage;
    public Image illustrate;
    //public List<Image> chessPieceDisplayList;

    private Transform canvas;
    private CanvasGroup canvasGroup;
    private DeckBuildingManager dbm;
    private GameObject draggedCard;
    private Vector2 initialMousePosition;
    private ScrollRect cardDisplayScroll;

    private void Awake()
    {
        canvas = FindObjectOfType<Canvas>().transform;
        canvasGroup = GetComponent<CanvasGroup>();
        dbm = GetComponentInParent<DeckBuildingManager>();
    }
    private void Start()
    {
        switch (cardType)
        {
            case Card.Type.Spell:
                soulElementPrefab.SetActive(false);
                cardTypeTMP.text = "마법";
                break;
            case Card.Type.Soul:
                break;
            default:
                break;
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        cardDisplayScroll = GetComponentInParent<ScrollRect>();
        if (cardDisplayScroll != null)
        {
            cardDisplayScroll.enabled = false;
        }

        draggedCard = Instantiate(gameObject, canvas.transform);
        
        initialMousePosition = eventData.position;
        draggedCard.transform.position = initialMousePosition;
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedCard != null)
            draggedCard.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (cardDisplayScroll != null)
        {
            cardDisplayScroll.enabled = true;
        }

        List<RaycastResult> results = new List<RaycastResult>();
        bool inDeckSignal = false;

        EventSystem.current.RaycastAll(eventData, results);

        foreach (RaycastResult result in results)
        {
            DeckManager zone = result.gameObject.GetComponent<DeckManager>();
            if (zone != null)
            {
                inDeckSignal = true;
            }
            else
            {
                canvasGroup.blocksRaycasts = true;
                Destroy(draggedCard);
            }
        }
        
        if (inDeckSignal)
        {
            dbm.deckManager.InsertCardInDeck(draggedCard.GetComponent<DisplayInfo>());
        }
    }

    public void DisplayUnAvailable()
    {
        Image[] images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            Color color = image.color;
            color.a = 0.5f;
            image.color = color;
        }

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            text.alpha = 0.5f;
        }

        canvasGroup.blocksRaycasts = false;
    }

    public void DisplayAvailable()
    {   
        Image[] images = GetComponentsInChildren<Image>();
        foreach (var image in images)
        {
            Color color = image.color;
            color.a = 1f;
            image.color = color;
        }

        TextMeshProUGUI[] texts = GetComponentsInChildren<TextMeshProUGUI>();
        foreach (var text in texts)
        {
            text.alpha = 1f;
        }

        canvasGroup.blocksRaycasts = true;
    }
}
