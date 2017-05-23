//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
//! @file   PlayerAction
//!
//! @brief  プレイヤーの移動
//!
//! @date   2017/04/27
//!
//! @author N.Sakuma
//__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/__/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//定数の定義
static class Constants
{
    public const int Attack = 1;        //attak
    public const int StageHeight = 2;  //ステージの高さ
    public const int RunPow = 2;       //走る距離
    public const int SuperAttack = 3;  //superattack
    public const int MaxEnemy = 4;     //敵の数
    public const int MaxJumpPow = 5;   //最大のジャンプ力
    public const int MaxAnimation = 6; //最大のアニメーションの数
    public const int MaxTime = 10;     //最大時間
    public const int MoveCount = 60;   //移動エフェクトのループ再生する間隔

    public const float Adjustment = 0.5f;   //調整
    public const float MassDistance = 2.2f; //マスの距離
}
//アニメーション
enum ANIMATION { RUN, JUMP, ATTACK, OVER };
public class PlayerAction : MonoBehaviour
{
    private int effect_count = 0;   //エフェクト再生用のカウント
    private int animationNum = 0;   //アニメーションの番号

    private float time = 0.5f; //時間
    private float jumpPower = 2.14f;//ジャンプ
    private float distance = 1.0f; //rayの長さを決める
    private float diff;             //経過時間
    private float startTime;        //走り始めた時間

    private bool[] animationFlag = new bool[Constants.MaxAnimation];   //アニメーションしているかどうかのフラグ
    private bool idleFlag;      //待機かどうかのフラグ
    private bool cardSetFlag;   //カードがセットされたかどうかのフラグ
    private bool isGround;      //地面についているかのフラグ
    private Vector3 middlePosition; 　                    //中間地点
    private Vector3 endPosition = new Vector3(2, 0, 0);  //走り終わる場所
    private Vector3 nextPosition = new Vector3(2, 0, 0);  //次の場所
    private Vector3 startPosition;                        //走り始める場所

    private System.String animationName;  //アニメーションの名前
    private GameObject[] enemy;           //敵
    private AudioSource audioSource;     //音
    private Animator animator;        //アニメーター
    private CharacterController controller;  //charactercontroller

    //Resultを動かすためのフラグ
    public bool ClearFlag = false;
    public bool OverFlag = false;

    // クリアコンポーネント
    public GameObject T_GameClear;
    // オーバーコンポーネント
    public GameObject T_GameOver;

    private GameObject GameOver;

    //ゲーム終了時に表示するボタン
    public GameObject TitleButton;
    public GameObject SelectButton;
    //ボードを消す
    public GameObject CanvasBord;
    public GameObject CanvasSpeedButton;
    public GameObject CanvasResetButton;
    public GameObject CanvasSetButton;
    public GameObject CanvasPlayButton;

    //カメラのポジション
    Vector3 CameraPos;

    //下降する座標
    private float FallPos = 8.0f;

    //下降する値
    private float FallNum = 0.05f;

    //ループする回数
    private int RoopCnt = 10;

    //下降する座標(y座標)
    private float FallPosY;

    //下降する座標(z座標)
    private float FallPosZ = 1;

    //音
    public AudioClip Attack;
    public AudioClip Jump;
    public AudioClip Hit;
    public AudioClip Move;

    //パーティクルの種類
    const int NONE = 0;
    const int MOVE = 1;
    const int ATTACK = 2;
    const int DAMAGE = 3;
    const int LANDING = 4;
    //パーティカルの種類判別用
    public int particleType;

