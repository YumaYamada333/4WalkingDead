using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassLand : MonoBehaviour
{

    bool setPass;
    BoxCollider colliderOfPass;

    // Use this for initialization
    void Start()
    {
        colliderOfPass = GetComponent<BoxCollider>();
    }

    // Update is called once per frame
    void Update()
    {

        if (setPass)
        {
            colliderOfPass.enabled = false;
        }
        if (!setPass)
        {
            colliderOfPass.enabled = true;
        }
    }

    //プレイヤーのIsTriggerがOnの側のコリジョンが床のIsTriggerがOnの側のコリジョンと接触している時
    void OnTriggerStay(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            setPass = true;
        }
    }

    void OnTriggerExit(Collider col)
    {
        if (col.gameObject.tag == "Player")
        {
            setPass = false;
        }
    }
}