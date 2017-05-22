﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StageSelectDirector : MonoBehaviour {

    // シーン名登録
    public enum SceneName
    {
        newStage1,
        newStage2,
        newStage3,
        newStage4,
    }

    [System.Serializable]
    struct Pamphlet
    {
        public GameObject pamphletPrefab;       // パンフレットのプレハブ
        public SceneName nextScene;             // 次のシーン
    }

    // パンフレットの配置空間
    struct PamphletSpace
    {
        public Vector3 pos;
        public int pamphlietIndex;
        public void Set(Vector3 pos, int index)
        {
            this.pos = pos;
            pamphlietIndex = index;
        }
    }

    [SerializeField]
    private Vector3 m_zeroPos = new Vector3(-2.0f, -4.5f, 0.0f);// 一番前のパンフレット座標
    [SerializeField]
    private Vector3 m_pos = new Vector3(1.5f, 1.5f, 0.1f);      // 重なりのずれ
    [SerializeField]
    private Pamphlet[] m_pamphletData = new Pamphlet[5];        // パンフレットのデータ
    private MathClass.Looper m_selectPamphlet;                  // 選択中のパンフレット

    private GameObject[] m_pamphlet = new GameObject[5];        // パンフレット
    PamphletSpace[] m_space;                                    // パンフレットの配置空間

    private Vector2 dragVecOld;

    [SerializeField]
    private float stepSpd = 0.01f;
    private float changeStep = 0.0f;                            // 切り替えのステップ
	// Use this for initialization
	void Start ()
    {
        // ループ変数初期化
        m_selectPamphlet = new MathClass.Looper(m_pamphlet.Length - 1, 0, 0, 0);

        // パンフレット配置位置の配列の生成
        m_space = new PamphletSpace[m_pamphlet.Length];

        // マウスのドラック量を初期化
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();

        // 初期パンフレットの配置 配置空間の設定
        for (int i = 0; i < m_pamphlet.Length; i++)
        {
            m_pamphlet[i] = Instantiate(m_pamphletData[i].pamphletPrefab);
            m_space[i].Set(m_zeroPos + (m_pos * i), i);
            m_pamphlet[i].transform.position = m_space[m_space[i].pamphlietIndex].pos;
        }

        // パンフが下から、リボンが上から規定位置まで移動
    }

    // Update is called once per frame
    void Update ()
    {
        // マウスのドラック量を取得
        Vector2 dragVec = GetComponent<MouseSystem>().GetDragVec() - dragVecOld;

        // 切り替えが行われていない
        if (changeStep <= 1.0f)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow)/*dragVec.y < -30*/)
            {
                // 一番前のパンフが下に移動

                // 他のパンフが手前に移動

                // 一番後ろにパンフが追加

                // 配置空間のパンフインデックスの更新
                for (int i = 0; i < m_space.Length; i++)
                {
                    m_selectPamphlet.Plus(1);
                    m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                }
                changeStep = 0.0f;
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow)/*dragVec.y > 30*/)
            {
                // 一番後ろのパンフが下に移動

                // 他のパンフが奥に移動

                // 一番前に下からパンフ挿入

                // 配置空間のパンフインデックスの更新
                for (int i = 0; i < m_space.Length; i++)
                {
                    m_selectPamphlet.Plus(-1);
                    m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                }
                changeStep = 0.0f;
            }
        }
        else changeStep = changeStep + stepSpd > 1.0f ? 1.0f : changeStep + stepSpd;

        Debug.Log(m_selectPamphlet.Get());
        //// 範囲外にならないように
        //if (m_selectPamphlet >= m_pamphletData.Length)
        //    m_selectPamphlet = 0;
        //if (m_selectPamphlet < 0)
        //    m_selectPamphlet = m_pamphletData.Length - 1;

        // 前フレーム更新
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();
    }

    // ボタンを押したときの処理
    public void PlayButton()
    {
        CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
        //カーテンを閉める
        CurtainSystem.curtainOut();
        //遷移先のシーンをロード
        Invoke("StageScene", 2);

        // パンフが下に、リボンが上にはける
    }

    // シーンの遷移
    private void StageScene()
    {
        SceneManager.LoadScene(m_pamphletData[m_selectPamphlet.Get()].nextScene.ToString());
    }
}
