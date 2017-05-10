using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ParticaleScript : MonoBehaviour
{
    //パーティカル
    ParticleSystem particle;

    //パーティカルの種類
    const int MOVE = 1;
    const int ATTACK = 2;
    const int DAMAGE = 3;
    const int LANDING = 4;

    const int GIMMICK = 0;
    const int BREAK = 1;

    //パーティカル種類の判別用
    private int particleCnt;
    private int BlockPartical;
    private bool flag = false;

    //プレイヤー取得用
    GameObject player;
    PlayerAction act;
    //アクションブロック取得用
    GameObject block;
    BlockMove blockAct;
    ActionCountDown actDown;

    // Use this for initialization
    void Start ()
    {
        //コンポーネントの取得
        particle = GetComponent<ParticleSystem>();

        player = GameObject.Find("unitychan");
        act = player.GetComponent<PlayerAction>();
        block = GameObject.Find("Block");
        actDown = block.GetComponent<ActionCountDown>();


        //パーティカルの停止
        particle.Stop();
    }

    // Update is called once per frame
    void Update()
    {
        //プレイヤーのアクションを判別
        particleCnt = act.particleCnt;
        //BlockPartical = actDown.m_action_type;
        //flag = actDown.PartTim;

        //アクションの種類によってパーティカルを発生
        switch (particleCnt)
        {
            case 0:
                particle.Stop();
                break;
            case 1:
                if (particle.name == "MoveParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case 2:
                if (particle.name == "AttackParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case 3:
                if (particle.name == "DamageParticle")
                {
                    particle.Play();
                }
                else
                {
                    particle.Stop();
                }
                break;
            case 4:
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

        //アクションの種類によってパーティカルを発生
        switch (BlockPartical)
        {
            case 0:
                if ((particle.name == "GimmickParticle" || particle.name == "GimmickParticle (1)") && flag == true)
                {
                    particle.Play();
                }
                break;
            //case 1:
            //    if (particle.name == "BreakParticle" && flag == true)
            //    {
            //        particle.Play();
            //    }
            //    break;
        }

    }

}