    ////int layerMask = 1 << LayerMask.NameToLayer("Untagged");
    RaycastHit slideHit;
    bool isSliding;
    bool isSlidisgOld;
    float h;
    float v;
    float speed;
    float jumpspped = 30f;
    public float gravity = 5.8f;
    float slideSpeed = 30.0f;
    Vector3 dir;
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
        h = v = 0;
        speed = 20;
        dir = Vector3.zero;
        //Physics.gravity = new Vector3(0, 20.81f, 0);
        //参照の取得
        animator = GetComponent<Animator>();
        audioSource = gameObject.GetComponent<AudioSource>();
        controller = GetComponent<CharacterController>();

    }

    // Update is called once per frame
    void Update()
    {

        //カメラのポジション
        CameraPos = GameObject.FindGameObjectWithTag("MainCamera").transform.position;
        //OverPosに代入
        FallPosY = CameraPos.y / 2 + 10;

        //走っている場合
        if (animationFlag[(int)ANIMATION.RUN])
        {
            if (Vector3.Angle(slideHit.normal, Vector3.up) > controller.slopeLimit)
            {
                isSliding = true;
                isSlidisgOld = true;
            }
            else
            {
                isSliding = false;
            }
        }
        Debug.Log(isSliding);
        //敵の数を取得
        enemy = GameObject.FindGameObjectsWithTag("Enemy");

        //エフェクトの再生
        PlayEffect(animationNum);

        //プレイヤーの重力による座標の処理
        GravityForPlayer();

        //待機中の場合
        if (IsIdle())
        {
            //中間地点yを取得
            middlePosition.y = transform.position.y;

            //中間地点xを取得
            middlePosition.x = endPosition.x - nextPosition.x / 2;
        }
        //アクションを決める
        SetAction(animationNum);
        //プレイヤーの移動
        PlayerMove(animationNum, animationName);

        if (Physics.Raycast(transform.position, Vector3.forward, out slideHit))
        {
            //敵との当たり判定
            for (int i = 0; i < enemy.Length; i++)
            {
                //attack
                if (animationFlag[(int)ANIMATION.ATTACK] == true)
                    //PlayerAttack(i, Constants.Attack);
                    Destroy(enemy[i]);
            }

        }
        //characterとgroundの判定
        if (controller.isGrounded)
        {
            isGround = true;
            if (isSliding)
            {
                //middlePosition.x = transform.position.x + Constants.RunPow / 2;
                //endPosition.x = transform.position.x + Constants.RunPow;
                ////カードセットの処理を止める
                //cardSetFlag = false;
                ////アニメーション
                //animator.SetBool(0, true);
                ////経過時間
                //diff = Time.time - startTime;
                ////進行率
                //var rate = diff / time;

                ////等速で移動させる
                //transform.position = Vector3.Lerp(startPosition, middlePosition, rate);
                ////中間地点を超えたら
                //if (diff > time)
                //{
                //    //middlePosの情報をstartPosに代入
                //    startPosition.y = middlePosition.y;
                //    //等速で移動させる
                //    transform.position = Vector3.Lerp(startPosition, endPosition, rate / 2);
                //    //endPositionに到着
                //    if (diff > time * 2)
                //    {

                //        //animationを止めるフラグ
                //        animationFlag[0] = false;
                //        //アニメーションを止める
                //        animator.SetBool(0, false);
                //        //次の場所との差
                //        endPosition += nextPosition;
                //        particleCnt = 0;
                //    }
                //}

                Vector3 hitNormal = slideHit.normal;
                dir.x = hitNormal.x * 10;
                dir.y = gravity * Time.deltaTime;
                dir.z = hitNormal.z;
                transform.position += dir * Time.deltaTime * 1.1f;
                //transform.position += new Vector3(0.95f, 0, 0);
                //controller.Move(dir * Time.deltaTime);
            }
        }
        else
        {
            isGround = false;
        }

        //ClearFlagがtrueだったら
        ClearControl();

        //OverFlagがtrueだったら
        OverControl();
    }

    //----------------------------------------------------------------------
    //! @brief プレイヤーの重力による座標の処理
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void GravityForPlayer()
    {
        //走っている場合
        if (animationFlag[(int)ANIMATION.RUN])
        {
            if (isGround)       //地面についている
                middlePosition.y = transform.position.y;    //中央地点yを今のプレイヤーの座標にする
            if (!isGround)      //地面についていない
                middlePosition.y -= 1.0f;                   //中央地点yを引く
        }
        //中央地点yがステージの高さより低い場合
        if (middlePosition.y < Constants.StageHeight)
            middlePosition.y = Constants.StageHeight;   //ステージの高さにする
        //今現在のy地点をに記憶させる
        endPosition.y = transform.position.y;
    }
    //----------------------------------------------------------------------
    //! @brief プレイヤーの攻撃
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayerAttack(int i, int num)
    {
        //プレイヤーの攻撃範囲に敵がいる場合
        if (enemy[i].transform.position.x - transform.position.x <= Constants.MassDistance * num
            && transform.position.y > enemy[i].transform.position.y
            && enemy[i].transform.position.y - transform.position.y >= -Constants.Adjustment)
        {
            //敵を消す
            Destroy(enemy[i]);
            //音を出す
            audioSource.PlayOneShot(Hit);
            ////エフェクト再生
            //EffekseerHandle e_damage = EffekseerSystem.PlayEffect("EnemyDamage", transform.position);
        }
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
        if (animationFlag[(int)ANIMATION.RUN] == false && animationFlag[(int)ANIMATION.JUMP] == false && animationFlag[(int)ANIMATION.ATTACK] == false)// &&
                                                                                                                                                       // animationFlag[(int)ANIMATION.SUPERRUN] == false && animationFlag[(int)ANIMATION.SUPERJUMP] == false && animationFlag[(int)ANIMATION.SUPERATTACK] == false)
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
    //! @brief アクションを決める
    //!
    //! @param[in] アニメーションの番号
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void SetAction(int animationFlagNum)
    {
        //カードをセットした
        if (cardSetFlag == true)
        {
            //場所を記憶させる
            startPosition = transform.position;
            //アニメーション
            animationFlag[animationFlagNum] = true;
            //時間の計測
            startTime = Time.timeSinceLevelLoad;
            //アニメーションの番号を取得
            switch (animationFlagNum)
            {
                //run
                case (int)ANIMATION.RUN:
                    middlePosition.x = transform.position.x + Constants.RunPow / 2;
                    endPosition.x = transform.position.x + Constants.RunPow;
                    break;
                //jump
                case (int)ANIMATION.JUMP:
                    //middlePosにjumpPowを足す
                    middlePosition = new Vector3(transform.position.x + Constants.RunPow, middlePosition.y += jumpPower, 0);
                    //終点を決める
                    endPosition = new Vector3(transform.position.x + Constants.RunPow * 2, endPosition.y, 0);
                    break;
                //attack
                case (int)ANIMATION.ATTACK:
                    //移動しない
                    middlePosition = new Vector3(transform.position.x, middlePosition.y, 0);
                    //移動しない
                    endPosition = new Vector3(transform.position.x, endPosition.y, 0);
                    break;
            }

        }

    }
    //----------------------------------------------------------------------
    //! @brief プレイヤーの移動
    //!
    //! @param[in] アニメーションの番号、アニメーションの名前
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void PlayerMove(int animationFlagNum, System.String animation)
    {
        //アニメーションが実行された
        if (animationFlag[animationFlagNum] == true)
        {
            //カードセットの処理を止める
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
                //middlePosの情報をstartPosに代入
                startPosition.y = middlePosition.y;
                //等速で移動させる
                transform.position = Vector3.Lerp(startPosition, endPosition, rate / 2);
                //endPositionに到着
                if (diff > time * 2)
                {
                    //animationを止めるフラグ
                    animationFlag[animationFlagNum] = false;
                    //アニメーションを止める
                    animator.SetBool(animation, false);
                    //カウントダウンフラグを立てる
                    CardBord board = GameObject.Find("ActionBord").GetComponent<CardBord>();
                    CountDown.SetCountDown(board.GetCardType(board.usingCard - 1));
                    //次の場所との差
                    endPosition += nextPosition;
                    particleType = NONE;        //パーティカルの種類決定
                }
            }

        }
        //テスト用
        ////止める
        //else if (animationFlag[animationFlagNum] == false)
        //{
        //    animator.SetBool(animation, false);

        //}

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
        //地面についている
        if (isGround == true)
        {
            switch (type)
            {
                //move
                case CardManagement.CardType.Move:
                    audioSource.PlayOneShot(Move);      //音
                    cardSetFlag = true;                 //カードセットフラグ
                    animationNum = (int)ANIMATION.RUN;  //アニメーションの番号
                    animationName = "Run";              //アニメーションの名前
                    particleType = MOVE;               //パーティクルの種類決定
                    break;
                //jump
                case CardManagement.CardType.Jump:
                    audioSource.PlayOneShot(Jump);      //音
                    cardSetFlag = true;                 //カードセットフラグ
                    animationNum = (int)ANIMATION.JUMP; //アニメーションの番号
                    animationName = "Jump";             //アニメーションの名前
                    particleType = NONE;        //パーティカルの種類決定
                    break;
                //attack
                case CardManagement.CardType.Attack:
                    audioSource.PlayOneShot(Attack);        //音
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.ATTACK;   //アニメーションの番号
                    animationName = "Attack";               //アニメーションの名前
                    particleType = ATTACK;        //パーティカルの種類決定
                    break;
                case CardManagement.CardType.Count:
                    audioSource.PlayOneShot(Attack);        //音
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.ATTACK;   //アニメーションの番号
                    animationName = "Attack";               //アニメーションの名前
                    //EffekseerHandle attack = EffekseerSystem.PlayEffect("attake", transform.position);
                    particleType = ATTACK;        //パーティカルの種類決定
                    CountDown.SetCountDown(type);
                    break;
                // スーパーシリーズ //
                ////superMove
                //case CardManagement.CardType.SuperMove:
                //    cardSetFlag = true;                         //カードセットフラグ
                //    animationNum = (int)ANIMATION.SUPERRUN;     //アニメーションの番号
                //    animationName = "Run";                      //アニメーションの名前
                //    break;
                ////superJump
                //case CardManagement.CardType.SuperJump:
                //    cardSetFlag = true;                         //カードセットフラグ
                //    animationNum = (int)ANIMATION.SUPERJUMP;    //アニメーションの番号
                //    animationName = "Jump";                     //アニメーションの名前
                //    break;
                ////superAttack
                //case CardManagement.CardType.SuperAttack:
                //    cardSetFlag = true;                         //カードセットフラグ
                //    animationNum = (int)ANIMATION.SUPERATTACK;  //アニメーションの番号
                //    animationName = "Attack";                   //アニメーションの名前
                //    break;

                //finish
                case CardManagement.CardType.Finish:
                    cardSetFlag = true;                     //カードセットフラグ
                    animationNum = (int)ANIMATION.ATTACK;   //アニメーションの番号
                    animationName = "Over";                 //アニメーションの名前
                    // 五秒後にゲームオーバー
                    GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(2);
                    if (GameOver == null)
                    {
                        //プレイヤーのアクションを止める
                        AnimationStop();
                        //"Over"を生成
                        GameOver = Instantiate(T_GameOver);
                        //Overを画面外にセット
                        GameOver.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
                        //Overの文字を移動するためのフラグをonに
                        OverFlag = true;
                    }

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
            // 五秒後にゲームオーバー
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(0, ToResultScene.OverType.FALL);
            particleType = DAMAGE;        //パーティカルの種類決定

            if (GameOver == null)
            {
                //カードボードなどの操作系を消す
                Invoke("SetCanvasActive", 0);

                //プレイヤーのアクションを止める
                AnimationStop();
                //"Over"を生成
                GameOver = Instantiate(T_GameOver);
                //Overを画面外にセット
                GameOver.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
                //Overの文字を移動するためのフラグをonに
                OverFlag = true;
            }
        }
        else
        {
            particleType = NONE;        //パーティカルの種類決定
        }

        //プレイヤーがゴールについたら
        if (coll.gameObject.tag == "Goal")
        {
            //カードボードなどの操作系を消す
            Invoke("SetCanvasActive", 0);
            //プレイヤーのアクションを止める
            AnimationStop();
            //"CLEAR"を生成
            T_GameClear = Instantiate(T_GameClear);
            //CLEARを画面外にセット
            T_GameClear.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
            //CLEARの文字を移動するためのフラグをonに
            ClearFlag = true;
        }

        //トゲ
        if (coll.gameObject.tag == "Thorn")
        {
            if (GameOver == null)
            {
                //カードボードなどの操作系を消す
                Invoke("SetCanvasActive", 0);
                //プレイヤーのアクションを止める
                AnimationStop();
                //"Over"を生成
                GameOver = Instantiate(T_GameOver);
                //Overを画面外にセット
                GameOver.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
                //Overの文字を移動するためのフラグをonに
                OverFlag = true;
            }
        }
        // ブロック
        if (coll.gameObject.tag == "Block")
        {
            if (GameOver == null)
            {
                //カードボードなどの操作系を消す
                Invoke("SetCanvasActive", 0);
                //プレイヤーのアクションを止める
                AnimationStop();
                //"Over"を生成
                GameOver = Instantiate(T_GameOver);
                //Overを画面外にセット
                GameOver.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
                //Overの文字を移動するためのフラグをonに
                OverFlag = true;
            }
        }

        //落下限界
        if (coll.gameObject.tag == "GameOverZone")
        {
            if (GameOver == null)
            {
                //カードボードなどの操作系を消す
                Invoke("SetCanvasActive", 0);
                //プレイヤーのアクションを止める
                AnimationStop();
                //"Over"を生成
                GameOver = Instantiate(T_GameOver);
                //Overを画面外にセット
                GameOver.transform.position = new Vector3(CameraPos.x, FallPosY, FallPosZ);
                //Overの文字を移動するためのフラグをonに
                OverFlag = true;
            }
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
            // 五秒後にクリア
            GameObject.Find("GameManager").GetComponent<ToResultScene>().ToClear(3);
        }
        ////トゲ
        //if (hit.gameObject.tag == "Thorn")
        //{
        //    // 五秒後にゲームオーバー
        //    GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(2);
        //}
        //地面
        if (!isGround)
        {
            //tagがUntagged
            if (hit.gameObject.tag == "Untagged")
            {
                //middlePosを超えたら
                if (diff > time)
                {
                    ////エフェクトの再生
                    //EffekseerHandle jump = EffekseerSystem.PlayEffect("Landing", transform.position);

                    particleType = LANDING;        //パーティカルの種類決定
                }
                else
                {
                    particleType = NONE;        //パーティカルの種類決定
                }
            }
        }
        if (hit.gameObject.tag == "Untagged" && IsIdle())
        {
            float pos = transform.position.x;
            if (pos % 2 != 0 /*&& pos != 0*/ && isSlidisgOld == true)
            {
                transform.position = new Vector3(Mathf.FloorToInt(transform.position.x) + 1, transform.position.y, transform.position.z);
                isSlidisgOld = false;
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
        if (!idleFlag)
        {
            switch (anime_num)
            {
                //run
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

    //----------------------------------------------------------------------
    //! @brief アニメーションを止める
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void AnimationStop()
    {
        animator.SetBool(animationName, false);
    }
    //----------------------------------------------------------------------
    //! @brief ボタンの表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetButtonOn()
    {
        TitleButton.SetActive(true);
        SelectButton.SetActive(true);
    }

    //----------------------------------------------------------------------
    //! @brief ボタンの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetButtonOff()
    {
        TitleButton.SetActive(false);
        SelectButton.SetActive(false);
    }
    //----------------------------------------------------------------------
    //! @brief キャンバスの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void SetCanvasActive()
    {
        CanvasBord.SetActive(false);
        CanvasSpeedButton.SetActive(false);
        CanvasResetButton.SetActive(false);
        CanvasSetButton.SetActive(false);
        CanvasPlayButton.SetActive(false);
    }

    //----------------------------------------------------------------------
    //! @brief ゲーム終了時のフラグ管理()
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void ClearControl()
    {
        //ClearFlagがtrueだったら
        if (ClearFlag)
        {
            //Clearの座標が画面の中心に行くまで下がっていく
            if (T_GameClear.transform.position.y > FallPos)
            {
                for (int i = 0; i < RoopCnt; i++)
                {
                    T_GameClear.transform.position -= new Vector3(0, FallNum, 0);
                }
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<ToResultScene>().ToClear(0);
                Invoke("SetButtonOn", 2.0f);
            }
        }
    }
    //----------------------------------------------------------------------
    //! @brief ゲーム終了時のフラグ管理()
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void OverControl()
    {
        //OverFlagがtrueだったら
        if (OverFlag)
        {
            //Overの座標が画面の中心に行くまで下がっていく
            if (GameOver.transform.position.y > FallPos)
            {
                for (int i = 0; i < RoopCnt; i++)
                {
                    GameOver.transform.position -= new Vector3(0, FallNum, 0);
                }
            }
            else
            {
                GameObject.Find("GameManager").GetComponent<ToResultScene>().ToOver(0);
                Invoke("SetButtonOn", 2.0f);
            }
        }
    }
}


