using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class deckObj : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public int deckIndex;
    public TextMeshProUGUI deckNameText;
    public Button loadDeckButton;

    private Transform canvas;
    private DeckBuildingManager dbm;
    private GameObject draggedDeckObj;
    private Vector2 initialMousePosition;
    private ScrollRect deckObjScroll;
    private bool isDragging;

    private void Awake()
    {
        loadDeckButton.onClick.AddListener(LoadDeck);
    }

    public void LoadDeck()
    {
        if (!isDragging)
        {
            GetComponentInParent<DeckBuildingSceneUI>().LoadDeckButtonFunction(deckIndex);
        }

    }

/*     public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button == PointerEventData.InputButton.Right)
        {
            GameManager.instance.deckList[deckIndex].index = -1;
            GameManager.instance.SaveDeckData();

            Destroy(gameObject);
        }
    } */

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        canvas = FindObjectOfType<Canvas>().transform;
        dbm = GetComponentInParent<DeckBuildingManager>();

        deckObjScroll = GetComponentInParent<ScrollRect>();
        if (deckObjScroll != null)
        {
            deckObjScroll.enabled = false;
        }

        draggedDeckObj = Instantiate(gameObject, canvas.transform);
        
        initialMousePosition = eventData.position;
        draggedDeckObj.transform.position = initialMousePosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggedDeckObj != null)
            draggedDeckObj.transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (deckObjScroll != null)
        {
            deckObjScroll.enabled = true;
        }

        bool removeSignal = RectTransformUtility.RectangleContainsScreenPoint(dbm.cardDisplayArea.GetComponent<RectTransform>(), eventData.position);

        Destroy(draggedDeckObj);
        isDragging = false;
        if (removeSignal)
        {
            GameManager.instance.deckList[deckIndex].index = -1;
            GameManager.instance.SaveDeckData();

            Destroy(gameObject);
        }

    }
}
