using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionCountDown : MonoBehaviour {

    private bool m_Action_flag = false;
    [SerializeField]
    private Vector3 m_destination = Vector3.zero;
    [SerializeField]
    private float m_move_time;

    private float m_time_step = 0.0f;

    // Use this for initialization
    void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
		if(m_Action_flag)
        {
            m_autoMoveTime = Time.time;
        }
	}
}
