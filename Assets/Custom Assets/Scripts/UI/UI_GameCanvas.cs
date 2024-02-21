using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI_GameCanvas : MonoBehaviour
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
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] Text p1NameText_Cp, p2NameText_Cp;
    [SerializeField] Text p1ApText_Cp, p2ApText_Cp;
    [SerializeField] Text turnIndexText_Cp;
    [SerializeField] Text rndIndexText_Cp;
    [SerializeField] GameObject rndIndexPanel_GO;
    [SerializeField] Text phaseText_Cp;
    [SerializeField] GameObject phasePanel_GO;
    [SerializeField] Animator cycleTimeAnim_Cp;
    [SerializeField] Text cycleTimeText_Cp;
    [SerializeField] GameObject cycleTimePanel_GO;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    Data_Phases data_Cp;

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
    GameObject hlEff_Pf { get { return data_Cp.hlEff_Pf; } }

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
        InitUI();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        data_Cp = controller_Cp.data_Cp;
    }

    //--------------------------------------------------
    void InitUI()
    {
        SetAp(0, 0);
        SetAp(1, 0);
        SetTurnIndex(0);
        SetRoundIndex(0);
        SetPhaseIndex(PhaseNames.Null);
        SetCycleTime(0);
        SetActiveRndIndexPanel(false);
        SetActiveCycleTimePanel(false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //--------------------------------------------------
    public void SetPlayerName(int playerId_tp, string playerName_tp)
    {
        if (playerId_tp == 0)
        {
            p1NameText_Cp.text = playerName_tp;
        }
        else if (playerId_tp == 1)
        {
            p2NameText_Cp.text = playerName_tp;
        }
    }

    //--------------------------------------------------
    public void SetAp(int playerId_tp, int ap_tp)
    {
        if (playerId_tp == 0)
        {
            p1ApText_Cp.text = "AP: " + ap_tp.ToString();
            GenHighlightEffect(p1ApText_Cp.transform);
        }
        else if (playerId_tp == 1)
        {
            p2ApText_Cp.text = "AP: " + ap_tp.ToString();
            GenHighlightEffect(p2ApText_Cp.transform);
        }
    }

    //--------------------------------------------------
    public void SetTurnIndex(int turnIndex_tp)
    {
        turnIndexText_Cp.text = "ターン " + turnIndex_tp.ToString();
        GenHighlightEffect(turnIndexText_Cp.transform);
    }

    //--------------------------------------------------
    public void SetPhaseIndex(string phaseName_tp)
    {
        phaseText_Cp.text = phaseName_tp;
        GenHighlightEffect(phaseText_Cp.transform);
    }

    //--------------------------------------------------
    public void SetRoundIndex(int roundIndex_tp)
    {
        rndIndexText_Cp.text = "ラウンド " + roundIndex_tp.ToString();
        GenHighlightEffect(rndIndexText_Cp.transform);
    }

    public void SetActiveRndIndexPanel(bool flag)
    {
        rndIndexPanel_GO.gameObject.SetActive(flag);
    }

    //--------------------------------------------------
    public void SetCycleTime(int cycleTime_tp)
    {
        cycleTimeText_Cp.text = cycleTime_tp.ToString();
        GenHighlightEffect(cycleTimeText_Cp.transform);
        //cycleTimeAnim_Cp.SetTrigger("pulse");
    }

    public void SetActiveCycleTimePanel(bool flag)
    {
        cycleTimePanel_GO.SetActive(flag);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //--------------------------------------------------
    void GenHighlightEffect(Transform pos_Tf_tp)
    {
        GameObject hlEff_GO_tp = Instantiate(hlEff_Pf, pos_Tf_tp);
        hlEff_GO_tp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        Destroy(hlEff_GO_tp, 1f);
    }

    #endregion
}
