using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using TcgEngine.FX;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class Unit_Bb : Unit
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, Playing, Dead,
        OnCompleteSpawnEff, OnCompleteDeadEff,
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] CanvasGroup canvas_Cp;
    [SerializeField] Button unitBtn_Cp;
    [SerializeField] Image unitImg_Cp;
    [SerializeField] GameObject costPanel_GO;
    [SerializeField] Text costText_Cp;
    [SerializeField] GameObject hpPanel_GO;
    [SerializeField] Text hpText_Cp;
    [SerializeField] float unitShowDur = 0.3f;
    [SerializeField] GameObject spawnEff_Pf, deadEff_Pf;
    [SerializeField] float spawnEffDur = 1.5f;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    UI_PanelCanvas panelUI_Cp;

    int m_playerId;
    UnitPosType m_posType;
    bool m_isShow;

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

    public int playerId { get { return m_playerId; } }
    public UnitPosType posType { get { return m_posType; } }
    public bool isShow { get { return m_isShow; } }

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

    //-------------------------------------------------- Init
    public void Init(UnitCardData unitData_tp)
    {
        AddMainGameState(GameState_En.Nothing);

        unitData = unitData_tp;
        SetComponents();
        InitUnit();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        panelUI_Cp = controller_Cp.ui_panelCanvas_Cp;

        unitImg_Cp.sprite = unitData.frontSide;
        costText_Cp.text = unitData.cost.ToString();
        hpText_Cp.text = unitData.hp.ToString();
    }

    //--------------------------------------------------
    void InitUnit()
    {
        
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// External interface
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region External interface

    //--------------------------------------------------
    public void OnSpawn(int playerId_tp, UnitPosType posType_tp, bool isShow_tp)
    {
        StartCoroutine(Corou_OnSpawn(playerId_tp, posType_tp, isShow_tp));
    }

    IEnumerator Corou_OnSpawn(int playerId_tp, UnitPosType posType_tp, bool isShow_tp)
    {
        m_playerId = playerId_tp;
        m_posType = posType_tp;
        m_isShow = isShow_tp;

        ShowSpawnEff();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.OnCompleteSpawnEff));
        RemoveGameStates(GameState_En.OnCompleteSpawnEff);

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    public void OnDead()
    {
        StartCoroutine(Corou_ShowDeadEff());
    }

    IEnumerator Corou_ShowDeadEff()
    {
        ShowDeadEff();
        yield return new WaitUntil(() => ExistGameStates(GameState_En.OnCompleteDeadEff));
        RemoveGameStates(GameState_En.OnCompleteDeadEff);

        mainGameState = GameState_En.Dead;
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal callback
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal callback

    //--------------------------------------------------
    void ShowSpawnEff()
    {
        SetAlpha(1f, unitShowDur, OnComplete_SpawnEff);

        GameObject spawnEff_GO = Instantiate(spawnEff_Pf, transform);
        Destroy(spawnEff_GO, spawnEffDur);
    }

    //--------------------------------------------------
    void OnComplete_SpawnEff()
    {
        AddGameStates(GameState_En.OnCompleteSpawnEff);
    }

    //--------------------------------------------------
    void ShowDeadEff()
    {
        SetAlpha(0f, unitShowDur, OnComplete_DeadEff);
    }

    //--------------------------------------------------
    void OnComplete_DeadEff()
    {
        AddGameStates(GameState_En.OnCompleteDeadEff);
    }

    //--------------------------------------------------
    void SetAlpha(float val, float dur, UnityAction action_tp)
    {
        DOTween.To(() => canvas_Cp.alpha, x => canvas_Cp.alpha = x, val, dur)
            .OnComplete(() => action_tp.Invoke());
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Callback from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Callback from external

    //--------------------------------------------------
    public void OnClickUnitBtn()
    {
        panelUI_Cp.ViewCard(this);
    }

    #endregion
}
