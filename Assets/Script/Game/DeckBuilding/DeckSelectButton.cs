using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;


public class DeckSelectButton : MonoBehaviour
{
    public TextMeshProUGUI deckname;
    public int deck_index;
    public void DeckSelect()
    {
        //기존의 선택되어 있던 덱의 쉐이더 끄기
        GetComponentInParent<LobbySceneUI>().GetSelectedDeckButton().ControlShader(false);

        //지금 덱 쉐이더 활성화
        ControlShader(true);

        //카드목록 띄운다면 여기서도 추가해야함
        ShowCardList();

        GetComponentInParent<LobbySceneUI>().SelectedDeckIndex = deck_index;
    }

    public void ControlShader(bool activate)
    { }

    public void ShowCardList()
    { }
}
