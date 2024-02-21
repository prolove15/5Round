using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerboard : MonoBehaviour
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
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] GameObject pbRnd_Pf;
    [SerializeField] Transform pbRndPointsGroup_Tf;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    public List<PlayerboardRound> pbRnd_Cps = new List<PlayerboardRound>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player_Cp;
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

    public RoundData roundsData { get { return player_Cp.roundsData; } }

    //-------------------------------------------------- private properties
    TokensData tokensData { get { return player_Cp.tokensData; } }
    MarkersData markersData {  get { return player_Cp.markersData; } }

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
    public void Init(PlayerFaction player_Cp_tp)
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents(player_Cp_tp);
        InstAndInitRoundBoards();
        InitRoundsValue();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents(PlayerFaction player_Cp_tp)
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player_Cp = player_Cp_tp;
    }

    //--------------------------------------------------
    void InstAndInitRoundBoards()
    {
        for (int i = 0; i < 5; i++)
        {
            PlayerboardRound pbRnd_Cp_tp = Instantiate(pbRnd_Pf, pbRndPointsGroup_Tf)
                .GetComponent<PlayerboardRound>();
            pbRnd_Cp_tp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            pbRnd_Cps.Add(pbRnd_Cp_tp);
        }

        for (int i = 0; i < pbRnd_Cps.Count; i++)
        {
            pbRnd_Cps[i].Init(player_Cp, i, OnPbRoundClicked);
        }
    }

    //--------------------------------------------------
    void InitRoundsValue()
    {
        for (int i = 0; i < pbRnd_Cps.Count; i++)
        {
            RoundValue rndValue_tp = new RoundValue();
            rndValue_tp.index = i;
            if (i == 0) rndValue_tp.minAgi = 6;
            else if (i == 1) rndValue_tp.minAgi = 4;
            else rndValue_tp.minAgi = 0;
            rndValue_tp.shienUnitId = -1;
            rndValue_tp.oriUnitIndex = -1;
            rndValue_tp.tarUnitIndex = -1;

            roundsData.rndValues.Add(rndValue_tp);
        }
    }

    #endregion

    //--------------------------------------------------
    public void SetActiveClickable(bool flag)
    {
        for (int i = 0; i < pbRnd_Cps.Count; i++)
        {
            pbRnd_Cps[i].SetActiveClikable(flag);
        }
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Events from external

    //--------------------------------------------------
    public void OnPbRoundClicked(int index)
    {
        controller_Cp.ui_panelCanvas_Cp.ShowActionPanel(player_Cp.playerId, index);
    }

    #endregion

}
