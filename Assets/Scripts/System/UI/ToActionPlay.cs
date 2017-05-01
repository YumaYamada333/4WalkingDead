using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToActionPlay : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void OnClick()
    {
        gameObject.transform.root.gameObject.SetActive(false);
    }
}
