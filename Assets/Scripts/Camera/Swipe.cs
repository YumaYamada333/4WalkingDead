using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Swipe : MonoBehaviour
{

    private Vector3 touchStartPos;
    private Vector3 touchEndPos;

    private string Direction;

    GameObject Camera;
    CameraControl cameraControl;



    Vector3 CameraPos;

    Vector3 CameraTmp;

    // Use this for initialization
    void Start ()
    {

        cameraControl = gameObject.GetComponent<CameraControl>();

        CameraPos = GameObject.Find("Main Camera").transform.position;

    }

    // Update is called once per frame
    void Update()
    {

        

        CameraTmp = GameObject.Find("Main Camera").transform.position;

        Flick();

    }

    void Flick()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            touchStartPos = new Vector3(Input.mousePosition.x,
                                        Input.mousePosition.y,
                                        Input.mousePosition.z);
        }

        if (Input.GetKeyUp(KeyCode.Mouse0))
        {
            touchEndPos = new Vector3(Input.mousePosition.x,
                                      Input.mousePosition.y,
                                      Input.mousePosition.z);
            GetDirection();
        }
    }

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

                cameraControl.Zoom(new Vector3(2, -230, 0),1.0f);
                
                break;

            case "down":
                //下フリックされた時の処理

                cameraControl.Zoom(new Vector3(2, -1, 0), 1.0f);

                break;

            case "touch":
                //タッチされた時の処理
                break;
        }
    }

}

