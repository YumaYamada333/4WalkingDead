//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   CameraSwipe
//!
//! @brief  カメラの移動
//!
//! @date   2017/05/18
//!
//! @author Y.Okada
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraSwipe : MonoBehaviour
{

    /*タッチのスタート座標*/
    private Vector3 touchStartPos;
    /*タッチのエンド座標*/
    private Vector3 touchEndPos;

    /*スワイプ処理判断用の名前*/
    private string Direction;

    /*スワイプ処理判断用*/
    private float moveUp_y;
    private float moveDown_y;

    /*カメラコントロールのスクリプト参照用*/
    CameraControl cameraControl;


    /*スタート時のカメラの座標*/
    Vector3 CameraPos;

    /*毎フレーム時のカメラの座標*/
    Vector3 CameraTmp;

    // Use this for initialization
    void Start ()
    {
        /*スクリプト参照*/
        cameraControl = gameObject.GetComponent<CameraControl>();

        /*カメラの座標代入*/
        CameraPos = GameObject.Find("Main Camera").transform.position;

        moveUp_y = -220.0f;
        moveDown_y = -240.0f;

    }

    // Update is called once per frame
    void Update()
    {

        
        /*カメラの座標代入*/
        CameraTmp = GameObject.Find("Main Camera").transform.position;

        /*スワイプ処理*/
        Flick();

    }

    //----------------------------------------------------------------------
    //! @brief フリック処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------

    void Flick()
    {
        /*マウスが押されていたら*/
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            /*マウスの座標代入*/
            touchStartPos = new Vector3(Input.mousePosition.x,
                                        Input.mousePosition.y,
                                        Input.mousePosition.z);
        }

        /*マウスが離されたら*/
        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            /*マウスの座標代入*/
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);
            /*関数呼び出し*/
            GetDirection();
        }
    }


    //----------------------------------------------------------------------
    //! @brief スワイプ判断と処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void GetDirection()
    {
        float directionX = touchEndPos.x - touchStartPos.x;
        float directionY = touchEndPos.y - touchStartPos.y;

        if (Mathf.Abs(directionX)<Mathf.Abs(directionY))
        {
            
                if (30 < directionY)
                {
                    //上向きにフリック
                    Direction = "up";
                }
                else if (-30 > directionY)
                {
                    //下向きのフリック
                    Direction = "down";
                }
            
        }
        else
        {
                //タッチを検出
                Direction = "touch";
        }

        switch (Direction)
        {
            case "up":
                //上フリックされた時の処理

                /*カメラ移動*/
                cameraControl.Zoom(new Vector3(2, -230, 0),1.0f);

                if (CameraTmp.y <= moveUp_y)
                {
                    /*カメラ移動*/
                    cameraControl.Zoom(new Vector3(2, -400, 0), 1.0f);
                }

                break;

            case "down":
                //下フリックされた時の処理

                /*カメラ移動*/
                cameraControl.Zoom(new Vector3(2, -1, 0), 1.0f);


                if(CameraTmp.y <= moveDown_y)
                {
                    /*カメラ移動*/
                    cameraControl.Zoom(new Vector3(2, -230, 0), 1.0f);
                }

                break;

            case "touch":
                //タッチされた時の処理
                break;
        }
    }

}

