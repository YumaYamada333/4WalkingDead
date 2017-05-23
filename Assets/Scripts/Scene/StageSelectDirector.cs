using System.Collections;
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

    [SerializeField]
    private Vector3 m_zeroPos = new Vector3(-2.0f, -4.5f, 0.0f);// 一番前のパンフレット座標
    [SerializeField]
    private Vector3 m_pos = new Vector3(1.5f, 1.5f, 0.1f);      // 重なりのずれ
    [SerializeField]
    private Pamphlet[] m_pamphletData = new Pamphlet[5];        // パンフレットのデータ
    private int m_selectPamphlet = 0;                           // 選択中のパンフレット

    private GameObject[] m_pamphlet = new GameObject[5];        // パンフレット

	// Use this for initialization
	void Start ()
    {
        // 初期パンフレットの配置
        for (int i = 0; i < m_pamphlet.Length; i++)
        {
            m_pamphlet[i] = Instantiate(m_pamphletData[i].pamphletPrefab);
            m_pamphlet[i].transform.position = m_zeroPos + (m_pos * i);
        }

        // パンフが下から、リボンが上から規定位置まで移動
    }

    // Update is called once per frame
    void Update ()
    {
        // マウスのドラック量を取得
        Vector2 dragVec = GetComponent<MouseSystem>().GetDragVec();

        if (dragVec.y < -30)
        {
            // 一番前のパンフが下に移動

            // 他のパンフが手前に移動

            // 一番後ろにパンフが追加

            m_selectPamphlet++;
        }
        else if (dragVec.y > 30)
        {
            // 一番後ろのパンフが下に移動

            // 他のパンフが奥に移動

            // 一番前に下からパンフ挿入

            m_selectPamphlet--;
        }

        // 範囲外にならないように
        if (m_selectPamphlet >= m_pamphletData.Length)
            m_selectPamphlet = 0;
        if (m_selectPamphlet < 0)
            m_selectPamphlet = m_pamphletData.Length - 1;
    }

    // ボタンを押したときの処理
    public void PlayButton()
    {
        if(SceneManager.GetActiveScene().name != "StageSelect")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("StageScene", 2);

        // パンフが下に、リボンが上にはける
    }

    // シーンの遷移
    private void StageScene()
    {
        SceneManager.LoadScene(m_pamphletData[m_selectPamphlet].nextScene.ToString());
    }
}
