using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBord : MonoBehaviour {

    // 最大セット枚数
    public const int numSetMax = 50;

    // 配置したカード
    public struct CardData
    {
        public CardManagement.CardType type;
        public GameObject obj;
    }
    //カードの掴んでる判定
    CardManagement.CursorForcusTag cursor;

    public CardData[] cards = new CardData[numSetMax];
    Vector3[] tmp = new Vector3[numSetMax];
    RaycastHit[] hit;

    // ボード上のカード数
    public int numSet;

    // カードのサイズ
    Vector2 cardSize;

    // 中心のカード
    int centerCard;

    // スクロールの状態
    float scrollStep;

    // 使用中のカードの進行状態
    public int stepUsing;
    // 使用中カード
    public int usingCard;

    // 選択中のスペース
    public int selectedSpace;

    //カードの初期ｚ座標
    const float zPos = -0.1f;

    //カードの枠越え判定
    bool exceedFlag = false;

    //フレーム計測
    int flameCnt = 0;

    // マウスのコンポーネント
    MouseSystem mouse_system;

    // マウスのクリック時の座標
    float dragPos;

    bool canScroll;
    
    //実行フラグ
    bool PlayFlag = false;

    //ゲームの状態
    GameManager state;

    // 画面のスクロールスクリプト
    [SerializeField]
    ScrollScript scroolScript;

    // Use this for initialization
    void Start ()
    {
        //cardSize = cards[0].obj.GetComponent<RectTransform>().sizeDelta;
        centerCard = 0;
        selectedSpace = 0;
        //numSet = 0;
        usingCard = stepUsing = 0;
        exceedFlag = false;
        scrollStep = 0.0f;

        // MouseSystemコンポーネントの取得
        mouse_system = GameObject.Find("MouseSystem").GetComponent<MouseSystem>();
        state = GameObject.Find("GameManager").GetComponent<GameManager>();

        Coordinate();

        //プレイフラグ
        PlayFlag = false;
    }

    //カードの初期座標取得関数
    public void Coordinate()
    {

        centerCard = usingCard;
        //カードの座標設定
        for (int i = 0; i < numSetMax; i++)
        {
            if (cards[i].obj == null) break;

            cards[i].obj.transform.localPosition =
                new Vector3(cardSize.x / 2 + (i - centerCard) * cardSize.x - GetComponent<RectTransform>().sizeDelta.x / 2 + scrollStep, 0.0f, zPos);
            //}
        }
    }

    // Update is called once per frame
    void Update ()
    {
        cardSize = cards[0].obj.GetComponent<RectTransform>().sizeDelta;

        //アクションモードになったら
        if (state.GetGameState() == GameManager.GameState.Acttion)
        {
            //プレイフラグを立てる
            PlayFlag = true;
        }

        //プレイフラグが立ったら
        if (PlayFlag == true)
        {
            //カードを初期位置に移動させる
            Coordinate();
        }

        //プレイフラグが立っていないなら
        if (PlayFlag != true)
        {
            // 左クリックしたら
            if (Input.GetMouseButton(0))
            {
                // Rayに触れたオブジェクトをすべて取得
                hit = mouse_system.GetReyhitObjects();

                if (hit.Length > 0)
                {
                    if (hit[hit.Length - 1].collider.tag == "Card")
                    {
                        //フレーム計測値を初期化
                        flameCnt = 0;
                    }
                }
            }
            else
            {
                //フレームを計測する
                flameCnt++;
            }

            //カードを掴んだらかカードを離して10フレーム未満なら
            if (cursor == CardManagement.CursorForcusTag.ActtionBord || (cursor == CardManagement.CursorForcusTag.HandsBord && flameCnt < 10))
            {
                //初期座標へ移動
                Coordinate();
            }

            //スクロールボタンが押されたら
            if (Input.GetButton("CardScroll"))
            {
                // カードの座標設定
                for (int i = 0; i < numSetMax; i++)
                {
                    if (cards[i].obj == null) continue;
                    //セットカードの枠を超えたら
                    if (cards[i].obj.transform.localPosition.x >= 0.5f)
                    {
                        //フラグを立てる
                        exceedFlag = true;
                    }

                    //枠を超えたら
                    if (exceedFlag == true)
                    {
                        // カードの座標設定
                        for (int j = 0; j < numSetMax; j++)
                        {
                            if (cards[j].obj == null) continue;
                            if (Input.GetAxis("CardScroll") > 0)
                            {
                                //右スクロール
                                cards[j].obj.transform.localPosition += new Vector3(0.5f, 0, 0);
                            }
                            else if (Input.GetAxis("CardScroll") < 0)
                            {
                                //左スクロール
                                cards[j].obj.transform.localPosition -= new Vector3(0.5f, 0, 0);
                            }
                        }
                    }
                }
            }

            //カードを離してかつ10フレームたったら
            if (cursor != CardManagement.CursorForcusTag.ActtionBord && flameCnt >= 10)
            {
                for (int i = 0; i < numSetMax; i++)
                {
                    //オブジェクトが存在しているならば
                    if (cards[i].obj != null)
                    {
                        //オブジェクトの座標を取得
                        tmp[i] = new Vector3(cards[i].obj.transform.localPosition.x, cards[i].obj.transform.localPosition.y, cards[i].obj.transform.localPosition.z);
                    }
                    else
                    {
                        tmp[i] = new Vector3(0, 0, 0);
                    }

                    //カード選択時の座標変更
                    //if ((selectedSpace == i) && selectedSpace >= 0)
                    //{
                    //    cards[i].obj.transform.localPosition = new Vector3(tmp[i].x, tmp[i].y, -0.3f);
                    //}
                    //else
                    {
                        //cards[i].obj.transform.localPosition = new Vector3(tmp[i].x, tmp[i].y, zPos);
                    }
                }
            }
        }

        Scroll();

        if (true)
        {
            // 使用済みカードの非表示化
            if (usingCard <= numSetMax && usingCard > 0)
            {
                if (cards[usingCard].obj && cards[usingCard - 1].obj)
                {
                    cards[usingCard - 1].obj.SetActive(false);
                    cards[usingCard].obj.SetActive(true);

                }
            }

            // boardからはみ出たＣａｒｄの非表示化
            RectTransform size = gameObject.GetComponent<RectTransform>();
            // boardの右端から出たカードを非表示
            for (int i = usingCard; i < numSetMax; i++)
            {
                if (cards[i].obj == null) continue;
                if (cards[i].obj.transform.localPosition.x >= 
                    transform.localPosition.x + GetComponent<RectTransform>().sizeDelta.x / 2 ||
                    cards[i].obj.transform.localPosition.x <=
                    transform.localPosition.x - GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    cards[i].obj.SetActive(false);
                }
                else
                {
                    cards[i].obj.SetActive(true);
                }
            }
        }

    }
    public bool CheckRightEnd()
    {
        if (cards[numSet - 1].obj == null) return false;
        if (numSet < GetComponent<RectTransform>().sizeDelta.x / cardSize.x + 1||
            (cards[numSet - 1].obj.transform.localPosition.x + cardSize.x >=
            transform.localPosition.x + GetComponent<RectTransform>().sizeDelta.x / 2 &&
            cards[numSet - 1].obj.activeSelf))
            {
                return true;
            }
        return false;
    }

    public bool CheckLeftEnd()
    {
        if (cards[usingCard].obj == null) return false;
        if (numSet < GetComponent<RectTransform>().sizeDelta.x / cardSize.x + 1||
            (cards[usingCard].obj.transform.localPosition.x - cardSize.x <=
            transform.localPosition.x - GetComponent<RectTransform>().sizeDelta.x / 2 &&
            cards[usingCard].obj.activeSelf))
        {
            return true;
        }
        return false;
    }


    // ボードにカードをセットする
    public bool SetCard(GameObject obj, CardManagement.CardType type)
    {
        cards[numSet].obj = obj;
        cards[numSet].obj.transform.parent = transform;
        cards[numSet].obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        cards[numSet].type = type;
        numSet++;
        return true;
    }

    // カードを削除する
    public bool DeleteCard(int no)
    {
        Destroy(cards[no].obj);
        cards[no].type = CardManagement.CardType.Nothing;  
        numSet--;
        for (int i = no; i < numSetMax - 1; i++)
        {
            cards[i] = cards[i + 1];
        }

        return true;
    }

    // カードを変更する
    public bool ChangeCard(CardData card, int no)
    {
        Destroy(cards[no].obj);
        cards[no] = card;

        return true;
    }

    public bool TuckCard(CardData card, int posNo)
    {
        CardData temp;
        temp = cards[posNo + 1];
        cards[posNo + 1] = cards[posNo];
        for (int i = posNo + 2; i < numSetMax - 1; i++)
        {
            CardData temp2 = cards[i];
            cards[i] = temp;
            temp = temp2;
        }
        
        cards[posNo].obj = Instantiate(card.obj);
        cards[posNo].obj.transform.parent = transform;
        cards[posNo].obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        //cards[posNo].obj.transform.localScale = card.obj.transform.localScale;
        cards[posNo].type = card.type;
        numSet++;
        return true;
    }

    void Scroll()
    {
        if (Input.GetMouseButtonDown(0) && mouse_system.Collider(gameObject)) canScroll = true;
        else if (Input.GetMouseButtonUp(0)) canScroll = false;

        if (canScroll)
        {
            scroolScript.isUpdate = false;
        }
        else
        {
            scroolScript.isUpdate = true;
        }

        if (Input.GetMouseButton(0) && canScroll)
        {
            //int num = (int)((mouse_system.GetDragVec().x - dragPos) / cardSize.x);
            //if (num != 0)
            //{
            //    dragPos = mouse_system.GetDragVec().x;
            //    num %= 2;
            //    if (num > 0)
            //    {
            //        for (int i = 0; i < num; i++)
            //        {
            //            if (CheckLeftEnd()) break;
            //            ScrollToRight();
            //        }
            //    }
            //    else if (num < 0)
            //    {
            //        for (int i = 0; i < -num; i++)
            //        {
            //            if (CheckRightEnd()) break;
            //            ScrollToLeft();
            //        }
            //    }
            //}
            int num = (int)(MouseSystem.GetFlickDistance().x / cardSize.x);
            if (num != 0)
            {
                dragPos = mouse_system.GetDragVec().x;
                num %= 2;
                if (num > 0)
                {
                    if (!CheckRightEnd())
                    {
                        ScrollToLeft();
                    }
                }
                else if (num < 0)
                {
                        if (!CheckLeftEnd())
                    {
                        ScrollToRight();
                    }
                }
            }

        }
    }

    public void ScrollToLeft()
    {
        //// カードの座標設定
        //for (int i = 0; i < numSetMax; i++)
        //{
        //    //セットカードの枠を超えたら
        //    if (cards[i].obj.transform.localPosition.x >= 0.5f)
        //    {
        //        //フラグを立てる
        //        exceedFlag = true;
        //    }

        //    //枠を超えたら
        //    if (exceedFlag == true)
        //    {
                // カードの座標設定
                for (int j = 0; j < numSetMax; j++)
                {
                    if (cards[j].obj == null) continue;

                    //左スクロール
                    cards[j].obj.transform.localPosition -= new Vector3(cardSize.x, 0, 0);
                }
    //        }
                scrollStep -= cardSize.x;
    //    }
    //}

}

public void ScrollToRight()
    {
        //// カードの座標設定
        //for (int i = 0; i < numSetMax; i++)
        //{
        //    //セットカードの枠を超えたら
        //    if (cards[i].obj.transform.localPosition.x >= 0.5f)
        //    {
        //        //フラグを立てる
        //        exceedFlag = true;
        //    }

        //    //枠を超えたら
        //    if (exceedFlag == true)
        //    {
        //        // カードの座標設定
        for (int j = 0; j < numSetMax; j++)
        {
            if (cards[j].obj == null) continue;
            //            if (Input.GetAxis("CardScroll") > 0)
            //            {
            //右スクロール
            cards[j].obj.transform.localPosition += new Vector3(cardSize.x, 0, 0);
            //            }
            //            else if (Input.GetAxis("CardScroll") < 0)
        }
        scrollStep += cardSize.x;
        //    }
        //}
    }

    // 使用中カードの取得
    public CardManagement.CardType GetCardType(int no = -1)
    {
        if (no == -1)
            return cards[usingCard].type;
        else
            return cards[no].type;
    }
}
