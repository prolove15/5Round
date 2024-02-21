using System.Collections;
using System.Collections.Generic;
using TcgEngine.UI;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class NoticePanel : MonoBehaviour
{

    [SerializeField] float showDur;
    [SerializeField] CanvasUI uiPanel_Cp;
    [SerializeField] Text titleText_Cp;
    [SerializeField] Text descText_Cp;

    UnityAction onHide;

    public void Init()
    {
        uiPanel_Cp.onHide = OnHide;
        uiPanel_Cp.Hide();
    }

    public void SetContent(string title_tp, string desc_tp, float showDur_tp = 0f, UnityAction onHide_tp = null)
    {
        onHide = onHide_tp;
        titleText_Cp.text = title_tp;
        descText_Cp.text = desc_tp;
        titleText_Cp.gameObject.SetActive(true);
        descText_Cp.gameObject.SetActive(true);
        if (showDur_tp == 0f)
        {
            showDur_tp = showDur;
        }
        ShowAndHidePanel();
    }

    public void SetTitle(string title_tp, float showDur_tp = 0f, UnityAction onHide_tp = null)
    {
        onHide = onHide_tp;
        titleText_Cp.text = title_tp;
        titleText_Cp.gameObject.SetActive(true);
        descText_Cp.gameObject.SetActive(false);
        if (showDur_tp == 0f)
        {
            showDur_tp = showDur;
        }
        ShowAndHidePanel();
    }

    public void SetDesc(string desc_tp, float showDur_tp = 0f, UnityAction onHide_tp = null)
    {
        onHide = onHide_tp;
        descText_Cp.text = desc_tp;
        titleText_Cp.gameObject.SetActive(false);
        descText_Cp.gameObject.SetActive(true);
        if (showDur_tp == 0f)
        {
            showDur_tp = showDur;
        }
        ShowAndHidePanel();
    }

    void ShowAndHidePanel(float showDur_tp = 0f)
    {
        if (showDur_tp == 0f)
        {
            showDur_tp = showDur;
        }
        uiPanel_Cp.Show();
        CancelInvoke("HidePanel");
        Invoke("HidePanel", showDur_tp);
    }

    void HidePanel()
    {
        uiPanel_Cp.Hide(false, onHide);
    }

    void OnHide()
    {
        onHide?.Invoke();
    }

}
