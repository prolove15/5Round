using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class TakedCards_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Finished,
        InitTakedUnitCardsFinished,
        LookHandStarted, LookHandFinished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    GameObject takedUnitCard_Pf;

    [SerializeField]
    Transform takedUnitCardCenter_Tf;

    [SerializeField]
    Transform takedUnitCardLookPoint_Tf;

    [SerializeField]
    int cardsNumPerRow = 4;

    [SerializeField]
    float rowInterval = 0.17f, columnInterval = 0.11f;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public int playerID;

    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public List<UnitCard> takedUnitCard_Cps = new List<UnitCard>();

    [ReadOnly]
    public List<Vector3> arrangeLocalPoints = new List<Vector3>();

    [ReadOnly]
    public List<Vector3> arrangePoints = new List<Vector3>();

    [ReadOnly]
    public int takedUnitCardsNum;

    //-------------------------------------------------- private fields
    Controller_StartPrepare controller_Cp;

    UI_StartPrepare uiManager_Cp;

    Data_StartPrepare data_Cp;

    Transform camera_Tf;

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Properties
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
    // Methods
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
    // ManageGameState
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
    // Initialize
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //------------------------------
    public void Init()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        InitUI();

        mainGameState = GameState_En.Inited;
    }

    //------------------------------
    void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_StartPrepare>();

        uiManager_Cp = controller_Cp.uiManager_Cp;

        data_Cp = controller_Cp.data_Cp;

        camera_Tf = controller_Cp.components.camera_Tf;
    }

    //------------------------------
    void InitUI()
    {
        uiManager_Cp.lookHandBtn_RT.gameObject.SetActive(true);
    }

    //------------------------------
    public void InitTakedUnitCards()
    {
        InstantTakedUnitCards();

        InitArrangePoints();

        ArrangeTakedUnitCards();

        mainGameState = GameState_En.InitTakedUnitCardsFinished;
    }

    //------------------------------
    void InstantTakedUnitCards()
    {
        int cardsNum = data_Cp.maxPartyUnitsCount;
        List<GameObject> takedUnitCard_GOs_tp = new List<GameObject>();

        for (int i = 0; i < cardsNum; i++)
        {
            takedUnitCard_GOs_tp.Add(Instantiate(takedUnitCard_Pf, takedUnitCardCenter_Tf));
        }

        for (int i = 0; i < takedUnitCard_GOs_tp.Count; i++)
        {
            takedUnitCard_Cps.Add(takedUnitCard_GOs_tp[i].GetComponent<UnitCard>());
        }

        for (int i = 0; i < takedUnitCard_Cps.Count; i++)
        {
            takedUnitCard_Cps[i].isHighlighted = false;
            takedUnitCard_Cps[i].activeCollider = false;
        }
    }

    //------------------------------
    void InitArrangePoints()
    {
        arrangeLocalPoints = ArrangeObjects.GetLocalArrangePoints(takedUnitCard_Cps.Count,
            cardsNumPerRow, rowInterval, columnInterval);

        arrangePoints = ArrangeObjects.GetArrangePoints(takedUnitCardCenter_Tf.position, takedUnitCard_Cps.Count,
            cardsNumPerRow, rowInterval, columnInterval);
    }

    #endregion

    //------------------------------
    public void SetTakedUnitCardValues(List<UnitCardData> takedUnitCardData_Cps_pr)
    {
        //
        for(int i = 0; i < takedUnitCardData_Cps_pr.Count; i++)
        {
            //
            takedUnitCard_Cps[i + takedUnitCardsNum].cardIndex = takedUnitCardData_Cps_pr[i].id;
            takedUnitCard_Cps[i + takedUnitCardsNum].cost = takedUnitCardData_Cps_pr[i].cost;
            takedUnitCard_Cps[i + takedUnitCardsNum].frontSide = takedUnitCardData_Cps_pr[i].frontSide;
            takedUnitCard_Cps[i + takedUnitCardsNum].backSide = takedUnitCardData_Cps_pr[i].backSide;

            //
            takedUnitCard_Cps[i + takedUnitCardsNum].isHighlighted = false;
        }

        takedUnitCardsNum += takedUnitCardData_Cps_pr.Count;
    }

    //------------------------------
    public void ArrangeTakedUnitCards()
    {
        for(int i = 0; i < arrangeLocalPoints.Count; i++)
        {
            takedUnitCard_Cps[i].transform.SetLocalPositionAndRotation(arrangeLocalPoints[i],
                Quaternion.identity);
        }
    }

    //------------------------------
    public void SetTakedUnitCards(List<UnitCard> takedUnitCard_Cps_pr)
    {
        //
        for(int i = 0; i < takedUnitCard_Cps_pr.Count; i++)
        {
            takedUnitCard_Cps.Add(takedUnitCard_Cps_pr[i]);
        }

        //
        List<UnitCardData> takedUnitCardsData_pr = new List<UnitCardData>();
        for(int i = 0; i < takedUnitCard_Cps_pr.Count; i++)
        {
            takedUnitCardsData_pr.Add(DataManager_Gameplay.GetUnitCardDataFromCardIndex(
                data_Cp.playersUnitCardsData[playerID].unitCards, takedUnitCard_Cps_pr[i].cardIndex));
        }
        SetTakedUnitCardValues(takedUnitCardsData_pr);

        //
        for(int i = 0; i < takedUnitCard_Cps_pr.Count; i++)
        {
            Destroy(takedUnitCard_Cps_pr[i].gameObject);

            takedUnitCard_Cps.Remove(takedUnitCard_Cps_pr[i]);
        }
    }

    //////////////////////////////////////////////////////////////////////
    // Look hand
    //////////////////////////////////////////////////////////////////////
    #region OnLookHand

    //------------------------------
    public void OnLookHand()
    {
        //
        gameStates.Add(GameState_En.LookHandStarted);

        //
        uiManager_Cp.lookHandBtn_RT.gameObject.SetActive(false);
        uiManager_Cp.SetActiveDecisionBtn(false, "CameraMoving");

        //
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnCompletedLookHandCameraTween);

        TargetTweening.TranslateGameObject(camera_Tf, camera_Tf.position, takedUnitCardLookPoint_Tf.position,
            camera_Tf.rotation, takedUnitCardLookPoint_Tf.rotation, unityEvent);
    }

    //------------------------------
    void OnCompletedLookHandCameraTween()
    {
        //
        uiManager_Cp.returnToSelectionBtn_RT.gameObject.SetActive(true);

        //
        gameStates.Add(GameState_En.LookHandFinished);
        RemoveGameStates(GameState_En.LookHandStarted);
    }

    #endregion

}
