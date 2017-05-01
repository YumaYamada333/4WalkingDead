using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scrollback : MonoBehaviour {

   


    // Use this for initialization
    void Awak ()
    {
        
    }
	
	// Update is called once per frame
	void Update ()
    {
        float scroll = Mathf.Repeat(Time.time * 0.08f,1);
        Vector2 offset = new Vector2(scroll, 0);
        GetComponent<Renderer>().sharedMaterial.SetTextureOffset("_MainTex", offset);
    }

   


    
}
