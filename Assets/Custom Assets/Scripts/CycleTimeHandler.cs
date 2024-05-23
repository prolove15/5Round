using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class CycleTimeHandler : MonoBehaviour
{
    [SerializeField] Text timeText_Cp;
    [SerializeField] Animator timeAnim_Cp;
    [SerializeField] int minWarningTime = 3;
    [SerializeField] int warningTimePercent = 5;

    UnityAction action;
    int restTime;
    Coroutine cycleCorou;

    //-------------------------------------------------- 
    public void Init()
    {
        timeText_Cp.text = 0.ToString();
        SetActiveTimePanel(false);
    }

    //-------------------------------------------------- 
    public void SetUnityAction(UnityAction action_tp)
    {
        action = action_tp;
    }

    //-------------------------------------------------- 
    public void StartCycleTime(int cycleTime_tp, int warningTime_tp = -1, UnityAction action_tp = null)
    {
        // init
        restTime = cycleTime_tp;
        if (warningTime_tp == -1)
        {
            warningTime_tp = Mathf.Clamp(cycleTime_tp / warningTimePercent, minWarningTime, cycleTime_tp);
        }
        action = action_tp;
        SetActiveTimePanel(true);

        //
        cycleCorou = StartCoroutine(Corou_StartCycleTime(cycleTime_tp, warningTime_tp, action_tp));
    }
         
    IEnumerator Corou_StartCycleTime(int cycleTime_tp, int warningTime_tp = -1, UnityAction action_tp = null)
    {
        // start time count
        while (restTime >= 0)
        {
            timeText_Cp.text = restTime.ToString();
            yield return new WaitForSeconds(1f);
            restTime -= 1;
            if (restTime <= warningTime_tp)
            {
                timeAnim_Cp.SetTrigger("pulse");
            }
        }

        // after time count
        StopCycleTime();
        action_tp?.Invoke();
    }

    //-------------------------------------------------- 
    public void StopCycleTime()
    {
        if (cycleCorou != null)
        {
            StopCoroutine(cycleCorou);
        }
        restTime = 0;
        timeAnim_Cp.SetTrigger("stop");
        SetActiveTimePanel(false);
    }

    //-------------------------------------------------- 
    public void SetActiveTimePanel(bool flag)
    {
        gameObject.SetActive(flag);
    }

}
