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
    public float time;

    /*カーテンカウント*/
    float curtain_cnt;

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
        /*curtain_cntがtime以上なら*/
		if(time<curtain_cnt)
        {
            /*関数呼び出し*/
            curtainIn();
        }
        /*カウントを足す*/
        curtain_cnt++;
    }
    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_
    //! @brief カーテンを開く関数
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_/_

    void curtainIn()
    {
        /*表示を段々と消していく*/
        this.left_curtain.GetComponent<Image>().fillAmount -= 0.01f;
        this.right_curtain.GetComponent<Image>().fillAmount -= 0.01f;
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
        /*表示を段々としていく*/
        this.left_curtain.GetComponent<Image>().fillAmount += 0.01f;
        this.right_curtain.GetComponent<Image>().fillAmount += 0.01f;
    }
}
