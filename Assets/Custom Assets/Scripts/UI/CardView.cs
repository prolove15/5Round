using System.Collections;
using System.Collections.Generic;
using TcgEngine.UI;
using UnityEngine;
using UnityEngine.UI;

public class CardView : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField] Image cardImage_Cp;
    [SerializeField] Transform statusGroup_Tf;
    [SerializeField] Transform itemCont_Tf;

    //-------------------------------------------------- public fields

    //-------------------------------------------------- private fields
    Controller_Phases controller_Cp;
    UIPanel uiPanel_Cp;

    GameObject statusText_Pf;
    Text nameText_Cp, costText_Cp, atrText_Cp, hpText_Cp, atkText_Cp, agiText_Cp,
        defCorrText_Cp, accuracyCorrText_Cp, CTCorrText_Cp, nlAtkCorrText_Cp, spcAtkCorrText_Cp,
        dmgCorrText_Cp, indirDmgCorrText_Cp, shienAbilCorrText_Cp, diceAmountCorrText_Cp;
    List<GameObject> statusText_GOs = new List<GameObject>();
    GameObject item_Pf;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Properties
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Properties

    //-------------------------------------------------- public properties

    //-------------------------------------------------- private properties

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
        SetComponents();
        InitStatusPanel();
        InitItemsPanel();
    }

    void SetComponents()
    {
        controller_Cp = GameObject.FindWithTag("GameController").GetComponent<Controller_Phases>();
        uiPanel_Cp = gameObject.GetComponent<UIPanel>();
    }

    //--------------------------------------------------
    void InitStatusPanel()
    {
        // destroy unnecessary objects
        statusText_Pf = statusGroup_Tf.GetChild(0).gameObject;
        for (int i = statusGroup_Tf.childCount - 1; i >= 1; i--)
        {
            Destroy(statusGroup_Tf.GetChild(i).gameObject);
        }

        // add objects
        statusText_GOs.Add(statusText_Pf);
        for (int i = 1; i < 15; i++)
        {
            statusText_GOs.Add(Instantiate(statusText_Pf, statusGroup_Tf));
        }

        // attach components
        nameText_Cp = statusText_GOs[0].GetComponent<Text>();
        costText_Cp = statusText_GOs[1].GetComponent<Text>();
        atrText_Cp = statusText_GOs[2].GetComponent<Text>();
        hpText_Cp = statusText_GOs[3].GetComponent<Text>();
        atkText_Cp = statusText_GOs[4].GetComponent<Text>();
        agiText_Cp = statusText_GOs[5].GetComponent<Text>();
        defCorrText_Cp = statusText_GOs[6].GetComponent<Text>();
        accuracyCorrText_Cp = statusText_GOs[7].GetComponent<Text>();
        CTCorrText_Cp = statusText_GOs[8].GetComponent<Text>();
        nlAtkCorrText_Cp = statusText_GOs[9].GetComponent<Text>();
        spcAtkCorrText_Cp = statusText_GOs[10].GetComponent<Text>();
        dmgCorrText_Cp = statusText_GOs[11].GetComponent<Text>();
        indirDmgCorrText_Cp = statusText_GOs[12].GetComponent<Text>();
        shienAbilCorrText_Cp = statusText_GOs[13].GetComponent<Text>();
        diceAmountCorrText_Cp = statusText_GOs[14].GetComponent<Text>();
    }

    //--------------------------------------------------
    void InitItemsPanel()
    {
        // destroy unnecessary objects
        item_Pf = itemCont_Tf.GetChild(0).gameObject;
        for (int i = itemCont_Tf.childCount - 1; i >= 1; i--)
        {
            Destroy(itemCont_Tf.GetChild(i).gameObject);
        }

        // disable item_Pf
        //item_Pf.SetActive(false);
    }

    #endregion

    //--------------------------------------------------
    public void ShowBbUnitUI(Unit_Bb bbUnit_Cp_tp)
    {
        // set data
        UnitCardData unitData_tp = bbUnit_Cp_tp.unitData;
        UnitInfo unitInfo_tp = bbUnit_Cp_tp.unitInfo;

        if (bbUnit_Cp_tp.playerId != controller_Cp.localPlayerId && !unitInfo_tp.visible)
        {
            cardImage_Cp.sprite = unitData_tp.backSide;
            nameText_Cp.text = "名前: " + "未知";
            costText_Cp.text = "コスト: " + "未知";
            atrText_Cp.text = "属性: " + "未知";
            hpText_Cp.text = "HP: " + "未知";
            atkText_Cp.text = "ATK: " + "未知";
            agiText_Cp.text = "AGI: " + "未知";
            defCorrText_Cp.text = "DEF補正: " + "未知";
            accuracyCorrText_Cp.text = "命中補正: " + "未知";
            CTCorrText_Cp.text = "CT値補正: " + "未知";
            nlAtkCorrText_Cp.text = "通常攻撃補正: " + "未知";
            spcAtkCorrText_Cp.text = "特殊攻撃補正: " + "未知";
            dmgCorrText_Cp.text = "ダメ補正: " + "未知";
            indirDmgCorrText_Cp.text = "間接ダメ補正: " + "未知";
            shienAbilCorrText_Cp.text = "しえん効果補正: " + "未知";
            diceAmountCorrText_Cp.text = "ダイス 効果補正: " + "未知";
        }
        else
        {
            cardImage_Cp.sprite = unitData_tp.frontSide;
            nameText_Cp.text = "名前: " + unitData_tp.name;
            costText_Cp.text = "コスト: " + unitData_tp.cost.ToString();
            atrText_Cp.text = "属性: ";
            for (int i = 0; i < unitData_tp.attrib.Count; i++)
            {
                atrText_Cp.text += unitData_tp.attrib[i].ToString();
                if (i < unitData_tp.attrib.Count - 1)
                {
                    atrText_Cp.text += " + ";
                }
            }
            hpText_Cp.text = "HP: " + unitInfo_tp.curHp.ToString() + " / " + unitInfo_tp.maxHP.ToString();
            atkText_Cp.text = "ATK: " + unitData_tp.atk.ToString();
            agiText_Cp.text = "AGI: " + unitData_tp.agi.ToString();
            defCorrText_Cp.text = "DEF補正: " + unitInfo_tp.defCorr.ToString();
            accuracyCorrText_Cp.text = "命中補正: 0";
            CTCorrText_Cp.text = "CT値補正: " + unitInfo_tp.ctCorr.ToString();
            nlAtkCorrText_Cp.text = "通常攻撃補正: " + unitInfo_tp.nlAtkCorr.ToString();
            spcAtkCorrText_Cp.text = "特殊攻撃補正: " + unitInfo_tp.spcAtkCorr.ToString();
            dmgCorrText_Cp.text = "ダメ補正: " + unitInfo_tp.dmgCorr.ToString();
            indirDmgCorrText_Cp.text = "間接ダメ補正: " + unitInfo_tp.indirDmgCorr.ToString();
            shienAbilCorrText_Cp.text = "しえん効果補正: " + unitInfo_tp.shienAbilCorr.ToString();
            diceAmountCorrText_Cp.text = "ダイス 効果補正: " + unitInfo_tp.diceAmountCorr.ToString();
        }

        // show panel
        uiPanel_Cp.Show();

        //
        CancelInvoke("HideBbUnit");
        Invoke("HideBbUnit", 3f);
    }

    void HideBbUnit()
    {
        uiPanel_Cp.Hide();
    }

}
