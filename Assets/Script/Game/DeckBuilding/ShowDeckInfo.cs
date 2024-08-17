using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ShowDeckInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject deckinfopanel;
    [SerializeField] private TextMeshProUGUI pawn_cost;
    [SerializeField] private TextMeshProUGUI knight_cost;
    [SerializeField] private TextMeshProUGUI bishop_cost;
    [SerializeField] private TextMeshProUGUI rook_cost;
    [SerializeField] private TextMeshProUGUI queen_cost;
    [SerializeField] private TextMeshProUGUI king_cost;
    [SerializeField] private TextMeshProUGUI legendary_cost;
    [SerializeField] private TextMeshProUGUI mythical_cost;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        DeckManager deckmanager = GetComponentInParent<DeckManager>();

        pawn_cost.text = deckmanager.local_chesspieces[0].ToString() + " / 8";
        knight_cost.text = deckmanager.local_chesspieces[1].ToString() + " / 2";
        bishop_cost.text = deckmanager.local_chesspieces[2].ToString() + " / 2";
        rook_cost.text = deckmanager.local_chesspieces[3].ToString() + " / 2";
        queen_cost.text = deckmanager.local_chesspieces[4].ToString() + " / 1";
        king_cost.text = deckmanager.local_chesspieces[5].ToString() + " / 1";

        legendary_cost.text = deckmanager.local_rarities[1].ToString() + " / 9";
        mythical_cost.text = deckmanager.local_rarities[2].ToString() + " / 3";

        deckinfopanel.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        deckinfopanel.SetActive(false);
    }


}
