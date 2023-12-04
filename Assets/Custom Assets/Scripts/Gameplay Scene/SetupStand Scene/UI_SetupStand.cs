using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_SetupStand : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, WillFinish
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    // common fields
    [SerializeField]
    Text dateTimeText_Cp;

    [SerializeField]
    Text batteryText_Cp;

    [SerializeField]
    Text factionNameText_Cp;

    [SerializeField]
    Text leftTimeText_Cp;

    [SerializeField]
    Button helpBtn_Cp;

    [SerializeField]
    Button interruptBtn_Cp;

    [SerializeField]
    Button surrenderBtn_Cp;

    [SerializeField]
    Text turnIndexText_Cp;

    [SerializeField]
    Text attackPointText_Cp;

    [SerializeField]
    Text goldText_Cp;

    [SerializeField]
    Text opponentReadyStateText_Cp;

    [SerializeField]
    Text instructionText_Cp;

    //
    [SerializeField]
    Button decisionBtn_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_SetupStand controller_Cp;
    
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

    // common properties
    public string dateTime
    {
        set { dateTimeText_Cp.text = value; }
    }

    public string battery
    {
        set { batteryText_Cp.text = value; }
    }

    public string factionName
    {
        set { factionNameText_Cp.text = value; }
    }

    public string leftTime
    {
        set { leftTimeText_Cp.text = value; }
    }

    public bool helpBtnActive
    {
        set { helpBtn_Cp.enabled = value; }
    }

    public bool helpBtnInteract
    {
        set { helpBtn_Cp.interactable = value; }
    }

    public bool interruptionBtnActive
    {
        set { interruptBtn_Cp.enabled = value; }
    }

    public bool interruptBtnInteract
    {
        set { interruptBtn_Cp.interactable = value; }
    }

    public bool surrenderBtnActive
    {
        set { surrenderBtn_Cp.enabled = value; }
    }

    public bool surrenderBtnInteract
    {
        set { surrenderBtn_Cp.interactable = value; }
    }

    public string turnIndex
    {
        set { turnIndexText_Cp.text = value; }
    }

    public string attackPoint
    {
        set { attackPointText_Cp.text = value; }
    }

    public string gold
    {
        set { goldText_Cp.text = value; }
    }

    public string opponentReadyState
    {
        set { opponentReadyStateText_Cp.text = value; }
    }

    public string instruction
    {
        set { instructionText_Cp.text = value; }
    }

    // normal properties
    public bool decisionBtnInteract
    {
        get { return decisionBtn_Cp.interactable; }
        set { decisionBtn_Cp.interactable = value; }
    }

    //-------------------------------------------------- private properties

    #endregion

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    /// Methods
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
    /// Manage gameStates
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
    /// Initialize
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
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_SetupStand>();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// On events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region OnEvents

    //-------------------------------------------------- commont events
    public void OnClickHelpBtn()
    {

    }

    public void OnClickInterruptionBtn()
    {

    }

    public void OnClickSurrenderBtn()
    {

    }

    //--------------------------------------------------
    public void OnClickDecisionBtn()
    {
        controller_Cp.OnClickDecisionBtn();
    }

    #endregion
}
