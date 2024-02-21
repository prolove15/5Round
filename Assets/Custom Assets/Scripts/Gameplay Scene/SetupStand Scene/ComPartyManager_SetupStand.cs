using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComPartyManager_SetupStand : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish
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
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_SetupStand controller_Cp;

    Party_SetupStand comParty_Cp;

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
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int stateCount = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateCount++;
            }
        }

        return stateCount;
    }

    //--------------------------------------------------
    public bool IsExistGameState(GameState_En gameState_pr)
    {
        return GetExistGameStatesCount(gameState_pr) > 0;
    }

    //--------------------------------------------------
    public void RemoveGameStates(GameState_En gameState_pr)
    {
        while (gameStates.Contains(gameState_pr))
        {
            gameStates.Remove(gameState_pr);
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
        gameStates.Add(GameState_En.Nothing);

        SetComponents();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    public void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_SetupStand>();

        comParty_Cp = controller_Cp.comParty_Cp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Act as com
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ActAsCom

    //--------------------------------------------------
    public void ExchangeUnits()
    {

    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// On events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //--------------------------------------------------
    public void OnPerson_UnitUnHolded()
    {
        comParty_Cp.ExchangeUnits();
    }

    //--------------------------------------------------
    public void OnPerson_ClickedDecisionBtn()
    {

    }

    #endregion
}
