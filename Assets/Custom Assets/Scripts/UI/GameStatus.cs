using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameStatus : MonoBehaviour
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
    [SerializeField] Animator panelAnim_Cp;
    [SerializeField] RectTransform p1TMStatusCont_RT, p2TMStatusCont_RT;
    [SerializeField] RectTransform p1BtlInfoCont_RT, p2BtlInfoCont_RT;

    //-------------------------------------------------- public fields
    [SerializeField][ReadOnly] List<GameState_En> gameStates = new List<GameState_En>();

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction player1_Cp, player2_Cp;

    GameObject statusPiece_Pf;
    List<GameObject> p1TMStatus_GOs = new List<GameObject>();
    List<GameObject> p2TMStatus_GOs = new List<GameObject>();
    List<GameObject> p1BtlInfo_GOs = new List<GameObject>();
    List<GameObject> p2BtlInfo_GOs = new List<GameObject>();
    TextMeshProUGUI p1apText_Cp, p1spText_Cp, p1shienText_Cp, p1move1Text_Cp, p1move2Text_Cp, p1move3Text_Cp,
        p1atk1Text_Cp, p1atk2Text_Cp;
    TextMeshProUGUI p2apText_Cp, p2spText_Cp, p2shienText_Cp, p2move1Text_Cp, p2move2Text_Cp, p2move3Text_Cp,
        p2atk1Text_Cp, p2atk2Text_Cp;
    TextMeshProUGUI p1mihariText_Cp, p1discardText_Cp, p1kenText_Cp, p1maText_Cp, p1yumiText_Cp, p1fushiText_Cp,
        p1ryuText_Cp;
    TextMeshProUGUI p2mihariText_Cp, p2discardText_Cp, p2kenText_Cp, p2maText_Cp, p2yumiText_Cp, p2fushiText_Cp,
        p2ryuText_Cp;

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
    TokensData p1TokensData { get { return player1_Cp.tokensData; } }
    TokensData p2TokensData { get { return player2_Cp.tokensData; } }
    MarkersData p1MarkersData { get { return player1_Cp.markersData; } }
    MarkersData p2MarkersData { get { return player2_Cp.markersData; } }
    BattleInfo p1BtlInfo { get { return player1_Cp.battleInfo; } }
    BattleInfo p2BtlInfo { get { return player2_Cp.battleInfo; } }

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

    //--------------------------------------------------
    public void Init()
    {
        AddMainGameState(GameState_En.Nothing);

        SetComponents();
        InitP1TMStatusPanel();
        InitP1BattleInfoPanel();
        InitP2TMStatusPanel();
        InitP2BattleInfoPanel();

        mainGameState = GameState_En.Inited;
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        player1_Cp = controller_Cp.player_Cps[0];
        player2_Cp = controller_Cp.player_Cps[1];
    }

    //--------------------------------------------------
    void InitP1TMStatusPanel()
    {
        statusPiece_Pf = p1TMStatusCont_RT.GetChild(1).gameObject;

        // destry unnecessary objects
        for (int i = p1TMStatusCont_RT.childCount - 1; i >= 2; i++)
        {
            Destroy(p1TMStatusCont_RT.GetChild(i).gameObject);
        }

        // add new objects
        p1TMStatus_GOs.Add(statusPiece_Pf);
        for (int i = 2; i < 9; i++)
        {
            p1TMStatus_GOs.Add(Instantiate(statusPiece_Pf, p1TMStatusCont_RT));
        }

        // set components
        p1apText_Cp = p1TMStatus_GOs[0].GetComponentInChildren<TextMeshProUGUI>();
        p1spText_Cp = p1TMStatus_GOs[1].GetComponentInChildren<TextMeshProUGUI>();
        p1shienText_Cp = p1TMStatus_GOs[2].GetComponentInChildren<TextMeshProUGUI>();
        p1move1Text_Cp = p1TMStatus_GOs[3].GetComponentInChildren<TextMeshProUGUI>();
        p1move2Text_Cp = p1TMStatus_GOs[4].GetComponentInChildren<TextMeshProUGUI>();
        p1move3Text_Cp = p1TMStatus_GOs[5].GetComponentInChildren<TextMeshProUGUI>();
        p1atk1Text_Cp = p1TMStatus_GOs[6].GetComponentInChildren<TextMeshProUGUI>();
        p1atk2Text_Cp = p1TMStatus_GOs[7].GetComponentInChildren<TextMeshProUGUI>();
    }

    //--------------------------------------------------
    void InitP2TMStatusPanel()
    {
        // destroy unnecessary objects
        for (int i = p2TMStatusCont_RT.childCount - 1; i >= 1; i--)
        {
            Destroy(p2TMStatusCont_RT.GetChild(i).gameObject);
        }

        // add new objects
        for (int i = 0; i < 8; i++)
        {
            p2TMStatus_GOs.Add(Instantiate(statusPiece_Pf, p2TMStatusCont_RT));
        }

        // set components
        p2apText_Cp = p2TMStatus_GOs[0].GetComponentInChildren<TextMeshProUGUI>();
        p2spText_Cp = p2TMStatus_GOs[1].GetComponentInChildren<TextMeshProUGUI>();
        p2shienText_Cp = p2TMStatus_GOs[2].GetComponentInChildren<TextMeshProUGUI>();
        p2move1Text_Cp = p2TMStatus_GOs[3].GetComponentInChildren<TextMeshProUGUI>();
        p2move2Text_Cp = p2TMStatus_GOs[4].GetComponentInChildren<TextMeshProUGUI>();
        p2move3Text_Cp = p2TMStatus_GOs[5].GetComponentInChildren<TextMeshProUGUI>();
        p2atk1Text_Cp = p2TMStatus_GOs[6].GetComponentInChildren<TextMeshProUGUI>();
        p2atk2Text_Cp = p2TMStatus_GOs[7].GetComponentInChildren<TextMeshProUGUI>();
    }

    //--------------------------------------------------
    void InitP1BattleInfoPanel()
    {
        // destroy unnecessary objects
        for (int i = p1BtlInfoCont_RT.childCount - 1; i >= 1; i--)
        {
            Destroy(p1BtlInfoCont_RT.GetChild(i).gameObject);
        }

        // add new objects
        for (int i = 0; i < 7; i++)
        {
            p1BtlInfo_GOs.Add(Instantiate(statusPiece_Pf, p1BtlInfoCont_RT));
        }

        // set components
        p1mihariText_Cp = p1BtlInfo_GOs[0].GetComponentInChildren<TextMeshProUGUI>();
        p1discardText_Cp = p1BtlInfo_GOs[1].GetComponentInChildren<TextMeshProUGUI>();
        p1kenText_Cp = p1BtlInfo_GOs[2].GetComponentInChildren<TextMeshProUGUI>();
        p1maText_Cp = p1BtlInfo_GOs[3].GetComponentInChildren<TextMeshProUGUI>();
        p1yumiText_Cp = p1BtlInfo_GOs[4].GetComponentInChildren<TextMeshProUGUI>();
        p1fushiText_Cp = p1BtlInfo_GOs[5].GetComponentInChildren<TextMeshProUGUI>();
        p1ryuText_Cp = p1BtlInfo_GOs[6].GetComponentInChildren<TextMeshProUGUI>();
    }

    //--------------------------------------------------
    void InitP2BattleInfoPanel()
    {
        // destroy unnecessary objects
        for (int i = p2BtlInfoCont_RT.childCount - 1; i >= 1; i--)
        {
            Destroy(p2BtlInfoCont_RT.GetChild(i).gameObject);
        }

        // add new objects
        for (int i = 0; i < 7; i++)
        {
            p2BtlInfo_GOs.Add(Instantiate(statusPiece_Pf, p2BtlInfoCont_RT));
        }

        // set components
        p2mihariText_Cp = p2BtlInfo_GOs[0].GetComponentInChildren<TextMeshProUGUI>();
        p2discardText_Cp = p2BtlInfo_GOs[1].GetComponentInChildren<TextMeshProUGUI>();
        p2kenText_Cp = p2BtlInfo_GOs[2].GetComponentInChildren<TextMeshProUGUI>();
        p2maText_Cp = p2BtlInfo_GOs[3].GetComponentInChildren<TextMeshProUGUI>();
        p2yumiText_Cp = p2BtlInfo_GOs[4].GetComponentInChildren<TextMeshProUGUI>();
        p2fushiText_Cp = p2BtlInfo_GOs[5].GetComponentInChildren<TextMeshProUGUI>();
        p2ryuText_Cp = p2BtlInfo_GOs[6].GetComponentInChildren<TextMeshProUGUI>();
    }

    #endregion

    //--------------------------------------------------
    public void OnClickOpenPanel()
    {
        gameObject.SetActive(true);
        RefreshTMStatusPanel();
        RefreshBattleInfoPanel();

        panelAnim_Cp.SetTrigger("show");
    }

    //--------------------------------------------------
    public void OnClickClosePanel()
    {
        panelAnim_Cp.SetTrigger("hide");
        gameObject.SetActive(false);
    }

    //--------------------------------------------------
    void RefreshTMStatusPanel()
    {
        // refresh p1 token, markers status panel
        p1apText_Cp.text = "AP: " + player1_Cp.playerAp.ToString();
        p1spText_Cp.text = "SPマ ーカー: " + p1MarkersData.useSp.ToString() + "/" + p1MarkersData.sp.ToString();
        p1shienText_Cp.text = "しえん: " + p1TokensData.useShien.ToString() + "/" + p1TokensData.shien.ToString();
        p1move1Text_Cp.text = "いどう 1: " + p1TokensData.useMove1.ToString() + "/" + p1TokensData.move1.ToString();
        p1move2Text_Cp.text = "いどう 2: " + p1TokensData.useMove2.ToString() + "/" + p1TokensData.move2.ToString();
        p1move3Text_Cp.text = "いどう 3: " + p1TokensData.useMove3.ToString() + "/" + p1TokensData.move3.ToString();
        p1atk1Text_Cp.text = "こうげき(赤): " + p1TokensData.useAtk1.ToString() + "/" + p1TokensData.atk1.ToString();
        p1atk2Text_Cp.text = "こうげき(紫): " + p1TokensData.useAtk2.ToString() + "/" + p1TokensData.atk2.ToString();

        // refresh p2 token, markers status panel
        p2apText_Cp.text = "AP: " + player2_Cp.playerAp.ToString();
        p2spText_Cp.text = "SPマ ーカー: " + p2MarkersData.useSp.ToString() + "/" + p2MarkersData.sp.ToString();
        p2shienText_Cp.text = "しえん: " + p2TokensData.useShien.ToString() + "/" + p2TokensData.shien.ToString();
        p2move1Text_Cp.text = "いどう 1: " + p2TokensData.useMove1.ToString() + "/" + p2TokensData.move1.ToString();
        p2move2Text_Cp.text = "いどう 2: " + p2TokensData.useMove2.ToString() + "/" + p2TokensData.move2.ToString();
        p2move3Text_Cp.text = "いどう 3: " + p2TokensData.useMove3.ToString() + "/" + p2TokensData.move3.ToString();
        p2atk1Text_Cp.text = "こうげき (赤): " + p2TokensData.useAtk1.ToString() + "/" + p2TokensData.atk1.ToString();
        p2atk2Text_Cp.text = "こうげき (紫): " + p2TokensData.useAtk2.ToString() + "/" + p2TokensData.atk2.ToString();
    }

    //--------------------------------------------------
    void RefreshBattleInfoPanel()
    {
        // refresh p1 battle info panel
        p1mihariText_Cp.text = "みはり: " + p1BtlInfo.mihariUnitCount.ToString();
        p1discardText_Cp.text = "捨て札: " + p1BtlInfo.discardUnitCount.ToString();
        p1kenText_Cp.text = "けん: " + p1BtlInfo.ken.ToString();
        p1maText_Cp.text = "ま: " + p1BtlInfo.ma.ToString();
        p1yumiText_Cp.text = "ゆみ: " + p1BtlInfo.yumi.ToString();
        p1fushiText_Cp.text = "ふし: " + p1BtlInfo.fushi.ToString();
        p1ryuText_Cp.text = "りゅう: " + p1BtlInfo.ryu.ToString();

        // refresh p2 battle info panel
        p2mihariText_Cp.text = "みはり: " + p2BtlInfo.mihariUnitCount.ToString();
        p2discardText_Cp.text = "捨て札: " + p2BtlInfo.discardUnitCount.ToString();
        p2kenText_Cp.text = "けん: " + p2BtlInfo.ken.ToString();
        p2maText_Cp.text = "ま: " + p2BtlInfo.ma.ToString();
        p2yumiText_Cp.text = "ゆみ: " + p2BtlInfo.yumi.ToString();
        p2fushiText_Cp.text = "ふし: " + p2BtlInfo.fushi.ToString();
        p2ryuText_Cp.text = "りゅう: " + p2BtlInfo.ryu.ToString();
    }

}
