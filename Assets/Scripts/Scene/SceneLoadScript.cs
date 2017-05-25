using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

using UnityEngine.UI;



public class SceneLoadScript : MonoBehaviour
{
    public AudioClip OK;
    CurtainControl CurtainSystem;
    PlayerAction ButtonActive;

    void Start()
    {
        CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
    }
    //----------------------------------------------------------------------
    //! @brief シーン遷移
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------

    //タイトル画面へ
    public void TitleSceneLoad()
    {
        if (SceneManager.GetActiveScene().name != "StageSelect")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("TitleScene", 2);
    }
    //ステージセレクト画面へ
    public void StageSelectSceneLoad()
    {

        if (SceneManager.GetActiveScene().name != "Title")
        {
            CurtainControl CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
            //カーテンを閉める
            CurtainSystem.curtainOut();
        }
        //遷移先のシーンをロード
        Invoke("StageSelectScene", 2);
        //カーテンを閉める
        CurtainSystem.curtainOut();
    }
    //ステージ1へ
    public void Stage1SceneLoad()
    {
        CurtainSystem = GameObject.Find("Canvas").GetComponent<CurtainControl>();
        //カーテンを閉める
        CurtainSystem.curtainOut();
        //遷移先のシーンをロード
        Invoke("Stage1Scene", 2);
    }

    //----------------------------------------------------------------------
    //! @brief シーンのロード
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    void StageSelectScene()
    {
        SceneManager.LoadScene("StageSelect");
    }
    void Stage1Scene()
    {
        SceneManager.LoadScene("newStage1");
    }
    void TitleScene()
    {
        SceneManager.LoadScene("Title");
    }
    //----------------------------------------------------------------------
    //! @brief ステージ上の遷移用ボタンの非表示
    //!
    //! @param[in] なし
    //!
    //! @return なし
    //----------------------------------------------------------------------
    public void StageButtonActive()
    {
        ButtonActive = GameObject.Find("unitychan").GetComponent<PlayerAction>();
        //ボタンを消す
        ButtonActive.SetButtonOff();
    }
}