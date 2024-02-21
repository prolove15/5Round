using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class GameActionHandler : MonoBehaviour
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
    [SerializeField]
    GameEventsHandler gEvHandler_Cp;

    //-------------------------------------------------- public fields
    List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields

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

    public List<Hash128> hashStates
    {
        get { return HashHandler.hashes; }
    }

    //-------------------------------------------------- private properties
    Controller_Phases controller_Cp
    {
        get { return gEvHandler_Cp.controller_Cp; }
    }

    UI_Phases uiManager_Cp
    {
        get { return controller_Cp.uiManager_Cp; }
    }

    UI_BattlePhase btlUI_Cp
    {
        get { return controller_Cp.btlController_Cp.btlUI_Cp; }
    }

    public Dictionary<Hash128, object> randObjects
    {
        get { return gEvHandler_Cp.randObjects; }
    }

    public ParEffects_Cs parEffs
    {
        get { return gEvHandler_Cp.parEffs; }
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
    /// <summary>
    /// Initialize
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Initialize

    //--------------------------------------------------
    public void Init()
    {
        AddMainGameState(GameState_En.Nothing);

        mainGameState = GameState_En.Inited;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Handle game actions
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Handle game actions

    //-------------------------------------------------- handle atk corr
    public void GameAction_AddAtkCorr(UnitCard unit_Cp_pr, Hash128 eff_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_AddAtkCorr(unit_Cp_pr, eff_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_AddAtkCorr(UnitCard unit_Cp_pr, Hash128 eff_hash_pr, Hash128 hash_pr)
    {
        GameObject eff_GO_tp = Instantiate(parEffs.atkCorr_Pf, unit_Cp_pr.effTri_Tf.position,
            Quaternion.identity);
        randObjects[eff_hash_pr] = eff_GO_tp;
        eff_GO_tp.transform.SetParent(unit_Cp_pr.effTri_Tf);

        yield return new WaitForSeconds(parEffs.atkCorrTriDur);

        hashStates.Remove(hash_pr);
    }

    public void GameAction_RemoveAtkCorr(Hash128 eff_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_RemoveAtkCorr(eff_hash_pr, hash_pr));
    }

    IEnumerator Corou_GameAction_RemoveAtkCorr(Hash128 eff_hash_pr, Hash128 hash_pr)
    {
        yield return new WaitForSeconds(parEffs.atkCorrEndDur);

        RemoveRandObj(eff_hash_pr);

        hashStates.Remove(hash_pr);
    }

    //-------------------------------------------------- select battle unit
    public void GameAction_SelectBUnitModal(int playerID_pr, List<UnitCard> bUnit_Cps_pr,
        Hash128 selectUnit_hash_pr, Hash128 hash_pr)
    {
        StartCoroutine(Corou_GameAction_SelectBUnitModal(playerID_pr, bUnit_Cps_pr, selectUnit_hash_pr,
            hash_pr));
    }

    IEnumerator Corou_GameAction_SelectBUnitModal(int playerId_pr, List<UnitCard> bUnit_Cps_pr,
        Hash128 selectUnit_hash_pr, Hash128 hash_pr)
    {
        List<UnitUI_Phases> unitUI_Cps_tp = uiManager_Cp.bUnitsUIPanel_GO
            .GetComponentsInChildren<UnitUI_Phases>().ToList();

        for (int i = 0; i < unitUI_Cps_tp.Count; i++)
        {
            int index_tp = i;
            unitUI_Cps_tp[i].unitBtn_Cp.onClick.AddListener(() =>
                OnClick_SelectBUnit(selectUnit_hash_pr, index_tp));
        }

        uiManager_Cp.SetbUnitsUIPanelStatus("playerID_pr = ", playerId_pr);

        uiManager_Cp.SetActive_bUnitsUIPanel(true);

        yield return new WaitUntil(() => randObjects[selectUnit_hash_pr] != null);

        uiManager_Cp.SetActive_bUnitsUIPanel(false);

        //
        hashStates.Remove(hash_pr);
    }

    void OnClick_SelectBUnit(Hash128 selectUnit_hash_pr, int unitIndex_pr)
    {
        randObjects[selectUnit_hash_pr] = unitIndex_pr;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Callback from UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Callback from UI

    //-------------------------------------------------- cb from select battle unit modal


    #endregion

}
