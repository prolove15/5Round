using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class UI_Phases : MonoBehaviour
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
    [SerializeField]
    public GameObject bUnitsUIPanel_GO;

    [SerializeField]
    public Text bUnitsUIText_Cp;

    [SerializeField]
    public Animator bUnitsAnim_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

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

        SetComponents();

        InitVariables();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
    }

    //--------------------------------------------------
    void InitVariables()
    {
        SetActive_bUnitsUIPanel(false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle selecting battle units ui
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle selecting battle units ui

    //--------------------------------------------------
    public void SetbUnitsUIPanelStatus(string text, int playerId_pr, params int[] unitIds_pr)
    {
        //
        bUnitsUIText_Cp.text = text;

        //
        List<UnitCard> bUnit_Cps_tp = controller_Cp.player_Cps_de[playerId_pr].bUnit_Cps;

        List<UnitUI_Phases> bUnitUI_Cps_tp = new List<UnitUI_Phases>();

        foreach (UnitUI_Phases bUnitUI_Cp_tp in bUnitsUIPanel_GO.GetComponentsInChildren<UnitUI_Phases>())
        {
            bUnitUI_Cps_tp.Add(bUnitUI_Cp_tp);
        }

        for (int i = 0; i < bUnit_Cps_tp.Count; i++)
        {
            bUnitUI_Cps_tp[i].frontSprite = bUnit_Cps_tp[i].frontSide;
            bUnitUI_Cps_tp[i].hp = bUnit_Cps_tp[i].unitInfo.curHp;

            if (unitIds_pr.Contains(i))
            {
                bUnitUI_Cps_tp[i].GetComponent<Button>().interactable = false;
            }
            else
            {
                bUnitUI_Cps_tp[i].GetComponent<Button>().interactable = true;
            }
        }
    }

    //--------------------------------------------------
    public void SetActive_bUnitsUIPanel(bool activeFlag)
    {
        bUnitsUIPanel_GO.SetActive(activeFlag);
    }

    #endregion

}
