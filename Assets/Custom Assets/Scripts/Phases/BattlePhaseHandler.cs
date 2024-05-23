using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattlePhaseHandler : MonoBehaviour
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
        PhaseStarted, PhaseFinished,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction localPlayer_Cp, otherPlayer_Cp;
    ProgressHandler progHandler_Cp;
    UI_GameCanvas gameUI_Cp;
    RoundHandler rndHandler_Cp;
    GameInfo gameInfo;

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
    public void Init()
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
        progHandler_Cp = controller_Cp.progHandler_Cp;
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
        rndHandler_Cp = controller_Cp.progHandler_Cp.rndHandler_Cp;
        gameInfo = progHandler_Cp.gameInfo;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play phase

    //--------------------------------------------------
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        mainGameState = GameState_En.PhaseStarted;

        // set phase name
        progHandler_Cp.SetPhaseName(PhaseNames.btlPhase);
        yield return new WaitForSeconds(2f);

        // show rounds data on player board
        ShowRoundsDataOnPb();

        // set active round index panel
        gameUI_Cp.SetActiveRndIndexPanel(true);

        // play round
        if (rndHandler_Cp.hasAuthority)
        {
            rndHandler_Cp.HandleRounds();
        }
        yield return new WaitUntil(() => rndHandler_Cp.ExistGameStates(RoundHandler.GameState_En.RoundsFinished));
        rndHandler_Cp.RemoveGameStates(RoundHandler.GameState_En.RoundsFinished);

        // reset round index
        gameUI_Cp.SetActiveRndIndexPanel(false);

        // set game state
        mainGameState = GameState_En.PhaseFinished;
    }

    //--------------------------------------------------
    void ShowRoundsDataOnPb()
    {
        otherPlayer_Cp.pBoard_Cp.ShowRoundsDataOnPb();
        otherPlayer_Cp.pBoard_Cp.ShowCurtain(false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle moving to next phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle moving to next phase

    //--------------------------------------------------
    public void OnClickToNextPhase()
    {
        rndHandler_Cp.OnClickToNextRound();
    }

    #endregion

}
