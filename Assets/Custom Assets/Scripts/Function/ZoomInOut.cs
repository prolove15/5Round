using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ZoomInOut : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //------------------------------
    public static void ZoomOut(Transform target_Tf_pr, Camera mainCam_Cp_pr, float zoomDist,
        Vector3 scale, Quaternion adjustRot, UnityEvent unityEvent_pr, float duration = 1f)
    {
        Vector3 camNearPos_tp = mainCam_Cp_pr.transform.position
            + mainCam_Cp_pr.transform.forward * zoomDist;
        Quaternion camNearRot_tp = Quaternion.Euler(mainCam_Cp_pr.transform.rotation.eulerAngles * -1f);
        camNearRot_tp *= adjustRot;

        //
        TargetTweening.TranslateGameObject(target_Tf_pr, target_Tf_pr.position, camNearPos_tp,
            target_Tf_pr.rotation, camNearRot_tp, unityEvent_pr, duration);

        TargetTweening.DoScaleTargetObject(target_Tf_pr, scale, null, duration);
    }

    //------------------------------
    public static void ZoomIn(Transform target_Tf_pr, Vector3 lastPos,
        Vector3 scale, Quaternion lastRot, UnityEvent unityEvent_pr, float duration = 1f)
    {
        TargetTweening.TranslateGameObject(target_Tf_pr, target_Tf_pr.position, lastPos,
            target_Tf_pr.rotation, lastRot, unityEvent_pr, duration);

        TargetTweening.DoScaleTargetObject(target_Tf_pr, scale, null, duration);
    }

}
