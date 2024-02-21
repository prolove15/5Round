using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_AtkPanel : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields and Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields and Properties

    //-------------------------------------------------- serialize fields
    [SerializeField] SelectionPanel atkerSelect1_Cp, atkerSelect2_Cp;
    [SerializeField] SelectionPanel atkTarSelect1_Cp, atkTarSelect2_Cp;
    [SerializeField] SelectionPanel atkMethodSelect1_Cp, atkMethodSelect2_Cp, atkMethodSelect3_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction localPlayer_Cp;
    ActionPanelHandler actionHandler_Cp;
    Data_Phases data_Cp;

    //-------------------------------------------------- properties
    int roundId { get { return actionHandler_Cp.GetCurRoundIndex(); } }
    RoundData roundsData { get { return localPlayer_Cp.roundsData; } }
    TokensData tokensData { get { return localPlayer_Cp.tokensData; } }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Init
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Init

    //-------------------------------------------------- 
    public void Init()
    {
        SetComponents();
        InitAtkerSelectPanels();
        InitTarSelectPanels();
        InitAtkMethodSelectPanels();
        ResetPanel();
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        actionHandler_Cp = controller_Cp.ui_panelCanvas_Cp.actionPanel_Cp;
        data_Cp = controller_Cp.data_Cp;
    }

    //-------------------------------------------------- 
    void InitAtkerSelectPanels()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        atkerSelect1_Cp.Init(1, hash_tp);
        atkerSelect2_Cp.Init(2, hash_tp);

        if (localPlayer_Cp.playerId == 0)
        {
            atkerSelect1_Cp.SetBgd(data_Cp.p1Van1Sprite);
            atkerSelect2_Cp.SetBgd(data_Cp.p1Van2Sprite);
        }
        else if (localPlayer_Cp.playerId == 1)
        {
            atkerSelect1_Cp.SetBgd(data_Cp.p2Van1Sprite);
            atkerSelect2_Cp.SetBgd(data_Cp.p2Van2Sprite);
        }
    }

    //-------------------------------------------------- 
    void InitTarSelectPanels()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        atkTarSelect1_Cp.Init(1, hash_tp);
        atkTarSelect2_Cp.Init(2, hash_tp);

        if (localPlayer_Cp.playerId == 0)
        {
            atkTarSelect1_Cp.SetBgd(data_Cp.p2Van1Sprite);
            atkTarSelect2_Cp.SetBgd(data_Cp.p2Van2Sprite);
        }
        else if (localPlayer_Cp.playerId == 1)
        {
            atkTarSelect1_Cp.SetBgd(data_Cp.p1Van1Sprite);
            atkTarSelect2_Cp.SetBgd(data_Cp.p1Van2Sprite);
        }
    }

    //-------------------------------------------------- 
    void InitAtkMethodSelectPanels()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        atkMethodSelect1_Cp.Init(1, hash_tp);
        atkMethodSelect2_Cp.Init(2, hash_tp);
        atkMethodSelect3_Cp.Init(3, hash_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Refresh and Reset
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Refresh and Reset

    //--------------------------------------------------
    public void RefreshPanel()
    {
        RoundValue rndValue_tp = roundsData[roundId];

        // refresh description
        if (rndValue_tp.actionType == ActionType.Atk)
        {
            string atkerColorText_tp = string.Empty;
            if (localPlayer_Cp.playerId == 0)
            {
                if (rndValue_tp.oriUnitIndex == 0) { atkerColorText_tp = "赤"; }
                else if (rndValue_tp.oriUnitIndex == 1) { atkerColorText_tp = "紫"; }
            }
            else if (localPlayer_Cp.playerId == 1)
            {
                if (rndValue_tp.oriUnitIndex == 0) { atkerColorText_tp = "青"; }
                else if (rndValue_tp.oriUnitIndex == 1) { atkerColorText_tp = "緑"; }
            }

            string victimColorText_tp = string.Empty;
            if (localPlayer_Cp.playerId == 0)
            {
                if (rndValue_tp.tarUnitIndex == 0) { victimColorText_tp = "青"; }
                else if (rndValue_tp.tarUnitIndex == 1) { victimColorText_tp = "緑"; }
            }
            else if (localPlayer_Cp.playerId == 1)
            {
                if (rndValue_tp.tarUnitIndex == 0) { victimColorText_tp = "赤"; }
                else if (rndValue_tp.tarUnitIndex == 1) { victimColorText_tp = "紫"; }
            }

            string atkMethodText_tp = string.Empty;
            if (rndValue_tp.atkType == AttackType.Normal)
            {
                atkMethodText_tp = "通常攻撃";
            }
            else if (rndValue_tp.atkType == AttackType.Spc1)
            {
                atkMethodText_tp = "特殊攻撃1";
            }
            else if (rndValue_tp.atkType == AttackType.Spc2)
            {
                atkMethodText_tp = "特殊攻撃2";
            }

            actionHandler_Cp.descText_Cp.text = atkerColorText_tp + "が" + victimColorText_tp + atkMethodText_tp;
        }
        else
        {
            actionHandler_Cp.descText_Cp.text = string.Empty;
        }

        // refresh victim units selection
        atkTarSelect1_Cp.SetInteract(true);
        atkTarSelect2_Cp.SetInteract(true);
        atkTarSelect1_Cp.SetSelectedStatus(false);
        atkTarSelect2_Cp.SetSelectedStatus(false);

        if (rndValue_tp.actionType == ActionType.Atk)
        {
            if (rndValue_tp.tarUnitIndex == 0)
            {
                atkTarSelect1_Cp.SetSelectedStatus(true);
            }
            else if (rndValue_tp.tarUnitIndex == 1)
            {
                atkTarSelect2_Cp.SetSelectedStatus(true);
            }
        }

        // refresh atker units selection
        atkerSelect1_Cp.SetInteract(false);
        atkerSelect2_Cp.SetInteract(false);
        atkerSelect1_Cp.SetSelectedStatus(false);
        atkerSelect2_Cp.SetSelectedStatus(false);
        if (tokensData.restAtk1 > 0) { atkerSelect1_Cp.SetInteract(true); }
        if (tokensData.restAtk2 > 0) { atkerSelect2_Cp.SetInteract(true); }

        if (rndValue_tp.actionType == ActionType.Atk)
        {
            if (rndValue_tp.oriUnitIndex == 0)
            {
                atkerSelect1_Cp.SetSelectedStatus(true);
                atkerSelect1_Cp.SetInteract(true);
            }
            else if (rndValue_tp.oriUnitIndex == 1)
            {
                atkerSelect2_Cp.SetSelectedStatus(true);
                atkerSelect2_Cp.SetInteract(true);
            }
        }

        // refresh atk method selection
        atkMethodSelect1_Cp.SetInteract(true);
        atkMethodSelect2_Cp.SetInteract(true);
        atkMethodSelect3_Cp.SetInteract(true);
        atkMethodSelect1_Cp.SetSelectedStatus(false);
        atkMethodSelect2_Cp.SetSelectedStatus(false);
        atkMethodSelect3_Cp.SetSelectedStatus(false);

        if (rndValue_tp.actionType == ActionType.Atk)
        {
            if (rndValue_tp.atkType == AttackType.Normal) { atkMethodSelect1_Cp.SetSelectedStatus(true); }
            else if (rndValue_tp.atkType == AttackType.Spc1) { atkMethodSelect2_Cp.SetSelectedStatus(true); }
            else if (rndValue_tp.atkType == AttackType.Spc2) { atkMethodSelect3_Cp.SetSelectedStatus(true); }
        }
    }

    //--------------------------------------------------
    public void ResetPanel()
    {
        atkerSelect1_Cp.ResetPanel();
        atkerSelect2_Cp.ResetPanel();
        atkTarSelect1_Cp.ResetPanel();
        atkTarSelect2_Cp.ResetPanel();
        atkMethodSelect1_Cp.ResetPanel();
        atkMethodSelect2_Cp.ResetPanel();
        atkMethodSelect3_Cp.ResetPanel();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Save and Remove
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Save and Remove

    //--------------------------------------------------
    public void Save()
    {
        RoundValue rndValue_tp = new RoundValue();
        rndValue_tp.index = roundId;
        rndValue_tp.actionType = ActionType.Atk;
        
        if (atkTarSelect1_Cp.IsSelected())
        {
            rndValue_tp.tarUnitIndex = 0;
        }
        else if (atkTarSelect2_Cp.IsSelected())
        {
            rndValue_tp.tarUnitIndex = 1;
        }
        else
        {
            return;
        }

        if (atkerSelect1_Cp.IsSelected())
        {
            rndValue_tp.tokenType = TokenType.Attack1;
            rndValue_tp.oriUnitIndex = 0;
        }
        else if (atkerSelect2_Cp.IsSelected())
        {
            rndValue_tp.tokenType = TokenType.Attack2;
            rndValue_tp.oriUnitIndex = 1;
        }
        else
        {
            return;
        }

        if (atkMethodSelect1_Cp.IsSelected()) { rndValue_tp.atkType = AttackType.Normal; }
        else if (atkMethodSelect2_Cp.IsSelected()) { rndValue_tp.atkType = AttackType.Spc1; }
        else if (atkMethodSelect3_Cp.IsSelected()) { rndValue_tp.atkType = AttackType.Spc2; }
        else { return; }

        //
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[roundId].SaveRoundValue_Atk(rndValue_tp);
    }

    //--------------------------------------------------
    public void Remove()
    {
        RoundValue rndValue_tp = roundsData[roundId];
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[roundId].RemoveRoundValue_Atk(rndValue_tp);
    }

    #endregion

}
