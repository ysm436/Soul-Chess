using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonHighlight : MonoBehaviour
{
    private Button button;
    private float defaultSizeX;
    private float defaultSizeY;
    private RectTransform rectTransform;

    private void Start()
    {
        button = GetComponent<Button>();
        rectTransform = GetComponent<RectTransform>();
        defaultSizeX = GetComponent<RectTransform>().localScale.x;
        defaultSizeY = GetComponent<RectTransform>().localScale.y;

        AddEvent();
    }

    private void AddEvent()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { StartCoroutine(Highlight(true)); });
        eventTrigger.triggers.Add(entry_PointerEnter);

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { StartCoroutine(Highlight(false)); });
        eventTrigger.triggers.Add(entry_PointerExit);
    }

    bool isHovering = false;
    float percent = 0;
    private IEnumerator Highlight(bool on)
    {
        isHovering = on;
        if (on)
        {
            for (float i = percent; i <= 1f; i += Time.deltaTime * 4)
            {
                percent = i;
                if (isHovering)
                {
                    rectTransform.localScale = new Vector3(defaultSizeX * (1 + i * 0.12f), defaultSizeY * (1 + i* 0.12f), 1f);
                    yield return null;
                }
            }
        }
        else
        {
            for (float i = percent; i >= 0; i -= Time.deltaTime * 4)
            {
                percent = i;
                rectTransform.localScale = new Vector3(defaultSizeX * (1 + i * 0.12f), defaultSizeY * (1 + i * 0.12f), 1f);
                yield return null;
            }
        }
    }
}
