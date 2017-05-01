using System.Collections;
using UnityEngine;

public class ScrollScript : MonoBehaviour
{
    //カメラ初期位置
    Vector3 CameraPos;
    //カメラの現在位置
    Vector3 CameraTmp;
    public int scrollstart = 3;         //スクロール開始座標
    bool click_flag = false;            //クリックフラグ
    Vector3 start_mouse_pos;            //初期マウス座標
    Vector3 mouse_pos;                  //マウス座標


    // Use this for initialization
    void Start ()
    {
        //カメラの初期位置取得
        CameraPos = GameObject.Find("Main Camera").transform.position;
    }

    // Update is called once per frame
    void Update ()
    {
        //カメラの現在位置取得
        CameraTmp = GameObject.Find("Main Camera").transform.position;

        //左クリック
        if (Input.GetMouseButton(0))
        {
            //クリック直後
            if (!click_flag)
            {
                //マウスの初期座標を取得
                start_mouse_pos = Input.mousePosition;
                click_flag = true;
            }

            //マウス座標を取得
            Vector3 mouse_pos = Input.mousePosition;

            //右にスクロール
            if (start_mouse_pos.x + scrollstart >= mouse_pos.x)
            {
                //カメラを右スクロールさせる
                transform.Translate(0.15f, 0, 0);
            }
            //カメラの現在位置が初期位置より右にあるのならば
            if (CameraTmp.x >= 0)
            {
                //左にスクロール
                if (start_mouse_pos.x - scrollstart <= mouse_pos.x)
                {
                    //カメラを左スクロールさせる
                    transform.Translate(-0.15f, 0, 0);
                }
            }
        }
        else
        {
            click_flag = false;
        }

        //実行ボタンが押されたら
        if (Input.GetButtonDown("Fire3"))
        {
            //カメラの初期位置に移動
            GameObject.Find("Main Camera").transform.position = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z);
        }
    }
}
