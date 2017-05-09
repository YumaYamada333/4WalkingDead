using System.Collections;
using UnityEngine;

public class ResetScript : MonoBehaviour
{
    //セットカードボードのオブジェクト用
    public struct INITDATA
    {
        public CardBord.CardData[] setCard;
        public int[] cardNum;
    }

    //カードのオブジェクト
    private INITDATA[] earlyCard;
    //データの保存数
    private int detaNum = 4;
    private int num = 0;

    // ActionBoardの情報を取得
    CardBord bord;
    CardManagement cards;

    private void Start()
    {
        //ボタンを非表示にしておく
        gameObject.SetActive(false);

        //コンポーネントの取得
        bord = GameObject.Find("ActionBord").GetComponent<CardBord>();
        cards = GameObject.Find("CardManager").GetComponent<CardManagement>();

        //データを保存する領域
        earlyCard = new INITDATA[detaNum];

        //起動時に情報を取得しておく
        for (int i = 0; i < earlyCard.Length; i++)
        {
            earlyCard[i].setCard = new CardBord.CardData[CardBord.numSetMax];
            earlyCard[i].cardNum = new int[CardManagement.numMax];
            for (int j = 0; j < earlyCard.Length; j++)
            {
                earlyCard[i].setCard[j].type = CardManagement.CardType.Nothing;
            }
        }
    }
    private void Update()
    {
        if (earlyCard[0].setCard[0].obj == null)
        {
            SaveCard(bord.cards, cards.cards);
        }
    }

    //ボタンクリック時の処理
    public void OnClick()
    {
        num = 0;

        cards.ReturnBoard(earlyCard[num]);
    }

    // 現在のボード情報を保存
    public void SaveCard(CardBord.CardData[] A_card, CardManagement.CardData[] H_card, GameObject tuckCard1 = null, GameObject tuckCard2 = null)
    {
        // カードを保存する領域がある
        //if (num < detaNum)
        //{

            for (int i = 0; i < A_card.Length; i++)
            {
                // ActionBoardのカード情報を保存
                //オブジェクト
                earlyCard[num].setCard[i].obj = A_card[i].obj;
                //カードタイプ
                if (earlyCard[num].setCard[i].obj != null)
                    earlyCard[num].setCard[i].type = A_card[i].type;
                else
                    earlyCard[num].setCard[i].type = CardManagement.CardType.Nothing;
            }

            //// HandsBoardのカード所持数を保存
            //for (int j = 0; j < H_card.Length; j++)
            //    earlyCard[num].cardNum[j] = H_card[j].numHold;

            num++;
        //}
    }
}
