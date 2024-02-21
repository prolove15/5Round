using System.Collections;
using System.Collections.Generic;
using TcgEngine.UI;
using UnityEngine;
using UnityEngine.Events;

public class UI_MainScene : MonoBehaviour
{

    [SerializeField] CanvasUI curtainUI_Cp;

    private void Start()
    {
        curtainUI_Cp.gameObject.SetActive(true);
    }

    public void Init(UnityAction onShow_tp = null, UnityAction onHide_tp = null)
    {
        curtainUI_Cp.onShow = onShow_tp;
        curtainUI_Cp.onHide = onHide_tp;
    }

    public void ShowCurtain()
    {
        curtainUI_Cp.Show();
    }

    public void HideCurtain()
    {
        curtainUI_Cp.Hide();
    }

}
