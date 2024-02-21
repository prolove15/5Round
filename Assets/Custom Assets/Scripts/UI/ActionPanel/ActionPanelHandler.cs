using DG.Tweening;
using Michsky.UI.Shift;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ActionPanelHandler : MonoBehaviour
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
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] Animator panelAnim_Cp;
    [SerializeField] MainPanelManager menuHandler_Cp;
    [SerializeField] TextMeshProUGUI headerText_Cp;
    [SerializeField] public TextMeshProUGUI descText_Cp;
    [SerializeField] Button guardBtn_Cp;
    [SerializeField] Button shienBtn_Cp;
    [SerializeField] Button moveBtn_Cp;
    [SerializeField] Button atkBtn_Cp;
    [SerializeField] Action_GuardPanel guard_Cp;
    [SerializeField] Action_ShienPanel shien_Cp;
    [SerializeField] Action_MovePanel move_Cp;
    [SerializeField] Action_AtkPanel atk_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly] public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    PlayerFaction localPlayer_Cp;

    int playerId;
    [SerializeField][ReadOnly] int curRndIndex;

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
    public void Init(PlayerFaction localPlayer_Cp_tp)
    {
        AddMainGameState(GameState_En.Nothing);

        gameObject.SetActive(true);
        SetComponents(localPlayer_Cp_tp);
        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents(PlayerFaction localPlayer_Cp_tp)
    {
        localPlayer_Cp = localPlayer_Cp_tp;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        guard_Cp.Init();
        shien_Cp.Init();
        move_Cp.Init();
        atk_Cp.Init();

        menuHandler_Cp.openPanelAction = OnMenuOpen;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //--------------------------------------------------
    public void ShowPanel(int playerId_tp, int rndId_tp)
    {
        // set variables
        playerId = playerId_tp;
        curRndIndex = rndId_tp;

        // refresh panel
        RefreshActionPanels();

        // show panel
        guard_Cp.RefreshPanel();
        menuHandler_Cp.OpenFirstTab();
        panelAnim_Cp.SetTrigger("show");
    }

    //--------------------------------------------------
    public int GetCurRoundIndex()
    {
        return curRndIndex;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //--------------------------------------------------
    void RefreshActionPanels()
    {
        headerText_Cp.text = "ラウンド " + (curRndIndex + 1).ToString() + " アクション選択";
    }

    //--------------------------------------------------
    void SaveAction()
    {
        switch (menuHandler_Cp.currentPanelIndex)
        {
            case 0: guard_Cp.Save(); break;
            case 1: shien_Cp.Save(); break;
            case 2: move_Cp.Save(); break;
            case 3: atk_Cp.Save(); break;
        }
    }

    //--------------------------------------------------
    void OnMenuOpen(int menuIndex_tp)
    {
        switch (menuIndex_tp)
        {
            case 0: guard_Cp.RefreshPanel(); break;
            case 1: shien_Cp.RefreshPanel(); break;
            case 2: move_Cp.RefreshPanel(); break;
            case 3: atk_Cp.RefreshPanel(); break;
        }
    }

    //--------------------------------------------------
    void ResetActionPanels()
    {
        guard_Cp.ResetPanel();
        shien_Cp.ResetPanel();
        move_Cp.ResetPanel();
        atk_Cp.ResetPanel();
    }

    //--------------------------------------------------
    void RemoveAction()
    {
        RoundValue rndValue_tp = localPlayer_Cp.roundsData[curRndIndex];
        if (rndValue_tp.spCount > 0)
        {
            guard_Cp.Remove();
        }        
        switch (rndValue_tp.actionType)
        {
            case ActionType.Shien: shien_Cp.Remove(); break;
            case ActionType.Move: move_Cp.Remove(); break;
            case ActionType.Atk: atk_Cp.Remove(); break;
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Events from external

    //--------------------------------------------------
    public void OnClickConfirmBtn()
    {
        SaveAction();
        ResetActionPanels();

        panelAnim_Cp.SetTrigger("hide");
    }

    //--------------------------------------------------
    public void OnClickCloseBtn()
    {
        RemoveAction();
        ResetActionPanels();

        panelAnim_Cp.SetTrigger("hide");
    }

    #endregion

}
