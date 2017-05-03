using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//各アクションの基底クラス
class BlockAction
{
    //準備関数(引数は参照渡し)
    public virtual void Preparation(ref GameObject obj) { }

    //実行関数(引数は参照渡し)
    public virtual void Execute(ref GameObject obj) { }
}

//動くアクションのクラス
class BlockMove : BlockAction 
{
    //スタートの時間
    private float m_start_time = 0.0f;
    //スタート位置
    private Vector3 m_start_pos = Vector3.zero;

    //準備関数、実行関数のオーバーライド(引数は参照渡し)
    override public void Preparation(ref GameObject obj) { Init(ref obj); }
    override public void Execute(ref GameObject obj) { Move(ref obj); }

    public void Init(ref GameObject obj)
    {
        //現在地と時間を取得
        m_start_time = Time.time;
        m_start_pos = obj.transform.position;
    }

    public void Move(ref GameObject obj)
    {
        //経過時間を移動時間で割る
        float timeStep = (Time.time - m_start_time) / obj.GetComponent<ActionCountDown>().m_move_time;

        //移動時間になったらフラグを止める
        if (timeStep > 1.0f)
        {
            obj.GetComponent<ActionCountDown>().SetActionFlag(false);
        }

        //フラグがたっていたら移動(補間)
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
           obj.transform.position = (1 - timeStep) * m_start_pos + timeStep * (m_start_pos + obj.GetComponent<ActionCountDown>().m_move_distance);
        }
    }
}

//壊れるアクションのクラス
class BlockBreak : BlockAction
{

    override public void Execute(ref GameObject obj) { Break(ref obj); }

    public void Break(ref GameObject obj)
    {
        //フラグがたったら壊れる
        if (obj.GetComponent<ActionCountDown>().GetActionFlag())
        {
            //オブジェクトを破棄
            obj.GetComponent<ActionCountDown>().SetActionFlag(false);
            obj.GetComponent<ActionCountDown>().DestroyObject();
        }
    }
}

//実行クラス
public class ActionCountDown : MonoBehaviour {

    //アクションの種類用定数
    const int ACT_MOVE = 0;
    const int ACT_BREAK = 1;

    //移動距離
    public Vector3 m_move_distance = Vector3.zero;
    //移動時間
    public float m_move_time = 1.0f;

    //フラグ
    private bool m_old_flag = false;
    private bool m_action_flag = false;

    //アクションの種類
    [SerializeField]
    private int m_action_type;

    //自分を取得するための変数
    GameObject obj;

    //アクション
    private BlockAction action = null;

    // Use this for initialization
    void Start () {
        //指定したアクションタイプによってアクションを変更
       switch(m_action_type)
        {
            case ACT_MOVE:
                action = new BlockMove();
                break;
            case ACT_BREAK:
                action = new BlockBreak();
                break;
            default:
                break;
        }

        //自身を取得
        obj = this.gameObject;
    }
	
	// Update is called once per frame
	void Update () {
        //カウントダウン
        GetCountZero();

        //アクションがnullでないならアクションを行う
        if (action != null)
        {
            //フラグが立ったらアクションの準備
            if (m_action_flag == true && 
                m_action_flag != m_old_flag)
            {
                action.Preparation(ref obj);
                m_old_flag = m_action_flag;
            }

            //アクションの実行
            action.Execute(ref obj);

        }
    }

    //カウントダウンによってフラグをあげる
    void GetCountZero()
    {
        //カウントが0になり、いままで一度もフラグが立っていなかったらフラグをあげる
        if (obj.GetComponent<CountDown>().GetCount() == 0 &&
            m_old_flag == false)
        {
            m_action_flag = true;
        }
      
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

    //自身を破棄
    public void DestroyObject()
    {
        Destroy(this.gameObject);
    }
}
