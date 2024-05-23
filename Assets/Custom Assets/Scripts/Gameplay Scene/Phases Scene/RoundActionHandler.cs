using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RoundActionHandler : MonoBehaviour
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
        RoundActionStarted, RoundActionFinished,
        ActionDone,
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
    PlayerFaction myPlayer_Cp;
    RoundValue rndValue;

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
    public void Init(PlayerFaction player_Cp_tp)
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents(player_Cp_tp);

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents(PlayerFaction player_Cp_tp)
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        myPlayer_Cp = player_Cp_tp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play round action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play round action

    //--------------------------------------------------
    public void PlayRoundAction(int rndIndex_tp)
    {
        StartCoroutine(Corou_PlayRoundAction(rndIndex_tp));
    }

    IEnumerator Corou_PlayRoundAction(int rndIndex_tp)
    {
        mainGameState = GameState_En.RoundActionStarted;

        // set round value
        rndValue = myPlayer_Cp.roundsData[rndIndex_tp];
        DebugHandler.Log("RoundActionHandler, rndIndex = " + rndIndex_tp);

        // play action
        Hash128 hash_tp = HashHandler.RegRandHash();
        switch (rndValue.actionType)
        {
            case ActionType.Guard:
                PlayGuardAction();
                break;
            case ActionType.Shien:
                PlayShienAction();
                break;
            case ActionType.Move:
                PlayMoveAction();
                break;
            case ActionType.Atk:
                PlayAtkAction();
                break;
            default:
                PlayEmptyAction();
                break;
        }

        yield return new WaitUntil(() => ExistGameStates(GameState_En.ActionDone));
        RemoveGameStates(GameState_En.ActionDone);

        //
        mainGameState = GameState_En.RoundActionFinished;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play guard action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play guard action

    //--------------------------------------------------
    void PlayGuardAction()
    {
        StartCoroutine(Corou_PlayGuardAction());
    }

    IEnumerator Corou_PlayGuardAction()
    {
        DebugHandler.Log("Corou_PlayGuardAction");
        yield return new WaitForSeconds(1f);
        AddGameStates(GameState_En.ActionDone);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play shien action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play shien action

    //--------------------------------------------------
    void PlayShienAction()
    {
        StartCoroutine(Corou_PlayShienAction());
    }

    IEnumerator Corou_PlayShienAction()
    {
        DebugHandler.Log("Corou_PlayShienAction");
        yield return new WaitForSeconds(1f);
        AddGameStates(GameState_En.ActionDone);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play move action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play move action

    //--------------------------------------------------
    void PlayMoveAction()
    {
        StartCoroutine(Corou_PlayMoveAction());
    }

    IEnumerator Corou_PlayMoveAction()
    {
        DebugHandler.Log("Corou_PlayMoveAction");
        yield return new WaitForSeconds(1f);
        AddGameStates(GameState_En.ActionDone);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play atk action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play atk action

    //--------------------------------------------------
    void PlayAtkAction()
    {
        StartCoroutine(Corou_PlayAtkAction());
    }

    IEnumerator Corou_PlayAtkAction()
    {
        DebugHandler.Log("Corou_PlayAtkAction");
        yield return new WaitForSeconds(1f);
        AddGameStates(GameState_En.ActionDone);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Play empty action
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Play empty action

    //--------------------------------------------------
    void PlayEmptyAction()
    {
        StartCoroutine(Corou_PlayEmptyAction());
    }

    IEnumerator Corou_PlayEmptyAction()
    {
        DebugHandler.Log("Corou_PlayEmptyAction");
        yield return new WaitForSeconds(5f);
        AddGameStates(GameState_En.ActionDone);
        yield return null;
    }

    #endregion

}
