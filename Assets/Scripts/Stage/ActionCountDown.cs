using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//アクションの種類用定数
struct ACT_TYPE
{
    public const int ACT_BREAK          = 0;
    public const int ACT_MOVE           = 1;
    //public const int ACT_MOVE_BUCK      = 2;
    //public const int ACT_MOVE_REPEAT    = 3;
    public const int ACT_ROT            = 2;
    public const int ACT_SCALE          = 3;
}

//各アクションの基底クラス
public class BlockAction
{
    //準備関数(引数は参照渡し)
    public virtual void Preparation(ref GameObject obj) { }

    //実行関数(引数は参照渡し)
    public virtual void Execute(ref GameObject obj) { }
}

//動くアクションのクラス
public class BlockMove : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private Vector3 m_start_pos = Vector3.zero;

    //パーティクルフラグ取得
    private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Move(ref obj); }

    //パーティカルフラグを返す関数
    public bool GetFlag()
    {
        return PartTim;
    }

    public void Init(ref GameObject obj)
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_pos = obj.transform.position;
    }

    public void Move(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<ActionCountDown>().m_act_time;

            obj.transform.position = (1 - timeStep) * m_start_pos + timeStep * (m_start_pos + obj.GetComponent<ActionCountDown>().m_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                obj.GetComponent<ActionCountDown>().SetActionFlag(false);

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);
                

                PartTim = true;

                //繰り返しパターン用初期化
                obj.GetComponent<ActionCountDown>().ActInit();
            }
            else
            {
                PartTim = false;
            }  
         }
     }
}

//回転アクションのクラス
public class BlockRot : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private Vector3 m_start_deg = Vector3.zero;

    //パーティクルフラグ取得
    //private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Rot(ref obj); }

    //パーティカルフラグを返す関数
    //public bool GetFlag()
    //{
    //    return PartTim;
    //}

    public void Init(ref GameObject obj)
    {
        //時間を取得
        m_start_time = Time.time;        
    }

    public void Rot(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<ActionCountDown>().m_act_time;

            //回転
            Vector3 obj_deg = timeStep * (obj.GetComponent<ActionCountDown>().m_act_distance);
            obj.transform.rotation = Quaternion.Euler(obj_deg);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                obj.GetComponent<ActionCountDown>().SetActionFlag(false);

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                //PartTim = true;

                //繰り返しパターン用初期化
                obj.GetComponent<ActionCountDown>().ActInit();
                
            }
            //else
            //{
            //    PartTim = false;
            //}
        }
    }
}

//スケールアクションのクラス
public class BlockScale : BlockAction
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート時の大きさ
    private Vector3 m_start_scale = Vector3.zero;

    //パーティクルフラグ取得
    private bool PartTim;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Scale(ref obj); }

    //パーティカルフラグを返す関数
    public bool GetFlag()
    {
        return PartTim;
    }

    public void Init(ref GameObject obj)
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_scale = obj.transform.localScale;
    }

    public void Scale(ref GameObject obj)
    {
        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
            //経過時間を移動時間で割る
            float timeStep = (Time.time - m_start_time) / obj.GetComponent<ActionCountDown>().m_act_time;

            obj.transform.localScale = (1 - timeStep) * m_start_scale + timeStep * (m_start_scale + obj.GetComponent<ActionCountDown>().m_act_distance);

            //移動時間になったらフラグを止める
            if (timeStep > 1.0f)
            {
                obj.GetComponent<ActionCountDown>().SetActionFlag(false);

                //GameObject manager = GameObject.Find("GameManager");
                /*manager*/
                obj.GetComponent<ActionCountDown>().m_game_manager.GetComponent<GameManager>().SetGimmickFlag(false);

                PartTim = true;

                //繰り返しパターン用初期化
                obj.GetComponent<ActionCountDown>().ActInit();
            }
            else
            {
                PartTim = false;
            }
        }
    }
}

//壊れるアクションのクラス
public class BlockBreak : BlockAction
{

    override public void Execute(ref GameObject obj) { Break(ref obj); }

    ////パーティクルフラグ取得
    //private bool PartTim;

    //public bool GetFlag()
    //{
    //    return PartTim;
    //}

    public void Break(ref GameObject obj)
    {
        //フラグがたったら壊れる
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
            //オブジェクトを破棄
            obj.GetComponent<ActionCountDown>().SetActionFlag(false);
            obj.GetComponent<ActionCountDown>().DestroyObject();
            //PartTim = true;
        }
        //else
        //{
        //    PartTim = false;
        //}
    }
}

