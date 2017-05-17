using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticaleScript : MonoBehaviour
{
    //パーティカル
    ParticleSystem particle;

    //パーティカルの種類
    const int NONE = 0;
    const int MOVE = 1;
    const int ATTACK = 2;
    const int DAMAGE = 3;
    const int LANDING = 4;

    //const int GIMMICK = 2;
    //const int BREAK = 1;

    //パーティカル種類の判別用
    private int particleCnt;
    //private int BlockPartical;
    //private bool flag = false;

    //プレイヤー取得用
    GameObject player;
    PlayerAction act;

    //アクションブロック取得用
    //GameObject block;
    //ActionCountDown actDown;
    //BlockAction blockAction;
    //BlockMove blockAct;

    // Use this for initialization
    void Start()
    {
        //コンポーネントの取得
        particle = GetComponent<ParticleSystem>();

        //プレイヤーの取得
        player = GameObject.Find("unitychan");
        act = player.GetComponent<PlayerAction>();

        //block = GameObject.Find("Block");
        //actDown = block.GetComponent<ActionCountDown>();
        //blockAction = block.GetComponent<BlockAction>() as BlockAction;
        //blockAct = blockAction.GetComponent<BlockMove>() as BlockMove;

        //パーティカルの停止
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのアクションを判別
        particleCnt = act.particleType;

        //BlockPartical = actDown.GetActionType();
        //flag = blockAct.GetFlag();

        //プレイヤーのパーティカルを発生
        PlayerParticle(particleCnt);

        ////ギミックのパーティカルを発生
        //GimmickParticle(int ParticleType)
    }

    //アクションの種類によってパーティカルを発生させる関数
    void PlayerParticle(int ParticleType)
    {
        //プレイヤーのアクションの種類によって発生させるパーティクルを決定
        switch (particleCnt)
        {
            case NONE:
                particle.Stop();
                break;
            case MOVE:
                if (particle.name == "MoveParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case ATTACK:
                if (particle.name == "AttackParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case DAMAGE:
                if (particle.name == "DamageParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case LANDING:
                if (particle.name == "LandingParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
        }
    }

    ////ギミックの種類によってパーティクルを発生させる関数
    //void GimmickParticle(int ParticleType)
    //{
    //    //ギミックの種類によって発生させるパーティクルを決定
    //    switch (BlockPartical)
    //    {
    //        case GIMMICK:
    //            if ((particle.name == "GimmickParticle" || particle.name == "GimmickParticle (1)") && flag == true)
    //            {
    //                particle.Play();
    //            }
    //            break;
    //            //case BREAK:
    //            //    if (particle.name == "BreakParticle" && flag == true)
    //            //    {
    //            //        particle.Play();
    //            //    }
    //            //    break;
    //    }

    //}
}
