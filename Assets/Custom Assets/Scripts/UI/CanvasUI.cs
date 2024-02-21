using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Base class for UI panels that can be hidden or shown, with a fade-in fade-out effect
/// </summary>

[RequireComponent(typeof(CanvasGroup))]
public class CanvasUI : MonoBehaviour
{
    public float display_speed = 4f;

    public UnityAction onShow;
    public UnityAction onHide;

    private CanvasGroup canvas_group;
    private bool visible;

    void Awake()
    {
        canvas_group = GetComponent<CanvasGroup>();
    }

    public void Toggle(bool instant = false)
    {
        if (IsVisible())
            Hide(instant);
        else
            Show(instant);
    }

    public void Show(bool instant = false, UnityAction onShow_tp = null)
    {
        visible = true;
        if (onShow_tp != null)
        {
            onShow = onShow_tp;
        }
        gameObject.SetActive(true);

        if (instant)
        {
            canvas_group.alpha = 1f;
            onShow?.Invoke();
        }
        else
        {
            DOTween.To(() => canvas_group.alpha, x => canvas_group.alpha = x, 1f, 1f / display_speed)
                .OnComplete(() => { onShow?.Invoke(); });
        }
    }

    public void Hide(bool instant = false, UnityAction onHide_tp = null)
    {
        visible = false;
        if (onHide_tp != null)
        {
            onHide = onHide_tp;
        }
        gameObject.SetActive(true);

        if (instant)
        {
            canvas_group.alpha = 0f;
            onHide?.Invoke();
        }
        else
        {
            DOTween.To(() => canvas_group.alpha, x => canvas_group.alpha = x, 0f, 1f / display_speed)
                .OnComplete(() => { AfterHide(); onHide?.Invoke(); });
        }
    }

    public void SetVisible(bool visi)
    {
        if (!visible && visi)
            Show();
        else if (visible && !visi)
            Hide();
    }

    public void AfterHide()
    {
        gameObject.SetActive(false);
    }

    public bool IsVisible()
    {
        return visible;
    }

    public bool IsFullyVisible()
    {
        return visible && canvas_group.alpha > 0.99f;
    }

    public float GetAlpha()
    {
        return canvas_group.alpha;
    }
}