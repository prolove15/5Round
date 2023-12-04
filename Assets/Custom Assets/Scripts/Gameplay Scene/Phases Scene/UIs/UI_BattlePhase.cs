using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_BattlePhase : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish,
        Will_ActionTxtShow, Done_ActionTxtShow,
        Will_NoticeTxtShow, Done_NoticeTxtShow,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    GameObject actionEffTextPanel_GO;

    [SerializeField]
    Text actionEffText_Cp;

    [SerializeField]
    Animator actionEffAnim_Cp;

    [SerializeField]
    GameObject noticeEffTextPanel_GO;

    [SerializeField]
    Text noticeEffText_Cp;

    [SerializeField]
    Animator noticeEffAnim_Cp;

    [SerializeField]
    public float actionTxtDur = 3f;

    [SerializeField]
    public float noticeTxtDur = 3f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

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
    public void AddMainGameState(GameState_En value)
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
        AddMainGameState(GameState_En.Nothing);

        //
        InitVariables();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void InitVariables()
    {
        SetActive_ActionEffTextPanel(false);

        SetActive_NoticeEffTextPanel(false);
    }

    //--------------------------------------------------
    public void Init_BtlPhase()
    {
        SetActive_ActionEffTextPanel(true);

        SetActive_NoticeEffTextPanel(true);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Action Effect Text
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Action Effect Text

    //--------------------------------------------------
    public void SetActive_ActionEffTextPanel(bool flag)
    {
        actionEffTextPanel_GO.SetActive(flag);
    }

    //--------------------------------------------------
    public void SetActionEffText(string text)
    {
        StartCoroutine(Corou_SetActionEffText(text));
    }

    IEnumerator Corou_SetActionEffText(string text)
    {
        //
        AddGameStates(GameState_En.Will_ActionTxtShow);

        //
        actionEffText_Cp.text = text;

        actionEffAnim_Cp.SetInteger("flag", 1);

        yield return new WaitForSeconds(actionTxtDur);

        //
        RemoveGameStates(GameState_En.Will_ActionTxtShow);
        AddGameStates(GameState_En.Done_ActionTxtShow);
    }

    //--------------------------------------------------
    public void FinishActionEffText()
    {
        RemoveGameStates(GameState_En.Done_ActionTxtShow);

        actionEffAnim_Cp.SetInteger("flag", -1);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Notice Effect Text
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Notice Effect Text

    //--------------------------------------------------
    public void SetActive_NoticeEffTextPanel(bool flag)
    {
        noticeEffTextPanel_GO.SetActive(flag);
    }

    //--------------------------------------------------
    public void SetNoticeEffText(string text)
    {
        StartCoroutine(Corou_SetNoticeEffText(text));
    }

    IEnumerator Corou_SetNoticeEffText(string text)
    {
        //
        AddGameStates(GameState_En.Will_NoticeTxtShow);

        //
        noticeEffText_Cp.text = text;
        noticeEffAnim_Cp.SetInteger("flag", 1);

        yield return new WaitForSeconds(noticeTxtDur);

        //
        RemoveGameStates(GameState_En.Will_NoticeTxtShow);
        AddGameStates(GameState_En.Done_NoticeTxtShow);
    }

    //--------------------------------------------------
    public void FinishNoticeEffText()
    {
        RemoveGameStates(GameState_En.Done_NoticeTxtShow);

        noticeEffAnim_Cp.SetInteger("flag", -1);
    }

    #endregion

}
