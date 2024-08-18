using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Buff;

// 기본 AD, HP, MoveCount 등은 저장 X, 버프된 값만 저장
public class Buff : Effect
{
    public enum BuffType                    // 일단 수치로 나타낼 수 있는 체력, 공격력, 이동횟수 버프만 따로 분류했습니다.
    {
        HP,                                 // 음수, 양수 상관 없음 (버프, 디버프 구분X)
        AD,
        MoveCount,
        Defense,                            // 방어도 (AddBuffByValue로 추가)
        Immunity,                           // 다른 Keyword 버프들은 AddBuffByKeyword로 추가
        Taunt,
        Shield,
        Stun,
        Restraint,
        Stealth,
        Silence,
        Rush,
        Description
    }

    public struct BuffInfo
    {
        public string sourceName;           // 버프를 준 주체
        public BuffType buffType;           // 버프 종류
        public int value;                   // 버프 값 (버프 종류가 HP, AD, MoveCount, Defense일 때 사용)
        public string description;          // 버프 설명 (버프 종류가 Description일 때 사용)
        public bool isRemovableByEffect;    // 침묵, 영혼 교체에 의해 제거되는 버프인지 나타냄
    }

    private List<BuffInfo> _buffList;
    public List<BuffInfo> buffList { get => _buffList; }

    public Buff()
    {
        _buffList = new();
    }

    public void AddBuffByValue(string sourceName, BuffType buffType, int value, bool isRemovableByEffect)   // 버프 종류가 HP, AD, MoveCount, Defense일 때 사용
    {
        if (buffType != BuffType.HP && buffType != BuffType.AD && buffType != BuffType.MoveCount && buffType != BuffType.Defense)
        {
            Debug.Log("AddBuffByValue에 잘못된 BuffType이 들어감");
            return;
        }

        if (FindBuff(sourceName, buffType, out int index))              // 기존에 있는 버프일 경우 수치만 조정
        {
            BuffInfo newBuffInfo = _buffList[index];
            if (buffType == BuffType.HP || buffType == BuffType.AD || buffType == BuffType.MoveCount || buffType == BuffType.Defense)
                newBuffInfo.value += value;
            _buffList[index] = newBuffInfo;
        }
        else                                                                                                // 기존에 없는 버프일 경우 새로 추가
        {
            BuffInfo buffInfo = new();

            buffInfo.sourceName = sourceName;
            buffInfo.buffType = buffType;
            buffInfo.value = value;
            buffInfo.description = "";
            buffInfo.isRemovableByEffect = isRemovableByEffect;

            _buffList.Add(buffInfo);
        }
    }

    public void AddBuffByKeyword(string sourceName, BuffType buffType)   // 버프 종류가 Defense를 제외한 Keyword 버프일 때 사용
    {
        if (buffType == BuffType.HP || buffType == BuffType.AD || buffType == BuffType.MoveCount || buffType == BuffType.Defense || buffType == BuffType.Description)
        {
            Debug.Log("AddBuffByKeyword에 잘못된 BuffType이 들어감");
            return;
        }

        if (!FindKeyword(buffType, out int index))              // 기존에 있는 키워드 버프일 경우 출처만 최신화
        {
            BuffInfo buffInfo = new();

            buffInfo.sourceName = sourceName;
            buffInfo.buffType = buffType;
            buffInfo.value = 0;
            switch (buffType)
            {
                case BuffType.Immunity : buffInfo.description = "면역 부여됨"; break;
                case BuffType.Taunt : buffInfo.description = "도발 부여됨"; break;
                case BuffType.Shield : buffInfo.description = "보호막 부여됨"; break;
                case BuffType.Stun : buffInfo.description = "기절 부여됨"; break;
                case BuffType.Restraint : buffInfo.description = "구속 부여됨"; break;
                case BuffType.Stealth : buffInfo.description = "은신 부여됨"; break;
                case BuffType.Silence : buffInfo.description = "침묵 부여됨"; break;
                case BuffType.Rush : buffInfo.description = "돌진 부여됨"; break;
                default : buffInfo.description = "오류"; break;
            }
            buffInfo.isRemovableByEffect = true;

            _buffList.Add(buffInfo);
        }
    }

    public void AddBuffByDescription(string sourceName, BuffType buffType, string description, bool isRemovableByEffect)              // 버프 종류가 Description일 때 사용
    {
        if (buffType != BuffType.Description)
        {
            Debug.Log("AddBuffByDescription에 잘못된 BuffType이 들어감");
            return;
        }

        BuffInfo buffInfo = new();

        buffInfo.sourceName = sourceName;
        buffInfo.buffType = buffType;
        buffInfo.value = 0;
        buffInfo.description = description;
        buffInfo.isRemovableByEffect = isRemovableByEffect;

        _buffList.Add(buffInfo);
    }

    public bool TryRemoveSpecificBuff(string sourceName, BuffType buffType) 
    {
        //키워드 버프인 경우 sourceName 상관없이 삭제
        if (buffType != BuffType.HP && buffType != BuffType.AD && buffType != BuffType.MoveCount && buffType != BuffType.Defense)
        {
            if (FindKeyword(buffType, out int index))
            {
                _buffList.RemoveAt(index);
                return true;
            }
            return false;
        }
        else if (FindBuff(sourceName, buffType, out int index))
        {
            _buffList.RemoveAt(index);
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ClearBuffList()
    {
        for (int i = _buffList.Count - 1; i >= 0; i--)
        {
            if (_buffList[i].isRemovableByEffect)
                _buffList.RemoveAt(i);
        }
    }

    private bool FindBuff(string sourceName, BuffType buffType, out int index)
    {
        for (int i = 0; i < _buffList.Count; i++)
        {
            if (_buffList[i].sourceName == sourceName && _buffList[i].buffType == buffType)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    private bool FindKeyword(BuffType buffType, out int index) //keyword 버프 탐색용
    {
        for (int i = 0; i < _buffList.Count; i++)
        {
            if (_buffList[i].buffType == buffType)
            {
                index = i;
                return true;
            }
        }
        index = -1;
        return false;
    }

    public override void EffectAction()
    {

    }
}
