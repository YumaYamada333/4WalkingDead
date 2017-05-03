using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountDown : MonoBehaviour {
    
    // カウント
    [SerializeField]
    private int count;

    // 基盤カウントUI
    [SerializeField]
    private GameObject countUI;

    // カウントUIオブジェクト
    private GameObject countObject;

    // カウントコンポーネント
    private Text T_count;

    // Use this for initialization
    void Start ()
    {
        // カウントUIをステージブロックに追加
        countObject = Instantiate(countUI);
        countObject.transform.parent = transform;
        // 位置調整
        countObject.transform.localPosition = new Vector3(0, 0, -0.5f);

        // カウント表示用Textコンポーネントを取得
        T_count = countObject.GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update ()
    {
        // カードをはさんだらカウントダウンさせる
        if (GameObject.Find("CardManager").GetComponent<CardManagement>().GetCountDownFlag() 
            && count > 0)
            count--;

        // カウントの表示
        T_count.text = count.ToString();

        // カウント0になったら
        if (count <= 0)
            Debug.Log(gameObject.ToString() + "のカウントが0になった");
    }

    // カウント状況の取得
    public int GetCount()
    {
        return count;
    }
}
