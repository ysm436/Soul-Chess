using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardFilter : MonoBehaviour
{
    private bool[] costToggleList = new bool[10] {true, true, true, true, true, true, true, true, true, true}; // {0, 1, 2, 3, 4, 5, 6, 7, 8, 9+}
    private bool[] typeToggleList = new bool[2] {true, true}; // {soul, spell}
    private bool[] regionToggleList = new bool[3] {true, true, true}; // {greek, western, norse}
    private bool[] rarityToggleList = new bool[3] {true, true, true}; // {common, legendary, mythical}
    private bool[] chesspieceToggleList = new bool[6] {true, true, true, true, true, true}; // {pawn, knight, bishop, rook, quene, king}
    [SerializeField] TMP_InputField searchinputfield;
    private bool search_signal = false;
    
    private void Awake()
    {
        searchinputfield.onValueChanged.AddListener(SearchCard);
    }

    public void SearchCard(string searchtext)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (searchtext != "" && !search_signal)
        {
            search_signal = true;
        }

        foreach (var card in DisplayCardList)
        {
            string cardname = card.GetComponent<DisplayCard>().CardName;
            int cardnamelength = cardname.Length;
            int searchlength = searchtext.Length;

            if (cardnamelength >= searchtext.Length)
            {
                for (int i = 0; i <= cardnamelength - searchlength; i++)
                {
                    if (searchtext.ToLower() == cardname.Substring(i, searchlength).ToLower())
                    {
                        card.SetActive(true);
                        break;
                    }
                    else
                    {
                        card.SetActive(false);
                    }
                }
            }
        }

        if (searchtext == "" && search_signal)
        {
            search_signal = false;
            foreach (var card in DisplayCardList)
            {
                card.SetActive(false);
            }

            ForOnToggle();
        }
    }

    //마나 토글
    public void Cost0Toggle(bool cost0)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost0)
        {
            costToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 0)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost1Toggle(bool cost1)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost1)
        {
            costToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 1)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost2Toggle(bool cost2)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost2)
        {
            costToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[2] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 2)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost3Toggle(bool cost3)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost3)
        {
            costToggleList[3] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[3] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 3)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost4Toggle(bool cost4)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost4)
        {
            costToggleList[4] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[4] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 4)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost5Toggle(bool cost5)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost5)
        {
            costToggleList[5] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[5] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 5)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost6Toggle(bool cost6)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost6)
        {
            costToggleList[6] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[6] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 6)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost7Toggle(bool cost7)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost7)
        {
            costToggleList[7] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[7] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 7)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost8Toggle(bool cost8)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost8)
        {
            costToggleList[8] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[8] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost == 8)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    public void Cost9pToggle(bool cost9)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (cost9)
        {
            costToggleList[9] = true;
            ForOnToggle();
        }
        else
        {
            costToggleList[9] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Cost >= 9)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //스펠 카드 토글
    public void SoulToggle(bool soul)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (soul)
        {
            typeToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().CardType == Card.Type.Soul)
                {
                    card.SetActive(false);
                }
            }
        }
    }
    //스펠 카드 토글
    public void SpellToggle(bool spell)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (spell)
        {
            typeToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            typeToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().CardType == Card.Type.Spell)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //그리스 지역 토글
    public void GreekToggle(bool western)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (western)
        {
            regionToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Reigon == Card.Reigon.Greek)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //서유럽 지역 토글
    public void WesternToggle(bool western)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (western)
        {
            regionToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Reigon == Card.Reigon.Western)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //북유럽 지역 토글
    public void NorseToggle(bool norse)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (norse)
        {
            regionToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            regionToggleList[2] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Reigon == Card.Reigon.Norse)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //일반 등급 토글
    public void CommonToggle(bool common)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (common)
        {
            rarityToggleList[0] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[0] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Rarity == Card.Rarity.Common)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //전설 등급 토글
    public void LegendaryToggle(bool legendary)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (legendary)
        {
            rarityToggleList[1] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[1] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Rarity == Card.Rarity.Legendary)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //신화 등급 토글
    public void MythicalToggle(bool mythical)
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;

        if (mythical)
        {
            rarityToggleList[2] = true;
            ForOnToggle();
        }
        else
        {
            rarityToggleList[2] = false;
            foreach (var card in DisplayCardList)
            {
                if (!search_signal && card.activeSelf && card.GetComponent<DisplayCard>().Rarity == Card.Rarity.Mythical)
                {
                    card.SetActive(false);
                }
            }
        }
    }

    //기물 관련 토글
    public void PawnToggle(bool pawn_on)
    {
        chesspieceToggleList[0] = pawn_on;
        if (pawn_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }
    public void KnightToggle(bool knight_on)
    {
        chesspieceToggleList[1] = knight_on;

        if (knight_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }
    public void BishopToggle(bool bishop_on)
    {
        chesspieceToggleList[2] = bishop_on;
        if (bishop_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }
    public void RookToggle(bool rook_on)
    {
        chesspieceToggleList[3] = rook_on;
        if (rook_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }
    public void QueneToggle(bool quene_on)
    {
        chesspieceToggleList[4] = quene_on;
        if (quene_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }
    public void KingToggle(bool king_on)
    {
        chesspieceToggleList[5] = king_on;
        if (king_on)
        {
            ForOnToggle();
        }
        else
        {
            ForOffChessPieceToggle();
        }
    }

    private void ForOnToggle()
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;
        
        foreach (var card in DisplayCardList)
        {
            DisplayCard cardinfo = card.GetComponent<DisplayCard>();
            bool onflag = false;

            if (!card.activeSelf && !search_signal)
            {   
                if (cardinfo.Cost == 0)
                {
                    if(!costToggleList[0]) continue;
                }
                else if (cardinfo.Cost == 1)
                {
                    if(!costToggleList[1]) continue;
                }
                else if (cardinfo.Cost == 2)
                {
                    if(!costToggleList[2]) continue;
                }
                else if (cardinfo.Cost == 3)
                {
                    if(!costToggleList[3]) continue;
                }
                else if (cardinfo.Cost == 4)
                {
                    if(!costToggleList[4]) continue;
                }
                else if (cardinfo.Cost == 5)
                {
                    if(!costToggleList[5]) continue;
                }
                else if (cardinfo.Cost == 6)
                {
                    if(!costToggleList[6]) continue;
                }
                else if (cardinfo.Cost == 7)
                {
                    if(!costToggleList[7]) continue;
                }
                else if (cardinfo.Cost == 8)
                {
                    if(!costToggleList[8]) continue;
                }
                else if (cardinfo.Cost >= 9)
                {
                    if(!costToggleList[9]) continue;
                }

                if (cardinfo.CardType == Card.Type.Soul)
                {
                    if (!typeToggleList[0]) continue; 
                }
                else
                {
                    if (!typeToggleList[1]) continue;
                }

                if (cardinfo.Reigon == Card.Reigon.Greek)
                {
                    if (!regionToggleList[0]) continue;
                }
                else if (cardinfo.Reigon == Card.Reigon.Western)
                {
                    if (!regionToggleList[1]) continue;
                }
                else
                {
                    if (!regionToggleList[2]) continue;
                }

                if (cardinfo.Rarity == Card.Rarity.Common)
                {
                    if (!rarityToggleList[0]) continue;
                }
                else if (cardinfo.Rarity == Card.Rarity.Legendary)
                {
                    if (!rarityToggleList[1]) continue;
                }
                else
                {
                    if (!rarityToggleList[2]) continue;
                }

                if (cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Pawn))
                {
                    if (chesspieceToggleList[0]) onflag = true;
                }
                if (!onflag && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Knight))
                {
                    if (chesspieceToggleList[1]) onflag = true;
                }
                if (!onflag && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Bishop))
                {
                    if (chesspieceToggleList[2]) onflag = true;
                }
                if (!onflag && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Rook))
                {
                    if (chesspieceToggleList[3]) onflag = true;
                }
                if (!onflag && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Quene))
                {
                    if (chesspieceToggleList[4]) onflag = true;
                }
                if (!onflag && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.King))
                {
                    if (chesspieceToggleList[5]) onflag = true;
                }
 
                if (onflag || cardinfo.CardType == Card.Type.Spell)
                {
                    card.SetActive(true);
                }
            }
        }
    }

    private void ForOffChessPieceToggle()
    {
        List<GameObject> DisplayCardList = GetComponent<DeckBuildingManager>().DisplayCardList;
        
        foreach (var card in DisplayCardList)
        {
            DisplayCard cardinfo = card.GetComponent<DisplayCard>();

            if (!search_signal && card.activeSelf)
            {   
                if (chesspieceToggleList[0] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Pawn)) continue;
                if (chesspieceToggleList[1] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Knight)) continue;
                if (chesspieceToggleList[2] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Bishop)) continue;
                if (chesspieceToggleList[3] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Rook)) continue;
                if (chesspieceToggleList[4] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.Quene)) continue;
                if (chesspieceToggleList[5] && cardinfo.ChessPiece.HasFlag(ChessPiece.PieceType.King)) continue;
                if (cardinfo.CardType == Card.Type.Spell) continue;
                
                card.SetActive(false);
            }
        }
    }
}
