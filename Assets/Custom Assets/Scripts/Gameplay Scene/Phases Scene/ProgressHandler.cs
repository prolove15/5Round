using FiveRound;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressHandler : MonoBehaviour
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
        TurnStarted, TurnFinished,
        RoundStarted, RoundFinished,
        StartPhaseStarted, StartPhaseFinished,
        StrPhaseStarted, StrPhaseFinished,
        BattlePhaseStarted, BattlePhaseFinished,
        SupplyPhaseStarted, SupplyPhaseFinished,
        EndPhaseStarted, EndPhaseFinished,
        NextPhaseClicked,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] public GameInfo gameInfo;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player1_Cp, player2_Cp;
    UI_GameCanvas gameUI_Cp;
    UI_PanelCanvas panelUI_Cp;
    NoticePanel modalNotice_Cp;
    RoundHandler rndHandler_Cp;

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
        InitComponents();
        InitVariables();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player1_Cp = controller_Cp.player_Cps[0];
        player2_Cp = controller_Cp.player_Cps[1];
        gameUI_Cp = controller_Cp.ui_gameCanvas_Cp;
        panelUI_Cp = controller_Cp.ui_panelCanvas_Cp;
        modalNotice_Cp = panelUI_Cp.modalNotice_Cp;
        rndHandler_Cp = controller_Cp.rndHandler_Cp;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        rndHandler_Cp.Init();
    }

    //--------------------------------------------------
    void InitVariables()
    {
        gameInfo.turnIndex = -1;
        gameInfo.rndIndex = -1;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play progress
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play progress

    //--------------------------------------------------
    public void PlayProgress()
    {
        StartCoroutine(Corou_PlayProgress());
    }

    IEnumerator Corou_PlayProgress()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayTurn(i);
            yield return new WaitUntil(() => ExistGameStates(GameState_En.TurnFinished));
            RemoveGameStates(GameState_En.TurnFinished);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play turn
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play turn

    //-------------------------------------------------- Play Turn
    void PlayTurn(int turnIndex_tp)
    {
        StartCoroutine(Corou_PlayTurn(turnIndex_tp));
    }

    IEnumerator Corou_PlayTurn(int turnIndex_tp)
    {
        AddGameStates(GameState_En.TurnStarted);

        // set turn index
        SetTurnIndex(turnIndex_tp);
        yield return new WaitForSeconds(1f);

        // play phases
        PlayStartPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.StartPhaseFinished));
        RemoveGameStates(GameState_En.StartPhaseFinished);

        PlayStrPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.StrPhaseFinished));
        RemoveGameStates(GameState_En.StrPhaseFinished);

        PlayBattlePhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.BattlePhaseFinished));
        RemoveGameStates(GameState_En.BattlePhaseFinished);

        PlaySupplyPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.SupplyPhaseFinished));
        RemoveGameStates(GameState_En.SupplyPhaseFinished);

        PlayEndPhase();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.EndPhaseFinished));
        RemoveGameStates(GameState_En.EndPhaseFinished);

        //
        RemoveGameStates(GameState_En.TurnStarted);
        AddGameStates(GameState_En.TurnFinished);
    }

    //--------------------------------------------------
    void SetTurnIndex(int turnIndex_tp)
    {
        gameInfo.turnIndex = turnIndex_tp;
        gameUI_Cp.SetTurnIndex(gameInfo.turnIndex + 1);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play start phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play start phase

    //-------------------------------------------------- 
    void PlayStartPhase()
    {
        StartCoroutine(Corou_PlayStartPhase());
    }

    IEnumerator Corou_PlayStartPhase()
    {
        AddGameStates(GameState_En.StartPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.startPhase);
        modalNotice_Cp.SetContent("開始フェーズ", "AP Up, SPマーカーを受信", 2f);
        yield return new WaitForSeconds(3f);

        // up ap
        UpAp();
        yield return new WaitForSeconds(1f);

        //
        RemoveGameStates(GameState_En.StartPhaseStarted);
        AddGameStates(GameState_En.StartPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play str phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play str phase

    //--------------------------------------------------
    void PlayStrPhase()
    {
        StartCoroutine(Corou_PlayStrPhase());
    }

    IEnumerator Corou_PlayStrPhase()
    {
        AddGameStates(GameState_En.StrPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.strPhase);
        modalNotice_Cp.SetTitle(PhaseNames.strPhase, 2f);
        yield return new WaitForSeconds(3f);

        // set cycle time
        SetActiveCycleTimePanel(true);
        SetCycleTime(120);

        // wait both player ready
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked)); // temporary code
        RemoveGameStates(GameState_En.NextPhaseClicked);

        // disable cycle time
        SetActiveCycleTimePanel(false);

        //
        RemoveGameStates(GameState_En.StrPhaseStarted);
        AddGameStates(GameState_En.StrPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play battle phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play battle phase

    //--------------------------------------------------
    void PlayBattlePhase()
    {
        StartCoroutine(Corou_PlayBattlePhase());
    }

    IEnumerator Corou_PlayBattlePhase()
    {
        AddGameStates(GameState_En.BattlePhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.btlPhase);
        modalNotice_Cp.SetTitle(PhaseNames.btlPhase, 2f);
        yield return new WaitForSeconds(3f);

        // set active round index panel
        gameUI_Cp.SetActiveRndIndexPanel(true);

        // play round
        for (int i = 0; i < 5; i++)
        {
            rndHandler_Cp.PlayRound(i);
            yield return new WaitUntil(() => rndHandler_Cp.ExistGameStates(RoundHandler.GameState_En.RoundFinished));
            rndHandler_Cp.RemoveGameStates(RoundHandler.GameState_En.RoundFinished);
        }

        // reset round index
        gameUI_Cp.SetActiveRndIndexPanel(false);
        gameInfo.rndIndex = -1;

        // wait next phase click
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked));
        RemoveGameStates(GameState_En.NextPhaseClicked);

        //
        RemoveGameStates(GameState_En.BattlePhaseStarted);
        AddGameStates(GameState_En.BattlePhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play supply phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play supply phase

    //--------------------------------------------------
    void PlaySupplyPhase()
    {
        StartCoroutine(Corou_PlaySupplyPhase());
    }

    IEnumerator Corou_PlaySupplyPhase()
    {
        AddGameStates(GameState_En.SupplyPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.supplyPhase);
        modalNotice_Cp.SetTitle(PhaseNames.supplyPhase, 2f);
        yield return new WaitForSeconds(3f);

        // play supply phase

        // wait next phase click
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked));
        RemoveGameStates(GameState_En.NextPhaseClicked);

        //
        RemoveGameStates(GameState_En.SupplyPhaseStarted);
        AddGameStates(GameState_En.SupplyPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play end phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play end phase

    //--------------------------------------------------
    void PlayEndPhase()
    {
        StartCoroutine(Corou_PlayEndPhase());
    }

    IEnumerator Corou_PlayEndPhase()
    {
        AddGameStates(GameState_En.EndPhaseStarted);

        // set phase name
        SetPhaseName(PhaseNames.endPhase);
        modalNotice_Cp.SetTitle(PhaseNames.endPhase, 2f);
        yield return new WaitForSeconds(3f);

        // play end phase

        // wait next phase click
        yield return new WaitUntil(() => ExistGameStates(GameState_En.NextPhaseClicked));
        RemoveGameStates(GameState_En.NextPhaseClicked);

        //
        RemoveGameStates(GameState_En.EndPhaseStarted);
        AddGameStates(GameState_En.EndPhaseFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //--------------------------------------------------
    void SetPhaseName(string phaseName_tp)
    {
        gameUI_Cp.SetPhaseIndex(phaseName_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External methods

    //--------------------------------------------------
    public void UpAp()
    {
        player1_Cp.playerAp++;
        player2_Cp.playerAp++;
        gameUI_Cp.SetAp(0, player1_Cp.playerAp);
        gameUI_Cp.SetAp(1, player2_Cp.playerAp);
    }

    //--------------------------------------------------
    public void SetCycleTime(int time_tp)
    {
        gameUI_Cp.SetCycleTime(time_tp);
    }

    //--------------------------------------------------
    public void SetActiveCycleTimePanel(bool flag)
    {
        gameUI_Cp.SetActiveCycleTimePanel(flag);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External callback
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External callback 

    //--------------------------------------------------
    public void OnClickNextPhase()
    {
        AddGameStates(GameState_En.NextPhaseClicked);
    }

    #endregion
}
