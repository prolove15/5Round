using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_PartyDecision : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    Button battleStandDecisionBtn_Cp;

    [SerializeField]
    Text battleCostText_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Properties
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public GameState_En mainGameState
    {
        get { return gameStates[0]; }
        set { gameStates[0] = value; }
    }

    public string battleCost
    {
        set { battleCostText_Cp.text = value; }
    }

    //-------------------------------------------------- private properties
    Controller_PartyDecision controller_Cp;

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //------------------------------ Start is called before the first frame update
    void Start()
    {

    }

    //------------------------------ Update is called once per frame
    void Update()
    {

    }

    //////////////////////////////////////////////////////////////////////
    /// ManageGameState
    //////////////////////////////////////////////////////////////////////
    #region ManageGameState

    //------------------------------
    public int GetExistGameStatesNum(GameState_En gameState_pr)
    {
        int stateNum = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                stateNum++;
            }
        }

        return stateNum;
    }

    public void RemoveGameStates(GameState_En gameState_pr)
    {
        while (gameStates.Contains(gameState_pr))
        {
            gameStates.Remove(gameState_pr);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_PartyDecision>();
    }

    #endregion

    //------------------------------
    public void SetActiveBattleStandDecisionBtn(bool flag)
    {
        battleStandDecisionBtn_Cp.interactable = flag;
    }

    //////////////////////////////////////////////////////////////////////
    /// OnEvent
    //////////////////////////////////////////////////////////////////////
    #region OnEvent

    //------------------------------
    public void OnClickDecisionBtn()
    {
        controller_Cp.OnPartyDecided();
    }

    #endregion

}
