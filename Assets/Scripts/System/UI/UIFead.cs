using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIFead : MonoBehaviour {
    float alfa;
    float speed = 0.02f;
    float red, green, blue;
    bool fadeState = false;

	// Use this for initialization
	void Start () {
        red = GetComponent<RawImage>().color.r;
        green = GetComponent<RawImage>().color.g;
        blue = GetComponent<RawImage>().color.b;
	}
	
	// Update is called once per frame
	void Update () {
        GetComponent<RawImage>().color = new Color(red, green, blue, alfa);

        FadeInAndOut();
    }

    //フェードインとフェードアウトする関数
    void FadeInAndOut()
    {
        if (!fadeState)
        {
            alfa += speed;
            if (alfa >= 1.0f)
            {
                fadeState = true;
            }
        }
        else
        {
            alfa -= speed;
            if (alfa <= 0.0f)
            {
                fadeState = false;
            }
        }

    }
}
