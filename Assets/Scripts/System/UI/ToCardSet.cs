using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToCardSet : MonoBehaviour {

    public GameObject CardSet;
    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update (){
		
	}

    public void OnClick()
    {
        CardSet.SetActive(true);
    }
}
