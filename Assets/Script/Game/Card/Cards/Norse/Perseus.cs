using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Perseus : SoulCard
{
    [SerializeField] private List<GameObject> selectionPrefabList;        // 메두사의 머리, 페가수스, 하데스의 투구 순으로 있어야 함

    private List<GameObject> selectionInstanceList;

    // 어떤 효과가 선택되었는지 저장, 이후 영혼 제거 및 그에 따른 이벤트 제거될 때 사용
    // 메두사의 머리: 0, 페가수스: 1, 하데스의 투구: 2
    public int selectionNumber { get; private set; }    
    
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InstantiateSelectionCard;
    }

    private void InstantiateSelectionCard(ChessPiece chessPiece)
    {
        if (selectionPrefabList.Count == 0)
        {
            Debug.Log(cardName + "'s Selection Prefab List is empty");
        }

        selectionInstanceList = new();
        foreach (GameObject selection in selectionPrefabList)
        {
            GameObject selectionInstance = Instantiate(selection);
            CardSelectionDisplay cardSelectionDisplay = selectionInstance.AddComponent<CardSelectionDisplay>();
            cardSelectionDisplay.selectionNumber = selectionInstanceList.IndexOf(selection);
            cardSelectionDisplay.OnSelected += GetAbility;

            cardSelectionDisplay.Initialize();

            selectionInstanceList.Add(selectionInstance);
        }
        foreach (GameObject selectionInstance in selectionInstanceList)
        {
            selectionInstance.GetComponent<CardSelectionDisplay>().selectionObjectList = selectionInstanceList;
        }
    }

    private void GetAbility(int selectionNumber)
    {
        this.selectionNumber = selectionNumber;

        if (selectionNumber == 0)           // 메두사의 머리 효과
        {
            //InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selectionNumber == 1)      // 페가수스 효과
        {
            
        }
        else                                // 하데스의 투구 효과
        {

        }
    }

    private void StunChessPiece(ChessPiece chessPiece)
    {
        // 구현 필요
    } 
}
