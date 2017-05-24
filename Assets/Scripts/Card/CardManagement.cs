using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardManagement : MonoBehaviour {

    /* 説明
    カード処理系の管理クラス
    処理はここで一括管理する
    */

    // カードを乗せるボードの取得
    public GameObject handsBord;
    Vector2 handsBordSize;
    public GameObject actionBord;
    Vector2 actionBordSize;

    // カード
    public GameObject startCard;
    public GameObject moveCard;
    public GameObject jumpCard;
    public GameObject attackCard;
    public GameObject countCard;
    //public GameObject superMoveCard;
    //public GameObject superJumpCard;
    //public GameObject superAttackCard;
    public GameObject finishCard;
    Vector2 cardSize;

    // オリジナルの数字UI
    public GameObject originalnumUI;

    // 所持カードの種類数
    int numCardSet;

    // カード最大の所持数
    public const int numMax = 99;  

    // カードの種類
    public enum CardType
    {
        Start,
        Move,
        Jump,
        Attack,
        Count,
        //SuperMove,
        //SuperJump,
        //SuperAttack,
        Finish,
        Nothing,
        NumType
    }

    // カードの種類数配列を確保
    public struct CardData
    {
        // 前のカード 後ろのカード
        public CardBord.CardData front;
        // 残り枚数の表示
        public GameObject numUI;
        // 所持数
        public int numHold;
    }
    public CardData[] cards = new CardData[numMax];

    // 最初のカードの配置位置
    Vector2 firstPos;
    // 現在の配置数
    int numSetting;
    // 配置時の追加空間
    public float cardSpace;
    // カード配置の間隔(m)
    float posInterval;
    // 選択中のカード
    int selectedCard;
    // 挟むカードデータを保持
    public CardData tuckCard;

    // カーソルのフォーカス
    public enum CursorForcusTag
    {
        HandsBord,
        ActtionBord
    }
    CursorForcusTag cursor;

    // 更新フラグ
    public bool isUpdateData;

    // カード操作を有効にする
    public bool isControlCard;

    // マウスのコンポーネント
    MouseSystem mouse_system;

    // Rayに触れたオブジェクト
    RaycastHit[] hit;

    //カード掴み判定フラグ
    bool gripFlag;

    //初期所持カード判断ステージ番号
    //public int stageNum;

    // 初期所持カード
    [SerializeField]
    int attackNum;
    [SerializeField]
    int jumpNum;
    [SerializeField]
    int countNum;

    private GameObject m_boardButton;
    private GameObject[] card = null;
    private int m_oldSelectCard;

    // Use this for initialization
    void Start () {
        isUpdateData = true;
        isControlCard = true;

        //// boardサイズの設定
        //handsBord.transform.localScale = new Vector3(10, 1.5f, 1);
        //actionBord.transform.localScale = new Vector3(10, 1.5f, 1);

        // サイズの取得(m)
        handsBordSize = handsBord.GetComponent<RectTransform>().sizeDelta;
        actionBordSize = actionBord.GetComponent<RectTransform>().sizeDelta;
        cardSize = attackCard.GetComponent<RectTransform>().sizeDelta;

        firstPos = new Vector2(cardSize.x / 2 + cardSpace - handsBordSize.x / 2, 0.0f);

        numCardSet = 0;
        numSetting = 0;
        selectedCard = 0;
        //stageNum = 1;

        cursor = CursorForcusTag.HandsBord;

        //所持カードの設定
        //switch (stageNum)
        //{
        //    case 1:
        //        // 仮所持カード
        //        SetCard(CardType.Attack, CardType.Attack);
        //        SetCard(CardType.Attack, CardType.Attack);
        //        SetCard(CardType.Attack, CardType.Attack);
        //        SetCard(CardType.Attack, CardType.Attack);
        //        SetCard(CardType.Jump, CardType.Jump);
        //        SetCard(CardType.Jump, CardType.Jump);
        //        SetCard(CardType.Jump, CardType.Jump);
        //        break;
        //}

        for (int i = 0; i < attackNum; i++)
        {
            SetCard(CardType.Attack, CardType.Attack);
        }
        for (int i =0; i < jumpNum; i++)
        {
            SetCard(CardType.Jump, CardType.Jump);
        }
        for (int i = 0; i < countNum; i++)
        {
            SetCard(CardType.Count, CardType.Count);
        }


        //// 仮所持カード
        //SetCard(CardType.Attack, CardType.Attack, 10);
        //SetCard(CardType.Move, CardType.Move);
        //SetCard(CardType.Jump, CardType.Jump);
        //SetCard(CardType.Jump, CardType.Jump);


        // ステージの仮のmoveカード配置
        CardBord bord = actionBord.GetComponent<CardBord>();
        bord.SetCard(Instantiate(startCard), CardType.Start);
        bord.SetCard(Instantiate(moveCard), CardType.Move);
        bord.SetCard(Instantiate(moveCard), CardType.Move);
        bord.SetCard(Instantiate(moveCard), CardType.Move);
        bord.SetCard(Instantiate(moveCard), CardType.Move);
        bord.SetCard(Instantiate(finishCard), CardType.Finish);

        // MouseSystemコンポーネントの取得
        mouse_system = GameObject.Find("MouseSystem").GetComponent<MouseSystem>();
        m_boardButton = GameObject.Find("BoardButton");

        gripFlag = false;
    }

    // Update is called once per frame
    void Update () {
        //つかむ判定を初期地に戻す
        gripFlag = false;

        // データの更新
        if (isUpdateData) UpdateData();

        if (!GameObject.Find("GameManager").GetComponent<GameManager>().GetGimmickFlag())
            // カード操作
            if (isControlCard) ControlCard();

        // 所持カードの更新
        for (int i = 0; i < numCardSet; i++)
        {
            // オブジェクトの存在しないカードの生成
            if (cards[i].numHold > 0 && cards[i].front.obj == null)
            {
                CreateCards(ref cards[i].front);
                // 残り枚数のUIの生成
                cards[i].numUI = Instantiate(originalnumUI);
                cards[i].numUI.transform.parent = handsBord.transform;
                
            }

            // 枚数0所持カードの破棄
            if (cards[i].numHold == 0 && cards[i].front.obj != null)
            {
                DestroyCards(ref cards[i].front);
                numCardSet--;
                for (int j = i; j < numCardSet; j++)
                {
                    cards[j] = cards[j + 1];
                }
                // 残り枚数のUIの破棄
                Destroy(cards[i].numUI);
            }

            // 残り枚数のUIの更新
            //cards[i].numUI.GetComponent<TextMesh>().text = cards[i].numHold.ToString();

            if (cursor == CursorForcusTag.HandsBord && cards[i].front.obj != null)
            {
                // カードの配置
                SetCardPosition(ref cards[i]);
            }

            // UIの配置
            //cards[i].numUI.transform.position = cards[i].back.obj.transform.position;
        }
        numSetting = 0;
    }

    // 設定する際に使用するデータの更新
    void UpdateData()
    {
        // 所持カードの所持数
        int numTypeHold = 0;
        for (int i = 0; i< numCardSet; i++)
        {
            if (cards[i].numHold > 0) numTypeHold++;
        };

        // カード間の距離
        if (cardSize.x * numTypeHold + cardSpace < handsBordSize.x)
            posInterval = cardSize.x + cardSpace;
        else
            posInterval = handsBordSize.x / numTypeHold;

        //isUpdateData = false;
        if (CountDown.GetCountDown() != CountDown.CountType.Nothing)
            CountDown.SetCountDown(CountDown.CountType.Nothing);
    }

    // カード操作
    void ControlCard()
    {
        // はさむカード選択中 ===============================================================
        if (cursor == CursorForcusTag.HandsBord )
        {
            // 左クリックした座標にあるカードをつかむ
            if (Input.GetMouseButton(0))
            {
                // マウスの座標上にあるカードを取得
                selectedCard = mouse_system.GetMouseHit(handsBord);
                if (selectedCard >= 0)
                {
                    //つかむ判定を立てる
                    gripFlag = true;

                    tuckCard = cards[selectedCard];
                    cursor = CursorForcusTag.ActtionBord;
                    actionBord.GetComponent<CardBord>().selectedSpace = -1;
                }
            }
        }
        // はさむ場所を選択中 ===============================================================
        else
        {
            // ActionBoardの情報を取得
            CardBord bord = actionBord.GetComponent<CardBord>();

            // 左クリックしてる
            if (Input.GetMouseButton(0))
            {
                //つかむ判定を立てる
                gripFlag = true;

                // カードを移動
                tuckCard.front.obj.transform.position = mouse_system.GetScreenPos();

                CloneMove();

            }
            // してない
            else
            {
                cursor = CursorForcusTag.HandsBord;

                CardActive(true);

                bord.selectedSpace = mouse_system.GetMouseHit(actionBord);
                if (bord.selectedSpace >= 0)
                {
                    CountDown.SetCountDown(CountDown.CountType.CardSet);

                    // 挟んだカードが同タイプ
                    if (bord.GetCardType(bord.selectedSpace) == tuckCard.front.type)
                    {
                        // カードの効果を変える
                        // CardBord.CardData newCard;
                        //newCard.type = DecideTuckCard(tuckCard.front.type, tuckCard.back.type);
                        //newCard.obj = null;
                        //CreateCards(ref newCard);
                        //if (newCard.obj != null)
                        {
                            //// 挟まれたカードの削除
                            //bord.DeleteCard(bord.selectedSpace);
                            //// 上記の位置に新しいカード
                            //bord.TuckCard(newCard, bord.selectedSpace);
                        }
                        //else
                        {
                            bord.TuckCard(tuckCard.front, bord.selectedSpace);
                        }
                        //Destroy(newCard.obj);
                    }
                    else
                    {
                        bord.TuckCard(tuckCard.front, bord.selectedSpace);
                    }
                    // セットしたカード枚数を減らす
                    cards[selectedCard].numHold--;
                    bord.Coordinate();
                }
                CleneDelete();

            }

            if (bord.usingCard > bord.selectedSpace)
            {
                bord.selectedSpace = bord.usingCard;
            }
            else if (bord.numSet - 1 <= bord.selectedSpace)
            {
                bord.selectedSpace = bord.numSet - 1;
            }
        }
    }

    // 存在しないカードの生成
    void CreateCards(ref CardBord.CardData card)
    {
        if (card.obj == null)
        {
            switch (card.type)
            {
                case CardType.Start:
                    card.obj = Instantiate(startCard);
                    break;
                case CardType.Move:
                    card.obj = Instantiate(moveCard);
                    break;
                case CardType.Jump:
                    card.obj = Instantiate(jumpCard);
                    break;
                case CardType.Attack:
                    card.obj = Instantiate(attackCard);
                    break;
                case CardType.Count:
                    card.obj = Instantiate(countCard);
                    break;

                //case CardType.SuperMove:
                //    card.obj = Instantiate(superMoveCard);
                //    break;
                //case CardType.SuperJump:
                //    card.obj = Instantiate(superJumpCard);
                //    break;
                //case CardType.SuperAttack:
                //    card.obj = Instantiate(superAttackCard);
                //    break;
                case CardType.Finish:
                    card.obj = Instantiate(finishCard);
                    break;
            }
            card.obj.transform.parent = handsBord.transform;
            card.obj.transform.localScale = new Vector3(1.0f, 1.0f, 1.0f);
        }
    }

    // カード破棄
    void DestroyCards(ref CardBord.CardData card)
    {
        Destroy(card.obj);
        card.obj = null;
    }

    // 存在する所持カードの配置
    void SetCardPosition(ref CardData card)
    {
        if (card.front.obj == null) return;
        const float zPos = -0.1f;
        // numSetting番目の位置に配置
        card.front.obj.transform.localPosition 
            = new Vector3(firstPos.x + numSetting * posInterval, firstPos.y, zPos);
        if (numSetting == selectedCard && cursor == CursorForcusTag.HandsBord)
            card.front.obj.transform.localPosition += new Vector3(0, 0, -0.1f);
        numSetting++;
    }

    // 所持カードに加える
    void SetCard(CardType front, CardType back, int num = 1)
    {
        cards[numCardSet].front.type = front;
        cards[numCardSet].numHold += num;
        numCardSet++;
    }

    // 挟んだ後のカードを決める
    CardType DecideTuckCard(CardType type, CardType type1)
    {
        CardType result;
        // 桁をずらす
        const int back = 10;
        switch ((int)type + (int)type1 * back)
        {
            //// 仮
            //case (int)CardType.Move + (int)CardType.Move * back:
            //    result = CardType.SuperMove;
            //    break;
            //case (int)CardType.Jump + (int)CardType.Jump * back:
            //    result = CardType.SuperJump;
            //    break;
            //case (int)CardType.Attack + (int)CardType.Attack * back:
            //    result = CardType.SuperAttack;
            //    break;
            default:
                result = CardType.Nothing;
                break;
        }
        return result;
    }


    // カードを進める
    public CardType ActtionCard(bool isReset)
    {
        CardBord bord = actionBord.GetComponent<CardBord>();
        if (!isReset)
        {
            CardType type = bord.GetCardType();
            if (bord.usingCard < bord.numSet)
                bord.stepUsing++;
            else
                type = CardType.Nothing;

            //if (type == CardType.Move)
            //{
            //    //countDownFlag = true;
            //}

            return type;
        }
        else
        {
            bord.usingCard = bord.stepUsing = 0;
            return bord.GetCardType();
        }
    }

    public void ApllyUsingCard()
    {
        CardBord bord = actionBord.GetComponent<CardBord>();
        bord.usingCard = bord.stepUsing;
    }

    //  カードの情報関係 /////////////////////////////////////////////////////////////////////////////////

    // ボード情報を初期状態に戻す
    public void ReturnBoard(ResetScript.INITDATA data)
    {
        // ActionBoardの情報を取得
        CardBord bord = actionBord.GetComponent<CardBord>();

        // カードの削除
        for (int i = 0; i < bord.cards.Length; i++)
        {
            if (bord.cards[i].obj != null)
                Destroy(bord.cards[i].obj);
        }

        // カードの再配置
        bord.numSet = 0;
        for (int i = 0; i < bord.cards.Length; i++)
        {
            switch (data.setCard[i].type)
            {
                case CardType.Start:
                    bord.SetCard(Instantiate(startCard), data.setCard[i].type);
                    break;
                case CardType.Move:
                    bord.SetCard(Instantiate(moveCard), data.setCard[i].type);
                    break;
                case CardType.Jump:
                    bord.SetCard(Instantiate(jumpCard), data.setCard[i].type);
                    break;
                case CardType.Attack:
                    bord.SetCard(Instantiate(attackCard), data.setCard[i].type);
                    break;
                case CardType.Count:
                    bord.SetCard(Instantiate(countCard), data.setCard[i].type);
                    break;

                case CardType.Finish:
                    bord.SetCard(Instantiate(finishCard), data.setCard[i].type);
                    break;
                default:
                    break;
            }
        }
        bord.Coordinate();

        //// カード所持数を戻す
        //for (int i = 0; i < cards.Length; i++)
        //    cards[i].numHold = data.cardNum[i];
    }

    //つかみ判定取得関数
    public bool GetGripFlag()
    {
        return gripFlag;
    }


    void CloneCreate()
    {
        // ActionBoardの情報を取得
        CardBord bord = actionBord.GetComponent<CardBord>();

        // カードのコピー
        if (card == null)
        {
            card = new GameObject[6];
            for (int i = 0; i < card.Length; i++)
            {
                if (bord.cards[i].obj.activeSelf)
                {
                    card[i] = Instantiate(bord.cards[i].obj);
                    card[i].transform.parent = GameObject.Find("ActionBord").transform;
                    card[i].transform.localPosition = bord.cards[i].obj.transform.localPosition;
                    card[i].transform.localScale = bord.cards[i].obj.transform.localScale;
                }
            }
        }
    }

    void CloneMove()
    {
        // ActionBoardの情報を取得
        CardBord bord = actionBord.GetComponent<CardBord>();
        int selectCard = mouse_system.GetMouseHit(bord.cards);

        // 1フレーム前のはさむ場所と違う場合
        if (selectCard != m_oldSelectCard)
            CleneDelete();

        // はさむ範囲内
        if (selectCard > 0 && selectCard < 6)
        {
            // カードを複製
            CloneCreate();
            // オリジナルカードを非表示
            CardActive(false);
            // コピーカードの移動
            if (card[0].transform.localPosition.x >= bord.cards[0].obj.transform.localPosition.x - cardSize.x / 2)
            {
                for (int i = 0; i < selectCard; i++)
                    card[i].transform.localPosition += new Vector3(-5, 0, 0);

                for (int i = selectCard; i < card.Length; i++)
                    card[i].transform.localPosition += new Vector3(5, 0, 0);
            }
        }
        else
        {
            CleneDelete();
        }
        m_oldSelectCard = selectCard;
    }

    void CleneDelete()
    {
        // ActionBoardの情報を取得
        CardBord bord = actionBord.GetComponent<CardBord>();
        if (card != null)
        {
            // コピーカードの削除
            for (int i = 0; i < card.Length; i++)
            {
                Destroy(card[i]);
            }
            card = null;
            // オリジナルカードを元に戻す
            CardActive(true);
        }
    }

    void CardActive(bool active)
    {
        CardBord bord = actionBord.GetComponent<CardBord>();

        if (active)
        {
            for (int i = bord.usingCard; i < CardBord.numSetMax; i++)
            {
                if (bord.cards[i].obj == null) continue;
                if (bord.cards[i].obj.transform.localPosition.x >=
                    transform.localPosition.x + bord.GetComponent<RectTransform>().sizeDelta.x / 2 ||
                    bord.cards[i].obj.transform.localPosition.x <=
                    transform.localPosition.x - bord.GetComponent<RectTransform>().sizeDelta.x / 2)
                {
                    bord.cards[i].obj.SetActive(false);
                }
                else
                {
                    bord.cards[i].obj.SetActive(true);
                }
            }
        }
        else for (int i = 0; i < bord.numSet; i++)
                bord.cards[i].obj.SetActive(active);
           
        m_boardButton.SetActive(active);
    }
}
