using TMPro;
using UnityEngine;


public class DeckSelectButton : MonoBehaviour
{
    public TextMeshProUGUI deckname;
    public int deckIndex;
    public void DeckSelect()
    {
        //������ ���õǾ� �ִ� ���� ���̴� ����
        if(GetComponentInParent<LobbySceneUI>().SelectedDeckIndex != -1)
            GetComponentInParent<LobbySceneUI>().GetSelectedDeckButton().ControlShader(false);

        //���� �� ���̴� Ȱ��ȭ
        ControlShader(true);

        GetComponentInParent<LobbySceneUI>().SelectedDeckIndex = deckIndex;

        //ī���� ���ٸ� ���⼭�� �߰��ؾ���
        GetComponentInParent<LobbySceneUI>().ShowSelectedDeckCardList();
    }

    public void ControlShader(bool activate)
    { }
}
