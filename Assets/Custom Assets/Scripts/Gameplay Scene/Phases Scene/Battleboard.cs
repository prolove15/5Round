using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;

public class Battleboard : MonoBehaviour
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
    [SerializeField] Transform bbPointsGroup_Tf;
    [SerializeField] GameObject bbUnit_Pf;

    //-------------------------------------------------- public fields
    [ReadOnly] public List<GameState_En> gameStates = new List<GameState_En>();

    public List<Unit_Bb> bbUnit_Cps = new List<Unit_Bb>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player_Cp;
    DataManager_Gameplay dataManager_Cp;
    List<Transform> bbPoint_Tfs = new List<Transform>();
    List<UnitCardData> bbUnitsData = new List<UnitCardData>();

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
        InitVariables();
        InstantAndInitBbUnits();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents(PlayerFaction player_Cp_tp)
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player_Cp = player_Cp_tp;
        dataManager_Cp = controller_Cp.dataManager_Cp;

        for (int i = 0; i < bbPointsGroup_Tf.childCount; i++)
        {
            bbPoint_Tfs.Add(bbPointsGroup_Tf.GetChild(i));
        }
    }

    //--------------------------------------------------
    void InitVariables()
    {
        bbUnitsData = dataManager_Cp.psBUnitCardsData[player_Cp.playerId].unitCards;
    }

    //--------------------------------------------------
    void InstantAndInitBbUnits()
    {
        for (int i = 0; i < bbPoint_Tfs.Count; i++)
        {
            Unit_Bb bbUnit_Cp_tp = Instantiate(bbUnit_Pf, bbPoint_Tfs[i]).GetComponent<Unit_Bb>();
            bbUnit_Cp_tp.transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            bbUnit_Cps.Add(bbUnit_Cp_tp);
        }

        for (int i = 0; i < bbUnit_Cps.Count; i++)
        {
            bbUnit_Cps[i].Init(bbUnitsData[i]);

            UnitPosType posType = i < 2 ? UnitPosType.Van : UnitPosType.Rear;
            bbUnit_Cps[i].OnSpawn(player_Cp.playerId, posType, false);
        }
    }

    #endregion

}
