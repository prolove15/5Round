using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public class Data_PartyDecision : MonoBehaviour
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
        NextScenePrepareFinished
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    public UnitCardsData[] playersUnitCardsData = new UnitCardsData[2]
        { new UnitCardsData(), new UnitCardsData() };

    public List<int>[] playersVanUnitIndexes = new List<int>[2]
        { new List<int>(), new List<int>() };

    public List<int>[] playersRearUnitIndexes = new List<int>[2]
        { new List<int>(), new List<int>() };

    public List<int>[] playersBattleUnitIndexes = new List<int>[2]
        { new List<int>(), new List<int>() };

    //-------------------------------------------------- private fields
    Controller_PartyDecision controller_Cp;

    DataManager_Gameplay dataManager_Cp;

    List<Cards_PartyDecision> cardsManager_Cps = new List<Cards_PartyDecision>();

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

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Manage gameStates
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region ManageGameStates

    //--------------------------------------------------
    public int GetExistGameStatesCount(GameState_En gameState_pr)
    {
        int result = 0;

        for (int i = 0; i < gameStates.Count; i++)
        {
            if (gameStates[i] == gameState_pr)
            {
                result++;
            }
        }

        return result;
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
    /// Initialize
    /// </summary>
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
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_PartyDecision>();

        dataManager_Cp = controller_Cp.dataManager_Cp;

        cardsManager_Cps = controller_Cp.cardsManager_Cps;
    }

    #endregion

    //------------------------------
    public void SetUnitCardsData(int playerID_tp, UnitCardsData unitCardsData_pr)
    {
        playersUnitCardsData[playerID_tp] = unitCardsData_pr;
    }

    //------------------------------
    public UnitCardsData GetUnitCardsData(int playerID_pr)
    {
        return playersUnitCardsData[playerID_pr];
    }

    //------------------------------
    public void GenerateRandomTransferData(DataManager_Gameplay dataManager_Cp_tp)
    {
        GenerateRandomUnitCardsData(dataManager_Cp_tp);

        GenerateRandomBattleUnitIndexes(0);
        GenerateRandomBattleUnitIndexes(1);
    }

    //------------------------------
    void GenerateRandomUnitCardsData(DataManager_Gameplay dataManager_Cp_tp)
    {
        dataManager_Cp_tp.GenerateRandomUnitCardsData_SetupStand();
        playersUnitCardsData = dataManager_Cp_tp.psUnitCardsData.ToArray();
    }

    //------------------------------
    void GenerateRandomBattleUnitIndexes(int playerID_pr)
    {
        UnitCardsData unitCardsData_tp = playersUnitCardsData[playerID_pr];
        List<int> vanUnitCardsIndexes_tp = playersVanUnitIndexes[playerID_pr];
        List<int> rearUnitCardsIndexes_tp = playersRearUnitIndexes[playerID_pr];
        List<int> battleUnitCardsIndexes_tp = playersBattleUnitIndexes[playerID_pr];

        while (vanUnitCardsIndexes_tp.Count < DataManager_Gameplay.maxVanUnitsCount)
        {
            int randUnitIndex_tp = Random.Range(0, unitCardsData_tp.unitCards.Count);
            if (!vanUnitCardsIndexes_tp.Contains(randUnitIndex_tp))
            {
                vanUnitCardsIndexes_tp.Add(randUnitIndex_tp);

                battleUnitCardsIndexes_tp.Add(randUnitIndex_tp);
            }
        }

        while (rearUnitCardsIndexes_tp.Count < DataManager_Gameplay.maxRearUnitsCount)
        {
            int randUnitIndex_tp = Random.Range(0, unitCardsData_tp.unitCards.Count);
            if (!vanUnitCardsIndexes_tp.Contains(randUnitIndex_tp)
                &&!rearUnitCardsIndexes_tp.Contains(randUnitIndex_tp))
            {
                rearUnitCardsIndexes_tp.Add(randUnitIndex_tp);

                battleUnitCardsIndexes_tp.Add(randUnitIndex_tp);
            }
        }
    }

    //------------------------------
    public void PrepareToFinish()
    {
        dataManager_Cp.psVanUnitCardsData.Clear();
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            List<UnitCard> vanUnit_Cps_tp = cardsManager_Cps[i].GetVanUnits();

            for (int j = 0; j < vanUnit_Cps_tp.Count; j++)
            {
                dataManager_Cp.psVanUnitCardsData[i].unitCards.Add(
                    dataManager_Cp.GetUnitCardDataFromCardIndex(vanUnit_Cps_tp[i].cardIndex));
            }

            dataManager_Cp.psVanUnitCardsData[i].playerID = cardsManager_Cps[i].playerID;
        }

        dataManager_Cp.psRearUnitCardsData.Clear();
        for (int i = 0; i < cardsManager_Cps.Count; i++)
        {
            List<UnitCard> rearUnit_Cps_tp = cardsManager_Cps[i].GetRearUnits();

            for (int j = 0; j < rearUnit_Cps_tp.Count; j++)
            {
                dataManager_Cp.psRearUnitCardsData[i].unitCards.Add(
                    dataManager_Cp.GetUnitCardDataFromCardIndex(rearUnit_Cps_tp[i].cardIndex));
            }

            dataManager_Cp.psRearUnitCardsData[i].playerID = cardsManager_Cps[i].playerID;
        }

        //
        mainGameState = GameState_En.NextScenePrepareFinished;
    }

}

