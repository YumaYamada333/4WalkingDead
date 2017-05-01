using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        GameObject.Find("GameManager").GetComponent<GameManager>().Play();
    }
}
