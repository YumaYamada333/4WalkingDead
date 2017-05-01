//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   PlayerAction
//!
//! @brief  プレイヤーの移動
//!
//! @date   2017/04/03
//!
//! @author N.Sakuma
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数の定義
static class Constants
{
    public const int MaxAnimation = 6;  //最大のアニメーションの数
    public const int MaxTime = 10;      //最大時間
    public const int MaxJumpPow = 5;      //最大のジャンプ力
    public const int MaxEnemy = 4;      //敵の数
    public const float MassDistance = 2.2f; //マスの距離
    public const int EffectJumpCount = 65;  //ジャンプしてから着地までのカウント
    public const int MoveCount = 60;    //移動エフェクトのループ再生する間隔
}
enum ANIMATION { RUN, JUMP, ATTACK ,SUPERRUN , SUPERJUMP , SUPERATTACK ,OVER};
public class PlayerAction : MonoBehaviour
{
    [SerializeField, Range(0, Constants.MaxTime)]
    float time = 0.5f; //時間
    [SerializeField, Range(0, Constants.MaxJumpPow)]
    float jumpPower = 2.14f;  //ジャンプ
    [SerializeField]
    Vector3 middlePosition; 　//中間地点
    [SerializeField]
    Vector3 endPosition = new Vector3(2, 0, 0);   //走り終わる場所
    [SerializeField]
    Vector3 nextPosition = new Vector3(2, 0, 0);  //次の場所
    private float startTime;        //走り始めた時間
    [SerializeField]
    private float downFall = 0;
    private Vector3 startPosition;  //走り始める場所
    private Animator animator;      //アニメーター
    private bool[] animationFlag = new bool[Constants.MaxAnimation];   //アニメーションしているかどうかのフラグ
    private bool idleFlag;      //待機かどうかのフラグ
    private bool cardSetFlag;   //カードがセットされたかどうかのフラグ
    private int animationNum = 0;         //アニメーションの番号
    private System.String animationName;  //アニメーションの名前
    private GameObject[] enemy;           //敵
    private AudioSource audioSource;      //音
    private int effect_count = 0;         //エフェクト再生用のカウント
    private int distance = 100;             //rayの長さを決める
    private int distance1 = 2;             //rayの長さを決める
    private float diff;              //経過時間

    private bool isGround;       //地面についているか
    private bool isGroundOld;    //地面についていたか
    private CharacterController controller;  //charactercontroller
    //音
    public AudioClip Attack;
    public AudioClip Jump;
    public AudioClip Hit;
    public AudioClip Move;

    void OnEnable() //objが生きている場合
    {
        if (time <= 0)
        {
            return;
        }
        //シーンが呼ばれた時点からの経過時間を取得
        startPosition = transform.position;
    }

    // Use this for initialization
    void Start()
    {
        //参照の取得
        animator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = new Ray(transform.position /*+ new Vector3(0,0.1f,0)*/, Vector3.down); //ray
       // Ray ray1 = new Ray(transform.position, transform.forward);
        RaycastHit hit; //rayと接触したcolliderの判定
        //debug//
        Debug.DrawLine(ray.origin, transform.position + Vector3.down , Color.red);

        //rayとの当たり判定
        if (Physics.Raycast(ray, out hit, distance))
        {
            Debug.Log(hit.transform.name);
        }
        else
        {
            //Run
            //if (animationFlag[(int)ANIMATION.RUN] || animationFlag[(int)ANIMATION.SUPERRUN])
            //    // if (isGround)
            //    middlePosition.y --;

        }

        //敵の数を取得
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        //エフェクトの再生
        PlayEffect(animationNum);

        //今現在のy地点をに記憶させる
        endPosition.y = transform.position.y;

        //待機中の場合
        if (IsIdle() == true)
        {
            //中間地点xを取得
            middlePosition.x = endPosition.x - nextPosition.x / 2;
        }
        //現在地の処理
        nowPosition(animationNum, animationName);

        //敵が目の前に居たら
        for (int i = 0; i < enemy.Length; i++)
        {
            //attack
            if (animationFlag[(int)ANIMATION.ATTACK] == true)
                if (enemy[i].transform.position.x - transform.position.x <= Constants.MassDistance && transform.position.y > enemy[i].transform.position.y && enemy[i].transform.position.y - transform.position.y >= -0.5f)
                {
                    Destroy(enemy[i]);
                    audioSource.PlayOneShot(Hit);
                    //エフェクト再生
                    EffekseerHandle e_damage = EffekseerSystem.PlayEffect("EnemyDamage", transform.position);
                }
            //superAttack
            if(animationFlag[(int)ANIMATION.SUPERATTACK] == true)
                if (enemy[i].transform.position.x - transform.position.x <= Constants.MassDistance * 3 && transform.position.y > enemy[i].transform.position.y && enemy[i].transform.position.y - transform.position.y >= -0.5f)
                {
                    Destroy(enemy[i]);

                }
        }

        //characterとgroundの判定
        if(controller.isGrounded)
        {
            isGround = true;
        }
        else
        {
            isGround = false;
        }
        isGroundOld = isGround;
    }

