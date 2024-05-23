using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnitsAbilityHandler_de;

public class GameEventsHandler : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Own variables
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Own variables

    //-------------------------------------------------- serialize fields
    // components    
    [SerializeField]
    UnitsAbilityHandler_de unitAbil_Cp;

    [SerializeField]
    GameActionHandler gActHandler_Cp;

    //-------------------------------------------------- public fields
    
    //-------------------------------------------------- private fields
    // components
    List<UnitUI_Phases> bUnitsUI_Cps = new List<UnitUI_Phases>();

    GameObject bUnitsUIPanel_GO
    {
        get { return controller_Cp.uiManager_Cp.bUnitsUIPanel_GO; }
    }

    Text bUnitsUIPanelText_Cp
    {
        get { return controller_Cp.uiManager_Cp.bUnitsUIText_Cp; }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Inherited variables
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Inherited variables

    //-------------------------------------------------- serialize fields
    // guard action
    GameObject guardEff_Pf
    {
        get { return unitAbil_Cp.guardEff_Pf; }
    }

    float guardEffDur
    {
        get { return unitAbil_Cp.guardEffDur; }
    }

    // shien action
    GameObject shienEff_Pf
    {
        get { return unitAbil_Cp.shienEff_Pf; }
    }

    float shienEffDur
    {
        get { return unitAbil_Cp.shienEffDur; }
    }

    // move action
    GameObject moveEff1_Pf
    {
        get { return unitAbil_Cp.moveEff1_Pf; }
    }

    GameObject moveEff2_Pf
    {
        get { return unitAbil_Cp.moveEff2_Pf; }
    }

    float moveUnitDur
    {
        get { return unitAbil_Cp.moveUnitDur; }
    }

    float moveEffDur
    {
        get { return unitAbil_Cp.moveEffDur; }
    }

    // atk action
    GameObject atkJumpEff_Pf
    {
        get { return unitAbil_Cp.atkJumpEff_Pf; }
    }

    float atkJumpDur
    {
        get { return unitAbil_Cp.atkJumpDur; }
    }

    float atkJumpStayDur
    {
        get { return unitAbil_Cp.atkJumpStayDur; }
    }

    // extra
    float emptyRoundDur
    {
        get { return unitAbil_Cp.emptyRoundDur; }
    }

    float actionEffTxtDur
    {
        get { return unitAbil_Cp.actionEffTextDur; }
    }

    float noticeDur
    {
        get { return unitAbil_Cp.noticeTextDur; }
    }

    //-------------------------------------------------- public fields
    // variables
    public UnitCard oriUnit_Cp
    {
        get { return unitAbil_Cp.oriUnit_Cp; }
    }

    public UnitCard tarUnit_Cp
    {
        get { return unitAbil_Cp.tarUnit_Cp; }
    }

    //-------------------------------------------------- private fields
    // components
    public Controller_Phases controller_Cp
    {
        get { return unitAbil_Cp.controller_Cp; }
    }

    Controller_StartPhase startController_Cp
    {
        get { return unitAbil_Cp.startController_Cp; }
    }

    Controller_BattlePhase btlController_Cp
    {
        get { return unitAbil_Cp.btlController_Cp; }
    }

    UI_BattlePhase btlUI_Cp
    {
        get { return unitAbil_Cp.btlUI_Cp; }
    }

    DataManager_Gameplay dataManager_Cp
    {
        get { return unitAbil_Cp.dataManager_Cp; }
    }

    Data_Phases data_Cp
    {
        get { return controller_Cp.data_Cp; }
    }

    Player_Phases mPlayer_Cp
    {
        get { return unitAbil_Cp.mPlayer_Cp; }
    }

    Player_Phases ePlayer_Cp
    {
        get { return unitAbil_Cp.ePlayer_Cp; }
    }

    List<Player_Phases> player_Cps
    {
        get { return unitAbil_Cp.player_Cps; }
    }

    List<UnitCard> bUnit_Cps
    {
        get { return unitAbil_Cp.bUnit_Cps; }
    }

    List<UnitCard> eBUnit_Cps
    {
        get { return unitAbil_Cp.eBUnit_Cps; }
    }

    List<TakaraCardData> takaraDatas
    {
        get { return unitAbil_Cp.takaraDatas; }
        set { unitAbil_Cp.takaraDatas = value; }
    }

    List<Transform> bUnitPoint_Tfs
    {
        get { return unitAbil_Cp.bUnitPoint_Tfs; }
    }

    List<Transform> eBUnitPoint_Tfs
    {
        get { return unitAbil_Cp.eBUnitPoint_Tfs; }
    }

    // player data
    DataStorage_Gameplay dataStorage
    {
        get { return unitAbil_Cp.dataStorage; }
    }

    GameEventsData gEventsData
    {
        get { return unitAbil_Cp.gEventsData; }
    }

    List<RoundValue_de> roundsData
    {
        get { return unitAbil_Cp.roundsData; }
    }

    int curRoundIndex
    {
        get { return unitAbil_Cp.curRoundIndex; }
    }

    RoundValue_de roundValue
    {
        get { return unitAbil_Cp.roundValue; }
    }

    bool isLocalPlayer
    {
        get { return unitAbil_Cp.isLocalPlayer; }
    }

    int mPlayerID
    {
        get { return unitAbil_Cp.mPlayerID; }
    }

    int ePlayerID
    {
        get { return unitAbil_Cp.ePlayerID; }
    }

    AttackType atkType
    {
        get { return unitAbil_Cp.atkType; }
    }

    AtkSuccType atkSucc
    {
        get { return unitAbil_Cp.atkSucc; }
    }

    bool isCritAtk
    {
        get { return unitAbil_Cp.isCritAtk; }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties
    public List<GameState_En> gameStates
    {
        get { return unitAbil_Cp.gameStates; }
    }

    public List<Hash128> hashStates
    {
        get { return HashHandler.instance.hashes; }
    }

    public Dictionary<Hash128, object> randObjects
    {
        get { return data_Cp.randObjects; }
    }

    public int playerAp
    {
        get { return startController_Cp.playerAPs[mPlayerID]; }
        set { startController_Cp.playerAPs[mPlayerID] = playerAp; }
    }

    public HashHandler hashHandler_Cp
    {
        get { return controller_Cp.dataManager_Cp.hashHandler_Cp; }
    }

    public GameEventsData curGEventsData
    {
        get { return mPlayer_Cp.curGEventsData; }
    }

    public GameEventsData waitGEventsData
    {
        get { return mPlayer_Cp.waitGEventsData; }
    }

    public ParEffects_Cs parEffs
    {
        get { return controller_Cp.data_Cp.parEffs; }
    }

    //-------------------------------------------------- private properties
    int actionTarUnitIndex
    {
        get { return unitAbil_Cp.actionTarUnitIndex; }
        set { unitAbil_Cp.actionTarUnitIndex = value; }
    }

    #endregion

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
    /// Handle hash states
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle hash states

    //--------------------------------------------------
    Hash128 RegRandHashValue()
    {
        return HashHandler.RegRandHash();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle random game objects
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region #Handle random game objects

    //--------------------------------------------------
    Hash128 RegRandObjHash()
    {
        return controller_Cp.data_Cp.RegRandObjHash();
    }

    //--------------------------------------------------
    void RemoveRandObj(Hash128 hash_pr)
    {
        controller_Cp.data_Cp.RemoveRandObj(hash_pr);
    }

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
    /// Handle game events timing
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        InitVariables();
    }

    void InitVariables()
    {
        
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle normal abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle normal abilities

    //-------------------------------------------------- 12th
    void Handle_NlAbil_12th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_12th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_12th(Hash128 hash_pr)
    {
        UnitCard unit_Cp_tp = oriUnit_Cp;
        UnitCardData unitData_tp = unit_Cp_tp.unitCardData;
        NormalAttack nlAtk_tp = unitData_tp.nlAtk;
        UnitInfo_de unitInfo_tp = unit_Cp_tp.unitInfo;

        // wait trigger events
        GameEventsTiming waitGEvTiming_tp = nlAtk_tp.contents[0].tgrEvents[0];
        waitGEventsData.IncEventCount(waitGEvTiming_tp);

        hashStates.Remove(hash_pr);

        yield return new WaitUntil(() => curGEventsData.Contains(waitGEvTiming_tp));

        // add ability
        unitInfo_tp.defCorr += nlAtk_tp.contents[0].amount;

        Hash128 hash_tp = RegRandHashValue();
        Hash128 effGO_hash_tp = RegRandObjHash();
        GameAction_AddDefCorrEff(unit_Cp_tp, nlAtk_tp.contents[0].amount, effGO_hash_tp, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        // mark the trigger event has been proceed
        waitGEventsData.DecEventCount(waitGEvTiming_tp);
        curGEventsData.IncEventCount(waitGEvTiming_tp);

        // wait end events
        GameEventsTiming waitGEventsData2_tp = new GameEventsTiming();
        waitGEventsData2_tp = nlAtk_tp.contents[0].endEvents[0];
        waitGEventsData.IncEventCount(waitGEventsData2_tp);

        yield return new WaitUntil(() => curGEventsData.Contains(waitGEventsData2_tp));

        // remove ability
        unitInfo_tp.defCorr -= nlAtk_tp.contents[0].amount;

        Hash128 hash2_tp = RegRandHashValue();
        GameAction_RemoveDefCorrEff(effGO_hash_tp, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        // mark the end event has been proceed
        waitGEventsData.DecEventCount(waitGEventsData2_tp);
        curGEventsData.IncEventCount(waitGEventsData2_tp);
    }

    //-------------------------------------------------- 22th
    void Handle_NlAbil_22th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_22th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_22th(Hash128 hash_pr)
    {
        UnitCard unit_Cp_tp = oriUnit_Cp;
        UnitCardData unitData_tp = unit_Cp_tp.unitCardData;
        NormalAttack nlAtk_tp = unitData_tp.nlAtk;
        UnitInfo_de unitInfo_tp = unit_Cp_tp.unitInfo;

        // wait trigger event
        GameEventsTiming waitTriTiming_tp = nlAtk_tp.contents[0].tgrEvents[0];
        waitGEventsData.IncEventCount(waitTriTiming_tp);

        hashStates.Remove(hash_pr);

        yield return new WaitUntil(() => curGEventsData.Contains(waitTriTiming_tp));

        // add ability
        List<Hash128> hash_tps = new List<Hash128>();
        Hash128 hash_tp = RegRandHashValue();
        GameAction_AddAlliesDmgCorr(hash_tps, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        
        // mark the trigger event has been proceed
        waitGEventsData.DecEventCount(waitTriTiming_tp);
        curGEventsData.IncEventCount(waitTriTiming_tp);

        // wait end event
        GameEventsTiming waitEndTiming_tp = nlAtk_tp.contents[0].endEvents[0];
        waitGEventsData.IncEventCount(waitEndTiming_tp);

        yield return new WaitUntil(() => curGEventsData.Contains(waitEndTiming_tp));
        
        // remove ability
        Hash128 hash2_tp = RegRandHashValue();
        GameAction_RemoveAlliesDmgCorr(hash_tps, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        // mark the end event has been proceed
        waitGEventsData.DecEventCount(waitEndTiming_tp);
        curGEventsData.IncEventCount(waitEndTiming_tp);
    }

    //-------------------------------------------------- 25th
    void Handle_NlAbil_25th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_25th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_25th(Hash128 hash_pr)
    {
        UnitCard unit_Cp_tp = oriUnit_Cp;
        UnitCardData unitData_tp = unit_Cp_tp.unitCardData;
        NormalAttack nlAtk_tp = unitData_tp.nlAtk;
        UnitInfo_de unitInfo_tp = unit_Cp_tp.unitInfo;

        // wait trigger event
        GameEventsTiming waitTriTiming_tp = nlAtk_tp.contents[0].tgrEvents[0];
        waitGEventsData.IncEventCount(waitTriTiming_tp);

        hashStates.Remove(hash_pr);

        yield return new WaitUntil(() => curGEventsData.Contains(waitTriTiming_tp));

        // check additional trigger conditions
        int fushiCount_tp = 0;
        for (int i = 0; i < bUnit_Cps.Count; i++)
        {
            if (bUnit_Cps[i].unitCardData.attrib.Contains(UnitAttribute.Fushi))
            {
                fushiCount_tp++;
            }
        }

        //if (fushiCount_tp == 3)
        if (fushiCount_tp < 0) // ::it will remove later
        {
            // mark the trigger event has been proceed
            waitGEventsData.DecEventCount(waitTriTiming_tp);
            curGEventsData.IncEventCount(waitTriTiming_tp);

            yield break;
        }

        // add ability
        unit_Cp_tp.unitInfo.atkCorr += nlAtk_tp.contents[0].amount;

        Hash128 hash_tp = RegRandHashValue();
        Hash128 eff_hash_tp = RegRandObjHash();
        gActHandler_Cp.GameAction_AddAtkCorr(unit_Cp_tp, eff_hash_tp, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        // mark the trigger event has been proceed
        waitGEventsData.DecEventCount(waitTriTiming_tp);
        curGEventsData.IncEventCount(waitTriTiming_tp);

        // wait end event
        GameEventsTiming waitEndTiming_tp = nlAtk_tp.contents[0].endEvents[0];
        waitGEventsData.IncEventCount(waitEndTiming_tp);

        yield return new WaitUntil(() => curGEventsData.Contains(waitEndTiming_tp));

        // remove ability
        unit_Cp_tp.unitInfo.atkCorr -= nlAtk_tp.contents[0].amount;
        
        Hash128 hash2_tp = RegRandHashValue();
        gActHandler_Cp.GameAction_RemoveAtkCorr(eff_hash_tp, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        // mark the end event has been proceed
        waitGEventsData.DecEventCount(waitEndTiming_tp);
        curGEventsData.IncEventCount(waitEndTiming_tp);
    }

    //-------------------------------------------------- 28th
    void Handle_NlAbil_28th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_28th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_28th(Hash128 hash_pr)
    {
        UnitCard unit_Cp_tp = oriUnit_Cp;
        UnitCardData unitData_tp = unit_Cp_tp.unitCardData;
        NormalAttack nlAtk_tp = unitData_tp.nlAtk;
        UnitInfo_de unitInfo_tp = unit_Cp_tp.unitInfo;

        // wait trigger event 1
        GameEventsTiming waitTriTiming1_tp = nlAtk_tp.contents[0].tgrEvents[0];        
        waitGEventsData.IncEventCount(waitTriTiming1_tp);
        hashStates.Remove(hash_pr);
        yield return new WaitUntil(() => curGEventsData.Contains(waitTriTiming1_tp));

        // implement ability 1
        unitInfo_tp.atkCorr += unitData_tp.nlAtk.contents[0].amount;

        Hash128 eff_hash_tp = RegRandObjHash();
        Hash128 hash3_tp = RegRandHashValue();
        gActHandler_Cp.GameAction_AddAtkCorr(unit_Cp_tp, eff_hash_tp, hash3_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash3_tp));

        // make waiting a trigger event
        GameEventsTiming waitTriTiming2_tp = nlAtk_tp.contents[1].tgrEvents[0];
        waitGEventsData.IncEventCount(waitTriTiming2_tp);

        // mark the trigger event 1 has been proceed
        waitGEventsData.DecEventCount(waitTriTiming1_tp);
        curGEventsData.IncEventCount(waitTriTiming1_tp);

        // wait and implement ability 2
        yield return new WaitUntil(() => curGEventsData.Contains(waitTriTiming2_tp));

        Hash128 tarUnitIndex_hash_tp = RegRandObjHash();
        Hash128 hash_tp = RegRandHashValue();
        gActHandler_Cp.GameAction_SelectBUnitModal(ePlayerID, eBUnit_Cps, tarUnitIndex_hash_tp, hash_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        int tarUnitIndex_tp = (int)randObjects[tarUnitIndex_hash_tp];
        unitAbil_Cp.tarUnit_Cp = eBUnit_Cps[tarUnitIndex_tp];
        roundValue.tarUnitIndex = tarUnitIndex_tp;
        roundValue.atkNum += 1;
        
        Hash128 hash4_tp = RegRandHashValue();
        unitAbil_Cp.PlayAtkAction(hash4_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash4_tp));

        // mark the trigger event 2 has been proceed
        waitGEventsData.DecEventCount(waitTriTiming2_tp);
        curGEventsData.IncEventCount(waitTriTiming2_tp);

        // wait end event
        GameEventsTiming waitEndTiming_tp = nlAtk_tp.contents[0].endEvents[0];
        waitGEventsData.IncEventCount(waitEndTiming_tp);
        yield return new WaitUntil(() => curGEventsData.Contains(waitEndTiming_tp));

        // remove ability 1
        unitInfo_tp.atkCorr -= unitData_tp.nlAtk.contents[0].amount;

        Hash128 hash2_tp = RegRandHashValue();
        gActHandler_Cp.GameAction_RemoveAtkCorr(eff_hash_tp, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash2_tp));

        // mark the end event has been proceed
        waitGEventsData.DecEventCount(waitEndTiming_tp);
        curGEventsData.IncEventCount(waitEndTiming_tp);
    }

    //-------------------------------------------------- 32th
    void Handle_NlAbil_32th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_4th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_4th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 34th
    void Handle_NlAbil_34th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_5th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_5th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 44th
    void Handle_NlAbil_44th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_6th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_6th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 45th
    void Handle_NlAbil_45th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_7th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_7th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 47th
    void Handle_NlAbil_47th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_8th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_8th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 48th
    void Handle_NlAbil_48th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_9th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_9th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle spc1 abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle spc1 abilities

    //-------------------------------------------------- 1th
    void Handle_Spc1Abil_1th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_1th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_1th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 2th
    void Handle_Spc1Abil_2th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_2th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_2th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 3th
    void Handle_Spc1Abil_3th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_3th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_3th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 4th
    void Handle_Spc1Abil_4th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_4th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_4th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 5th
    void Handle_Spc1Abil_5th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_5th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_5th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 6th
    void Handle_Spc1Abil_6th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_6th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_6th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 7th
    void Handle_Spc1Abil_7th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_7th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_7th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 8th
    void Handle_Spc1Abil_8th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_8th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_8th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 9th
    void Handle_Spc1Abil_9th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_9th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_9th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 10th
    void Handle_Spc1Abil_10th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_10th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_10th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 11th
    void Handle_Spc1Abil_11th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_11th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_11th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 12th
    void Handle_Spc1Abil_12th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc1Abil_12th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc1Abil_12th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle spc2 abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle spc2 abilities

    //-------------------------------------------------- 1th
    void Handle_Spc2Abil_1th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_1th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_1th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 2th
    void Handle_Spc2Abil_2th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_2th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_2th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 3th
    void Handle_Spc2Abil_3th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_3th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_3th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 4th
    void Handle_Spc2Abil_4th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_4th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_4th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 5th
    void Handle_Spc2Abil_5th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_5th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_5th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 6th
    void Handle_Spc2Abil_6th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_6th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_6th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 7th
    void Handle_Spc2Abil_7th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_7th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_7th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 8th
    void Handle_Spc2Abil_8th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_8th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_8th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 9th
    void Handle_Spc2Abil_9th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_9th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_9th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 10th
    void Handle_Spc2Abil_10th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_10th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_10th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 11th
    void Handle_Spc2Abil_11th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_11th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_11th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 12th
    void Handle_Spc2Abil_12th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_Spc2Abil_12th(hash_pr));
    }

    IEnumerator Corou_Handle_Spc2Abil_12th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle unique abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle unique abilities

    //-------------------------------------------------- 1th
    void Handle_UniqAbil_1th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_1th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_1th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 2th
    void Handle_UniqAbil_2th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_2th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_2th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 3th
    void Handle_UniqAbil_3th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_3th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_3th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 4th
    void Handle_UniqAbil_4th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_4th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_4th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 5th
    void Handle_UniqAbil_5th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_5th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_5th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 6th
    void Handle_UniqAbil_6th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_6th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_6th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 7th
    void Handle_UniqAbil_7th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_7th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_7th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 8th
    void Handle_UniqAbil_8th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_8th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_8th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 9th
    void Handle_UniqAbil_9th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_9th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_9th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 10th
    void Handle_UniqAbil_10th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_10th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_10th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 11th
    void Handle_UniqAbil_11th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_11th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_11th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 12th
    void Handle_UniqAbil_12th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_12th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_12th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 22th
    void Handle_UniqAbil_22th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_22th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_22th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 25th
    void Handle_UniqAbil_25th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_25th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_25th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    //-------------------------------------------------- 28th
    void Handle_UniqAbil_28th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_UniqAbil_28th(hash_pr));
    }

    IEnumerator Corou_Handle_UniqAbil_28th(Hash128 hash_pr)
    {

        hashStates.Remove(hash_pr);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game actions
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game actions

    //--------------------------------------------------
    public void GameAction_AtkTargetUnit()
    {
        StartCoroutine(Corou_GameAction_AtkTargetUnit());
    }

    IEnumerator Corou_GameAction_AtkTargetUnit()
    {
        UnityEvent unityEvent = new UnityEvent();

        // jump
        unityEvent.AddListener(OnComplete_Atk_CalcDmg_AtkerJumped);
        TargetTweening.JumpObject(oriUnit_Cp.transform, oriUnit_Cp.transform.position,
            eBUnitPoint_Tfs[roundValue.tarUnitIndex].position, unityEvent, atkJumpDur);

        yield return new WaitUntil(() => ExistGameStates(GameState_En.Done_AtkerJumped));
        RemoveGameStates(GameState_En.Done_AtkerJumped);

        // stay
        GameObject atkJumpEff_GO_tp = Instantiate(atkJumpEff_Pf, oriUnit_Cp.transform);
        yield return new WaitForSeconds(atkJumpStayDur);
        Destroy(atkJumpEff_GO_tp);

        // return
        unityEvent.RemoveAllListeners();
        unityEvent.AddListener(OnComplete_Atk_CalcDmg_AtkerReturned);
        TargetTweening.JumpObject(oriUnit_Cp.transform, oriUnit_Cp.transform.position,
            bUnitPoint_Tfs[roundValue.originUnitIndex].position, unityEvent, atkJumpDur);
    }

    void OnComplete_Atk_CalcDmg_AtkerJumped()
    {
        AddGameStates(GameState_En.Done_AtkerJumped);
    }

    void OnComplete_Atk_CalcDmg_AtkerReturned()
    {
        AddGameStates(GameState_En.Done_AtkerReturned);
    }

    //-------------------------------------------------- Move action
    public void GameAction_Move(int oriUnitIndex_pr, int tarUnitIndex_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_Move(oriUnitIndex_pr, tarUnitIndex_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_Move(int oriUnitIndex_pr, int tarUnitIndex_pr, Hash128 hash_pr)
    {
        //        
        UnitCard oriUnit_Cp_pr = bUnit_Cps[oriUnitIndex_pr];
        UnitCard tarUnit_Cp_pr = bUnit_Cps[tarUnitIndex_pr];

        // handle effect
        float dur = moveUnitDur;
        DoMoveEff(oriUnit_Cp_pr, moveEff1_Pf, bUnitPoint_Tfs[oriUnitIndex_pr].position,
            bUnitPoint_Tfs[tarUnitIndex_pr].position, dur);
        DoMoveEff(tarUnit_Cp_pr, moveEff2_Pf, bUnitPoint_Tfs[tarUnitIndex_pr].position,
            bUnitPoint_Tfs[oriUnitIndex_pr].position, dur);

        // handle UI
        string actionText = "Player" + (mPlayerID + 1) + "の" + (tarUnit_Cp_pr.posType_Phases
            == UnitCard.UnitPositionType_Phases.Van ? "前衛" : "後衛") + tarUnit_Cp_pr.posIndex_Phases + "が"
            + (oriUnit_Cp_pr.posType_Phases == UnitCard.UnitPositionType_Phases.Rear ? "後衛" : "前衛")
            + oriUnit_Cp_pr.posIndex_Phases + "に移動";
        btlUI_Cp.SetActionEffText(actionText);

        // set data
        yield return new WaitForSeconds(dur);
        DoMoveAction(oriUnit_Cp_pr, tarUnit_Cp_pr);

        // finish action on UI
        yield return new WaitForSeconds(moveEffDur - dur);
        btlUI_Cp.FinishActionEffText();

        //
        hashStates.Remove(hash_pr);
    }

    void DoMoveEff(UnitCard moveUnit_Cp_pr, GameObject eff_Pf_pr, Vector3 oriPos, Vector3 lastPos, float dur)
    {
        //
        UnityEvent unityEvent = new UnityEvent();
        TargetTweening.JumpObject(moveUnit_Cp_pr.transform, oriPos, lastPos, unityEvent, dur);

        //
        GameObject moveEff_GO_tp = Instantiate(eff_Pf_pr, moveUnit_Cp_pr.effTri_Tf);
        Destroy(moveEff_GO_tp, dur);
    }

    void DoMoveAction(UnitCard oriUnit_Cp_pr, UnitCard targetUnit_Cp_pr)
    {
        //
        UnitCard.UnitPositionType_Phases oriPosType_tp = oriUnit_Cp_pr.posType_Phases;
        int oriPosIndex_tp = oriUnit_Cp_pr.posIndex_Phases;

        oriUnit_Cp_pr.posType_Phases = targetUnit_Cp_pr.posType_Phases;
        oriUnit_Cp_pr.posIndex_Phases = targetUnit_Cp_pr.posIndex_Phases;

        targetUnit_Cp_pr.posType_Phases = oriPosType_tp;
        targetUnit_Cp_pr.posIndex_Phases = oriPosIndex_tp;

        //
        if (oriUnit_Cp_pr.posType_Phases == UnitCard.UnitPositionType_Phases.Van)
        {
            bUnit_Cps[oriUnit_Cp_pr.posIndex_Phases] = oriUnit_Cp_pr;
        }
        else if (oriUnit_Cp_pr.posType_Phases == UnitCard.UnitPositionType_Phases.Rear)
        {
            bUnit_Cps[oriUnit_Cp_pr.posIndex_Phases + 2] = oriUnit_Cp_pr;
        }

        if (targetUnit_Cp_pr.posType_Phases == UnitCard.UnitPositionType_Phases.Van)
        {
            bUnit_Cps[targetUnit_Cp_pr.posIndex_Phases] = targetUnit_Cp_pr;
        }
        else if (targetUnit_Cp_pr.posType_Phases == UnitCard.UnitPositionType_Phases.Rear)
        {
            bUnit_Cps[targetUnit_Cp_pr.posIndex_Phases + 2] = targetUnit_Cp_pr;
        }
    }

    //-------------------------------------------------- Shien action
    public void GameAction_Shien(UnitCard tarUnit_Cp_pr, UnitCard shienUnit_Cp_pr)
    {
        StartCoroutine(Corou_GameAction_Shien(tarUnit_Cp_pr, shienUnit_Cp_pr));
    }

    IEnumerator Corou_GameAction_Shien(UnitCard tarUnit_Cp_pr, UnitCard shienUnit_Cp_pr)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        switch (shienUnit_Cp_pr.unitCardData.shienAbil.type)
        {
            case ShienAbilityType.Type01:

                tarUnit_Cp_pr.shienUnit_Cps.Add(tarUnit_Cp_pr);

                break;
            case ShienAbilityType.Type02:

                tarUnit_Cp_pr.shienUnit_Cps.Add(tarUnit_Cp_pr);

                break;

            case ShienAbilityType.Type03:

                tarUnit_Cp_pr.shienUnit_Cps.Add(tarUnit_Cp_pr);

                break;

            case ShienAbilityType.Type04:

                tarUnit_Cp_pr.shienUnit_Cps.Add(tarUnit_Cp_pr);

                break;

            case ShienAbilityType.Type05:

                tarUnit_Cp_pr.unitInfo.baseAgi += 1;
                tarUnit_Cp_pr.unitInfo.hitCorr += 1;
                tarUnit_Cp_pr.unitInfo.ctCorr += 1;

                break;

            case ShienAbilityType.Type06:

                tarUnit_Cp_pr.shienUnit_Cps.Add(tarUnit_Cp_pr);

                break;
        }

        //
        string shienEffText = "Player" + (mPlayerID + 1) + "が前衛" + (tarUnit_Cp.posIndex_Phases + 1)
            + "にしえん : " + shienUnit_Cp_pr.unitCardData.shienAbil.dsc;
        SetActionEffText(shienEffText);
        yield return new WaitForSeconds(shienEffDur);
        FinishActionEffText();

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    //--------------------------------------------------
    public void GameAction_FlipOverCard(UnitCard unit_Cp_pr, bool isConfirmed, Hash128 hash_pr)
    {
        float dur = 0.4f;
        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(() => OnComplete_GameAction_FlipOverCard(hash_pr));

        //
        if (unit_Cp_pr.unitInfo.visible == isConfirmed)
        {
            unityEvent.Invoke();
            return;
        }

        //
        unit_Cp_pr.unitInfo.visible = isConfirmed;
        unit_Cp_pr.m_placedPosture = isConfirmed;

        TargetTweening.DoFlipOverCard(unit_Cp_pr.transform, unityEvent, dur);
    }

    public void OnComplete_GameAction_FlipOverCard(Hash128 hash_pr)
    {
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    public void GameAction_GetSpMarker(Player_Phases player_Cp_pr, int count_pr, string dsc)
    {
        StartCoroutine(Corou_GameAction_GetSpMarker(player_Cp_pr, count_pr, dsc));
    }

    IEnumerator Corou_GameAction_GetSpMarker(Player_Phases player_Cp_pr, int count_pr, string dsc)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        player_Cp_pr.markersData.totalSpMarkers.count += count_pr;

        //
        btlUI_Cp.SetActionEffText(dsc);
        yield return new WaitForSeconds(actionEffTxtDur);
        btlUI_Cp.FinishActionEffText();

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    //-------------------------------------------------- def correction
    public void GameAction_AddDefCorrEff(UnitCard unit_Cp_pr, int amount, Hash128 eff_hash_pr,
        Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_AddDefCorr(unit_Cp_pr, amount, eff_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_AddDefCorr(UnitCard unit_Cp_pr, int amount, Hash128 eff_hash_pr,
        Hash128 hash_pr)
    {
        //
        GameObject eff_GO_tp = Instantiate(parEffs.defCorr_Pf, unit_Cp_pr.effTri_Tf);
        randObjects[eff_hash_pr] = eff_GO_tp;
        eff_GO_tp.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(parEffs.defCorrTriDur);

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    public void GameAction_RemoveDefCorrEff(Hash128 effGO_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_RemoveDefCorr(effGO_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_RemoveDefCorr(Hash128 effGO_hash_pr, Hash128 hash_pr)
    {
        //
        RemoveRandObj(effGO_hash_pr);

        yield return new WaitForSeconds(parEffs.defCorrEndDur);

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    public void GameAction_HitCorr(UnitCard unit_Cp_pr, int amount, string dsc)
    {
        StartCoroutine(Corou_GameAction_HitCorr(unit_Cp_pr, amount, dsc));
    }

    IEnumerator Corou_GameAction_HitCorr(UnitCard unit_Cp_pr, int amount, string dsc)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        unit_Cp_pr.unitInfo.hitCorr += amount;

        if (string.IsNullOrEmpty(dsc))
        {
            yield return new WaitForSeconds(Time.deltaTime);
        }
        else
        {
            btlUI_Cp.SetActionEffText(dsc);

            float dur = actionEffTxtDur;
            yield return new WaitForSeconds(dur);

            btlUI_Cp.FinishActionEffText();
        }

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    //--------------------------------------------------
    public void GameAction_AddAlliesDmgCorr(List<Hash128> effGO_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_AddAlliesDmgCorr(effGO_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_AddAlliesDmgCorr(List<Hash128> effGO_hash_pr, Hash128 hash_pr)
    {
        for (int i = 0; i < mPlayer_Cp.bUnit_Cps.Count; i++)
        {
            Hash128 hash_tp = RegRandObjHash();
            effGO_hash_pr.Add(hash_tp);

            randObjects[hash_tp] = Instantiate(parEffs.dmgCorr_Pf, mPlayer_Cp.bUnit_Cps[i].effTri_Tf);
        }

        yield return new WaitForSeconds(parEffs.dmgCorrTriDur);
        
        hashStates.Remove(hash_pr);
    }

    public void GameAction_RemoveAlliesDmgCorr(List<Hash128> effGO_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_RemoveAlliesDmgCorr(effGO_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_RemoveAlliesDmgCorr(List<Hash128> effGO_hash_pr, Hash128 hash_pr)
    {
        for (int i = effGO_hash_pr.Count - 1; i >= 0; i--)
        {
            RemoveRandObj(effGO_hash_pr[i]);
        }
        
        yield return new WaitForSeconds(parEffs.dmgCorrEndDur);

        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    public void GameAction_AddDmgCorr(UnitCard unit_Cp_pr, Hash128 effGO_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_AddDmgCorr(unit_Cp_pr, effGO_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_AddDmgCorr(UnitCard unit_Cp_pr, Hash128 effGO_hash_pr,
        Hash128 hash_pr)
    {
        GameObject eff_GO_tp = Instantiate(parEffs.dmgCorr_Pf, unit_Cp_pr.effTri_Tf);
        randObjects[effGO_hash_pr] = eff_GO_tp;
        eff_GO_tp.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(parEffs.dmgCorrTriDur);

        hashStates.Remove(hash_pr);
    }

    public void GameAction_RemoveDmgCorr(Hash128 effGO_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_RemoveDmgCorr(effGO_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_RemoveDmgCorr(Hash128 effGO_hash_pr, Hash128 hash_pr)
    {
        RemoveRandObj(effGO_hash_pr);

        yield return new WaitForSeconds(parEffs.dmgCorrEndDur);

        hashStates.Remove(hash_pr);
    }

    //-------------------------------------------------- GameAction Type07
    public void GameAction_SetMihari(int playerID_pr)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //


        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    //--------------------------------------------------
    public void GameAction_HandleDeadUnit()
    {
        StartCoroutine(Corou_GameAction_HandleDeadUnit());
    }

    IEnumerator Corou_GameAction_HandleDeadUnit()
    {
        // up ap, getting takara card
        playerAp = Mathf.Clamp(playerAp + 1, 0, 9);

        takaraDatas = dataManager_Cp.takaraDatas;

        mPlayer_Cp.MoveTakaraToExcavArea(takaraDatas[0]);
        yield return new WaitUntil(() => mPlayer_Cp.ExistGameStates(Player_Phases.GameState_En.Done_RearrangeTakara));
        mPlayer_Cp.RemoveGameStates(Player_Phases.GameState_En.Done_RearrangeTakara);

        takaraDatas.RemoveAt(0);

        // move units
        float moveTime = 1f;
        int deadUnitIndex = tarUnit_Cp.posIndex_Phases;

        UnityEvent unityEvent = new UnityEvent();
        unityEvent.AddListener(() => OnComplete_TranslateBUnits(deadUnitIndex));

        TargetTweening.TranslateGameObject(eBUnit_Cps[deadUnitIndex].transform, eBUnitPoint_Tfs[4], unityEvent,
            moveTime);
        TargetTweening.TranslateGameObject(eBUnit_Cps[2].transform, eBUnitPoint_Tfs[deadUnitIndex], unityEvent,
            moveTime);
        TargetTweening.TranslateGameObject(eBUnit_Cps[3].transform, eBUnitPoint_Tfs[2], unityEvent, moveTime);
        TargetTweening.TranslateGameObject(eBUnit_Cps[4].transform, eBUnitPoint_Tfs[3], unityEvent, moveTime);

        // exchange battle unit position infos
        ExchBUnitsPosInfo(eBUnit_Cps, deadUnitIndex, 2);
        ExchBUnitsPosInfo(eBUnit_Cps, 2, 3);
        ExchBUnitsPosInfo(eBUnit_Cps, 3, 4);

        //
        AddGameStates(GameState_En.Done_DeadUnit);
    }

    int translatedBUnitsCount;
    void OnComplete_TranslateBUnits(int deadUnitIndex)
    {
        translatedBUnitsCount++;

        if (translatedBUnitsCount == 4)
        {
            eBUnit_Cps[deadUnitIndex].transform.SetParent(eBUnitPoint_Tfs[4], true);
            eBUnit_Cps[2].transform.SetParent(eBUnitPoint_Tfs[deadUnitIndex], true);
            eBUnit_Cps[3].transform.SetParent(eBUnitPoint_Tfs[2], true);
            eBUnit_Cps[4].transform.SetParent(eBUnitPoint_Tfs[3], true);

            //
            translatedBUnitsCount = 0;
        }
    }

    void ExchBUnitsPosInfo(List<UnitCard> unit_Cps_pr, int oriIndex_pr, int tarIndex_pr)
    {
        //
        if (tarIndex_pr == 0 || tarIndex_pr == 1)
        {
            unit_Cps_pr[oriIndex_pr].posType_Phases = UnitCard.UnitPositionType_Phases.Van;
            unit_Cps_pr[oriIndex_pr].posIndex_Phases = tarIndex_pr;
        }
        else if (tarIndex_pr == 2 || tarIndex_pr == 3 || tarIndex_pr == 4)
        {
            unit_Cps_pr[oriIndex_pr].posType_Phases = UnitCard.UnitPositionType_Phases.Rear;
            unit_Cps_pr[oriIndex_pr].posIndex_Phases = tarIndex_pr - 2;
        }

        //
        if (oriIndex_pr == 0 || oriIndex_pr == 1)
        {
            unit_Cps_pr[tarIndex_pr].posType_Phases = UnitCard.UnitPositionType_Phases.Van;
            unit_Cps_pr[tarIndex_pr].posIndex_Phases = oriIndex_pr;
        }
        else if (oriIndex_pr == 2 || oriIndex_pr == 3 || oriIndex_pr == 4)
        {
            unit_Cps_pr[tarIndex_pr].posType_Phases = UnitCard.UnitPositionType_Phases.Rear;
            unit_Cps_pr[tarIndex_pr].posIndex_Phases = oriIndex_pr - 2;
        }

        //
        UnitCard tempUnit_Cp_tp = unit_Cps_pr[tarIndex_pr];
        unit_Cps_pr[tarIndex_pr] = unit_Cps_pr[oriIndex_pr];
        unit_Cps_pr[oriIndex_pr] = tempUnit_Cp_tp;
    }

    //--------------------------------------------------
    public void GameAction_DestMihariUnit(UnitCard unit_Cp_pr)
    {

    }

    //--------------------------------------------------
    public void GameActionEff_Guard(Transform effTgr_Tf_pr, float dur)
    {
        GameObject eff_GO_tp = Instantiate(guardEff_Pf, effTgr_Tf_pr);
        eff_GO_tp.transform.rotation = Quaternion.identity;

        Destroy(eff_GO_tp, dur);
    }

    //--------------------------------------------------
    public void GameAction_HpRec(UnitCard unit_Cp_pr, int amount)
    {
        //
        AddGameStates(GameState_En.GameEventsStarted);

        //
        RemoveGameStates(GameState_En.GameEventsStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle UI

    //--------------------------------------------------
    public string GetActionEffText(UnitCard unit_Cp_pr, AbilityType abilType_pr, string cont_pr = "")
    {
        string value = string.Empty;

        switch (abilType_pr)
        {
            case AbilityType.Normal:
                value = "通常攻撃能力発動" + "\r\n";
                break;
            case AbilityType.Spc1:
                value = "特殊攻撃能力1発動" + "\r\n";
                break;
            case AbilityType.Spc2:
                value = "特殊攻撃能力2発動" + "\r\n";
                break;
            case AbilityType.Uniq:
                value = "固有能力発動" + "\r\n";
                break;
            case AbilityType.Shien:
                value = "しえん効果発動" + "\r\n";
                break;
            case AbilityType.Item:
                value = "アイテム効果発動" + "\r\n";
                break;
        }

        value += cont_pr;

        return value;
    }

    //--------------------------------------------------
    public void SetNoticeEffText(string text_pr)
    {
        btlUI_Cp.SetNoticeEffText(text_pr);
    }

    //--------------------------------------------------
    public void FinishNoticeEffText()
    {
        btlUI_Cp.FinishNoticeEffText();
    }

    //--------------------------------------------------
    public void SetActionEffText(string text_pr)
    {
        btlUI_Cp.SetActionEffText(text_pr);
    }

    //--------------------------------------------------
    public void FinishActionEffText()
    {
        btlUI_Cp.FinishActionEffText();
    }

    #endregion

}
