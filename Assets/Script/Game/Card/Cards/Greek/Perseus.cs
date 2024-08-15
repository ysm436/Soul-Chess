using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Perseus : SoulCard
{
    protected override int CardID => Card.cardIdDict["페르세우스"];

    [SerializeField] private List<GameObject> selectionPrefabList;        // �޵λ��� �Ӹ�, �䰡����, �ϵ����� ���� ������ �־�� ��

    private List<GameObject> selectionInstanceList;

    // � ȿ���� ���õǾ����� ����, ���� ��ȥ ���� �� �׿� ���� �̺�Ʈ ���ŵ� �� ���
    // �޵λ��� �Ӹ�: 0, �䰡����: 1, �ϵ����� ����: 2
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

        if (selectionNumber == 0)           // �޵λ��� �Ӹ� ȿ��
        {
            //InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selectionNumber == 1)      // �䰡���� ȿ��
        {
            
        }
        else                                // �ϵ����� ���� ȿ��
        {

        }
    }

    private void StunChessPiece(ChessPiece chessPiece)
    {
        // ���� �ʿ�
    }

    public override void AddEffect()
    {

    }

    public override void RemoveEffect()
    {

    }
}
