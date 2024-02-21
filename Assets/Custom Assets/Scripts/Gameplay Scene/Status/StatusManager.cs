using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StatusVariables
{
    public static string[] factionNames = new string[2] { "赤＆紫陣営", "青＆緑陣営" };

    public static string[] opponentReadyStateTexts = new string[2] { "準備中", "準備完了" };
}

public class StatusManager : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Types
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Types

    public enum GameState_En
    {
        Nothing, Inited, AllCounting, WillFinish
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
    public StatusUI statusUI;

    //-------------------------------------------------- public fields
    [ReadOnly]
    public List<GameState_En> gameStates = new List<GameState_En>();

    [ReadOnly]
    public int maxLeftTime;

    //-------------------------------------------------- private fields
    // components
    GameObject controller_GO;

    // normal fields
    [SerializeField]
    [ReadOnly]
    int m_localFactionID;

    [SerializeField]
    [ReadOnly]
    int m_leftTime;

    [SerializeField]
    [ReadOnly]
    int m_turnIndex;

    [SerializeField]
    [ReadOnly]
    int m_attackPoint;

    [SerializeField]
    [ReadOnly]
    int m_gold;

    [SerializeField]
    [ReadOnly]
    bool m_opponentReadyState;

    List<string> instr_startPhase = new List<string>();

    List<string> instr_strPhase = new List<string>();

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

    public int localFactionID
    {
        get { return m_localFactionID; }
        set
        {
            m_localFactionID = value;

            if (value < 0)
            {
                statusUI.factionName = string.Empty;
            }
            else
            {
                statusUI.factionName = StatusVariables.factionNames[value];
            }
        }
    }

    public int leftTime
    {
        get { return m_leftTime; }
        set
        {
            m_leftTime = value;

            if (value < 0)
            {
                statusUI.leftTime = string.Empty;
            }
            else
            {
                statusUI.leftTime = value.ToString() + "秒";
            }
        }
    }

    public int turnIndex
    {
        get { return m_turnIndex; }
        set
        {
            m_turnIndex = value;

            if (value < 0)
            {
                statusUI.turnIndex = string.Empty;
            }
            else
            {
                statusUI.turnIndex = value.ToString();
            }
        }
    }

    public int attackPoint
    {
        get { return m_attackPoint; }
        set
        {
            m_attackPoint = value;

            if (value < 0)
            {
                statusUI.attackPoint = string.Empty;
            }
            else
            {
                statusUI.attackPoint = value.ToString();
            }
        }
    }

    public int gold
    {
        get { return m_gold; }
        set
        {
            m_gold = value;

            if (value < 0)
            {
                statusUI.gold = string.Empty;
            }
            else
            {
                statusUI.gold = value.ToString();
            }
        }
    }

    public bool opponentReadyState
    {
        get { return m_opponentReadyState; }
        set
        {
            m_opponentReadyState = value;
            
            statusUI.opponentReadyState = StatusVariables.opponentReadyStateTexts[value ? 1 : 0];
        }
    }

    public string instruction
    {
        set
        {
            statusUI.instruction = value;
        }
    }

    //-------------------------------------------------- private properties
    string dateTime
    {
        get { return DateTime.Now.ToString("HH:mm"); }
    }

    int battery
    {
        get { return (int)(SystemInfo.batteryLevel * 100f); }
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

        InitComponents();

        InitVariables();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_GO = GameObject.FindWithTag("GameController");
    }

    //--------------------------------------------------
    void InitComponents()
    {
        statusUI.instr_GO.SetActive(true);
    }

    //--------------------------------------------------
    void InitVariables()
    {
        SetDateTimeUI();

        SetBatteryUI();
    }

    //--------------------------------------------------
    public void InitInstructions_StartPhase()
    {
        instr_startPhase.Clear();

        //
        instr_startPhase.Add(new string("タ ーンカウントとAPを１増やします。"));
    }

    //--------------------------------------------------
    public void InitInstructions_StrPhase()
    {
        instr_strPhase.Clear();

        //
        instr_strPhase.Add(new string("各ROUNDをタップして作戦を決めましょう。\r\n" +
            "「みはり台」から敵の作戦の確認をしたり、\r\n「ゲームボード」から今の状況を確認できます。"));

        instr_strPhase.Add(new string("ROUNDの作戦を決めたら「更新」しましょう。\r\n" +
            "ゲームボードのカードを長押しで\r\n「現在のステ ータス」を確認できます。"));

        instr_strPhase.Add(new string("カ ードをタップするとステータスや装備中アイテム、\r\n" +
            "長押しでカ ードの拡大表示が可能です。"));

        instr_strPhase.Add(new string("カ ードを長押しでカードの拡大表示が可能です。\r\n" +
            "ステ ータスは現在の補正後の数値を表示しています。"));
    }

    //--------------------------------------------------
    public void DisableInstructionPanel()
    {
        statusUI.instr_GO.SetActive(false);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Counting status by time
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region StatusCounting

    //--------------------------------------------------
    public void StartAllCounting()
    {
        mainGameState = GameState_En.AllCounting;

        //
        StartDateTimeCounting();

        if (battery >= 0)
        {
            StartBatteryCounting();
        }

        StartLeftTimeCounting();
    }

    //--------------------------------------------------
    public void StartDateTimeCounting()
    {
        StartCoroutine(CorouStartDateTimeCounting());
    }

    IEnumerator CorouStartDateTimeCounting()
    {
        while (mainGameState == GameState_En.AllCounting)
        {
            SetDateTimeUI();

            yield return new WaitForSeconds(1f);
        }
    }

    //--------------------------------------------------
    public void StartBatteryCounting()
    {
        StartCoroutine(CorouStartBatteryCounting());
    }

    IEnumerator CorouStartBatteryCounting()
    {
        while (mainGameState == GameState_En.AllCounting)
        {
            SetBatteryUI();

            yield return new WaitForSeconds(1f);
        }
    }

    //--------------------------------------------------
    public void StartLeftTimeCounting()
    {
        StartCoroutine(CorouStartLeftTimeCounting());
    }

    IEnumerator CorouStartLeftTimeCounting()
    {
        while (leftTime > 0)
        {
            leftTime = leftTime;
            yield return new WaitForSeconds(1f);
            leftTime--;
        }

        controller_GO.SendMessage("OnFinishLeftTimeCounting");
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Set status UI
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region SetStatusUI

    //--------------------------------------------------
    void SetDateTimeUI()
    {
        statusUI.dateTime = dateTime;
    }

    //--------------------------------------------------
    void SetBatteryUI()
    {
        if (battery < 0)
        {
            statusUI.battery = "なし";
        }
        else
        {
            statusUI.battery = battery.ToString() + "%";
        }
    }

    //--------------------------------------------------
    public void SetInstructions_StartPhase()
    {
        instruction = instr_startPhase[0];
    }

    //--------------------------------------------------
    public void SetInstructions_StrPhase(int index)
    {
        instruction = instr_strPhase[index];
    }

    #endregion


}
