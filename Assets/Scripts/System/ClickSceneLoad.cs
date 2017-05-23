using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ClickSceneLoad : MonoBehaviour {

	// Use this for initialization
	void Start () {

    }

    // Update is called once per frame
    void Update () {
        //タイトルシーン内の画面内でクリックしたらシーン遷移
        if (Input.GetMouseButtonUp(0))
        {
            ClickLoad();
        }

    }
    public void ClickLoad()
    {
        if (SceneManager.GetActiveScene().name == "Title")
        {
            SceneManager.LoadScene("StageSelect");
        }
    }

}
