using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Data_StartPrepare : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    // Types
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, FinishPrepared, WillFinish,
        LoadUnitCardsDataFinished, ExchUnitsReqProc
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    // Fields
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public UnitCardsData unitCardsData = new UnitCardsData();

    [ReadOnly]
    public UnitCardsData p1UnitCardsData = new UnitCardsData();

    [ReadOnly]
    public UnitCardsData p2UnitCardsData = new UnitCardsData();

    //-------------------------------------------------- private fields
    Controller_StartPrepare controller_Cp;

    DataManager_Gameplay dataManager_Cp;

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

    public int maxPartyUnitsCount
    {
        get { return DataManager_Gameplay.maxPartyUnitsCount; }
    }

    public List<UnitCardsData> playersUnitCardsData
    {
        get
        {
            List<UnitCardsData> result = new List<UnitCardsData>();

            result.Add(p1UnitCardsData);
            result.Add(p2UnitCardsData);

            return result;
        }
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
    public int ExistGameStatesNum(GameState_En gameState_pr)
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
        StartCoroutine(CorouInit());
    }

    IEnumerator CorouInit()
    {
        gameStates.Add(GameState_En.Nothing);

        InitComponents();

        mainGameState = GameState_En.Inited;

        yield return null;
    }

    //------------------------------
    void InitComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_StartPrepare>();

        dataManager_Cp = controller_Cp.dataManager_Cp;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Load UnitCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region LoadUnitCardsData

    //--------------------------------------------------
    public void LoadUnitCardsData()
    {
        StartCoroutine(CorouLoadUnitCardsData());
    }

    IEnumerator CorouLoadUnitCardsData()
    {
        //
        UnitCardsData allUnitCardsData = dataManager_Cp.dataStorage.unitCardsData;

        int allUnitsCount = allUnitCardsData.unitCards.Count;

        // Create a list to store the selected indices
        List<int> selectedIndices = new List<int>();

        // Ensure that you have at least maxPartyUnitsCount * 2 units available
        if (allUnitsCount >= maxPartyUnitsCount * 2)
        {
            // Use a while loop to randomly select maxPartyUnitsCount * 2 unique indices
            while (selectedIndices.Count < maxPartyUnitsCount * 2)
            {
                int randomIndex = Random.Range(0, allUnitsCount);

                // Check if the random index is not already selected
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                }
            }
        }
        else
        {
            Debug.LogWarning("There are not enough units available to select cardNum * 2.");
        }

        //
        for (int i = 0; i < maxPartyUnitsCount; i++)
        {
            // Access the unit card data using the selected index
            p1UnitCardsData.unitCards.Add(allUnitCardsData.unitCards[selectedIndices[i]]);
        }
        p1UnitCardsData.playerID = 0;

        for (int i = maxPartyUnitsCount; i < maxPartyUnitsCount * 2; i++)
        //for(int i = 0; i < maxPartyUnitsCount; i++) // It's for the test
        {
            p2UnitCardsData.unitCards.Add(allUnitCardsData.unitCards[selectedIndices[i]]);
        }
        p2UnitCardsData.playerID = 1;

        //
        mainGameState = GameState_En.LoadUnitCardsDataFinished;

        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Exchange units and unitCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ExchangeUnits

    public List<UnitCard> p1ExchUnit_Cps = new List<UnitCard>();

    public List<UnitCard> p2ExchUnit_Cps = new List<UnitCard>();

    public int exchReqCount = 0;

    public int twoUnitsExchCount = 0;

    //--------------------------------------------------
    public void ExchangeUnitsRequest(int playerID_tp, List<UnitCard> unit_Cps_pr)
    {
        //
        if ((playerID_tp != 0 && playerID_tp != 1) || unit_Cps_pr.Count == 0)
        {
            Debug.Log("ExchangeUnitsRequest, params are not valid");
            return;
        }

        //
        if (playerID_tp == 0)
        {
            p1ExchUnit_Cps = unit_Cps_pr;
            exchReqCount++;
        }
        else if(playerID_tp == 1)
        {
            p2ExchUnit_Cps = unit_Cps_pr;
            exchReqCount++;
        }

        //
        gameStates.Add(GameState_En.ExchUnitsReqProc);

        //
        if(exchReqCount == 2)
        {
            ExchangeUnitsPositions(p1ExchUnit_Cps, p2ExchUnit_Cps);
        }
    }

    //--------------------------------------------------
    void ExchangeUnitsPositions(List<UnitCard> firstUnit_Cps_pr, List<UnitCard> sndUnit_Cps_pr)
    {
        StartCoroutine(Corou_ExchangeUnitsPositions(firstUnit_Cps_pr, sndUnit_Cps_pr));
    }

    IEnumerator Corou_ExchangeUnitsPositions(List<UnitCard> firstUnit_Cps_pr, List<UnitCard> sndUnit_Cps_pr)
    {
        int totalExchCount = firstUnit_Cps_pr.Count;
        Transform firstUnitsCenter_Tf_tp = firstUnit_Cps_pr[0].transform.parent;
        Transform sndUnitsCenter_Tf_tp = sndUnit_Cps_pr[0].transform.parent;

        //
        for (int i = 0; i < totalExchCount; i++)
        {
            ExchangeTwoUnitsPositions(firstUnit_Cps_pr[i], sndUnit_Cps_pr[i]);
        }

        yield return new WaitUntil(() => twoUnitsExchCount == totalExchCount * 2);

        //
        for (int i = 0; i < totalExchCount; i++)
        {
            firstUnit_Cps_pr[i].transform.SetParent(sndUnitsCenter_Tf_tp);
            sndUnit_Cps_pr[i].transform.SetParent(firstUnitsCenter_Tf_tp);
        }

        //
        OnComplete_ExchangeUnitsPositions();
    }

    void OnComplete_ExchangeUnitsPositions()
    {
        exchReqCount = 0;

        twoUnitsExchCount = 0;

        //
        ExchangePlayersUnitCardsData();
    }

    //--------------------------------------------------
    void ExchangeTwoUnitsPositions(UnitCard firstUnit_Cp_pr, UnitCard sndUnit_Cp_pr)
    {
        //
        Vector3 firstUnitPos_tp = firstUnit_Cp_pr.transform.position;
        Quaternion firstUnitRot_tp = firstUnit_Cp_pr.transform.rotation;
        Vector3 sndUnitPos_tp = sndUnit_Cp_pr.transform.position;
        Quaternion sndUnitRot_tp = sndUnit_Cp_pr.transform.rotation;

        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(OnComplete_ExchangeTwoUnitsPositions);

        //
        TargetTweening.TranslateGameObject(firstUnit_Cp_pr.transform, firstUnitPos_tp, sndUnitPos_tp,
            firstUnitRot_tp, sndUnitRot_tp, unityEvent, 1.5f);
        TargetTweening.TranslateGameObject(sndUnit_Cp_pr.transform, sndUnitPos_tp, firstUnitPos_tp,
            sndUnitRot_tp, firstUnitRot_tp, unityEvent, 1.5f);
    }

    void OnComplete_ExchangeTwoUnitsPositions()
    {
        twoUnitsExchCount++;
    }

    //--------------------------------------------------
    void ExchangePlayersUnitCardsData()
    {
        //
        UnitCardsData p2UnitCardsData_tp = new UnitCardsData();
        for (int i = 0; i < p1ExchUnit_Cps.Count; i++)
        {
            p2UnitCardsData_tp.unitCards.Add(DataManager_Gameplay.GetUnitCardDataFromCardIndex(
                p1UnitCardsData.unitCards, p1ExchUnit_Cps[i].cardIndex));
        }

        UnitCardsData p1UnitCardsData_tp = new UnitCardsData();
        for (int i = 0; i < p2ExchUnit_Cps.Count; i++)
        {
            p1UnitCardsData_tp.unitCards.Add(DataManager_Gameplay.GetUnitCardDataFromCardIndex(
                p2UnitCardsData.unitCards, p2ExchUnit_Cps[i].cardIndex));
        }

        p1UnitCardsData.unitCards = p1UnitCardsData_tp.unitCards;
        p2UnitCardsData.unitCards = p2UnitCardsData_tp.unitCards;

        //
        List<UnitCard> tempExchUnit_Cps_tp = new List<UnitCard>(p1ExchUnit_Cps);
        p1ExchUnit_Cps.Clear();
        p1ExchUnit_Cps.AddRange(p2ExchUnit_Cps);
        p2ExchUnit_Cps.Clear();
        p2ExchUnit_Cps.AddRange(tempExchUnit_Cps_tp);

        //
        gameStates.RemoveAll(item => item == GameState_En.ExchUnitsReqProc);
    }

    #endregion
    //////////////////////////////////////////////////////////////////////

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Exchange units and unitCardsData
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region PrepareToFinish

    public void PrepareToFinish()
    {
        dataManager_Cp.psUnitCardsData.Clear();
        dataManager_Cp.psUnitCardsData.AddRange(playersUnitCardsData);

        mainGameState = GameState_En.FinishPrepared;
    }

    #endregion
    //////////////////////////////////////////////////////////////////////

    //------------------------------
    public UnitCardsData GetUnitCardsData(DataManager_Gameplay dataManager_Cp_pr)
    {
        UnitCardsData unitCardsData_tp = null;

        unitCardsData_tp = GetRandomUnitCardsData(dataManager_Cp_pr);

        return unitCardsData_tp;
    }

    UnitCardsData GetRandomUnitCardsData(DataManager_Gameplay dataManager_Cp_pr)
    {
        UnitCardsData unitCardsData_tp = new UnitCardsData();

        //
        UnitCardsData allUnitCardsData = dataManager_Cp_pr.dataStorage.unitCardsData;

        int allUnitsCount = allUnitCardsData.unitCards.Count;

        // Create a list to store the selected indices
        List<int> selectedIndices = new List<int>();

        // Ensure that you have at least maxPartyUnitsCount units available
        if (allUnitsCount >= maxPartyUnitsCount)
        {
            // Use a while loop to randomly select maxPartyUnitsCount unique indices
            while (selectedIndices.Count < maxPartyUnitsCount)
            {
                int randomIndex = Random.Range(0, allUnitsCount);

                // Check if the random index is not already selected
                if (!selectedIndices.Contains(randomIndex))
                {
                    selectedIndices.Add(randomIndex);
                }
            }
        }
        else
        {
            Debug.LogWarning("There are not enough units available to select cardNum.");
        }

        //
        for (int i = 0; i < maxPartyUnitsCount; i++)
        {
            // Access the unit card data using the selected index
            unitCardsData_tp.unitCards.Add(allUnitCardsData.unitCards[selectedIndices[i]]);
        }

        return unitCardsData_tp;
    }
}
