using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToResultScene : MonoBehaviour {

    GameObject player;

    //
    public enum OverType
    {
        NONE,
        FALL,
    }

	// Use this for initialization
	void Start () {
        player = GameObject.Find("unitychan");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void ToClear(int waitTime = 0)
    {
        Invoke("ToClearScene", waitTime);
    }

    public void ToOver(int waitTime = 0, OverType type = OverType.FALL)
    {
        // 演出処理
        switch (type)
        {
            case OverType.FALL:
                player.GetComponent<PlayerAction>().AnimationStop();
                player.GetComponent<Animator>().SetBool("Over", true);
                player.GetComponent<PlayerAction>().enabled = false;
                break;

            default:

                break;
        }
        Invoke("ToOverScene", waitTime);
    }

    void ToClearScene()
    {
        Application.LoadLevel("Result");
    }

    void ToOverScene()
    {
        Application.LoadLevel("Over");
    }
}
