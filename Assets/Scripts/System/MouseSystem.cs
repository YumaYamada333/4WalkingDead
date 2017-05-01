using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseSystem : MonoBehaviour {

    Vector3 screen_pos;    //マウスのスクリーン座標
    Vector3 world_pos;     //マウスのワールド座標

    // Use this for initialization
    void Start ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        //マウスの座標を取得
        screen_pos = Input.mousePosition;

        //ワールド座標に変換
        screen_pos.z = 5;  //マウスのz座標を適当に代入
        world_pos = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>().ScreenToWorldPoint(screen_pos);
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
}
