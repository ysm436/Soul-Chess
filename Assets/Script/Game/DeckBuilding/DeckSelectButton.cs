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
        if (GetComponentInParent<LobbySceneUI>() == null)
        {
            //기존의 선택되어 있던 덱의 쉐이더 끄기
            if (GetComponentInParent<PvELobbySceneUI>().SelectedDeckIndex != -1)
                GetComponentInParent<PvELobbySceneUI>().GetSelectedDeckButton().ControlShader(false);

            //지금 덱 쉐이더 활성화
            ControlShader(true);

            GetComponentInParent<PvELobbySceneUI>().SelectedDeckIndex = deck_index;

            //카드목록 띄운다면 여기서도 추가해야함
            GetComponentInParent<PvELobbySceneUI>().ShowSelectedDeckCardList();
        }

        else
        {
            //기존의 선택되어 있던 덱의 쉐이더 끄기
            if (GetComponentInParent<LobbySceneUI>().SelectedDeckIndex != -1)
                GetComponentInParent<LobbySceneUI>().GetSelectedDeckButton().ControlShader(false);

            //지금 덱 쉐이더 활성화
            ControlShader(true);

            GetComponentInParent<LobbySceneUI>().SelectedDeckIndex = deck_index;

            //카드목록 띄운다면 여기서도 추가해야함
            GetComponentInParent<LobbySceneUI>().ShowSelectedDeckCardList();
        }
    }

    public void ControlShader(bool activate)
    { }
}