    //----------------------------------------------------------------------
    //! @brief 今待機中かどうか
    //!
    //! @param[in] なし
    //!
    //! @return idelFlag
    //----------------------------------------------------------------------
    public bool IsIdle()
    {
        //待機中の場合
        if (animationFlag[(int)ANIMATION.RUN] == false && animationFlag[(int)ANIMATION.JUMP] == false && animationFlag[(int)ANIMATION.ATTACK] == false &&
            animationFlag[(int)ANIMATION.SUPERRUN] == false && animationFlag[(int)ANIMATION.SUPERJUMP] == false && animationFlag[(int)ANIMATION.SUPERATTACK] == false)
        {
            idleFlag = true;
        }
        //そうでない場合
        else
        {
            idleFlag = false;
        }
        return idleFlag;
    }

    //----------------------------------------------------------------------
    //! @brief 現在地の処理
    //!
    //! @param[in] アニメーションの番号、アニメーションの名前
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void nowPosition(int animationFlagNum, System.String animation)
    {
        //カードをセットした
        if (cardSetFlag == true)
        {

            //中間地点yを取得
            middlePosition.y = endPosition.y;

            //場所を記憶させる
            startPosition = transform.position;
            //アニメーション
            animationFlag[animationFlagNum] = true;
            //時間の計測
            startTime = Time.timeSinceLevelLoad;
            switch (animationFlagNum)
            {
                case (int)ANIMATION.RUN:
                    break;
                case (int)ANIMATION.JUMP:
                    middlePosition = new Vector3(middlePosition.x + nextPosition.x / 2, middlePosition.y += jumpPower, 0);
                    endPosition = new Vector3(endPosition.x + nextPosition.x, endPosition.y, 0);
                    break;
                case (int)ANIMATION.ATTACK:
                    middlePosition = new Vector3(transform.position.x, middlePosition.y, 0);
                    endPosition = new Vector3(transform.position.x, endPosition.y, 0);
                    break;
                //スーパーシリーズ//
                case (int)ANIMATION.SUPERRUN:
                    middlePosition = new Vector3(middlePosition.x + nextPosition.x, middlePosition.y, 0);
                    endPosition = new Vector3(endPosition.x + nextPosition.x * 2, endPosition.y, 0);
                    break;
                case (int)ANIMATION.SUPERJUMP:
                    middlePosition = new Vector3(middlePosition.x + nextPosition.x / 2, middlePosition.y += jumpPower * 2, 0);
                    endPosition = new Vector3(endPosition.x + nextPosition.x, endPosition.y, 0);
                    break;
                case (int)ANIMATION.SUPERATTACK:
                    middlePosition = new Vector3(transform.position.x, middlePosition.y, 0);
                    endPosition = new Vector3(transform.position.x, endPosition.y, 0);
                    break;

            }

        }
        //アニメーションが実行された
        if (animationFlag[animationFlagNum] == true)
        {

            cardSetFlag = false;
            //アニメーション
            animator.SetBool(animation, true);
            //経過時間
            diff = Time.timeSinceLevelLoad - startTime;
            //進行率
            var rate = diff / time;

            //等速で移動させる
            transform.position = Vector3.Lerp(startPosition, middlePosition, rate);
            //中間地点を超えたら
            if (diff > time)
            {
                startPosition.y = middlePosition.y;
                //等速で移動させる
                transform.position = Vector3.Lerp(startPosition, endPosition, rate / 2);
                //endPositionに到着
                if (diff > time * 2)
                {
                    animationFlag[animationFlagNum] = false;
                    //次の場所との差
                    endPosition += nextPosition;
                }
            }

        }
        //止める
        else if (animationFlag[animationFlagNum] == false)
        {
            animator.SetBool(animation, false);
           
        }

    }

