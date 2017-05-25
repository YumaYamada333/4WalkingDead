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
    static PamphletSpace[] m_space;                                    // パンフレットの配置空間

    [SerializeField]
    private GameObject m_ribbon;

    private Vector2 dragVecOld;

    [SerializeField]
    private float stepSpd = 0.01f;
    private float changeStep = 0.0f;                            // 切り替えのステップ
    private float curtainUpStep = 0.0f;

    static bool isEnd;                                         // scene切り替えフラグ
    Vector3 posBasePamphlet;
    Vector3 posRibbon;

    private enum StepDirction
    {
        Up,
        Down,
    };
    StepDirction stepDir = StepDirction.Down;

    // Use this for initialization
    void Start ()
    {
        // ループ変数初期化
        m_selectPamphlet = new MathClass.Looper(m_pamphlet.Length - 1, 0, 0, 0);

        // パンフレット配置位置の配列の生成
        m_space = new PamphletSpace[m_pamphlet.Length];

        // ベース位置の初期化
        posBasePamphlet = m_zeroPos;
        posRibbon = m_ribbon.transform.position;

        // マウスのドラック量を初期化
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();
        changeStep = 1.0f;
        isEnd = false;
        // 初期パンフレットの配置 配置空間の設定
        for (int i = 0; i < m_pamphlet.Length; i++)
        {
            m_pamphlet[i] = Instantiate(m_pamphletData[i].pamphletPrefab);
            m_space[i].Set(m_zeroPos + (m_pos * i), i);
            m_pamphlet[i].transform.position = new Vector3(0, -80, 0) + (m_pos * i)/*m_space[m_space[i].pamphlietIndex].pos*/;
            //m_pamphlet[i].GetComponent<Button>().
            GameObject childObject = m_pamphlet[i].transform.FindChild("PamphletCanvas").transform.FindChild("PlayButton").gameObject;
            childObject.SetActive(false);
            if (i == m_space[0].pamphlietIndex) childObject.SetActive(true);
        }
    }

    // Update is called once per frame
    void Update ()
    {
        if (isEnd != true) curtainUpStep += 0.01f;
        else curtainUpStep -= 0.01f;
        if (curtainUpStep > 1.0f) curtainUpStep = 1.0f;
        if (curtainUpStep < 0.0f) curtainUpStep = 0.0f;
        
        // マウスのドラック量を取得
        Vector2 dragVec = GetComponent<MouseSystem>().GetDragVec() - dragVecOld;

        if (!(curtainUpStep < 1.0f))
        {
            // 切り替えが行われていない
            if (changeStep >= 1.0f)
            {
                if (/*Input.GetKeyDown(KeyCode.DownArrow)*/dragVec.y < -30)
                {
                    // 配置空間のパンフインデックスの更新
                    for (int i = 0; i < m_space.Length; i++)
                    {
                        m_selectPamphlet.Plus(1);
                        m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                    }
                    stepDir = StepDirction.Down;
                    m_selectPamphlet.Plus(1);
                    changeStep = 0.0f;
                }
                else if (/*Input.GetKeyDown(KeyCode.UpArrow)*/dragVec.y > 30)
                {
                    m_selectPamphlet.Plus(-2);
                    // 配置空間のパンフインデックスの更新
                    for (int i = 0; i < m_space.Length; i++)
                    {
                        m_selectPamphlet.Plus(1);
                        m_space[i].pamphlietIndex = m_selectPamphlet.Get();
                    }
                    stepDir = StepDirction.Up;
                    m_selectPamphlet.Plus(1);
                    changeStep = 0.0f;
                }
            }
            else
            {
                changeStep = changeStep + stepSpd > 1.0f ? 1.0f : changeStep + stepSpd;
            }
            //Debug.Log(m_space[0].pamphlietIndex);
            // 何故かバグル
            //Debug.Log(m_selectPamphlet.Get());

            /////////////////////////////////////////////////////////////////////////////



            // パンフレットのラープ処理
            for (int i = 0; i < m_pamphlet.Length; i++)
            {
                if (stepDir == StepDirction.Up)
                {
                    if (i == 0 && changeStep < 0.3f)
                    {
                        m_pamphlet[m_space[0].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[0].pamphlietIndex].transform.position,
                        new Vector3(m_space[0].pos.x, m_space[0].pos.y - 20, m_space[0].pos.z),
                        changeStep);
                    }
                    else
                    {
                        //m_pamphlet[m_space[0].pamphlietIndex].transform.position =
                        //    m_space[0].pos;
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position, m_space[i].pos, changeStep);
                    }
                }
                else
                {
                    if (i == m_pamphlet.Length - 1 && changeStep < 0.3f)
                    {
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position,
                        new Vector3(m_pamphlet[m_space[i].pamphlietIndex].transform.position.x, -100, m_pamphlet[m_space[i].pamphlietIndex].transform.position.z), changeStep);
                    }
                    else
                    {
                        m_pamphlet[m_space[i].pamphlietIndex].transform.position =
                            MathClass.Lerp(m_pamphlet[m_space[i].pamphlietIndex].transform.position, m_space[i].pos, changeStep);
                    }
                }
                GameObject childObject = m_pamphlet[i].transform.FindChild("PamphletCanvas").transform.FindChild("PlayButton").gameObject;
                childObject.SetActive(false);
                if (i == m_space[0].pamphlietIndex) childObject.SetActive(true);

            }


        }
        else
        {
            for (int i = 0; i < m_pamphlet.Length; i++)
            {
                // 始まりの演出
                m_pamphlet[i].transform.position = MathClass.Lerp(new Vector3(0, -80, 0) + (m_pos * i), posBasePamphlet, curtainUpStep);
                m_ribbon.transform.position = MathClass.Lerp(new Vector3(0, 80, 0), posRibbon, curtainUpStep);
            }

        }

        // 前フレーム更新
        dragVecOld = GetComponent<MouseSystem>().GetDragVec();
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
        isEnd = true;
    }

    // シーンの遷移
    private void StageScene()
    {
        SceneManager.LoadScene(m_pamphletData[m_space[0].pamphlietIndex].nextScene.ToString());
    }
}
