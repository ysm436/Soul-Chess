using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Perseus : SoulCard
{
    protected override int CardID => Card.cardIdDict["페르세우스"];

    [SerializeField] private List<GameObject> selectionPrefabList;
    private List<GameObject> selectionInstanceList;

    private int targethp;
    private int is_stealth;
    private int selection;
    private bool affectbyhelmetofhades = false;

    protected override void Awake()
    {
        base.Awake();
    }

    public void InstantiateSelectionCard(ChessPiece chessPiece)
    {
        selectionInstanceList = new();
        foreach (GameObject selection in selectionPrefabList)
        {
            GameObject selectionInstance = Instantiate(selection);
            CardSelectionDisplay cardSelectionDisplay = selectionInstance.AddComponent<CardSelectionDisplay>();
            cardSelectionDisplay.selectionNumber = selectionPrefabList.IndexOf(selection);
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

    public void GetAbility(int selectionNumber)
    {
        Destroy(selectionInstanceList[selectionNumber]);
        if (selectionNumber == 0)
        {
            selection = 0;
            InfusedPiece.OnStartAttack += GetHPInfoTarget;
            InfusedPiece.buff.AddBuffByDescription("메두사의 머리", Buff.BuffType.Description, "이 기물에게 피해를 받은 기물은 기절합니다.", true);
            InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selectionNumber == 1)
        {
            selection = 1;
            InfusedPiece.moveCount += 1;
            InfusedPiece.buff.AddBuffByValue("페가수스", Buff.BuffType.MoveCount, 1, true);
        }
        else
        {
            selection = 2;
            InfusedPiece.SetKeyword(Keyword.Type.Stealth);
            InfusedPiece.buff.AddBuffByKeyword("하데스의 투구", Buff.BuffType.Stealth);
            InfusedPiece.buff.AddBuffByDescription("하데스의 투구", Buff.BuffType.Description, "이 기물이 적을 처치하면 다시 은신합니다.", true);
            InfusedPiece.OnKill += AffectHelmetOfHades;
            InfusedPiece.OnMove += StealthInfusedPiece;
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
            chessPiece.buff.AddBuffByKeyword("메두사의 머리", Buff.BuffType.Stun);
        }
    }

    private void AffectHelmetOfHades(ChessPiece chessPiece)
    {
        affectbyhelmetofhades = true;
    }

    private void StealthInfusedPiece(Vector2Int targetcoordinate)
    {
        if (affectbyhelmetofhades == true)
        {
            InfusedPiece.SetKeyword(Keyword.Type.Stealth);
            InfusedPiece.buff.AddBuffByKeyword("하데스의 투구", Buff.BuffType.Stealth);
            affectbyhelmetofhades = false;
        }
    }


    public override void AddEffect()
    {
        if (selection == 0)
        {
            InfusedPiece.OnStartAttack += GetHPInfoTarget;
            InfusedPiece.OnEndAttack += StunChessPiece;
        }
        else if (selection == 1)
        {
            InfusedPiece.moveCount += 1;
            InfusedPiece.buff.AddBuffByValue("페가수스", Buff.BuffType.MoveCount, 2, true);
        }
        else
        {
            InfusedPiece.SetKeyword(Keyword.Type.Stealth, is_stealth);
            if (is_stealth == 1)
            {
                InfusedPiece.buff.AddBuffByKeyword("하데스의 투구", Buff.BuffType.Stealth);
            }
            InfusedPiece.OnKill += AffectHelmetOfHades;
            InfusedPiece.buff.AddBuffByDescription("하데스의 투구", Buff.BuffType.Description, "이 기물이 적을 처치하면 다시 은신합니다.", true);
        }
    }

    public override void RemoveEffect()
    {
        if (selection == 0)
        {
            InfusedPiece.OnStartAttack -= GetHPInfoTarget;
            InfusedPiece.OnEndAttack -= StunChessPiece;
            InfusedPiece.buff.TryRemoveSpecificBuff("메두사의 머리", Buff.BuffType.Description);
        }
        else if (selection == 1)
        {
            InfusedPiece.moveCount -= 1;
            InfusedPiece.buff.TryRemoveSpecificBuff("페가수스", Buff.BuffType.MoveCount);
        }
        else
        {
            is_stealth = InfusedPiece.GetKeyword(Keyword.Type.Stealth);
            InfusedPiece.SetKeyword(Keyword.Type.Stealth, 0);
            if (is_stealth == 1)
            {
                InfusedPiece.buff.TryRemoveSpecificBuff("하데스의 투구", Buff.BuffType.Stealth);
            }
            InfusedPiece.buff.TryRemoveSpecificBuff("하데스의 투구", Buff.BuffType.Description);
            InfusedPiece.OnKill -= AffectHelmetOfHades;
        }
    }
}
