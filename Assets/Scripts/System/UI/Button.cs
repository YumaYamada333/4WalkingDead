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


    MathClass lerp;

    private GameObject imagebord;

    private GameObject imagebord2;

    private GameObject imagebord3;

    private GameObject imagebord4;

    private bool Move_flag;

    private float m_autoMoveTime = 0.0f;

    float timeStep;

    float LerpMovement_x;
    float LerpMovement_y1;
    float LerpMovement_y2;
    float LerpMovement_y3;


    // Use this for initialization
    void Awake ()
    {
        ActionBord = GameObject.Find("ActionBord");
        HandsBord = GameObject.Find("HandsBord");
        SpeedButton = GameObject.Find("SpeedButton");
        ExecutionButton = GameObject.Find("PlayButton");
        //リセットボタン
        ResetButton = GameObject.Find("Reset");

        //上イメージボード
        imagebord = GameObject.Find("Imagebord");
        //下イメージボード
        imagebord2 = GameObject.Find("Imagebord2");

        //右イメージボード
        imagebord3 = GameObject.Find("Imagebord3");

        //左イメージボード
        imagebord4 = GameObject.Find("Imagebord4");


        //移動フラグ
        Move_flag = true;

        
        m_autoMoveTime = Time.time+100;
        
        timeStep = 1;
        
        // actionboardの配置
        ActionBord.transform.localPosition = actPosActionBord;

        // 初期位置取得
        firstPos = mainCamera.transform.position;

        HandsBord.SetActive(false);

        /*初期化*/
        LerpMovement_x = 350.0f;
        LerpMovement_y1 = 170.0f;
        LerpMovement_y2 = 188.0f;
        LerpMovement_y3 = 300.0f;
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

        timeStep = (Time.time - m_autoMoveTime) /*/ 10.0f*/;
        if (timeStep > 1.0f) timeStep = 1.0f;

        MoveInLerp();
        
    }

    public void OnClick()
    {

        //timeStep = (Time.time - m_autoMoveTime);
        m_autoMoveTime = Time.time;
        


        if (flag)
        {
            // カメラの切り替え　ボタンの配置
            flag = false;
            //position_Flag = true;
            //SpeedButton.SetActive(false);
            //ExecutionButton.SetActive(false);
            //リセットボタン
            //ResetButton.SetActive(true);

            //RetuneButton.SetActive(true);
            

            // boardの配置
            HandsBord.transform.localPosition = setPosHandsBord;
            ActionBord.transform.localPosition = setPosActionBord;
            // HandsBordを表示
            HandsBord.SetActive(true);

            Move_flag = true;
        }
        else
        {
            flag = true;
            // カメラの切り替え　ボタンの配置
            //position_Flag = false;
            //SpeedButton.SetActive(true);
            //ExecutionButton.SetActive(true);
            //リセットボタン
            //ResetButton.SetActive(false);

            //RetuneButton.SetActive(false);
            //ResetButton.SetActive(false);

            //actionbordの配置
            ActionBord.transform.localPosition = actPosActionBord;

            // HandsBordを非表示
            //HandsBord.SetActive(false);

            Move_flag = false;
        }
    }


    //----------------------------------------------------------------------
    //! @brief ボタンとボードのLerp処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void MoveInLerp()
    {
        if (Move_flag)
        {
            HandsBord.transform.localPosition =
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y3, 0),
                new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


            ResetButton.transform.localPosition = 
                MathClass.Lerp(new Vector3(LerpMovement_x, -LerpMovement_y3, 0), 
                new Vector3(350.0f, -LerpMovement_y2, 0), timeStep);


            ActionBord.transform.localPosition = 
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                new Vector3(0.0f, LerpMovement_y1, 0), timeStep);


            imagebord.transform.localPosition =
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                new Vector3(0.0f, LerpMovement_y1, 0), timeStep);


            imagebord2.transform.localPosition =
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y3, 0), 
                new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


           


        }
        else
        {
            HandsBord.transform.localPosition = 
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                new Vector3(0.0f, -LerpMovement_y3, 0), timeStep);


            ResetButton.transform.localPosition =
                MathClass.Lerp(new Vector3(LerpMovement_x, -LerpMovement_y2, 0),
                new Vector3(LerpMovement_x, -LerpMovement_y3, 0), timeStep);


            ActionBord.transform.localPosition = 
                MathClass.Lerp(new Vector3(0.0f, LerpMovement_y1, 0),
                new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


            imagebord.transform.localPosition = 
                MathClass.Lerp(new Vector3(0.0f, LerpMovement_y1, 0), 
                new Vector3(0.0f, -LerpMovement_y2, 0), timeStep);


            imagebord2.transform.localPosition = 
                MathClass.Lerp(new Vector3(0.0f, -LerpMovement_y2, 0),
                new Vector3(0.0f, -LerpMovement_y3, 0), timeStep);
        }
    }
}
