using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI cardName;
    [SerializeField] private TextMeshProUGUI cost;
    [SerializeField] private TextMeshProUGUI ADText;
    [SerializeField] private TextMeshProUGUI HPText;
    [SerializeField] private TextMeshProUGUI description;
    [SerializeField] private GameObject AD;
    [SerializeField] private GameObject HP;
    [SerializeField] private Image illustration;
    [SerializeField] private GameObject[] cardFrames;
    [SerializeField] private GameObject[] cardKeywords;
    [SerializeField] private TextMeshProUGUI[] cardKeywordTitle;
    [SerializeField] private TextMeshProUGUI[] cardKeywordDescriptions;
    [SerializeField] private GameObject spell;
    [SerializeField] private GameObject soul;
    [SerializeField] private GameObject pieceImage;

    public void SetCardUI(Card card)
    {
        if (card is SoulCard)
        {
            AD.SetActive(true);
            HP.SetActive(true);
            ADText.text = card.GetComponent<SoulCard>().AD.ToString();
            HPText.text = card.GetComponent<SoulCard>().HP.ToString();
            spell.SetActive(false);
            soul.SetActive(true);
        }
        else
        {
            AD.SetActive(false);
            HP.SetActive(false);
            spell.SetActive(true);
            soul.SetActive(false);
        }

        foreach (var cardFrame in cardFrames)
            cardFrame.SetActive(false);
        foreach (var cardKeyword in cardKeywords)
            cardKeyword.SetActive(false);
        pieceImage.SetActive(false);

        cardFrames[(int)card.rarity].SetActive(true);

        cardName.text = card.cardName;
        cost.text = card.cost.ToString();
        description.text = card.description;
        illustration.sprite = card.illustration;

        Debug.Log(card.cardKeywords.Count);
        for (int i = 0; i < card.cardKeywords.Count; i++)
        {
            cardKeywords[i].SetActive(true);
            cardKeywordTitle[i].text = keywordTitle[card.cardKeywords[i]];
            cardKeywordDescriptions[i].text = keywordDescription[card.cardKeywords[i]];
        }
    }

    public void SetCardUI(ChessPiece piece)
    {
        spell.SetActive(false);
        soul.SetActive(false);
        AD.SetActive(true);
        HP.SetActive(true);
        ADText.text = piece.AD.ToString();
        HPText.text = piece.GetHP.ToString();

        foreach (var cardFrame in cardFrames)
            cardFrame.SetActive(false);
        foreach (var cardKeyword in cardKeywords)
            cardKeyword.SetActive(false);
        pieceImage.SetActive(true);

        cardFrames[cardFrames.Length - 1].SetActive(true);

        if(piece.pieceColor == GameBoard.PlayerColor.White)
            cardName.text = "화이트 " + pieceName[piece.pieceType];
        else
            cardName.text = "블랙 " + pieceName[piece.pieceType];
        cost.text = "0";
        description.text = "";
        pieceImage.GetComponent<Image>().sprite = piece.GetComponent<SpriteRenderer>().sprite;
    }

    public Dictionary<ChessPiece.PieceType, string> pieceName = new Dictionary<ChessPiece.PieceType, string>()
    {
        { ChessPiece.PieceType.Bishop , "비숍"},
        { ChessPiece.PieceType.Knight , "나이트"},
        { ChessPiece.PieceType.Pawn , "폰"},
        { ChessPiece.PieceType.King , "킹"},
        { ChessPiece.PieceType.Quene , "퀸"},
        { ChessPiece.PieceType.Rook , "룩"},
    };

    public Dictionary<Keyword.Type, string> keywordTitle = new Dictionary<Keyword.Type, string>()
    {
        { Keyword.Type.Infusion,"강림"},
        { Keyword.Type.Shield,"보호막"},
        { Keyword.Type.Stun,"기절"},
        { Keyword.Type.Silence,"침묵"},
        { Keyword.Type.Testament,"유언"},
    };
    public Dictionary<Keyword.Type, string> keywordDescription = new Dictionary<Keyword.Type, string>()
    {
        { Keyword.Type.Infusion,"영혼을 부여할 때\n발동됩니다."},
        { Keyword.Type.Shield,"이 기물은 피해를\n1번 무시합니다."},
        { Keyword.Type.Stun,"기절 시 다음 턴까지\n움직이지 못합니다."},
        { Keyword.Type.Silence,"기물의 특수 능력을\n제거합니다."},
        { Keyword.Type.Testament,"기물이 파괴될 때\n발동됩니다."},
    };
}