    //----------------------------------------------------------------------
    //! @brief カードの情報を取得
    //!
    //! @param[in] カードの種類
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void ActionPlay(CardManagement.CardType type)
    {
        if (isGround == true)
        {
            switch (type)
            {
                case CardManagement.CardType.Move:
                    audioSource.PlayOneShot(Move);
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.RUN;
                    animationName = "Run";
                    //エフェクト再生
                    EffekseerHandle run = EffekseerSystem.PlayEffect("smoke", transform.position);
                    break;

                case CardManagement.CardType.Jump:
                    audioSource.PlayOneShot(Jump);
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.JUMP;
                    animationName = "Jump";
                    break;

                case CardManagement.CardType.Attack:
                    audioSource.PlayOneShot(Attack);
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.ATTACK;
                    animationName = "Attack";
                    //エフェクト再生
                    EffekseerHandle attack = EffekseerSystem.PlayEffect("attake", transform.position);
                    break;

                // 辻 // スーパーシリーズ // 辻 //
                case CardManagement.CardType.SuperMove:
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.SUPERRUN;
                    animationName = "Run";
                    break;

                case CardManagement.CardType.SuperJump:
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.SUPERJUMP;
                    animationName = "Jump";
                    break;

                case CardManagement.CardType.SuperAttack:
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.SUPERATTACK;
                    animationName = "Attack";
                    break;

                //終了カード
                case CardManagement.CardType.Finish:
                    cardSetFlag = true;
                    animationNum = (int)ANIMATION.ATTACK;
                    animationName = "Over";
                    // 五秒後にゲームオーバー
                    GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(2);

                    break;
            }
        }
    }

    //----------------------------------------------------------------------
    //! @brief 敵との当たり判定
    //!
    //! @param[in] Collider
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OnTriggerEnter(Collider coll)
    {
        //プレイヤーと敵が当たったら
        if (coll.gameObject.tag == "Enemy")
        {
            //エフェクト再生
            EffekseerHandle p_damage = EffekseerSystem.PlayEffect("PlayerDamage", transform.position);
            // 五秒後にゲームオーバー
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(2, ToResultScene.OverType.FALL);
        }

    }
    //----------------------------------------------------------------------
    //! @brief ステージオブジェクトとの当たり判定
    //!
    //! @param[in] ControllerColliderHit
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OnControllerColliderHit(ControllerColliderHit hit)
    {
        //ゴール
        if (hit.gameObject.tag == "Goal")
        {
            // 五秒後にゲームオーバー
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToClear(5);
        }
        //トゲ
        if (hit.gameObject.tag == "Thorn")
        {
            // 五秒後にゲームオーバー
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(5);
        }
        //地面
        if (isGround == false && isGroundOld == false)
        {
            if (hit.gameObject.tag == "Untagged")
            {
                if (diff > time)
                {
                    EffekseerHandle jump = EffekseerSystem.PlayEffect("Landing", transform.position);
                }
            }
        }
    }

    //----------------------------------------------------------------------
    //! @brief エフェクトを再生する関数
    //!
    //! @param[in] inputAnimeNum(今の動作を取得)
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayEffect(int anime_num)
    {
        //待機中でないなら
        if (idleFlag == false)
        {
            switch (anime_num)
            {
                case (int)ANIMATION.RUN:
                    //エフェクトを設定した間隔で再生
                    effect_count++;
                    if (effect_count >= Constants.MoveCount)
                    {
                        EffekseerHandle attack = EffekseerSystem.PlayEffect("smoke", transform.position);
                        effect_count = 0;
                    }
                    break;
            }
        }
    }
    //----------------------------------------------------------------------
    //! @brief 地面についているか
    //!
    //! @param[in] なし
    //!
    //! @return 地面についているか
    //----------------------------------------------------------------------
    public bool IsGround()
    {
        return isGround;
    }

    // アニメーションを止める
    public void AnimationStop()
    {
        animator.SetBool(animationName, false);
    }
}

