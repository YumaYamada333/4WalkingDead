using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {

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
    }

    // Update is called once per frame
    void Update ()
    {
        // カードをはさんだらカウントダウンさせる
        if (GameObject.Find("CardManager").GetComponent<CardManagement>().GetCountDownFlag() 
            && count > 0)
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
}
