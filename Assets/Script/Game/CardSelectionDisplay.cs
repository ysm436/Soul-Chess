using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardSelectionDisplay : MonoBehaviour, IPointerClickHandler
{
    [HideInInspector] public int selectionNumber;                       //0���� ����
    [HideInInspector] public List<GameObject> selectionObjectList;

    public Action<int> OnSelected;                                      //int �Ű�����: ���õ� ���� selectionNumber

    private readonly Vector2 firstPostion = new Vector2(-1.6f, 0f);
    private readonly float gapBetweenSelection = 3.45f;

    public void OnPointerClick(PointerEventData eventData)
    {
        OnSelected?.Invoke(selectionNumber);
        DestroyOtherSelectionObject();
    }

    public void Initialize()
    {
        Vector2 myPosition = firstPostion + Vector2.right * gapBetweenSelection * selectionNumber;
        transform.position = myPosition;
    }

    private void DestroyOtherSelectionObject()
    {
        for (int i = 0; i < selectionObjectList.Count; i++)
        {
            if (i == selectionNumber) continue;

            Destroy(selectionObjectList[i]);
        }
    }
}
