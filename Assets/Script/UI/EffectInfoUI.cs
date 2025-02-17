using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class EffectInfoUI : MonoBehaviour
{
    [SerializeField] private GameObject effectInfoPanel;

    void Start()
    {
        AddEvent();
    }

    public void AddEvent()
    {
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        EventTrigger.Entry entry_PointerEnter = new EventTrigger.Entry();
        entry_PointerEnter.eventID = EventTriggerType.PointerEnter;
        entry_PointerEnter.callback.AddListener((data) => { ShowEffectInfoPanel(); });
        eventTrigger.triggers.Add(entry_PointerEnter);

        EventTrigger.Entry entry_PointerExit = new EventTrigger.Entry();
        entry_PointerExit.eventID = EventTriggerType.PointerExit;
        entry_PointerExit.callback.AddListener((data) => { HideEffectInfoPanel(); });
        eventTrigger.triggers.Add(entry_PointerExit);
    }

    public void ShowEffectInfoPanel()
    {
        effectInfoPanel.SetActive(true);
    }

    public void HideEffectInfoPanel()
    {
        effectInfoPanel.SetActive(false);
    }
}
