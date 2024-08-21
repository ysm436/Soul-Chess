using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;
using System.Linq;

public class ShowDeckInfo : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private GameObject deckinfopanel;
    [SerializeField] private Transform cost_graph;
    [SerializeField] private TextMeshProUGUI pawn_cost;
    [SerializeField] private TextMeshProUGUI knight_cost;
    [SerializeField] private TextMeshProUGUI bishop_cost;
    [SerializeField] private TextMeshProUGUI rook_cost;
    [SerializeField] private TextMeshProUGUI queen_cost;
    [SerializeField] private TextMeshProUGUI king_cost;
    [SerializeField] private TextMeshProUGUI soul_cost;
    [SerializeField] private TextMeshProUGUI spell_cost;
    [SerializeField] private TextMeshProUGUI common_cost;
    [SerializeField] private TextMeshProUGUI legendary_cost;
    [SerializeField] private TextMeshProUGUI mythical_cost;

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        DeckManager deckmanager = GetComponentInParent<DeckManager>();

        for (int i = 0; i < cost_graph.childCount; i++)
        {
            RectTransform recttransform = cost_graph.GetChild(i).GetComponent<RectTransform>();
            Vector2 graph_size = recttransform.sizeDelta;
            graph_size.y = 20 * deckmanager.local_costs[i];
            recttransform.sizeDelta = graph_size;
        }

        pawn_cost.text = deckmanager.local_chesspieces[0].ToString() + "(" + deckmanager.local_extra_chesspieces[0].ToString() + ")";
        knight_cost.text = deckmanager.local_chesspieces[1].ToString() + "(" + deckmanager.local_extra_chesspieces[1].ToString() + ")";
        bishop_cost.text = deckmanager.local_chesspieces[2].ToString() + "(" + deckmanager.local_extra_chesspieces[2].ToString() + ")";
        rook_cost.text = deckmanager.local_chesspieces[3].ToString() + "(" + deckmanager.local_extra_chesspieces[3].ToString() + ")";
        queen_cost.text = deckmanager.local_chesspieces[4].ToString() + "(" + deckmanager.local_extra_chesspieces[4].ToString() + ")";
        king_cost.text = deckmanager.local_chesspieces[5].ToString() + "(" + deckmanager.local_extra_chesspieces[5].ToString() + ")";

        int soul_sum = deckmanager.local_chesspieces.Sum();
        soul_cost.text = soul_sum.ToString() + " / 16";
        spell_cost.text = (deckmanager.local_card_count - soul_sum).ToString();

        common_cost.text = deckmanager.local_rarities[0].ToString();
        legendary_cost.text = deckmanager.local_rarities[1].ToString();
        mythical_cost.text = deckmanager.local_rarities[2].ToString() + " / 5";

        deckinfopanel.SetActive(true);
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        deckinfopanel.SetActive(false);
    }


}
