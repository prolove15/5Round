using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Action_GuardPanel : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields and Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields and Properties

    //-------------------------------------------------- serialize fields
    [SerializeField] TextMeshProUGUI spMarkerText_Cp;
    [SerializeField] Button incBtn_Cp, decBtn_Cp;

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    PlayerFaction localPlayer_Cp;
    ActionPanelHandler actionPanel_Cp;

    int assignedSp;

    //-------------------------------------------------- properties
    int roundId { get { return actionPanel_Cp.GetCurRoundIndex(); } }
    RoundData roundsData { get { return localPlayer_Cp.roundsData; } }
    TextMeshProUGUI descText_Cp { get { return actionPanel_Cp.descText_Cp; } }

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
        ResetPanel();
    }

    //--------------------------------------------------
    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        localPlayer_Cp = controller_Cp.localPlayer_Cp;
        actionPanel_Cp = controller_Cp.ui_panelCanvas_Cp.actionPanel_Cp;
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
        //
        descText_Cp.text = "ラウンド中前衛 DEF +" + (roundsData[roundId].spCount + assignedSp).ToString();

        //
        spMarkerText_Cp.text = (assignedSp + localPlayer_Cp.markersData.useSp).ToString() + "/"
            + localPlayer_Cp.markersData.sp.ToString() + "使用";

        //
        if (assignedSp + localPlayer_Cp.markersData.useSp < localPlayer_Cp.markersData.sp)
        {
            incBtn_Cp.interactable = true;
        }
        else
        {
            incBtn_Cp.interactable = false;
        }

        if ((roundsData[roundId].spCount + assignedSp) > 0)
        {
            decBtn_Cp.interactable = true;
        }
        else
        {
            decBtn_Cp.interactable = false;
        }
    }

    //--------------------------------------------------
    public void ResetPanel()
    {
        assignedSp = 0;
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
        rndValue_tp.spCount = (roundsData[roundId].spCount + assignedSp);

        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[roundId].SaveRoundValue_Guard(rndValue_tp);
    }

    //--------------------------------------------------
    public void Remove()
    {
        RoundValue rndValue_tp = roundsData[roundId];
        localPlayer_Cp.pBoard_Cp.pbRnd_Cps[roundId].RemoveRoundValue_Guard(rndValue_tp);
    }

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Events from external
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Events from external

    //--------------------------------------------------
    public void OnClickIncBtn()
    {
        assignedSp++;
        RefreshPanel();
    }

    //--------------------------------------------------
    public void OnClickDecBtn()
    {
        assignedSp--;
        RefreshPanel();
    }

    #endregion

}
