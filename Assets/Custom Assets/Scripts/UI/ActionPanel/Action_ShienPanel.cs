using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Action_ShienPanel : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields and Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields and Properties

    //-------------------------------------------------- serialize fields
    [SerializeField] Transform shienUnitList_Tf;
    [SerializeField] GameObject shienUnit_Pf;
    [SerializeField] SelectionPanel shienTarSelect1_Cp, shienTarSelect2_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction localPlayer_Cp;
    ActionPanelHandler actionPanel_Cp;
    Data_Phases data_Cp;
    List<SelectionPanel> shienUnitSelection_Cps = new List<SelectionPanel>();
    List<Unit_Mb_UI> mUnitUI_Cps = new List<Unit_Mb_UI>();

    //-------------------------------------------------- properties
    int roundId { get { return actionPanel_Cp.GetCurRoundIndex(); } }
    RoundData roundsData { get { return localPlayer_Cp.roundsData; } }
    TokensData tokensData { get { return localPlayer_Cp.tokensData; } }
    List<UnitCardData> mUnitsData { get { return controller_Cp.dataManager_Cp
                .psMihariUnitCardsData[localPlayer_Cp.playerId].unitCards; } }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Init
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Init

    //-------------------------------------------------- init
    public void Init()
    {
        SetComponents();
        InstAndInitShienUnitsUI();
        InitMihariUnitsUI();
        InitShienTarSelectPanel();
        ResetPanel();
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        actionPanel_Cp = controller_Cp.ui_panelCanvas_Cp.actionPanel_Cp;
        data_Cp = controller_Cp.data_Cp;
    }

    //--------------------------------------------------
    void InstAndInitShienUnitsUI()
    {
        for (int i = 0; i < 7; i++)
        {
            SelectionPanel selectionPanel_Cp = Instantiate(shienUnit_Pf, shienUnitList_Tf)
                .GetComponent<SelectionPanel>();
            shienUnitSelection_Cps.Add(selectionPanel_Cp);            
        }

        Hash128 hash_tp = HashHandler.RegRandHash();
        for (int i = 0; i < shienUnitSelection_Cps.Count; i++)
        {
            shienUnitSelection_Cps[i].Init(i, hash_tp, OnClickShienUnitSelectionPanel);
        }
    }

    //--------------------------------------------------
    void InitMihariUnitsUI()
    {
        for (int i = 0; i < shienUnitSelection_Cps.Count; i++)
        {
            Unit_Mb_UI mUnitUI_Cp_tp = shienUnitSelection_Cps[i].GetComponent<Unit_Mb_UI>();
            mUnitUI_Cps.Add(mUnitUI_Cp_tp);
            mUnitUI_Cp_tp.Init(mUnitsData[i]);
        }
    }

    //--------------------------------------------------
    void InitShienTarSelectPanel()
    {
        Hash128 hash_tp = HashHandler.RegRandHash();
        shienTarSelect1_Cp.Init(hash_tp);
        shienTarSelect2_Cp.Init(hash_tp);

        if (localPlayer_Cp.playerId == 0)
        {
            shienTarSelect1_Cp.SetBgd(data_Cp.p1Van1Sprite);
            shienTarSelect2_Cp.SetBgd(data_Cp.p1Van2Sprite);
        }
        else if (localPlayer_Cp.playerId == 1)
        {
            shienTarSelect1_Cp.SetBgd(data_Cp.p2Van1Sprite);
            shienTarSelect2_Cp.SetBgd(data_Cp.p2Van2Sprite);
        }
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Refresh and Reset
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Refresh and Reset

    //-------------------------------------------------- refresh
    public void RefreshPanel()
    {
        RoundValue rndValue_tp = roundsData[roundId];

        // refresh description
        if (rndValue_tp.actionType == ActionType.Shien)
        {
            UnitCardData shienUnitData_tp = controller_Cp.dataManager_Cp.
                GetUnitCardDataFromCardIndex(rndValue_tp.shienUnitId);
            actionPanel_Cp.descText_Cp.text = shienUnitData_tp.shienAbil.dsc;
        }
        else
        {
            actionPanel_Cp.descText_Cp.text = string.Empty;
        }

        // refresh shien units
        for (int i = 0; i < mUnitsData.Count; i++)
        {
            if (roundsData.ContainsShienUnitId(mUnitsData[i].id))
            {
                if (rndValue_tp.actionType == ActionType.Shien && rndValue_tp.shienUnitId == mUnitsData[i].id)
                {
                    shienUnitSelection_Cps[i].SetSelectedStatus(true);
                    shienUnitSelection_Cps[i].gameObject.SetActive(true);
                }
                else
                {
                    shienUnitSelection_Cps[i].SetSelectedStatus(false);
                    shienUnitSelection_Cps[i].gameObject.SetActive(false);
                }
            }
            else
            {
                shienUnitSelection_Cps[i].SetSelectedStatus(false);
                shienUnitSelection_Cps[i].gameObject.SetActive(true);
            }
        }

        // refresh tar units
        if (rndValue_tp.actionType == ActionType.Shien)
        {
            if (rndValue_tp.tarUnitIndex == 0)
            {
                shienTarSelect1_Cp.SetSelectedStatus(true);
            }
            else if (rndValue_tp.tarUnitIndex == 1)
            {
                shienTarSelect2_Cp.SetSelectedStatus(true);
            }

            shienTarSelect1_Cp.SetInteract(true);
            shienTarSelect2_Cp.SetInteract(true);
        }
        else if (tokensData.restShien == 0)
        {
            shienTarSelect1_Cp.SetInteract(false);
            shienTarSelect2_Cp.SetInteract(false);
        }
        else
        {
            shienTarSelect1_Cp.SetInteract(true);
            shienTarSelect2_Cp.SetInteract(true);
        }
    }

    //-------------------------------------------------- reset
    public void ResetPanel()
    {
        // reset shien unit panels
        for (int i = 0; i < shienUnitSelection_Cps.Count; i++)
        {
            shienUnitSelection_Cps[i].ResetPanel();
        }

        // reset shien tar select panels
        shienTarSelect1_Cp.ResetPanel();
        shienTarSelect2_Cp.ResetPanel();
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Internal methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Internal methods

    //--------------------------------------------------
    int GetSelectedShienUnitIndex()
    {
        int result = -1;
        for (int i = 0; i < shienUnitSelection_Cps.Count; i++)
        {
            if (shienUnitSelection_Cps[i].IsSelected())
            {
                result = i;
            }
        }
        return result;
    }

    //-------------------------------------------------- callback
    void OnClickShienUnitSelectionPanel(int id_tp, bool selected)
    {

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
        rndValue_tp.actionType = ActionType.Shien;
        rndValue_tp.tokenType = TokenType.Shien;

        int shienUnitIndex_tp = GetSelectedShienUnitIndex();
        if (shienUnitIndex_tp == -1)
        {
            return;
        }
        rndValue_tp.shienUnitId = localPlayer_Cp.mUnitsData[shienUnitIndex_tp].id;
        rndValue_tp.oriUnitIndex = shienUnitIndex_tp;

        int tarUnitIndex_tp = -1;
        if (shienTarSelect1_Cp.IsSelected())
        {
            tarUnitIndex_tp = 0;
        }
        else if (shienTarSelect2_Cp.IsSelected())
        {
            tarUnitIndex_tp = 1;
        }
        else
        {
            return;
        }
        rndValue_tp.tarUnitIndex = tarUnitIndex_tp;

        //
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[rndValue_tp.index].SaveRoundValue_Shien(rndValue_tp);
    }

    //--------------------------------------------------
    public void Remove()
    {
        RoundValue rndValue_tp = roundsData[roundId];
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[rndValue_tp.index].RemoveRoundValue_Shien(rndValue_tp);
    }

    #endregion

}
