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

    //実行フラグ
    bool PlayFlag = false;

    //ゲームの状態
    GameManager state;

    // Use this for initialization
    void Start ()
    {
        cardSize = new Vector2(0.8f, 1.0f);
        centerCard = 0;
        selectedSpace = 0;
        //numSet = 0;
        usingCard = 0;
        exceedFlag = false;

        // MouseSystemコンポーネントの取得
        mouse_system = GameObject.Find("MouseSystem").GetComponent<MouseSystem>();
        state = GameObject.Find("GameManager").GetComponent<GameManager>();

        Coordinate();

        //プレイフラグ
        PlayFlag = false;
    }

    //カードの初期座標取得関数
    void Coordinate()
    {

        centerCard = usingCard;
        //カードの座標設定
        for (int i = 0; i < numSetMax; i++)
        {
            if (cards[i].obj == null) break;
            ////カード選択時の座標変更
            //if ((selectedSpace == i) && selectedSpace >= 0)
            //{
            //    cards[i].obj.transform.localPosition = new Vector3((i - centerCard) * cardSize.x, 0, -3f) / transform.localPosition.x;
            //}
            //else
            //{
            cards[i].obj.transform.localPosition = new Vector3((i - centerCard) * cardSize.x - 4.0f, 0.0f, zPos) / transform.localScale.x;
            //}
        }
    }

    // Update is called once per frame
    void Update ()
    {
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

                            if (Input.GetAxis("CardScroll") > 0)
                            {
                                //右スクロール
                                cards[j].obj.transform.localPosition += new Vector3(0.005f, 0, 0);
                            }
                            else if (Input.GetAxis("CardScroll") < 0)
                            {
                                //左スクロール
                                cards[j].obj.transform.localPosition -= new Vector3(0.005f, 0, 0);
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

        // 使用済みカードの非表示化
        if (true)
        {
            if (usingCard <= numSetMax && usingCard > 0)
            {
                if (cards[usingCard].obj && cards[usingCard - 1].obj)
                {
                    cards[usingCard - 1].obj.SetActive(false);
                    cards[usingCard].obj.SetActive(true);

                }
            }
        }
	}

    // ボードにカードをセットする
    public bool SetCard(GameObject obj, CardManagement.CardType type)
    {
        cards[numSet].obj = obj;
        cards[numSet].obj.transform.parent = transform;
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
        cards[posNo].obj.transform.localScale = card.obj.transform.localScale;
        cards[posNo].type = card.type;
        numSet++;
        return true;
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
