using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToPlayScene : MonoBehaviour {

    public AudioClip OK;

	// Use this for initialization
	void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            AudioSource audioSource = gameObject.GetComponent<AudioSource>();
            audioSource.PlayOneShot(OK);

            Application.LoadLevel("Play");
        }
    }
}
