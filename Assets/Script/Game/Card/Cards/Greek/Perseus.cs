using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Perseus : SoulCard
{
    protected override int CardID => Card.cardIdDict["페르세우스"];

    [SerializeField] private List<GameObject> selectionPrefabList;

    private List<GameObject> selectionInstanceList;

    public int selectionNumber { get; private set; }

    private int targethp;
    private int is_stealth;
    
    protected override void Awake()
    {
        base.Awake();
        OnInfuse += InstantiateSelectionCard;
    }

    public void InstantiateSelectionCard(ChessPiece chessPiece)
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
            selectionInstance.GetComponent<Card>().isInSelection = true;
        }
    }

    private void GetAbility(int selectionNumber)
    {
        this.selectionNumber = selectionNumber;

        if (selectionNumber == 0)
        {
            InfusedPiece.OnStartAttack += GetHPInfoTarget;
            InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selectionNumber == 1)
        {
            InfusedPiece.moveCount += 1;
            InfusedPiece.buff.AddBuffByValue("페가수스", Buff.BuffType.MoveCount, 2, true);
        }
        else
        {
            InfusedPiece.SetKeyword(Keyword.Type.Stealth);
            //버프 관련 내용 머지 후 추가 예정
            InfusedPiece.OnKill += StealthInfusedPiece;
        }
    }

    private void GetHPInfoTarget(ChessPiece chessPiece) //피해를 입었는지 확인하기 위해 hp 정보를 가져옴
    {
        targethp = chessPiece.GetHP;
    }

    private void StunChessPiece(ChessPiece chessPiece)
    {
        if (chessPiece.GetHP < targethp) // 피해를 입었는지 확인
        {
            chessPiece.SetKeyword(Keyword.Type.Stun);
            //버프 관련 내용 머지 후 추가 예정
        }
    }

    private void StealthInfusedPiece(ChessPiece chessPiece)
    {
        InfusedPiece.SetKeyword(Keyword.Type.Stealth);
        //버프 관련 내용 머지 후 추가 예정
    }


    public override void AddEffect()
    {
        if (selectionNumber == 0)
        {
            InfusedPiece.OnStartAttack += GetHPInfoTarget;
            InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selectionNumber == 1)
        {
            InfusedPiece.moveCount += 1;
            InfusedPiece.buff.AddBuffByValue("페가수스", Buff.BuffType.MoveCount, 2, true);
        }
        else
        {
            InfusedPiece.SetKeyword(Keyword.Type.Stealth, is_stealth);
            InfusedPiece.OnKill += StealthInfusedPiece;
        }
    }

    public override void RemoveEffect()
    {
        if (selectionNumber == 0)
        {
            InfusedPiece.OnStartAttack -= GetHPInfoTarget;
            InfusedPiece.OnEndAttack -= StunChessPiece;
        }
        else if (selectionNumber == 1)
        {
            InfusedPiece.moveCount -= 1;
            InfusedPiece.buff.TryRemoveSpecificBuff("페가수스", Buff.BuffType.MoveCount);
        }
        else
        {
            is_stealth = InfusedPiece.GetKeyword(Keyword.Type.Stealth);
            InfusedPiece.SetKeyword(Keyword.Type.Stealth, 0);
            InfusedPiece.OnKill -= StealthInfusedPiece;
        }
    }
}