//実行クラス
public class ActionCountDown : MonoBehaviour
{
    //アクション量
    public Vector3  m_act_distance  = Vector3.zero;
    //移動時間
    public float    m_act_time      = 1.0f;
    //待機時間
    public float    m_wait_time     = 0.0f;
    //繰り返しフラグ
    [SerializeField]
    private bool    m_repeat_flag;
    //反転フラグ
    [SerializeField]
    private bool    m_back_flag;
    //反転タイミング
    [SerializeField, Range(1, 10)]
    private int     m_back_timing = 1;
    //アクションの種類
    [SerializeField, Range(0, 3)]
    private int     m_action_type;
    //カウントを行うオブジェクト
    public GameObject m_count_obj;
    //ゲームマネージャー
    public GameObject m_game_manager;

    //初期待機時間
    private float   m_init_wait_time;

    //フラグ
    private bool    m_old_flag      = false;
    private bool    m_action_flag   = false;

    //アクションの実行カウント
    private int     m_action_count = 0;

    //自分を取得するための変数
    GameObject      m_obj;

    //アクション
    private BlockAction action = null;
    //GameObjectでColliderと触れているの要素数
    private List<GameObject> ride = new List<GameObject>();

    //パーティクルフラグ取得
    public bool PartTim;

    // Use this for initialization
    void Start()
    {
        //指定したアクションタイプによってアクションを変更
        switch (m_action_type)
        {
           
            case ACT_TYPE.ACT_BREAK:
                action = new BlockBreak();
                break;
            case ACT_TYPE.ACT_MOVE:
                action = new BlockMove();
                break;
            case ACT_TYPE.ACT_ROT:
                action = new BlockRot();
                break;
            case ACT_TYPE.ACT_SCALE:
                action = new BlockScale();
                break;
            default:
                break;
        }

        //自身を取得
        m_obj = this.gameObject;
        //初期待機時間を取得
        m_init_wait_time = m_wait_time;
    }

    // Update is called once per frame
    void Update()
    {
        //カウントダウン
        GetCountZero();

        //アクションがnullでないならアクションを行う
        if (action != null)
        {
            //フラグが立ったらアクションの準備
            if (m_action_flag == true &&
                m_action_flag != m_old_flag)
            {
                action.Preparation(ref m_obj);
                m_old_flag = m_action_flag;
            }
            //アクションの実行
            action.Execute(ref m_obj);
            //GameObjectの移動
            foreach (GameObject otherObj in ride)
            {
                //動く床の位置にObjectの座標を合わせる
                Vector3 v = otherObj.transform.position;
                otherObj.transform.position = new Vector3(m_obj.transform.position.x, v.y, v.z);
            }
            PartTim = true;
        }
        else
        {
            PartTim = false;
        }
    }


    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、フラグがたっておらず、いままで一度もフラグが立っていなかったら
        if (/*m_obj.transform.FindChild("CountUI")*/m_count_obj.GetComponent<CountDown>().GetCount() == 0 &&
            m_action_flag == false &&
            m_old_flag == false)
        {
            //ゲームマネージャーのギミックが移動している判定用のフラグをあげる
            //GameObject manager = GameObject.Find("GameManager");
            /*manager*/m_game_manager.GetComponent<GameManager>().SetGimmickFlag(true);

            //待機時間があるなら待機する
            if (m_wait_time > 0.0f)
            {
                //60fps
                m_wait_time -= 1 / 60.0f;
            }
            else
            {
                //フラグを上げる
                m_action_flag = true;
            }  
        }

    }

    //アクションタイプを返す
    public int GetActionType()
    {
        return m_action_type;
    }

    //アクションフラグを返す
    public bool GetActionFlag()
    {
        return m_action_flag;
    }

    //アクションフラグを設定
    public void SetActionFlag(bool flag)
    {
        m_action_flag = flag;
    }

    //繰り返し用アクションフラグを設定
    public void SetOldActionFlag(bool flag)
    {
        m_old_flag = flag;
    }

    //繰り返し用初期化
    public void ActInit()
    {
        //アクション実行回数を増やす
        m_action_count++;

        //繰り返しフラグがたっているなら
        if (m_repeat_flag)
        {
            //カウント初期化
            /*m_obj.transform.FindChild("CountUI")*/
            m_count_obj.GetComponent<CountDown>().SetCount();
            //待機時間初期化
            m_wait_time = m_init_wait_time;
            //繰り返しフラグをセット
            m_old_flag = false;
        }

        //反転フラグがたっている,かつタイミングになっていたら
        if (m_back_flag &&
            m_action_count >= m_back_timing)
        {
            //アクションカウントを初期化
            m_action_count = 0;

            //移動量を反転
            m_act_distance *= -1;
        }
    }

    //自身を破棄
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
    //他のObjectが接触している時
    void OnTriggerEnter(Collider otherObj)
    {
        ride.Add(otherObj.gameObject);
    }
    //他のObjectが離れている時
    void OnTriggerExit(Collider otherObj)
    {
        //床から離れたので削除
        ride.Remove(otherObj.gameObject);
    }
}
