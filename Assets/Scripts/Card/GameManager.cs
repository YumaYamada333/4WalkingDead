﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    // カード操作のインターフェース
    public CardManagement cardManage;

    // カード一枚あたりの時間(s)
    public float CPS;

    //// カード間のインターバルタイム
    //public float spaceTime;

    // カード時間
    float cardTime;

    //player
    private GameObject playerAction;

    [SerializeField]
    bool m_gimmick_move_flag = false;

    private bool m_oldGimmickMoveFlag = true;
    // カメラのコントローラー
    CameraControl m_camera;

    // ゲームの状態
    public enum GameState
    {
        SetCard,
        Acttion
    }
    GameState gameState;

    public AudioClip OK;

    // Use this for initialization
    void Start()
    {
        gameState = GameState.SetCard;
        playerAction = GameObject.Find("unitychan");
        m_camera = GameObject.Find("MainCamera").GetComponent<CameraControl>();
    }

    // Update is called once per frame
    void Update()
    {
        // 仮　アクションとカードセットを切り替える
        //if (Input.GetKeyDown(KeyCode.LeftShift))
        //{
        //    gameState++;
        //    if (gameState == GameState.Acttion + 1) gameState = GameState.SetCard;
        //    AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        //    audioSource.PlayOneShot(OK);

        //}

        switch (gameState)
        {

            // カードセット時の処理
            case GameState.SetCard:

                // カードセットの操作を受けつけるようにする
                cardManage.isControlCard = true;
                //cardManage.ActtionCard(true);
                break;

            // アクション時の処理
            case GameState.Acttion:
                // カードセットの操作を受け付けないようにする
                //cardManage.isControlCard = false;
                cardTime += Time.deltaTime;
                //PlayrActionの情報を取得
                PlayerAction player = playerAction.GetComponent<PlayerAction>();
                //待機中で、ギミックが動ていなくて、カウントダウンの値がなくて、滑る床による補完をしていない場合
                if (player.IsIdle() && !GetGimmickFlag()&& CountDown.GetCountDown()== CountDown.CountType.Nothing && !player.IsSlideLerp())
                {
                    //プレイヤーがいることを確認
                    if (player != null)
                    {
                        

                        //プレイヤーが地面にいるなら
                        if (player.IsGround())
                        {
                            GameObject.Find("unitychan").GetComponent<PlayerAction>().ActionPlay(cardManage.ActtionCard(false));
                            cardManage.ApllyUsingCard();


                        }

                    }
                    //cardManage.ActtionCard(false);
                    cardTime = 0.0f;
                }

                // カメラの制御
                if (m_oldGimmickMoveFlag != GetGimmickFlag() && !m_camera.GetCameraMove())
                {
                    if (GetGimmickFlag())
                        m_camera.ResetCamera(0.5f);
                    else
                        m_camera.SetFocusObject(playerAction, new Vector3(0, 1, -10), true, 0.5f);

                    m_oldGimmickMoveFlag = m_gimmick_move_flag;
                }
                break;

        }

    }

    public void Play()
    {
        gameState++;
        if (gameState == GameState.Acttion + 1) gameState = GameState.SetCard;
        AudioSource audioSource = gameObject.GetComponent<AudioSource>();
        audioSource.PlayOneShot(OK);
    }
    public GameState GetGameState()
    {
        return gameState;
    }

    public bool GetGimmickFlag()
    {
        return m_gimmick_move_flag;
    }

    public void SetGimmickFlag(bool flag)
    {
        m_gimmick_move_flag = flag;
    }
}
