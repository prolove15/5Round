using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class LongPressDetector : MonoBehaviour
{

    public Transform targetObject_Tf; // The GameObject you want to detect long presses on

    public bool enableLongPress = false;

    public bool enableClickDetect = false;

    public float requiredPressDuration = 0.5f; // Adjust as needed

    public UnityEvent onLongPress;

    public UnityEvent onClicked;

    private bool m_isPressing = false;

    bool isPressing
    {
        get { return m_isPressing; }
        set
        {
            m_isPressing = value;
            pressStartTime = 0f;
        }
    }

    private float pressStartTime;

    void Update()
    {
        if (enableLongPress && onLongPress != null)
        {
            DetectLongPress();
        }

        if (enableClickDetect && onClicked != null)
        {
            Transform clickedObject_Tf_tp = GetClickedObject();
            if (CheckObjectLineage(clickedObject_Tf_tp, targetObject_Tf))
            {
                onClicked.Invoke();
            }
        }
    }

    //------------------------------
    void DetectLongPress()
    {
        if (Application.isMobilePlatform)
        {
            DetectLongPressOnMobile();
        }
        else
        {
            DetectLongPressOnComputer();
        }
    }

    void DetectLongPressOnMobile()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                if (IsPointerOverTargetObjectOnMobile())
                {
                    isPressing = true;
                    pressStartTime = Time.time;

                    return;
                }
            }
            else if (touch.phase == TouchPhase.Ended || touch.phase == TouchPhase.Canceled)
            {
                isPressing = false;
            }

            if (isPressing)
            {
                Transform pointedObject_Tf_tp = LongPressDetector.GetPointedObjectOnMobile();
                if (!pointedObject_Tf_tp)
                {
                    isPressing = false;
                    return;
                }
                else if (!LongPressDetector.CheckObjectLineage(pointedObject_Tf_tp, targetObject_Tf))
                {
                    isPressing = false;
                    return;
                }

                if (Time.time - pressStartTime >= requiredPressDuration)
                {
                    isPressing = false;
                    if (IsPointerOverTargetObjectOnMobile())
                    {
                        onLongPress.Invoke();
                    }
                }
            }
        }
    }

    void DetectLongPressOnComputer()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (IsPointerOverTargetObjectOnComputer())
            {
                isPressing = true;
                pressStartTime = Time.time;

                return;
            }
        }
        else if (Input.GetMouseButtonUp(0))
        {
            isPressing = false;
        }

        if (isPressing)
        {
            Transform pointedTarget_Tf_tp = LongPressDetector.GetPointedObjectOnComputer();
            if(!pointedTarget_Tf_tp)
            {
                isPressing = false;
                return;
            }
            else if (!LongPressDetector.CheckObjectLineage(pointedTarget_Tf_tp, targetObject_Tf))
            {
                isPressing = false;
                return;
            }

            if (Time.time - pressStartTime >= requiredPressDuration)
            {
                isPressing = false;
                if (IsPointerOverTargetObjectOnComputer())
                {
                    onLongPress.Invoke();
                }
            }
        }
    }

    //------------------------------
    bool IsPointerOverTargetObject()
    {
        bool result = false;

        if (Application.isMobilePlatform)
        {
            result = IsPointerOverTargetObjectOnMobile();
        }
        else
        {
            result = IsPointerOverTargetObjectOnComputer();
        }

        return result;
    }

    bool IsPointerOverTargetObjectOnMobile()
    {
        bool result = false;

        if (targetObject_Tf == null)
        {
            Debug.LogWarning("Target object is not assigned.");

            result = false;

            return result;
        }

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit))
            {
                Transform hitted_Tf = hit.collider.transform;

                do
                {
                    if (hitted_Tf == targetObject_Tf)
                    {
                        result = true;

                        break;
                    }
                    else
                    {
                        hitted_Tf = hitted_Tf.parent;
                    }
                } while (hitted_Tf);
            }
        }

        return result;
    }

    bool IsPointerOverTargetObjectOnComputer()
    {
        bool result = false;

        if (targetObject_Tf == null)
        {
            Debug.LogWarning("Target object is not assigned.");

            result = false;

            return result;
        }

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit))
        {
            Transform hitted_Tf = hit.collider.transform;

            do
            {
                if (hitted_Tf == targetObject_Tf)
                {
                    result = true;

                    break;
                }
                else
                {
                    hitted_Tf = hitted_Tf.parent;
                }
            } while (hitted_Tf);
        }

        return result;
    }

    //------------------------------
    public static Transform GetPointedObject()
    {
        Transform pointedObject_tp = null;

        if (Application.isMobilePlatform)
        {
            pointedObject_tp = GetPointedObjectOnMobile();
        }
        else
        {
            pointedObject_tp = GetPointedObjectOnComputer();
        }

        return pointedObject_tp;
    }

    static Transform GetPointedObjectOnMobile()
    {
        Transform pointedObject_tp = null;

        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            Ray ray = Camera.main.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                pointedObject_tp = hit.transform;
            }
        }

        return pointedObject_tp;
    }

    static Transform GetPointedObjectOnComputer()
    {
        Transform pointedObject_tp = null;

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            pointedObject_tp = hit.transform;
        }

        return pointedObject_tp;
    }

    //------------------------------
    public static Transform GetClickedObject()
    {
        Transform result = null;

        if (Application.isMobilePlatform)
        {
            if (Input.touchCount > 0)
            {
                if (Input.GetTouch(0).phase == TouchPhase.Began)
                {
                    result = GetPointedObject();
                }
            }
        }
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                result = GetPointedObject();
            }
        }

        return result;
    }

    //------------------------------
    public static bool CheckObjectLineage(Transform child_Tf_pr, Transform ancestor_Tf_pr)
    {
        bool result = false;
        Transform child_Tf_tp = child_Tf_pr;

        while (child_Tf_tp)
        {
            if (child_Tf_tp == ancestor_Tf_pr)
            {
                result = true;
                break;
            }
            else
            {
                child_Tf_tp = child_Tf_tp.parent;
            }
        };

        return result;
    }

}
