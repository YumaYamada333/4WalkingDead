using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    // 注目するオブジェクト
    private GameObject m_focusObject = null;

    // 注目点のずれ
    private Vector3 m_focusPos = Vector3.zero;

    // カメラの初期位置保存
    private Vector3 m_defaultPos = Vector3.zero; 

    // カメラ移動時始点・終点
    private Vector3 m_startPosition = Vector3.zero;
    private Vector3 m_targetPosition = Vector3.zero;

    // 初期時間
    private float m_time = 0.0f;

    // ズーム時間
    private float m_moveTime = 0.0f;

    // オブジェクトを追尾するかどうか
    private bool m_follow = false;

    // ラープ中フラグ
    private bool m_init = false;

    // Use this for initialization
    void Start ()
    {
        // 初期のカメラ座標を保存
        m_defaultPos = this.transform.position;
        m_targetPosition = this.transform.position;
        
        // 時間を取得
        m_time = Time.time;
    }
	
	// Update is called once per frame
	void Update ()
    {
        // 新しいターゲット座標が設定されている
        if (!ValueCompare(this.transform.position,m_targetPosition))
        {
            // 初期設定
            if (!m_init)
            {
                m_startPosition = this.transform.position;
                m_time = Time.time;
                m_init = true;
            }
            // カメラの移動
            float timeStep = (Time.time - m_time) / m_moveTime;
            if (timeStep <= 1)
                this.transform.position = Lerp(m_startPosition, m_targetPosition, timeStep);
        }

        // カメラの追尾
        if (!GetCameraMove() && m_follow && m_init)
            this.transform.position = m_focusObject.transform.position + m_focusPos;
        else if (GetCameraMove() && m_follow && m_init)
            m_targetPosition = m_focusObject.transform.position + m_focusPos;
    }

    // 注目するオブジェクトを変更(カメラのずれを設定)
    public void SetFocusObject(GameObject obj, Vector3 pos, bool follow = false, float moveTime = 0.0f)
    {
        m_focusObject = obj;
        m_focusPos = pos;
        m_targetPosition = obj.transform.position + pos;
        m_follow = follow;
        m_moveTime = moveTime;
        m_init = false;
    }

    // 注目するオブジェクトを変更(奥行きz指定)
    public void SetFocusObject(GameObject obj, float posZ, bool follow = false, float moveTime = 0.0f)
    {
        m_focusObject = obj;
        m_focusPos = new Vector3(0.0f, 0.0f, posZ);
        m_targetPosition = obj.transform.position + new Vector3(0.0f, 0.0f, posZ);
        m_follow = follow;
        m_moveTime = moveTime;
        m_init = false;
    }

    // 注目する座標の変更
    public void SetFocusPos(Vector3 pos, float moveTime = 0.0f)
    {
        m_targetPosition = pos;
        m_moveTime = moveTime;
        m_init = false;
        m_follow = false;
    }

    // ズームイン・アウト(x,y,z指定)
    public void Zoom(Vector3 zoompos, float moveTime = 0.0f)
    {
        m_targetPosition = zoompos;
        m_moveTime = moveTime;
        m_init = false;
    }

    // ズームイン・アウト(z指定)
    public void Zoom(float zoomposZ, float moveTime = 0.0f)
    {
        m_targetPosition = this.transform.position + new Vector3(0.0f, 0.0f, zoomposZ);
        m_moveTime = moveTime;
        m_init = false;
    }

    // カメラ位置を初期に戻す
    public void ResetCamera(float moveTime = 0.0f)
    {
        m_targetPosition = m_defaultPos;
        m_moveTime = moveTime;
        m_init = false;
        m_follow = false;
    }

    // カメラが移動中かどうか取得
    public bool GetCameraMove()
    {
        float timeStep = (Time.time - m_time) / m_moveTime;
        return timeStep <= 1;
    }

    // オブジェクトを追尾させるか
    public void SetFollow(bool follow)
    {
        m_follow = follow;
    }

    // 値の比較
    private bool ValueCompare(Vector3 value1, Vector3 value2)
    {
        return value1 == value2;
    }

    // 補間
    static Vector3 Lerp(Vector3 startPosition, Vector3 targetPosition, float t)
    {
        Vector3 lerpPosition = Vector3.zero;

        lerpPosition = (1 - Trinity(t)) * startPosition + Trinity(t) * targetPosition;

        return lerpPosition;
    }

    // 三次補間
    static float Trinity(float time)
    {
        float v1 = 0.0f;

        v1 = (time * time) * (3 - 2 * time);

        return v1;
    }
}
