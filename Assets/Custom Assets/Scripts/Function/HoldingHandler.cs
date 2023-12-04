using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;

public class HoldingHandler : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Fields
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region Fields

    //-------------------------------------------------- serialize fields
    [SerializeField]
    float holdReadyTime = 0f;

    [SerializeField]
    float holdExtraOffsetCoef = 0.03f;

    [SerializeField]
    float doMoveDuration = 0.2f;

    [SerializeField]
    float doRotateDuration = 0.2f;

    //-------------------------------------------------- public fields
    // it will be input from outside script
    public UnityEvent onHolded;

    public UnityEvent onHoldEnded;

    public bool enableHoldDetect;

    // it will be used at the outside of script
    [ReadOnly]
    public Transform holdedCard_Tf;

    [ReadOnly]
    public Transform castedObject_Tf;

    //-------------------------------------------------- private fields
    Transform holdedObject_Tf;

    bool isHolding = false;

    Vector3 holdOffset;

    Transform pointedObject_Tf;

    bool isPointing = false;

    float pointStartTime;

    #endregion

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////

    //-------------------------------------------------- Start is called before the first frame update
    void Start()
    {

    }

    //-------------------------------------------------- Update is called once per frame
    void Update()
    {
        if (enableHoldDetect)
        {
            SetHoldedObject();

            if (holdedCard_Tf)
            {
                DragObject();
            }
        }
    }

    //--------------------------------------------------
    Transform GetParentCard(Transform child_Tf_pr)
    {
        Transform holdedCard_Tf_tp = null;

        Transform curChild_Tf_tp = child_Tf_pr;

        while(curChild_Tf_tp != null)
        {
            if (DataManager_Gameplay.IsCard(curChild_Tf_tp.gameObject))
            {
                holdedCard_Tf_tp = curChild_Tf_tp;
                break;
            }

            curChild_Tf_tp = curChild_Tf_tp.parent;
        }

        return holdedCard_Tf_tp;
    }

    void SetHoldedObject()
    {
        if (isPressStarted)
        {
            Vector3 inputPosition;

            if (Input.GetMouseButtonDown(0))
            {
                inputPosition = Input.mousePosition;
            }
            else
            {
                inputPosition = Input.GetTouch(0).position;
            }

            RaycastHit hit;
            Ray ray = Camera.main.ScreenPointToRay(inputPosition);

            if (Physics.Raycast(ray, out hit))
            {
                pointedObject_Tf = hit.transform;
                isPointing = true;
                pointStartTime = Time.time;
            }
        }

        if (isPointing && !isHolding && isPressing)
        {
            if (Time.time - pointStartTime >= holdReadyTime)
            {
                if (pointedObject_Tf != LongPressDetector.GetPointedObject())
                {
                    pointedObject_Tf = null;
                    isPointing = false;
                }
                else if (onHolded != null)
                {
                    Transform holdedCard_Tf_tp = GetParentCard(pointedObject_Tf);
                    if (holdedCard_Tf_tp)
                    {
                        holdedObject_Tf = pointedObject_Tf;
                        holdedCard_Tf = holdedCard_Tf_tp;
                        isHolding = true;

                        StartDragObject(holdedCard_Tf_tp);

                        onHolded.Invoke();
                    }
                }
            }
        }

        if (isPressEnded)
        {
            if (isPointing)
            {
                isPointing = false;
                pointedObject_Tf = null;
            }
            if (isHolding)
            {
                OnDragEnded();
                onHoldEnded.Invoke();

                isHolding = false;
                holdedObject_Tf = null;
                holdedCard_Tf = null;
            }
        }
    }

    public void StartDragObject(Transform holdedCard_Tf_tp)
    {
        holdedCard_Tf = holdedCard_Tf_tp;
        holdOffset = GetCastedPosition() - holdedCard_Tf.position;
    }

    void DragObject()
    {
        Vector3 pointedPosition_tp = GetCastedPosition();
        if (pointedPosition_tp == Vector3.zero)
        {
            return;
        }

        Vector3 newPosition = pointedPosition_tp - holdOffset + castedObject_Tf.up * holdExtraOffsetCoef;
        holdedCard_Tf.DOMove(newPosition, doMoveDuration);
        holdedCard_Tf.DORotate(castedObject_Tf.rotation.eulerAngles, doRotateDuration);
    }

    void OnDragEnded()
    {
        
    }

    Vector3 GetCastedPosition()
    {
        Vector3 result = Vector3.zero;

        Ray ray = new Ray();
        RaycastHit hit;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
            }
        }
        else
        {
            ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        }

        // Create a layer mask to ignore the holdedObject's layer
        int layerMask = ~(1 << holdedObject_Tf.gameObject.layer);

        if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
        {
            result = hit.point;

            //
            castedObject_Tf = hit.transform;
        }

        return result;
    }

    //////////////////////////////////////////////////////////////////////
    /// <summary>
    /// Static methods
    /// </summary>
    //////////////////////////////////////////////////////////////////////
    #region StaticMethods

    public static bool isPressStarted
    {
        get
        {
            return Input.GetMouseButtonDown(0) || (Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Began);
        }
    }

    public static bool isPressing
    {
        get
        {
            return Input.GetMouseButton(0) || (Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Moved);
        }
    }

    public static bool isPressEnded
    {
        get
        {
            return Input.GetMouseButtonUp(0) || (Input.touchCount > 0
                && Input.GetTouch(0).phase == TouchPhase.Ended);
        }
    }

    public static Transform GetCastedObjectByIgnoreLayers(params int[] ignoreLayers)
    {
        Transform castedObject_Tf_tp = null;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.GetTouch(0).position);
                RaycastHit hit;

                int layerMask = 0;
                foreach (int layer in ignoreLayers)
                {
                    layerMask |= 1 << layer;
                }
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    castedObject_Tf_tp = hit.transform;
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                int layerMask = 0;
                foreach (int layer in ignoreLayers)
                {
                    layerMask |= 1 << layer;
                }
                layerMask = ~layerMask;

                if (Physics.Raycast(ray, out hit, Mathf.Infinity, layerMask))
                {
                    castedObject_Tf_tp = hit.transform;
                }
            }
        }

        return castedObject_Tf_tp;
    }

    public static int GetNearestValidPointIndex(Vector3[] points_pr, Vector3 point_pr, float validDist_pr)
    {
        int validIndex = -1;

        for (int i = 0; i < points_pr.Length; i++)
        {
            if (Vector3.Distance(points_pr[i], point_pr) <= validDist_pr)
            {
                if (validIndex == -1)
                {
                    validIndex = i;
                }
                
                if (Vector3.Distance(points_pr[i], point_pr) < Vector3.Distance(points_pr[validIndex], point_pr))
                {
                    validIndex = i;
                }
            }
        }

        return validIndex;
    }

    #endregion

}
