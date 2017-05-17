using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Button : MonoBehaviour {
    // カメラ
    public GameObject mainCamera;
    // ボード  
    private GameObject ActionBord;
    private GameObject HandsBord;
    //private GameObject RetuneButton;
    //private GameObject ResetButton;
    // ボタン
    private GameObject SpeedButton;
    private GameObject ExecutionButton;
    //リセットボタン
    private GameObject ResetButton;

    [SerializeField]
    private GameObject ScrollLeftButton;
    [SerializeField]
    private GameObject ScrollRifhtButton;

    private bool flag = true;
    //private float BordPosition_x;
    //private float BordPosition_y;
    //private float BordPosition_z;
    //private float SetPosition_x;
    //private float SetPosition_y;
    //private float SetPosition_z;
    //private bool position_Flag;

    // handsboradの初期位置
    private Vector3 firstPos = Vector3.zero;

    // セット時　アクション時のカードboard位置
    [SerializeField]
    private Vector3 setPosActionBord = Vector3.zero;
    [SerializeField]
    private Vector3 setPosHandsBord = Vector3.zero;
    [SerializeField]
    private Vector3 actPosActionBord = Vector3.zero;

    // Use this for initialization
    void Awake ()
    {
        ActionBord = GameObject.Find("ActionBord");
        HandsBord = GameObject.Find("HandsBord");
        //RetuneButton = GameObject.Find("RetuneButton");
        //ResetButton = GameObject.Find("ResetButton");
        SpeedButton = GameObject.Find("SpeedButton");
        ExecutionButton = GameObject.Find("PlayButton");
        //リセットボタン
        ResetButton = GameObject.Find("Reset");

        //SetPosition_x = 11.0f;
        //SetPosition_y = 2.0f;
        //SetPosition_z = 1.2f;
        //BordPosition_x = 2.0f;
        //BordPosition_y = -2.5f;
        //BordPosition_z = 1.0f;
        //position_Flag = false;
        //if (mainCamera.activeSelf)
        //{
        //    RetuneButton.SetActive(false);
        //    ResetButton.SetActive(false);
        //    SpeedButton.SetActive(false);
        //}

        // actionboardの配置
        ActionBord.transform.localPosition = actPosActionBord;

        // 初期位置取得
        firstPos = mainCamera.transform.position;

        HandsBord.SetActive(false);
    }

    // Update is called once per frame
    void Update ()
    {
        // actionboardの位置更新
        if (flag)
        {
            ActionBord.transform.localPosition = actPosActionBord/* + mainCamera.transform.position - firstPos*/;
        }
        CardBord board = ActionBord.GetComponent<CardBord>();

        if (board.CheckLeftEnd()) ScrollRifhtButton.SetActive(false);
        else ScrollRifhtButton.SetActive(true);

        if (board.CheckRightEnd()) ScrollLeftButton.SetActive(false);
        else ScrollLeftButton.SetActive(true);

    }

    public void OnClick()
    {

        

        if (flag)
        {
            // カメラの切り替え　ボタンの配置
            flag = false;
            //position_Flag = true;
            SpeedButton.SetActive(false);
            ExecutionButton.SetActive(false);
            //リセットボタン
            ResetButton.SetActive(true);

            //RetuneButton.SetActive(true);
            //ResetButton.SetActive(true);

            // boardの配置
            HandsBord.transform.localPosition = setPosHandsBord;
            ActionBord.transform.localPosition = setPosActionBord;
            // HandsBordを表示
            HandsBord.SetActive(true);
        }
        else
        {
            flag = true;
            // カメラの切り替え　ボタンの配置
            //position_Flag = false;
            SpeedButton.SetActive(true);
            ExecutionButton.SetActive(true);
            //リセットボタン
            ResetButton.SetActive(false);

            //RetuneButton.SetActive(false);
            //ResetButton.SetActive(false);

            //actionbordの配置
            ActionBord.transform.localPosition = actPosActionBord;

            // HandsBordを非表示
            HandsBord.SetActive(false);
        }
    }
}
