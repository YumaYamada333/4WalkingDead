using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

    //カウントのタイプ
    public enum CountType
    {
        Nothing,                // 値なし
        ActionMove,             // MoveCardが実行されたとき
        ActionCountDown,        // CountDownCardが実行されたとき
        CardSet,                // ボードにカードをはさんだとき
        TypeNum,                // カウントのタイプ数
    }

    [SerializeField]
    // このUIのカウントダウン条件
    private CountType[] m_countType = new CountType[1];

    // カウントダウンフラグ
    private static CountType m_countDownFlag;

    // カウント
    private int count;
    //初期カウント数
    private int init_count;

    // カウントコンポーネント
    private Text T_count;

    // Use this for initialization
    void Start()
    {
        // カウント表示用Textコンポーネントを取得
        T_count = GetComponent<Text>();

        // カウント数を取得
        count = int.Parse(T_count.text);
        init_count = count;

        SetUIColor();
    }

    // Update is called once per frame
    void Update ()
    {
        // カウントダウンの条件
        if (IsCountDown()&& count > 0)
            count--;

        // カウントの表示
        if (T_count != null)
        T_count.text = count.ToString();
    }

    // カウント状況の取得
    public int GetCount()
    {
        return count;
    }

    // カウントの設定(無入力or0を入力すると初期値に変更)
    public void SetCount(int num = 0)
    {
        if (num != 0)
        { count = num; }
        else
        { count = init_count; }
    }

    // カウントダウンフラグの取得
    private bool IsCountDown()
    {
        for (int i = 0; i < m_countType.Length; i++)
            if (m_countType[i] == m_countDownFlag &&
                m_countType[i] != CountType.Nothing)
                return true;

        return false;
    }

    // テキストの色変更
    private void SetUIColor()
    {
        Color color = Color.clear;
        T_count.color = color;
        for (int i = 0; i < m_countType.Length; i++)
        {
            switch (m_countType[i])
            {
                case CountType.ActionMove:
                    color = Color.red;
                    break;
                case CountType.ActionCountDown:
                    color = Color.blue;
                    break;
                case CountType.CardSet:
                    color = Color.yellow;
                    break;
                default:
                    color = Color.clear;
                    break;
            }
            T_count.color += color;
        }
    }

    // カウントダウンの設定
    static public void SetCountDown(CardManagement.CardType type)
    {
        switch (type)
        {
            case CardManagement.CardType.Start:
                break;
            case CardManagement.CardType.Move:
                CountDown.SetCountDown(CountDown.CountType.ActionMove);
                break;
            case CardManagement.CardType.Jump:
                break;
            case CardManagement.CardType.Attack:
                break;
            case CardManagement.CardType.Finish:
                break;
            case CardManagement.CardType.Nothing:
                break;
            case CardManagement.CardType.NumType:
                break;
            default:
                break;
        }
    }

    // カウントダウンの設定
    static public void SetCountDown(CountType type)
    {
        m_countDownFlag = type;
    }

    // カウントダウンの設定
    static public CountType GetCountDown()
    {
        return m_countDownFlag;
    }
}
