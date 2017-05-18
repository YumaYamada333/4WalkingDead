//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   CurtainControl
//!
//! @brief  カーテンの挙動
//!
//! @date   2017/05/11
//!
//! @author Y.Okada
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/


using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class CurtainControl : MonoBehaviour
{
    /*左カーテン*/
    private GameObject left_curtain;
    /*右カーテン*/
    private GameObject right_curtain;

    /*秒数*/
    float time;

    /*カーテンカウント*/
    float curtain_cnt;

    //カーテンの状態
    int CurtainState = -1;

	// Use this for initialization
	void Start ()
    {
        /*初期化*/
        this.left_curtain = GameObject.Find("left_curtain");
        this.right_curtain = GameObject.Find("right_curtain");
        curtain_cnt = 0;
        time = 60.0f;
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (CurtainState == 1) 
        {
            /*カーテンをだんだん表示(閉じる)*/
            this.left_curtain.GetComponent<Image>().fillAmount += 0.01f;
            this.right_curtain.GetComponent<Image>().fillAmount += 0.01f;
            curtain_cnt++;
            if (time <= curtain_cnt) 
            {
                CurtainState = 0;
                curtain_cnt = 0;
            }
        }
        if (CurtainState == -1)
        {
            this.left_curtain.GetComponent<Image>().fillAmount -= 0.01f;
            this.right_curtain.GetComponent<Image>().fillAmount -= 0.01f;
            curtain_cnt++;
            /*カーテンをだんだん消す(開く)*/
            if (time <= curtain_cnt)
            {
                CurtainState = 0;
                curtain_cnt = 0;
            }
        }
        else
        {

        }
    }
    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_
    //! @brief カーテンを開く関数
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_

    public void curtainIn()
    {
        CurtainState = -1;
    }

    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_
    //! @brief カーテンを閉じる関数
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_
    public void curtainOut()
    {
        CurtainState = 1;
    }
}
