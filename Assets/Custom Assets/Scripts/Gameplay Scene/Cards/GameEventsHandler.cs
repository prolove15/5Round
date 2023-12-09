using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static UnitsAbilityHandler;

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
    UnitsAbilityHandler unitAbil_Cp;

    [SerializeField]
    GameObject bUnitsUIPanel_GO;

    [SerializeField]
    Text bUnitsUIPanelText_Cp;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<Hash128> hashStates = new List<Hash128>();

    //-------------------------------------------------- private fields
    // components
    List<UnitUI_Phases> bUnitsUI_Cps = new List<UnitUI_Phases>();

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
        get { return unitAbil_Cp.actionEffTxtDur; }
    }

    float noticeDur
    {
        get { return unitAbil_Cp.noticeTxtDur; }
    }

    EffectPrefab_Cs effPfs
    {
        get { return unitAbil_Cp.effPfs; }
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
    Controller_Phases controller_Cp
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

    List<RoundValue> roundsData
    {
        get { return unitAbil_Cp.roundsData; }
    }

    int curRoundIndex
    {
        get { return unitAbil_Cp.curRoundIndex; }
    }

    RoundValue roundValue
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

    string atkFailedMsg
    {
        get { return unitAbil_Cp.atkFailedMsg; }
    }

    AttackType atkType
    {
        get { return unitAbil_Cp.atkType; }
    }

    bool atkSucc
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

    public int playerAp
    {
        get { return startController_Cp.playerAPs[mPlayerID]; }
        set { startController_Cp.playerAPs[mPlayerID] = playerAp; }
    }

    //-------------------------------------------------- private properties
    Data_Phases.ParEffects_Cs parEffects
    {
        get { return controller_Cp.data_Cp.parEffects; }
    }

    List<GameEventsInfo> curGEventCollr
    {
        get { return controller_Cp.curGEventCollr; }
    }

    List<GameEventsInfo> waitGEventCollr
    {
        get { return controller_Cp.waitGEventCollr; }
    }

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
    /// Manage hash states
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Manage hash states

    //--------------------------------------------------
    public Hash128 RegRandHashValue()
    {
        Hash128 result = new Hash128();

        do
        {
            result = new Hash128();
            result.Append(Time.time.ToString() + UnityEngine.Random.value.ToString());
        }
        while (hashStates.Contains(result));

        hashStates.Add(result);

        return result;
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
        InitbUnitsUIPanel();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game events timing
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game events timing

    //--------------------------------------------------
    public void HandleGameEventsTiming(GameEventsTiming timing_pr)
    {
        StartCoroutine(Corou_HandleGameEventsTiming(timing_pr));
    }

    IEnumerator Corou_HandleGameEventsTiming(GameEventsTiming timing_pr)
    {
        //
        AddGameStates(GameState_En.GameEventsStarted);

        //
        UnitCard oriUnit_Cp_pr = oriUnit_Cp;
        AttackType atkType_pr = atkType;

        // handle waiting list events
        Hash128 hashState_curGEvent_tp = RegRandHashValue();

        HandleCurGameEvents(timing_pr, hashState_curGEvent_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hashState_curGEvent_tp));

        // handle timing
        GameEventData gEventData_tp = gEventsData[timing_pr];

        if (gEventData_tp.nlEvent != NormalAttackEvent.Null)
        {
            if (atkType_pr == AttackType.Normal || atkType_pr == AttackType.All)
            {
                Hash128 hashState_Abil_pr = RegRandHashValue();

                HandleNormalAtkEvents(oriUnit_Cp_pr, gEventData_tp.nlEvent, hashState_Abil_pr);
                yield return new WaitUntil(() => !hashStates.Contains(hashState_Abil_pr));
            }
        }

        if (gEventData_tp.spc1Event != GameEventsTiming.Null)
        {
            if (atkType_pr == AttackType.Spc1 || atkType_pr == AttackType.All)
            {
                Hash128 hashState_Abil_pr = RegRandHashValue();

                HandleSpcAtk1Events(oriUnit_Cp_pr, gEventData_tp.spc1Event, hashState_Abil_pr);
                yield return new WaitUntil(() => !hashStates.Contains(hashState_Abil_pr));
            }
        }

        if (gEventData_tp.spc2Event != GameEventsTiming.Null)
        {
            if (atkType_pr == AttackType.Spc2 || atkType_pr == AttackType.All)
            {
                Hash128 hashState_Abil_pr = RegRandHashValue();

                HandleSpcAtk2Events(oriUnit_Cp_pr, gEventData_tp.spc2Event, hashState_Abil_pr);
                yield return new WaitUntil(() => !hashStates.Contains(hashState_Abil_pr));
            }
        }

        if (gEventData_tp.uniqEvent != UniqueAbilityEvent.Null)
        {
            Hash128 hashState_Abil_pr = RegRandHashValue();

            HandleUniqAbil(oriUnit_Cp_pr, gEventData_tp.uniqEvent, hashState_Abil_pr);
            yield return new WaitUntil(() => !hashStates.Contains(hashState_Abil_pr));
        }

        // temporary remove
        //if (gEventData_tp.shienEvent != ShienAbilityEvent.Null)
        //{
        //    HandleShienAbil(unit_Cp_pr, gEventData_tp.shienEvent);

        //    yield return new WaitUntil(() => ExistGameStates(GameState_En.HandleAbilFinished));
        //    RemoveGameStates(GameState_En.HandleAbilFinished);
        //}

        //if (gEventData_tp.itemEvent != ItemEvent.Null)
        //{
        //    HandleItemAbil(unit_Cp_pr, gEventData_tp.itemEvent);

        //    yield return new WaitUntil(() => ExistGameStates(GameState_En.HandleAbilFinished));
        //    RemoveGameStates(GameState_En.HandleAbilFinished);
        //}

        //
        RemoveGameStates(GameState_En.GameEventsStarted);
        AddGameStates(GameState_En.GameEventsFinished);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game events

    //--------------------------------------------------
    public void HandleCurGameEvents(GameEventsTiming gTiming_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleCurGameEvents(gTiming_pr, hash_pr));
    }

    IEnumerator Corou_HandleCurGameEvents(GameEventsTiming gTiming_pr, Hash128 hash_pr)
    {
        //
        GameEventsInfo gEventCollr_tp = new GameEventsInfo();
        gEventCollr_tp.gTiming = gTiming_pr;

        curGEventCollr.Add(gEventCollr_tp);

        //
        yield return new WaitUntil(() => CompareCurAndWaitGEvents(gEventCollr_tp));

        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    bool CompareCurAndWaitGEvents(GameEventsInfo curGEventCollr_pr)
    {
        bool result = true;

        for (int i = 0; i < waitGEventCollr.Count; i++)
        {
            if (waitGEventCollr[i].gTiming == curGEventCollr_pr.gTiming)
            {
                result = false;
                break;
            }
        }

        return result;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle normal attack events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle normal attack events

    //--------------------------------------------------
    public void HandleNormalAtkEvents(UnitCard unit_Cp_pr, NormalAttackEvent nlAtkEvent_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleNormalAtkEvents(unit_Cp_pr, nlAtkEvent_pr, hash_pr));
    }

    IEnumerator Corou_HandleNormalAtkEvents(UnitCard unit_Cp_pr, NormalAttackEvent nlAtkEvent_pr, Hash128 hash_pr)
    {
        print("HandleNormalAtkEvents, playerID = " + unit_Cp_pr.playerID
            + ", roundIndex = " + curRoundIndex
            + ", unit ID = " + unit_Cp_pr.unitCardData.id
            + ", nlAtkEvent_pr = " + nlAtkEvent_pr);

        //
        GameEventData gEvent_tp = gEventsData.GetGEventFromAEvent(nlAtkEvent_pr);
        UnitCardData unitData_tp = unit_Cp_pr.unitCardData;

        if (unitData_tp.nlAtk.contents.Count > 0)
        {
            Hash128 hash_tp = RegRandHashValue();

            if (unitData_tp.id == 12)
            {
                Handle_NlAbil_12th(hash_tp);
            }
            else if (unitData_tp.id == 22)
            {

            }
            else if (unitData_tp.id == 25)
            {

            }
            else if (unitData_tp.id == 28)
            {

            }
            else if (unitData_tp.id == 32)
            {

            }
            else if (unitData_tp.id == 34)
            {

            }
            else if (unitData_tp.id == 41)
            {

            }
            else if (unitData_tp.id == 44)
            {

            }
            else if (unitData_tp.id == 45)
            {

            }
            else if (unitData_tp.id == 47)
            {

            }
            else if (unitData_tp.id == 48)
            {

            }

            yield return new WaitUntil(() => !hashStates.Contains(hash_tp));
        }

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle spc1 atk events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle spc1 atk events

    //--------------------------------------------------
    public void HandleSpcAtk1Events(UnitCard unit_Cp_pr, GameEventsTiming spcAtk1Event_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleSpcAtk1Events(unit_Cp_pr, spcAtk1Event_pr, hash_pr));
    }

    IEnumerator Corou_HandleSpcAtk1Events(UnitCard unit_Cp_pr, GameEventsTiming spcAtk1Event_pr, Hash128 hash_pr)
    {
        //
        GameEventData eventData_tp = gEventsData.GetGEventFromAEvent(spcAtk1Event_pr);
        UnitCardData unitData_tp = unit_Cp_pr.unitCardData;
        SpecialAttack1 spcAtk1_tp = unitData_tp.spcAtk1;

        //
        yield return null;

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle spc2 atk events
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle spc2 atk events

    //--------------------------------------------------
    public void HandleSpcAtk2Events(UnitCard unit_Cp_pr, GameEventsTiming spcAtk2Event_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleSpcAtk2Events(unit_Cp_pr, spcAtk2Event_pr, hash_pr));
    }

    IEnumerator Corou_HandleSpcAtk2Events(UnitCard unit_Cp_pr, GameEventsTiming spcAtk2Event_pr, Hash128 hash_pr)
    {
        //
        GameEventData eventData_tp = gEventsData.GetGEventFromAEvent(spcAtk2Event_pr);
        UnitCardData unitData_tp = unit_Cp_pr.unitCardData;
        SpecialAttack2 spcAtk2_tp = unitData_tp.spcAtk2;

        //
        yield return null;

        //
        hashStates.Remove(hash_pr);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle unique abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region HandleUniqAbil

    //--------------------------------------------------
    public void HandleUniqAbil(UnitCard unit_Cp_pr, UniqueAbilityEvent uniqAbilEvent_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_HandleUniqAbil(unit_Cp_pr, uniqAbilEvent_pr, hash_pr));
    }

    IEnumerator Corou_HandleUniqAbil(UnitCard unit_Cp_pr, UniqueAbilityEvent uniqAbilEvent_pr, Hash128 hash_pr)
    {
        // handle trigger events
        UnitCardData unitData = unit_Cp_pr.unitCardData;
        UniqueAbility uniqAbil = unitData.uniqAbil;
        UnitInfo unitInfo = unit_Cp_pr.unitInfo;

        //print("unit_Cp_pr name is " + unit_Cp_pr.unitCardData.name + ", uniqAbilEvent_pr is " + uniqAbilEvent_pr
        //    + ", unit id is " + unit_Cp_pr.unitCardData.id);

        //
        hashStates.Remove(hash_pr);

        yield break;

        if (uniqAbil.tgrEvents.Contains(uniqAbilEvent_pr))
        {
            //print("uniqAbil contains uniqAbilEvent_pr as tgrEvents");
            if (uniqAbil.type == UniqueAbilityType.Type02 || uniqAbil.type == UniqueAbilityType.Type16)
            {
                //GameAction_ChangeDefCorr(unit_Cp_pr, 1, GetActionEffText(AbilityType.Uniq, uniqAbil.dsc), eff_GO_tp);
                //yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                //RemoveGameStates(GameState_En.GameActionFinished);
            }
            else if (uniqAbil.type == UniqueAbilityType.Type17)
            {
                int playerID_tp = unit_Cp_pr.playerID;
                int kenCount = 0;

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    if (player_Cps[playerID_tp].bUnit_Cps[i].unitCardData.attrib.Contains(UnitAttribute.Ken))
                    {
                        kenCount++;
                    }
                    if (kenCount >= 2)
                    {
                        GameAction_GetSpMarker(player_Cps[playerID_tp], 1, "固有能力発動\r\n" + uniqAbil.dsc);
                        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                        RemoveGameStates(GameState_En.GameActionFinished);

                        //
                        break;
                    }
                }
            }
            else if (uniqAbil.type == UniqueAbilityType.Type20)
            {
                int playerID_tp = unit_Cp_pr.playerID;
                int maCount = 0;

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    if (player_Cps[playerID_tp].bUnit_Cps[i].unitCardData.attrib.Contains(UnitAttribute.Ma))
                    {
                        maCount++;
                    }
                    if (maCount >= 2)
                    {
                        GameAction_GetSpMarker(player_Cps[playerID_tp], 1, "固有能力発動\r\n" + uniqAbil.dsc);
                        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                        RemoveGameStates(GameState_En.GameActionFinished);

                        //
                        break;
                    }
                }
            }
            else if (uniqAbil.type == UniqueAbilityType.Type24)
            {
                int playerID_tp = unit_Cp_pr.playerID;
                int yumiCount = 0;

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    if (player_Cps[playerID_tp].bUnit_Cps[i].unitCardData.attrib.Contains(UnitAttribute.Yumi))
                    {
                        yumiCount++;
                    }

                    if (yumiCount >= 2)
                    {
                        GameAction_GetSpMarker(player_Cps[playerID_tp], 1, "固有能力発動\r\n" + uniqAbil.dsc);
                        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                        RemoveGameStates(GameState_En.GameActionFinished);

                        //
                        break;
                    }
                }
            }
            else if (uniqAbil.type == UniqueAbilityType.Type25)
            {
                int playerID_tp = unit_Cp_pr.playerID;
                int fushiCount = 0;

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    if (player_Cps[playerID_tp].bUnit_Cps[i].unitCardData.attrib.Contains(UnitAttribute.Fushi))
                    {
                        fushiCount++;
                    }

                    if (fushiCount >= 3)
                    {
                        //
                        GameAction_DestMihariUnit(unit_Cp_pr);
                        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                        RemoveGameStates(GameState_En.GameActionFinished);

                        //
                        GameAction_GetSpMarker(player_Cps[playerID_tp], 1, "固有能力発動\r\n" + uniqAbil.dsc);
                        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                        RemoveGameStates(GameState_En.GameActionFinished);

                        //
                        break;
                    }
                }
            }
            else if (uniqAbil.type == UniqueAbilityType.Type32)
            {
                int playerID_tp = unit_Cp_pr.playerID;

                btlUI_Cp.SetActionEffText("固有能力発動\r\n" + uniqAbil.dsc);

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    //GameAction_ChangeDefCorr(player_Cps[playerID_tp].bUnit_Cps[i], 1, string.Empty);
                    //yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                    //RemoveGameStates(GameState_En.GameActionFinished);
                }

                yield return new WaitForSeconds(noticeDur);
                btlUI_Cp.FinishActionEffText();
            }

            //
            uniqAbil.occured = true;
        }
        else
        {
            //print("uniqAbil tgrEvent 1 is " + uniqAbil.tgrEvents[0]);
        }

        // handle end events
        if (uniqAbil.endEvents.Contains(uniqAbilEvent_pr) && uniqAbil.occured)
        {
            if (uniqAbil.type == UniqueAbilityType.Type02 || uniqAbil.type == UniqueAbilityType.Type16)
            {
                //GameAction_ChangeDefCorr(unit_Cp_pr, -1, GetActionEffText(AbilityType.Uniq, uniqAbil.dsc));
            }
            else if (uniqAbil.type == UniqueAbilityType.Type32)
            {
                int playerID_tp = unit_Cp_pr.playerID;

                for (int i = 0; i < player_Cps[playerID_tp].bUnit_Cps.Count; i++)
                {
                    //GameAction_ChangeDefCorr(player_Cps[playerID_tp].bUnit_Cps[i], -1,
                    //    GetActionEffText(AbilityType.Uniq, uniqAbil.dsc));
                    //yield return new WaitUntil(() => ExistGameStates(GameState_En.GameActionFinished));
                    //RemoveGameStates(GameState_En.GameActionFinished);
                }
            }

            //
            uniqAbil.occured = false;
        }

    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle shien abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region HandleShienAbil

    //--------------------------------------------------
    public void HandleShienAbil(UnitCard unit_Cp_pr, ShienAbilityEvent shienAbilEvent_pr)
    {
        StartCoroutine(Corou_HandleShienAbil(unit_Cp_pr, shienAbilEvent_pr));
    }

    IEnumerator Corou_HandleShienAbil(UnitCard unit_Cp_pr, ShienAbilityEvent shienAbilEvent_pr)
    {
        //
        AddGameStates(GameState_En.HandleAbilStarted);

        //
        //for (int i = 0; i < unit_Cp_pr.shienUnit_Cps.Count; i++)
        //{
        //    ShienAbility shienAbil = unit_Cp_pr.shienUnit_Cps[i].unitCardData.shienAbil;

        //    if (shienAbil.tgrEvent == shienAbilEvent_pr)
        //    {
        //        switch (shienAbilEvent_pr)
        //        {
        //            case ShienAbilityEvent.NextAtk:
        //                //
        //                break;
        //            case ShienAbilityEvent.NextDmg:
        //                //
        //                break;
        //        }
        //    }

        //    if (shienAbil.endEvent == shienAbilEvent_pr)
        //    {
        //        switch (shienAbilEvent_pr)
        //        {
        //            case ShienAbilityEvent.EndPhase:
        //                //
        //                break;
        //        }
        //    }
        //}


        //
        AddGameStates(GameState_En.HandleAbilStarted);

        //
        GameEventData eventData_tp = gEventsData.GetGEventFromAEvent(shienAbilEvent_pr);
        UnitCardData unitData_tp = unit_Cp_pr.unitCardData;
        ShienAbility shienAbil_tp = unitData_tp.shienAbil;

        string dsc = GetActionEffText(unit_Cp_pr, AbilityType.Shien, shienAbil_tp.dsc);
        if (unit_Cp_pr.unitInfo.visible)
        {
            btlUI_Cp.SetActionEffText(dsc);
            yield return new WaitForSeconds(actionEffTxtDur);
            btlUI_Cp.FinishActionEffText();
        }

        //
        RemoveGameStates(GameState_En.HandleAbilStarted);
        AddGameStates(GameState_En.HandleAbilDone);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle item abilities
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region HandleItemAbil

    //--------------------------------------------------
    public void HandleItemAbil(UnitCard unit_Cp_pr, ItemEvent itemEvent_pr)
    {
        StartCoroutine(Corou_HandleItemAbil(unit_Cp_pr, itemEvent_pr));
    }

    IEnumerator Corou_HandleItemAbil(UnitCard unit_Cp_pr, ItemEvent itemEvent_pr)
    {
        //
        AddGameStates(GameState_En.HandleAbilStarted);

        //
        //for (int i = 0; i < unit_Cp_pr.equipItems.Count; i++)
        //{
        //    ItemCardData itemData = unit_Cp_pr.equipItems[i];

        //    for (int j = 0; j < itemData.contents.Count; j++)
        //    {
        //        if (itemData.tgrEvents.Contains(itemEvent_pr))
        //        {
        //            switch (itemEvent_pr)
        //            {
        //                case ItemEvent.ImmTgr:
        //                    //
        //                    break;
        //                case ItemEvent.HitSuccOver8:
        //                    //
        //                    break;
        //            }
        //        }

        //        if (itemData.endEvents.Contains(itemEvent_pr))
        //        {
        //            switch (itemEvent_pr)
        //            {
        //                case ItemEvent.UnitDead:
        //                    //
        //                    break;
        //            }
        //        }
        //    }
        //}

        //
        RemoveGameStates(GameState_En.HandleAbilStarted);
        AddGameStates(GameState_En.HandleAbilDone);
        yield return null;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle normal ability
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle normal ability

    //--------------------------------------------------
    public void Handle_NlAbil_12th(Hash128 hash_pr)
    {
        StartCoroutine(Corou_Handle_NlAbil_12th(hash_pr));
    }

    IEnumerator Corou_Handle_NlAbil_12th(Hash128 hash_pr)
    {
        //
        UnitCard unit_Cp_tp = oriUnit_Cp;
        UnitCardData unitData_tp = unit_Cp_tp.unitCardData;
        NormalAttack nlAtk_tp = unitData_tp.nlAtk;
        UnitInfo unitInfo_tp = unit_Cp_tp.unitInfo;

        // add ability
        unitInfo_tp.defCorr += nlAtk_tp.contents[0].amount;
        print("unit defCorr = " + unitInfo_tp.defCorr);

        Hash128 hash_tp = RegRandHashValue();
        GameObject eff_GO_tp = null;
        GameAction_AddDefCorrEff(unit_Cp_tp, nlAtk_tp.contents[0].amount, hash_tp, eff_GO_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        //
        GameEventsInfo waitGEventCollr_tp = new GameEventsInfo();
        waitGEventCollr_tp.gTiming = GameEventsTiming.RndEnd;
        waitGEventCollr.Add(waitGEventCollr_tp);

        //
        hashStates.Remove(hash_pr);

        //
        yield return new WaitUntil(() => CompareCurAndWaitGEvents(waitGEventCollr_tp));

        // remove ability
        unitInfo_tp.defCorr -= nlAtk_tp.contents[0].amount;

        Hash128 hash2_tp = RegRandHashValue();
        GameAction_RemoveDefCorrEff(eff_GO_tp, hash2_tp);
        yield return new WaitUntil(() => !hashStates.Contains(hash_tp));

        waitGEventCollr.Remove(waitGEventCollr_tp);
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
            eBUnitPoint_Tfs[roundValue.targetUnitIndex].position, unityEvent, atkJumpDur);

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
    public void GameAction_Move()
    {
        StartCoroutine(Corou_GameAction_Move());
    }

    IEnumerator Corou_GameAction_Move()
    {
        //
        int oriUnitIndex = roundValue.originUnitIndex + 2;
        int targetUnitIndex = roundValue.targetUnitIndex;

        UnitCard oriUnit_Cp_tp = bUnit_Cps[oriUnitIndex];
        UnitCard targetUnit_Cp_tp = bUnit_Cps[targetUnitIndex];

        // handle effect
        float dur = moveUnitDur;
        DoMoveEff(oriUnit_Cp_tp, moveEff1_Pf, bUnitPoint_Tfs[oriUnitIndex].position,
            bUnitPoint_Tfs[targetUnitIndex].position, dur);
        DoMoveEff(targetUnit_Cp_tp, moveEff2_Pf, bUnitPoint_Tfs[targetUnitIndex].position,
            bUnitPoint_Tfs[oriUnitIndex].position, dur);

        // handle UI
        string actionText = "Player" + (mPlayerID + 1) + "の" + (targetUnit_Cp_tp.posType_Phases
            == UnitCard.UnitPositionType_Phases.Van ? "前衛" : "後衛") + targetUnit_Cp_tp.posIndex_Phases + "が"
            + (oriUnit_Cp_tp.posType_Phases == UnitCard.UnitPositionType_Phases.Rear ? "後衛" : "前衛")
            + oriUnit_Cp_tp.posIndex_Phases + "に移動";
        btlUI_Cp.SetActionEffText(actionText);

        // set data
        yield return new WaitForSeconds(dur);
        DoMoveAction(oriUnit_Cp_tp, targetUnit_Cp_tp);

        // finish action on UI
        yield return new WaitForSeconds(moveEffDur - dur);
        btlUI_Cp.FinishActionEffText();

        //
        AddGameStates(GameState_En.GameActionFinished);
    }

    void DoMoveEff(UnitCard moveUnit_Cp_pr, GameObject eff_Pf_pr, Vector3 oriPos, Vector3 lastPos, float dur)
    {
        //
        UnityEvent unityEvent = new UnityEvent();
        TargetTweening.JumpObject(moveUnit_Cp_pr.transform, oriPos, lastPos, unityEvent, dur);

        //
        GameObject moveEff_GO_tp = Instantiate(eff_Pf_pr, moveUnit_Cp_pr.effTgr_Tf);
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
    public void GameAction_Shien(UnitCard targetUnit_Cp_pr, UnitCard shienUnit_Cp_pr)
    {
        StartCoroutine(Corou_GameAction_Shien(targetUnit_Cp_pr, shienUnit_Cp_pr));
    }

    IEnumerator Corou_GameAction_Shien(UnitCard targetUnit_Cp_pr, UnitCard shienUnit_Cp_pr)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        switch (shienUnit_Cp_pr.unitCardData.shienAbil.type)
        {
            case ShienAbilityType.Type01:

                targetUnit_Cp_pr.shienUnit_Cps.Add(targetUnit_Cp_pr);

                break;
            case ShienAbilityType.Type02:

                targetUnit_Cp_pr.shienUnit_Cps.Add(targetUnit_Cp_pr);

                break;

            case ShienAbilityType.Type03:

                targetUnit_Cp_pr.shienUnit_Cps.Add(targetUnit_Cp_pr);

                break;

            case ShienAbilityType.Type04:

                targetUnit_Cp_pr.shienUnit_Cps.Add(targetUnit_Cp_pr);

                break;

            case ShienAbilityType.Type05:

                targetUnit_Cp_pr.unitInfo.baseAgi += 1;
                targetUnit_Cp_pr.unitInfo.hitCorr += 1;
                targetUnit_Cp_pr.unitInfo.ctCorr += 1;

                break;

            case ShienAbilityType.Type06:

                targetUnit_Cp_pr.shienUnit_Cps.Add(targetUnit_Cp_pr);

                break;
        }

        //
        string shienEffText = "Player" + (mPlayerID + 1) + "が前衛" + (tarUnit_Cp.posIndex_Phases + 1)
            + "にしえん : " + shienUnit_Cp_pr.unitCardData.shienAbil.dsc;
        btlUI_Cp.SetActionEffText(shienEffText);

        yield return new WaitForSeconds(shienEffDur);

        btlUI_Cp.FinishActionEffText();

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
        yield return null;
    }

    //--------------------------------------------------
    public void GameAction_FlipOverCard(UnitCard unit_Cp_pr, bool flag, UnityEvent unityEvent, float dur = 0.4f)
    {
        AddGameStates(GameState_En.GameActionStarted);

        //
        if (unit_Cp_pr.unitInfo.visible == flag)
        {
            unityEvent.Invoke();
            return;
        }

        //
        unit_Cp_pr.unitInfo.visible = flag;
        unit_Cp_pr.m_placedPosture = flag;

        TargetTweening.DoFlipOverCard(unit_Cp_pr.transform, unityEvent, dur);
    }

    public void OnComplete_GameAction_FlipOverCard()
    {
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
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

    //--------------------------------------------------
    public void GameAction_ChangeAtkCorr(UnitCard unit_Cp_pr, int amount, string dsc)
    {
        StartCoroutine(Corou_GameAction_ChangeAtkCorr(unit_Cp_pr, amount, dsc));
    }

    IEnumerator Corou_GameAction_ChangeAtkCorr(UnitCard unit_Cp_pr, int amount, string dsc)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        unit_Cp_pr.unitInfo.atkCorr += amount; // rewrite this code

        //
        if (string.IsNullOrEmpty(dsc))
        {
            yield return new WaitForSeconds(guardEffDur);
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
        yield return null;
    }

    //--------------------------------------------------
    public void GameAction_AddDefCorrEff(UnitCard unit_Cp_pr, int amount, Hash128 hash_pr, GameObject eff_GO_pr)
    {
        StartCoroutine(Corou_GameAction_AddDefCorr(unit_Cp_pr, amount, hash_pr, eff_GO_pr));
    }

    IEnumerator Corou_GameAction_AddDefCorr(UnitCard unit_Cp_pr, int amount, Hash128 hash_pr, GameObject eff_GO_pr)
    {
        //
        eff_GO_pr = Instantiate(guardEff_Pf, unit_Cp_pr.effTgr_Tf);
        eff_GO_pr.transform.rotation = Quaternion.identity;

        yield return new WaitForSeconds(guardEffDur);

        //
        hashStates.Remove(hash_pr);
    }

    //--------------------------------------------------
    public void GameAction_RemoveDefCorrEff(GameObject eff_GO_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_RemoveDefCorr(eff_GO_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_RemoveDefCorr(GameObject eff_GO_pr, Hash128 hash_pr)
    {
        //
        Destroy(eff_GO_pr);
        yield return new WaitForSeconds(Time.deltaTime);

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
    public void GameAction_AlliesDmgCorr(int playerID_pr, int amount)
    {
        List<UnitCard> bUnit_Cps_tp = player_Cps[playerID_pr].bUnit_Cps;

        for (int i = 0; i < bUnit_Cps_tp.Count; i++)
        {
            bUnit_Cps_tp[i].unitInfo.defCorr += amount;
        }
    }

    //-------------------------------------------------- GameAction Type06
    public void GameAction_DoubleAtk(UnitCard oriUnit_Cp_pr, UnitCard tarUnit_Cp_pr, string dsc)
    {
        StartCoroutine(Corou_GameAction_DoubleAtk(oriUnit_Cp_pr, tarUnit_Cp_pr, dsc));
    }

    IEnumerator Corou_GameAction_DoubleAtk(UnitCard unit_Cp_pr, UnitCard tarUnit_Cp_pr, string dsc)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //
        if (!string.IsNullOrEmpty(dsc))
        {
            btlUI_Cp.SetActionEffText(dsc);
        }

        if (!mPlayer_Cp.isCom)
        {
            int tarPlayerID_tp = unit_Cp_pr.playerID == 0 ? 1 : 0;
            SetbUnitsUIPanelStatus("攻撃対象選択", tarPlayerID_tp, 0, 1);

            yield return new WaitUntil(() => ExistGameStates(GameState_En.BUnitUISelected));
            RemoveGameStates(GameState_En.BUnitUISelected);

            // write here code
        }
        else
        {
            // write here code
        }

        if (!string.IsNullOrEmpty(dsc))
        {
            btlUI_Cp.FinishActionEffText();
        }

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
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
        //******************** 14th timing. when unit dead


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
    public void GameAction_GetOrDestItem(UnitCard unit_Cp_pr, ItemCardData itemData_pr)
    {
        StartCoroutine(Corou_GameAction_GetOrDestItem(unit_Cp_pr, itemData_pr));
    }

    IEnumerator Corou_GameAction_GetOrDestItem(UnitCard unit_Cp_pr, ItemCardData itemData_pr)
    {
        //
        AddGameStates(GameState_En.GameActionStarted);

        //******************** 10th timing. get or destroy item
        HandleGameEventsTiming(GameEventsTiming.ItemGetOrDest);
        yield return new WaitUntil(() => ExistGameStates(GameState_En.GameEventsFinished));
        RemoveGameStates(GameState_En.GameEventsFinished);

        //
        RemoveGameStates(GameState_En.GameActionStarted);
        AddGameStates(GameState_En.GameActionFinished);
    }

    //--------------------------------------------------
    public void GameAction_HpRec(UnitCard unit_Cp_pr, int amount)
    {
        //
        AddGameStates(GameState_En.GameEventsStarted);

        //******************** 25th timing. Hp recover


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

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle selecting battle unit
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle selecting battle unit

    //--------------------------------------------------
    void InitbUnitsUIPanel()
    {
        //
        bUnitsUI_Cps = bUnitsUIPanel_GO.GetComponentsInChildren<UnitUI_Phases>().ToList();
        
        for (int i = 0; i < bUnitsUI_Cps.Count; i++)
        {
            int id = i;
            bUnitsUI_Cps[i].GetComponent<Button>().onClick.AddListener(() => OnClick_bUnitUIBtn(id));
        }

        //
        SetActive_bUnitsUIPanel(false);
    }

    //--------------------------------------------------
    public void SetbUnitsUIPanelStatus(string text, int playerID_pr, params int[] unitIds_pr)
    {
        //
        bUnitsUIPanelText_Cp.text = text;

        //
        List<UnitCard> bUnit_Cps_tp = controller_Cp.player_Cps[playerID_pr].bUnit_Cps;
        for (int i = 0; i < bUnit_Cps_tp.Count; i++)
        {
            bUnitsUI_Cps[i].frontSprite = bUnit_Cps_tp[i].frontSide;
            bUnitsUI_Cps[i].hp = bUnit_Cps_tp[i].unitInfo.curHp;

            if (unitIds_pr.Contains(i))
            {
                bUnitsUI_Cps[i].GetComponent<Button>().interactable = true;
            }
            else
            {
                bUnitsUI_Cps[i].GetComponent<Button>().interactable = false;
            }
        }

        //
        SetActive_bUnitsUIPanel(true);

        //
        AddGameStates(GameState_En.BUnitUIStarted);
    }

    public void SetActive_bUnitsUIPanel(bool activeFlag)
    {
        bUnitsUIPanel_GO.SetActive(activeFlag);

        //
        RemoveGameStates(GameState_En.BUnitUIStarted);
    }

    //--------------------------------------------------
    public void OnClick_bUnitUIBtn(int id)
    {
        print("OnClick_SelectBUnitBtn, id = " + id);
        actionTarUnitIndex = id;

        //
        AddGameStates(GameState_En.BUnitUISelected);
    }

    #endregion

}
