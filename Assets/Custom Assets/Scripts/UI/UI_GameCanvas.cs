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
    [SerializeField] Text turnIndexText_Cp;
    [SerializeField] Text rndIndexText_Cp;
    [SerializeField] GameObject rndIndexPanel_GO;
    [SerializeField] Text phaseText_Cp;
    [SerializeField] GameObject phasePanel_GO;
    [SerializeField] GameObject cycleTimePanel_GO;
    [SerializeField] GameObject nextPhaseBtn_GO;
    [SerializeField] public List<GameUI_PlayerUI> playerUI_Cps;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    Data_Phases data_Cp;
    [ReadOnly] public CycleTimeHandler cycleHandler_Cp;
    [ReadOnly] public GameUI_PlayerUI localPlayerUI_Cp, otherPlayerUI_Cp;

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
        InitComponents();
        InitUI();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        data_Cp = controller_Cp.data_Cp;
        cycleHandler_Cp = cycleTimePanel_GO.GetComponent<CycleTimeHandler>();
        localPlayerUI_Cp = playerUI_Cps[0]; // it will be modified later
        otherPlayerUI_Cp = playerUI_Cps[1]; // it will be modified later
    }

    //--------------------------------------------------
    void InitComponents()
    {
        cycleHandler_Cp.Init();
    }

    //--------------------------------------------------
    void InitUI()
    {
        SetAp(0, 0);
        SetAp(1, 0);
        SetTurnIndex(0);
        SetRoundIndex(0);
        SetPhaseIndex(PhaseNames.Null);
        SetActiveRndIndexPanel(false);
        SetActiveNextPhaseBtn(false);
        SetIsReadyForNextPhase(0, false);
        SetIsReadyForNextPhase(1, false);
        for (int i = 0; i < 2; i++)
        {
            playerUI_Cps[i].SetActiveFireImage(false);
        }
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
        playerUI_Cps[playerId_tp].SetPlayerName(playerName_tp);
    }

    //--------------------------------------------------
    public void SetAp(int playerId_tp, int ap_tp)
    {
        playerUI_Cps[playerId_tp].SetPlayerAp(ap_tp);
        GenHighlightEffect(playerUI_Cps[playerId_tp].playerApText_Cp.transform);
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
    public void SetActiveNextPhaseBtn(bool flag)
    {
        nextPhaseBtn_GO.SetActive(flag);
    }

    //--------------------------------------------------
    public void SetIsReadyForNextPhase(int playerId_tp, bool flag)
    {
        playerUI_Cps[playerId_tp].SetActiveIsReady(flag);
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
