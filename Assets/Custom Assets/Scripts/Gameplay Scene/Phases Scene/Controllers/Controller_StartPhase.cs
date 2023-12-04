using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Controller_StartPhase : MonoBehaviour
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
        PhaseStarted, PhaseFinished,
        APMarkerMoved, TurnMarkerMoved,
        CamMovedToGameBLookPoint,
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
    UI_StartPhase startUI_Cp;

    [SerializeField]
    public Transform p1CamPoint_Tf, p2CamPoint_Tf;

    [SerializeField]
    Transform p1APPointsGroup_Tf, p2APPointsGroup_Tf;

    [SerializeField]
    Transform turnPointsGroup_Tf;

    [SerializeField]
    Transform p1APMarker_Tf, p2APMarker_Tf;

    [SerializeField]
    Transform turnMarker_Tf;

    [SerializeField]
    float delayAfterAPMarkerMoved = 1f;

    [SerializeField]
    float delayAfterTurnMarkerMoved = 1f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public List<Transform> p1APPoint_Tfs = new List<Transform>();

    [ReadOnly]
    public List<Transform> p2APPoint_Tfs = new List<Transform>();

    [ReadOnly]
    public List<Transform> turnPoint_Tfs = new List<Transform>();

    [ReadOnly]
    public List<int> playerAPs = new List<int> { 0, 0 };

    [ReadOnly]
    public int turnIndex;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;

    Transform cam_Tf;

    StatusManager statusManager_Cp;

    Player_Phases localPlayer_Cp;

    int localPlayerID;

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

        //
        SetComponents();

        InitComponents();

        SetVariables();

        InitStatusPanel();

        //
        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();

        cam_Tf = controller_Cp.cam_Tf;

        statusManager_Cp = controller_Cp.statusManager_Cp;
    }

    //--------------------------------------------------
    void InitComponents()
    {
        startUI_Cp.Init();
    }

    //--------------------------------------------------
    void SetVariables()
    {
        //
        localPlayerID = controller_Cp.localPlayerID;

        //
        for (int i = 0; i < p1APPointsGroup_Tf.childCount; i++)
        {
            p1APPoint_Tfs.Add(p1APPointsGroup_Tf.GetChild(i));
        }

        for (int i = 0; i < p2APPointsGroup_Tf.childCount; i++)
        {
            p2APPoint_Tfs.Add(p2APPointsGroup_Tf.GetChild(i));
        }

        for (int i = 0; i < turnPointsGroup_Tf.childCount; i++)
        {
            turnPoint_Tfs.Add(turnPointsGroup_Tf.GetChild(i));
        }
    }

    //--------------------------------------------------
    void InitStatusPanel()
    {
        statusManager_Cp.Init();
        statusManager_Cp.InitInstructions_StartPhase();
        statusManager_Cp.SetInstructions_StartPhase();
        statusManager_Cp.StartDateTimeCounting();
        statusManager_Cp.StartBatteryCounting();

        statusManager_Cp.leftTime = 180;
    }

    #endregion

    //--------------------------------------------------
    public void PlayPhase()
    {
        StartCoroutine(Corou_PlayPhase());
    }

    IEnumerator Corou_PlayPhase()
    {
        mainGameState = GameState_En.PhaseStarted;

        //******************** 31th timing. turn start

        //
        MoveCamToGameBLookPoint();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.CamMovedToGameBLookPoint));
        RemoveGameStates(GameState_En.CamMovedToGameBLookPoint);

        //
        UpAPs();
        yield return new WaitUntil(() => GetExistGameStatesCount(GameState_En.APMarkerMoved) == 2);
        RemoveGameStates(GameState_En.APMarkerMoved);

        yield return new WaitForSeconds(delayAfterAPMarkerMoved);

        UpTurnIndex();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.TurnMarkerMoved));
        RemoveGameStates(GameState_En.TurnMarkerMoved);

        yield return new WaitForSeconds(delayAfterTurnMarkerMoved);

        //
        mainGameState = GameState_En.PhaseFinished;
    }

    //--------------------------------------------------
    public void MoveCamToGameBLookPoint()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MoveCamToGameBLookPoint);

        if (localPlayerID == 0)
        {
            TargetTweening.TranslateGameObject(cam_Tf, p1CamPoint_Tf, unityEvent);
        }
        else if (localPlayerID == 1)
        {
            TargetTweening.TranslateGameObject(cam_Tf, p2CamPoint_Tf, unityEvent);
        }        
    }

    void OnComplete_MoveCamToGameBLookPoint()
    {
        AddGameStates(GameState_En.CamMovedToGameBLookPoint);
    }

    //--------------------------------------------------
    public void UpAPs()
    {
        for (int i = 0; i < playerAPs.Count; i++)
        {
            playerAPs[i] += 1;
        }

        //
        MoveAPMarkers();
    }

    void MoveAPMarkers()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MoveAPMarkers);
        TargetTweening.TranslateGameObject(p1APMarker_Tf, p1APPoint_Tfs[playerAPs[0] - 1], unityEvent);
        TargetTweening.TranslateGameObject(p2APMarker_Tf, p2APPoint_Tfs[playerAPs[1] - 1], unityEvent);
    }

    void OnComplete_MoveAPMarkers()
    {
        AddGameStates(GameState_En.APMarkerMoved);

        //
        statusManager_Cp.attackPoint = playerAPs[localPlayerID];
    }

    //--------------------------------------------------
    public void UpTurnIndex()
    {
        turnIndex += 1;

        //
        MoveTurnMarker();
    }

    void MoveTurnMarker()
    {
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_MoveTurnMarker);
        TargetTweening.TranslateGameObject(turnMarker_Tf, turnPoint_Tfs[turnIndex - 1], unityEvent);
    }

    void OnComplete_MoveTurnMarker()
    {
        AddGameStates(GameState_En.TurnMarkerMoved);

        //
        statusManager_Cp.turnIndex = turnIndex;
    }

}
