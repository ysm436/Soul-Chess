using System;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class DeckBuildingManager : MonoBehaviour
{
    public List<GameObject> DisplayCardList = new List<GameObject>();

    [SerializeField] private List<Sprite> Icon_sprites;
    [SerializeField] private int max_card_index;
    [SerializeField] private int quantity_setting;

    public Transform DynamicDisplay;
    public Transform DisplayStorage; // 디스플레이 카드가 덱에 들어가 남아있는 카드의 개수가 0개가 되었을 때 저장되는 창고
    public Transform TrashCan; // 더 이상 쓰지 않는 object를 넣어두는 쓰레기통
    public GameObject display_prefab_odd;
    public GameObject display_prefab_even;

    private void Awake()
    {
        MakeDisplayCard();
    }

    // 카드들을 화면에 나타냅니다.
    private void MakeDisplayCard()
    {
        for (int i = 0; i < GameManager.instance.AllCards.Length; i++)
        {
            if (GameManager.instance.AllCards[i])
            {
                AddDisplayCard(i, quantity_setting);
            }
        }
    }

    //디스플레이 object를 생성합니다.
    public void AddDisplayCard(int card_index, int quantity)
    {
        GameObject card = GameManager.instance.AllCards[card_index];
        Card cardinfo = card.GetComponent<Card>();
        
        ChessPiece.PieceType piecelist = ChessPiece.PieceType.None;
        List<ChessPiece.PieceType> includedChesspieces = new List<ChessPiece.PieceType>();
        GameObject newDisplay;
        int chesspiece_count;
        
        if (card.GetComponent<SpellCard>()) // 스펠 카드
        {
            chesspiece_count = 0;
        }
        else // 소울 카드
        {
            piecelist = card.GetComponent<SoulCard>().pieceRestriction;
            foreach (ChessPiece.PieceType piecetype in Enum.GetValues(typeof(ChessPiece.PieceType)))
            {
                if (piecetype != ChessPiece.PieceType.None && piecelist.HasFlag(piecetype))
                {
                    includedChesspieces.Add(piecetype);
                }
            }
            chesspiece_count = includedChesspieces.Count;
        }

        DisplayCard displaycard;
        if (chesspiece_count % 2 == 0) // 디스플레이에 표시될 기물 짝수개
        {
            newDisplay = Instantiate(display_prefab_even, DynamicDisplay);
            displaycard = newDisplay.GetComponent<DisplayCard>();
            int child_number = 0;
            
            foreach (var piece in includedChesspieces)
            {
                Image piece_display = displaycard.chesspiecedisplay_list[child_number];
                piece_display.sprite = ChessPieceDisplay(piece);
                piece_display.gameObject.SetActive(true);
                child_number++;
            }
        }
        else // 홀수개
        {
            newDisplay = Instantiate(display_prefab_odd, DynamicDisplay);
            displaycard = newDisplay.GetComponent<DisplayCard>();
            int child_number = 0;
            
            foreach (var piece in includedChesspieces)
            {
                Image piece_display = displaycard.chesspiecedisplay_list[child_number];
                piece_display.sprite = ChessPieceDisplay(piece);
                piece_display.gameObject.SetActive(true);
                child_number++;
            }
        }

        newDisplay.name = card.name + "_display";
        displaycard.cardindex = card_index;
        displaycard.CardName = cardinfo.cardName;
        displaycard.Cost = cardinfo.cost;
        displaycard.Description = cardinfo.description;
        displaycard.Reigon = cardinfo.reigon;
        displaycard.Rarity = cardinfo.rarity;
        displaycard.quantity = quantity;
        displaycard.illustrate.sprite = cardinfo.illustration;
        displaycard.cardframe.sprite = cardinfo.GetComponent<SpriteRenderer>().sprite;

        if (cardinfo is SoulCard)
        {
            displaycard.HP = (cardinfo as SoulCard).HP;
            displaycard.AD = (cardinfo as SoulCard).AD;
        }

        if (chesspiece_count == 0)
        {
            displaycard.CardType = Card.Type.Spell;
            displaycard.ChessPiece = ChessPiece.PieceType.None;
        }
        else
        {
            displaycard.CardType = Card.Type.Soul;
            displaycard.ChessPiece = piecelist;
        }

        DisplayCardList.Add(newDisplay);
    }

    //디스플레이될 카드들을 재설정 합니다.
    public void ReloadDisplayCard()
    {
        foreach (var display_card in DisplayCardList)
        {
            display_card.GetComponent<DisplayCard>().quantity = quantity_setting;
        }

        for (int i = DisplayStorage.childCount; i > 0; i--)
        {
            Transform card = DisplayStorage.GetChild(i - 1);
            AddDisplayCard(card.gameObject.GetComponent<DisplayCard>().cardindex, quantity_setting);
            Transform added_display = DynamicDisplay.GetChild(DynamicDisplay.childCount - 1);
            for (int card_order = 0; card_order < DynamicDisplay.childCount - 1; card_order++)
            {
                if (DynamicDisplay.GetChild(card_order).GetComponent<DisplayCard>().cardindex > added_display.GetComponent<DisplayCard>().cardindex)
                {
                    added_display.SetSiblingIndex(card_order);
                    break;
                }
            }
            card.SetParent(TrashCan);
            card.gameObject.SetActive(false);
        }
    }

    private Sprite ChessPieceDisplay(ChessPiece.PieceType piece)
    {
        if (piece.HasFlag(ChessPiece.PieceType.King))
        {
            return Icon_sprites[0];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Quene))
        {
            return Icon_sprites[1];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Rook))
        {
            return Icon_sprites[2];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Bishop))
        {
            return Icon_sprites[3];
        }
        else if (piece.HasFlag(ChessPiece.PieceType.Knight))
        {
            return Icon_sprites[4];
        }
        else
        {
            return Icon_sprites[5];
        }
    }
}