using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSystem : MonoBehaviour {

    Vector3 screen_pos;    //マウスのスクリーン座標
    Vector3 world_pos;     //マウスのワールド座標

    Vector2 touchPos;      // タッチ位置
    Vector2 dragVec;      // 移動ベクトル

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //マウスの座標を取得
        screen_pos = Input.mousePosition;
        //Debug.Log(screen_pos);

        //ワールド座標に変換
        screen_pos.z = 5;  //マウスのz座標を適当に代入
        world_pos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(screen_pos);
        //Debug.Log(world_pos);

        // タッチ位置の保存
        if (Input.GetMouseButtonDown(0))
        {
            touchPos = screen_pos;
        }

        // ドラッグ中
        if (Input.GetMouseButton(0))
        {
            dragVec = new Vector3(screen_pos.x - touchPos.x, screen_pos.y - touchPos.y);
        }
        else dragVec = Vector2.zero;

    }
    public Vector2 GetDragVec()
    {
        return dragVec;
    }
    public Vector2 GetTouchPos()
    {
        return touchPos;
    }

    public RaycastHit GetReyhitObject()
    {
        //Ray座標の取得
        Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(screen_pos);

        //Rayの触れているオブジェクトを取得
        RaycastHit hit = new RaycastHit();
        Physics.Raycast(ray, out hit);

        return hit;
    }

    public RaycastHit[] GetReyhitObjects()
    {
        //Ray座標の取得
        Ray ray = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenPointToRay(screen_pos);

        //Rayの触れているオブジェクトを取得
        RaycastHit[] hits = Physics.RaycastAll(ray);

        return hits;
    }

    public Vector3 GetScreenPos()
    {
        return screen_pos;
    }

    public Vector3 GetWorldPos()
    {
        return world_pos;
    }

    public int GetMouseHit(GameObject board)
    {
        if (board.activeSelf/*Collider(board)*/)
        {
            if (board.name == "HandsBord")
            {
                // ボードのカード情報取得
                CardManagement.CardData[] cards = GameObject.Find("CardManager").GetComponent<CardManagement>().cards;

                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].front.obj != null)
                    {
                        if (Collider(cards[i].front.obj))
                        {
                            return i;
                        }
                    }
                }
            }
            else
            {
                // ボードのカード情報取得
                CardBord.CardData[] cards = GameObject.Find("ActionBord").GetComponent<CardBord>().cards;

                for (int i = 0; i < cards.Length; i++)
                {
                    if (cards[i].obj != null)
                    {
                        if (Collider(cards[i].obj))
                        {
                            if (cards[i].type != CardManagement.CardType.Finish)
                            {
                                if (cards[i].obj.transform.position.x <= screen_pos.x)
                                    return i + 1;
                                else
                                    return i;
                            }
                        }
                    }
                }
            }
        }
        return -1;
    }

    // マウスカーソルとオブジェクトの当たり判定
    public bool Collider(GameObject obj)
    {
        if (obj.activeSelf != true) return false;
        // カードボードのサイズ取得
        Vector2 halfSize = obj.GetComponent<RectTransform>().sizeDelta / 2;
        if (screen_pos.x >= obj.transform.position.x - halfSize.x &&
            screen_pos.x <= obj.transform.position.x + halfSize.x &&
            screen_pos.y >= obj.transform.position.y - halfSize.y &&
            screen_pos.y <= obj.transform.position.y + halfSize.y)
        {
            return true;
        }
        return false;
    }
}
