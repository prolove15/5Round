using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StrPhaseHandler : MonoBehaviour
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
        NextPhase
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] int strPhaseDur = 50;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    ProgressHandler progHandler_Cp;
    List<PlayerFaction> player_Cps = new List<PlayerFaction>();
    PlayerFaction localPlayer_Cp, otherPlayer_Cp, comPlayer_Cp;
    CycleTimeHandler cycleHandler_Cp;

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

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        progHandler_Cp = controller_Cp.progHandler_Cp;
        player_Cps = controller_Cp.player_Cps;
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        otherPlayer_Cp = controller_Cp.otherPlayer_Cp;
        comPlayer_Cp = controller_Cp.comPlayer_Cp;
        cycleHandler_Cp = controller_Cp.ui_gameCanvas_Cp.cycleHandler_Cp;
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
        // set phase name
        progHandler_Cp.SetPhaseName(PhaseNames.strPhase);
        yield return new WaitForSeconds(2f);

        // show curtain
        otherPlayer_Cp.pBoard_Cp.ShowCurtain(true);

        // play phase
        for (int i = 0; i < player_Cps.Count; i++)
        {
            player_Cps[i].PlayStrPhase();
        }
        if (comPlayer_Cp != null)
        {
            SimulateComPlayer();
        }

        // start cycle time
        cycleHandler_Cp.StartCycleTime(strPhaseDur, 5, ForceToNextPhase);
        yield return new WaitForSeconds(1f);

        // activate next phase button
        progHandler_Cp.SetActiveNextPhaseBtn(true);

        mainGameState = GameState_En.PhaseStarted;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// To next phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region To next phase

    //--------------------------------------------------
    public void ToNextPhase()
    {
        StartCoroutine(Corou_ToNextPhase());
    }

    IEnumerator Corou_ToNextPhase()
    {
        progHandler_Cp.SetActiveNextPhaseBtn(false);
        cycleHandler_Cp.StopCycleTime();
        localPlayer_Cp.pBoard_Cp.SetActiveClickable(false);

        // set is ready
        localPlayer_Cp.EndStrPhase();
        otherPlayer_Cp.EndStrPhase();

        // wait all is ready
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => player_Cps[i].isReadyForNextPhase);
        }
        yield return new WaitForSeconds(1f);

        for (int i = 0; i < player_Cps.Count; i++)
        {
            if (player_Cps[i].hasAuthority) { player_Cps[i].isReadyForNextPhase = false; }
        }
        for (int i = 0; i < player_Cps.Count; i++)
        {
            yield return new WaitUntil(() => !player_Cps[i].isReadyForNextPhase);
        }

        //
        mainGameState = GameState_En.PhaseFinished;
    }

    //--------------------------------------------------
    public void ForceToNextPhase()
    {
        if (comPlayer_Cp != null)
        {
            if (corouSimComPlayer != null) { StopCoroutine(corouSimComPlayer); }
            comPlayer_Cp.EndStrPhase();
        }
        ToNextPhase();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Callback from ui
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Callback from ui

    //--------------------------------------------------
    public void OnClickNextPhaseBtn()
    {
        ToNextPhase();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Simulate computer player
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region SimuateComPlayer

    //--------------------------------------------------
    public void SimulateComPlayer()
    {
        corouSimComPlayer = StartCoroutine(Corou_SimulateComPlayer());
    }

    Coroutine corouSimComPlayer;
    IEnumerator Corou_SimulateComPlayer()
    {
        // set token        
        for (int i = 0; i < comPlayer_Cp.roundsData.rndValues.Count; i++)
        {
            bool setSuccessFlag = false;

            while (!setSuccessFlag)
            {
                // check token or marker are exist
                if (comPlayer_Cp.tokensData.restShien == 0 &&
                    comPlayer_Cp.tokensData.restMove == 0 &&
                    comPlayer_Cp.tokensData.restAtk == 0)
                {
                    break;
                }

                //
                int randIndex = Random.Range(0, 4);
                switch (randIndex)
                {
                    case 0:
                        setSuccessFlag = Com_SetShienTokenToRound(i);
                        break;
                    case 1:
                        setSuccessFlag = Com_SetMoveTokenToRound(i);
                        break;
                    case 2:
                        setSuccessFlag = Com_SetAtkTokenToRound(i);
                        break;
                    case 3:
                        setSuccessFlag = true;
                        break;
                }
            }

            yield return new WaitForSeconds(0.5f);
        }

        // set sp marker
        while (comPlayer_Cp.markersData.restSp > 0)
        {
            int randRoundIndex = Random.Range(0, comPlayer_Cp.roundsData.rndValues.Count);

            RoundValue roundValue = comPlayer_Cp.roundsData[randRoundIndex];
            if (roundValue.tokenType == TokenType.Attack || roundValue.tokenType == TokenType.Null)
            {
                Com_SetSpMarkerToRound(randRoundIndex);
            }
        }

        // prepare to finish
        corouSimComPlayer = null;
        comPlayer_Cp.EndStrPhase();
    }

    //--------------------------------------------------
    bool Com_SetSpMarkerToRound(int roundIndex_pr)
    {
        if (comPlayer_Cp.markersData.restSp == 0)
        {
            return false;
        }

        int randCount = Random.Range(1, comPlayer_Cp.markersData.restSp + 1);

        // set round value
        if (comPlayer_Cp.roundsData[roundIndex_pr].actionType == ActionType.Null)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].actionType = ActionType.Guard;
        }
        comPlayer_Cp.roundsData[roundIndex_pr].spCount = randCount;
        comPlayer_Cp.markersData.useSp += randCount;

        return true;
    }

    //--------------------------------------------------
    bool Com_SetShienTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        if (comPlayer_Cp.tokensData.restShien == 0)
        {
            return false;
        }

        //
        int randShienUnitIndex_tp = -1;
        int randShienUnitId_tp = -1;
        do
        {
            randShienUnitIndex_tp = Random.Range(0, comPlayer_Cp.mUnitsData.Count);
            randShienUnitId_tp = comPlayer_Cp.mUnitsData[randShienUnitIndex_tp].id;
        }
        while (comPlayer_Cp.roundsData.ContainsShienUnitId(randShienUnitId_tp));

        int randShienTarUnitIndex_tp = Random.Range(0, 2);

        // set round value
        comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Shien;
        comPlayer_Cp.roundsData[roundIndex_pr].actionType = ActionType.Shien;
        comPlayer_Cp.roundsData[roundIndex_pr].shienUnitId = randShienUnitId_tp;
        comPlayer_Cp.roundsData[roundIndex_pr].oriUnitIndex = randShienUnitIndex_tp;
        comPlayer_Cp.roundsData[roundIndex_pr].tarUnitIndex = randShienTarUnitIndex_tp;
        comPlayer_Cp.tokensData.useShien++;

        return result;
    }

    //--------------------------------------------------
    bool Com_SetMoveTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        if (comPlayer_Cp.tokensData.restMove == 0)
        {
            return false;
        }

        //
        int randMoveRearUnitIndex = Random.Range(0, 3);
        int randMoveVanUnitIndex = Random.Range(0, 2);

        // set round value
        if (randMoveRearUnitIndex == 0)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Move1;
            comPlayer_Cp.tokensData.useMove1++;
        }
        else if (randMoveRearUnitIndex == 1)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Move2;
            comPlayer_Cp.tokensData.useMove2++;
        }
        else if (randMoveRearUnitIndex == 2)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Move3;
            comPlayer_Cp.tokensData.useMove3++;
        }
        comPlayer_Cp.roundsData[roundIndex_pr].actionType = ActionType.Move;
        comPlayer_Cp.roundsData[roundIndex_pr].oriUnitIndex = randMoveVanUnitIndex;
        comPlayer_Cp.roundsData[roundIndex_pr].tarUnitIndex = randMoveRearUnitIndex;

        //
        return result;
    }

    //--------------------------------------------------
    bool Com_SetAtkTokenToRound(int roundIndex_pr)
    {
        bool result = true;

        //
        if (comPlayer_Cp.tokensData.restAtk == 0)
        {
            return false;
        }

        //
        int randAtkAllyUnitIndex = Random.Range(0, 2);
        int randAtkEnemyUnitIndex = Random.Range(0, 2);
        AttackType randAtkType = (AttackType)(Random.Range(1, 4));

        // set round value
        comPlayer_Cp.roundsData[roundIndex_pr].actionType = ActionType.Atk;
        comPlayer_Cp.roundsData[roundIndex_pr].atkType = randAtkType;
        comPlayer_Cp.roundsData[roundIndex_pr].oriUnitIndex = randAtkAllyUnitIndex;
        comPlayer_Cp.roundsData[roundIndex_pr].tarUnitIndex = randAtkEnemyUnitIndex;
        if (randAtkAllyUnitIndex == 0)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Attack1;
            comPlayer_Cp.tokensData.useAtk1++;
        }
        else if (randAtkAllyUnitIndex == 1)
        {
            comPlayer_Cp.roundsData[roundIndex_pr].tokenType = TokenType.Attack2;
            comPlayer_Cp.tokensData.useAtk2++;
        }

        return result;
    }

    #endregion
	//////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle moving to next phase
    /// </summary>
    //////////////////////////////////////////////////////////////////////

}
