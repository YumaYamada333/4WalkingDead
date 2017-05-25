using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToResultScene : MonoBehaviour {

    GameObject player;
    GameObject GameOver;
    GameObject GameClear;

    //"GAME OVER"と"CLEAR"を動かすための始点と終点と時間
    Vector3 resultStartPos = new Vector3(0, 300, 0);
    Vector3 resultEndPos = new Vector3(0, 20, 0);
    private float resultTime;
    float timeStep;
    bool OverFlag = false;
    bool Flag = false;
    public enum OverType
    {
        NONE,
        FALL,
    }

	// Use this for initialization
	void Start () {
        player = GameObject.Find("unitychan");
        GameOver = GameObject.Find("OVER");
        GameClear = GameObject.Find("CLEAR");
        timeStep = 0;
        resultTime = Time.time + 50;

    }

    // Update is called once per frame
    void Update () {
        if(OverFlag)
        {
            timeStep = (Time.time - resultTime) / 0.3f;
            GameOver.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        }
        if(Flag)
        {
            timeStep = 0;

            timeStep = (Time.time - resultTime) / 0.3f;
            GameClear.transform.localPosition = MathClass.Lerp(resultStartPos, resultEndPos, timeStep);
        }
    }

    public void ToClear(int waitTime = 0)
    {
        resultTime = Time.time;

        if (!player.GetComponent<Animator>().GetBool("Clear"))
            player.GetComponent<PlayerAction>().AnimationStop();
        player.GetComponent<Animator>().SetBool("Clear", true);
        player.GetComponent<PlayerAction>().enabled = false;

        Flag = true;
    }

    public void ToOver(int waitTime = 0, OverType type = OverType.FALL)
    {
        // 演出処理
        switch (type)
        {
            case OverType.FALL:
                resultTime = Time.time;
                player.GetComponent<PlayerAction>().AnimationStop();
                player.GetComponent<Animator>().SetBool("Over", true);
                player.GetComponent<PlayerAction>().enabled = false;
                OverFlag = true;
                break;

            default:

                break;
        }
        //Invoke("ToOverScene", waitTime);
    }

}
