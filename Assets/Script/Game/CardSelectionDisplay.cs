using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionDisplay : MonoBehaviour
{
    [HideInInspector] public int selectionNumber;                       //0부터 시작
    [HideInInspector] public List<GameObject> selectionObjectList;

    public Action<int> OnSelected;                                      //int 매개변수: 선택된 것의 selectionNumber

    private readonly Vector2 firstPostion = new Vector2(-1.6f, 0f);
    private readonly float gapBetweenSelection = 3.45f;

    private void OnMouseUp()
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
        for (int i = 0;  i < selectionObjectList.Count; i++)
        {
            if (i == selectionNumber) continue;

            Destroy(selectionObjectList[i]);
        }
    }
}
