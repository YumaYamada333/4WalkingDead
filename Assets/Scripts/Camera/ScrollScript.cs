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

    public GameObject StartBlock;       //スタートブロック
    public GameObject GoalBlock;        //ゴールブロック

    private float StartBlockX;          //スタートブロックのｘ座標
    private float GoalBlockX;           //ゴールブロックのｘ座標

    public bool isUpdate;

    CardManagement cardmanegement;      //カードマネジメント

    // Use this for initialization
    void Start ()
    {
        //カメラの初期位置取得
        CameraPos = GameObject.Find("MainCamera").transform.position;
        //スタート、ゴールブロックのｘ座標取得
        StartBlockX = StartBlock.transform.position.x;
        GoalBlockX = GoalBlock.transform.position.x;

        //コンポーネントの取得
        cardmanegement = GameObject.Find("CardManager").GetComponent<CardManagement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isUpdate)
        {
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

                //マウスクリック時カードをつかんでいないなら
                if (cardmanegement.GetGripFlag() == false)
                {
                    //カメラがゴールオブジェクトのx座標より左にあるのならば
                    if (CameraTmp.x <= GoalBlockX)
                    {
                        //右にスクロール
                        if (start_mouse_pos.x + scrollstart >= mouse_pos.x)
                        {
                            //カメラを右スクロールさせる
                            transform.Translate(0.15f, 0, 0);
                        }
                    }
                    //カメラがスタートオブジェクトのx座標より右にあるのならば
                    if (CameraTmp.x >= StartBlockX)
                    {
                        //左にスクロール
                        if (start_mouse_pos.x - scrollstart <= mouse_pos.x)
                        {
                            //カメラを左スクロールさせる
                            transform.Translate(-0.15f, 0, 0);
                        }
                    }
                }
            }
            else
            {
                click_flag = false;
            }
        }
            //カメラの現在位置取得
            CameraTmp = GameObject.Find("MainCamera").transform.position;


            //実行ボタンが押されたら
            if (Input.GetButtonDown("Fire3"))
            {
                //カメラの初期位置に移動
                GameObject.Find("Main Camera").transform.position = new Vector3(CameraPos.x, CameraPos.y, CameraPos.z);
            }
        }
}
