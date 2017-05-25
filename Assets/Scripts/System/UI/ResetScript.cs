using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

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

    //カウントアクションをするオブジェクトのコンポーネント取得用
    ActionCountDown ActCountDown;

    private void Start()
    {
        //ボタンを非表示にしておく
        //gameObject.SetActive(false);

        //コンポーネントの取得
        bord = GameObject.Find("ActionBord").GetComponent<CardBord>();
        cards = GameObject.Find("CardManager").GetComponent<CardManagement>();

        ActCountDown = GetComponent<ActionCountDown>();

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

        ////カウントアクションをするオブジェクトを保存
        //SaveObj(ref ActCountDown.obj, ActCountDown.m_action_type);

    }

    //ボタンクリック時の処理
    public void OnClick()
    {
        num = 0;

        //初期カードを再生成
        cards.ReturnBoard(earlyCard[num]);

        //現在読み込んでいるシーンの名前を取得
        string currentScene = SceneManager.GetActiveScene().name;
        //取得したシーン名で再読み込み
        SceneManager.LoadScene(currentScene);
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

    //現在のオブジェクトの状態を保存
    //private void SaveObj(ref GameObject obj,int ActType)
    //{
    //    //アクションタイプ
    //    const int Move = 0;
    //    const int Break = 1;

    //    //オブジェクトの座標
    //    Vector2 ObjTmp;
    //    //オブジェクトの情報
    //    GameObject InitObj;

    //    //アクションタイプの判別
    //    switch(ActType)
    //    {
    //        //座標を保存
    //        case Move:
    //            ObjTmp = new Vector2(obj.transform.position.x, obj.transform.position.y);            
    //            break;
    //        //オブジェクト情報を保存
    //        case Break:
    //            InitObj = obj;
    //            break;
    //    }
    //}

}
