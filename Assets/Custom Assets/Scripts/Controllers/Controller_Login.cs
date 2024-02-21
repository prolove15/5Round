using FiveRound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Controller_Login : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing,
        SignInUpFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] UI_TopCanvas topUI_Cp;
    [SerializeField] CanvasUI curtainUI_Cp;
    [SerializeField] NoticePanel noticePanel_Cp;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {
        curtainUI_Cp.gameObject.SetActive(true);

        Init();
    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public void AddMainGameState(GameState_En value = GameState_En.Nothing)
    {
        if (gameStates.Count == 0)
        {
            gameStates.Add(value);
        }
    }

    //--------------------------------------------------
    public void AddGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value_tp in values)
        {
            gameStates.Add(value_tp);
        }
    }

    //--------------------------------------------------
    public bool ExistGameStates(params GameState_En[] values)
    {
        bool result = true;
        foreach (GameState_En value in values)
        {
            if (!gameStates.Contains(value))
            {
                result = false;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public bool ExistAnyGameStates(params GameState_En[] values)
    {
        bool result = false;
        foreach (GameState_En value in values)
        {
            if (gameStates.Contains(value))
            {
                result = true;
                break;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En value)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == value)
            {
                result++;
            }
        }

        return result;
    }

    //--------------------------------------------------
    public void RemoveGameStates(params GameState_En[] values)
    {
        foreach (GameState_En value in values)
        {
            gameStates.RemoveAll(gameState_tp => gameState_tp == value);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        StartCoroutine(Corou_Init());
    }

    IEnumerator Corou_Init()
    {
        AddMainGameState(GameState_En.Nothing);

        noticePanel_Cp.Init();

        topUI_Cp.Init();
        yield return new WaitUntil(() => topUI_Cp.mainGameState
            == UI_TopCanvas.GameState_En.Inited);
        topUI_Cp.SetUnityActions(OnLoginBtnClick, OnCreateAccountBtnClick, OnSignupCancelBtnClick, OnSignupBtnClick);

        curtainUI_Cp.Hide(false, () => Handle_SignInUp());

        mainGameState = GameState_En.Inited;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //-------------------------------------------------- ReturnToMain
    public void ReturnToMain()
    {
        curtainUI_Cp.Show(false, () => HandleReturnToMain());
    }

    void HandleReturnToMain()
    {
        SceneManager.LoadScene("Main");
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //-------------------------------------------------- Handle sign in & up
    void Handle_SignInUp()
    {
        StartCoroutine(Corou_Handle_SignInUp());
    }

    IEnumerator Corou_Handle_SignInUp()
    {
        topUI_Cp.Play_Login();
        yield return new WaitUntil(() => topUI_Cp.mainGameState
            == UI_TopCanvas.GameState_En.LoadStarted);

        mainGameState = GameState_En.SignInUpFinished;
    }

    //--------------------------------------------------
    public void Login()
    {

    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Callback from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Callback from external

    //--------------------------------------------------
    void OnLoginBtnClick()
    {
        StartCoroutine(Corou_OnLoginBtnClick());
    }

    IEnumerator Corou_OnLoginBtnClick()
    {
        Login();
        yield return new WaitForSeconds(1f);

        topUI_Cp.Finish_Load();
        yield return new WaitUntil(() => topUI_Cp.mainGameState == UI_TopCanvas.GameState_En.LoadFinished);

        noticePanel_Cp.SetTitle("成果的にログイン", 1f, ReturnToMain);
    }

    //--------------------------------------------------
    void OnCreateAccountBtnClick()
    {

    }

    //--------------------------------------------------
    void OnSignupCancelBtnClick()
    {

    }

    //--------------------------------------------------
    void OnSignupBtnClick()
    {

    }

    #endregion

}
