using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Action_MovePanel : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields & Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] SelectionPanel moveOriSelect1_Cp, moveOriSelect2_Cp;
    [SerializeField] SelectionPanel moveTarSelect1_Cp, moveTarSelect2_Cp, moveTarSelect3_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    ActionPanelHandler actionHandler_Cp;
    PlayerFaction localPlayer_Cp;
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

    //-------------------------------------------------- Init
    public void Init()
    {
        SetComponents();
        InitMoveOriSelectPanels();
        InitMoveTarSelectPanels();
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
    void InitMoveOriSelectPanels()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        moveOriSelect1_Cp.Init(0, hash_tp);
        moveOriSelect2_Cp.Init(1, hash_tp);

        if (localPlayer_Cp.playerId == 0)
        {
            moveOriSelect1_Cp.SetBgd(data_Cp.p1Van1Sprite);
            moveOriSelect2_Cp.SetBgd(data_Cp.p1Van2Sprite);
        }
        else if (localPlayer_Cp.playerId == 1)
        {
            moveOriSelect1_Cp.SetBgd(data_Cp.p2Van1Sprite);
            moveOriSelect2_Cp.SetBgd(data_Cp.p2Van2Sprite);
        }
    }

    //--------------------------------------------------
    void InitMoveTarSelectPanels()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        moveTarSelect1_Cp.Init(0, hash_tp);
        moveTarSelect2_Cp.Init(1, hash_tp);
        moveTarSelect3_Cp.Init(2, hash_tp);
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
        if (rndValue_tp.actionType == ActionType.Move)
        {
            string colorText_tp = string.Empty;
            if (localPlayer_Cp.playerId == 0)
            {
                if (rndValue_tp.oriUnitIndex == 0) { colorText_tp = "赤"; }
                else if (rndValue_tp.oriUnitIndex == 1) { colorText_tp = "青"; }
            }
            else if (localPlayer_Cp.playerId == 1)
            {
                if (rndValue_tp.oriUnitIndex == 0) { colorText_tp = "緑"; }
                else if (rndValue_tp.oriUnitIndex == 1) { colorText_tp = "紫"; }
            }
            actionHandler_Cp.descText_Cp.text = colorText_tp + "が" + "後衛" + rndValue_tp.tarUnitIndex.ToString() + "と入替";
        }
        else
        {
            actionHandler_Cp.descText_Cp.text = string.Empty;
        }

        // refresh ori units selection
        moveOriSelect1_Cp.SetSelectedStatus(false);
        moveOriSelect2_Cp.SetSelectedStatus(false);
        if (rndValue_tp.actionType == ActionType.Move)
        {
            moveOriSelect1_Cp.SetInteract(true);
            moveOriSelect2_Cp.SetInteract(true);
            if (rndValue_tp.oriUnitIndex == 0)
            {
                moveOriSelect1_Cp.SetSelectedStatus(true);
            }
            else if (rndValue_tp.oriUnitIndex == 1)
            {
                moveOriSelect2_Cp.SetSelectedStatus(true);
            }
        }
        else
        {
            moveOriSelect1_Cp.SetInteract(false);
            moveOriSelect2_Cp.SetInteract(false);
            if (tokensData.restMove > 0)
            {
                moveOriSelect1_Cp.SetInteract(true);
                moveOriSelect2_Cp.SetInteract(true);
            }
        }

        // refresh tar units selection
        moveTarSelect1_Cp.SetSelectedStatus(false);
        moveTarSelect2_Cp.SetSelectedStatus(false);
        moveTarSelect3_Cp.SetSelectedStatus(false);
        moveTarSelect1_Cp.SetInteract(false);
        moveTarSelect2_Cp.SetInteract(false);
        moveTarSelect3_Cp.SetInteract(false);

        if (tokensData.restMove1 > 0)
        {
            moveTarSelect1_Cp.SetInteract(true);
        }
        if (tokensData.restMove2 > 0)
        {
            moveTarSelect2_Cp.SetInteract(true);
        }
        if (tokensData.restMove3 > 0)
        {
            moveTarSelect3_Cp.SetInteract(true);
        }

        if (rndValue_tp.actionType == ActionType.Move)
        {
            if (rndValue_tp.tarUnitIndex == 0)
            {
                moveTarSelect1_Cp.SetSelectedStatus(true);
                moveTarSelect1_Cp.SetInteract(true);
            }
            else if (rndValue_tp.tarUnitIndex == 1)
            {
                moveTarSelect2_Cp.SetSelectedStatus(true);
                moveTarSelect2_Cp.SetInteract(true);
            }
            else if (rndValue_tp.tarUnitIndex == 2)
            {
                moveTarSelect3_Cp.SetSelectedStatus(true);
                moveTarSelect3_Cp.SetInteract(true);
            }
        }
    }

    //--------------------------------------------------
    public void ResetPanel()
    {
        moveOriSelect1_Cp.ResetPanel();
        moveOriSelect2_Cp.ResetPanel();
        moveTarSelect1_Cp.ResetPanel();
        moveTarSelect2_Cp.ResetPanel();
        moveTarSelect3_Cp.ResetPanel();
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
        rndValue_tp.actionType = ActionType.Move;

        if (moveOriSelect1_Cp.IsSelected()) { rndValue_tp.oriUnitIndex = 0; }
        else if (moveOriSelect2_Cp.IsSelected()) { rndValue_tp.oriUnitIndex = 1; }
        else { return; }

        if (moveTarSelect1_Cp.IsSelected()) { rndValue_tp.tarUnitIndex = 0; rndValue_tp.tokenType = TokenType.Move1; }
        else if (moveTarSelect2_Cp.IsSelected()) { rndValue_tp.tarUnitIndex = 1; rndValue_tp.tokenType = TokenType.Move2; }
        else if (moveTarSelect3_Cp.IsSelected()) { rndValue_tp.tarUnitIndex = 2; rndValue_tp.tokenType = TokenType.Move3; }
        else { return; }

        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[rndValue_tp.index].SaveRoundValue_Move(rndValue_tp);
    }

    //--------------------------------------------------
    public void Remove()
    {
        RoundValue rndValue_tp = roundsData[roundId];
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[roundId].RemoveRoundValue_Move(rndValue_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Events from external

    //--------------------------------------------------
    public void OnClickSave()
    {
        
    }

    //--------------------------------------------------

    #endregion

}
