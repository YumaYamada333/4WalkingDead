using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedUp : MonoBehaviour {

    public float speed = 2;         //倍速の速さ

	// Use this for initialization
	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
    }

    public void OnClick()
    {
        if (Time.timeScale == 1)
        {
            Time.timeScale = speed;
        }
        else
        {
            Time.timeScale = 1;
        }
    }

}
