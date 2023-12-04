using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.Events;

public class TargetTweening : MonoBehaviour
{

    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////
    // Methods
    //////////////////////////////////////////////////////////////////////
    //////////////////////////////////////////////////////////////////////

    //------------------------------ Start is called before the first frame update
    void Start()
    {

    }

    //------------------------------ Update is called once per frame
    void Update()
    {

    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Vector3 startPos, Vector3 lastPos,
        Quaternion startRot, Quaternion lastRot, UnityEvent onCompleted, float duration = 0.7f)
    {
        // Create a sequence of tweens to move and rotate the target_Tf
        Sequence sequence = DOTween.Sequence();

        // Set the initial position and rotation
        target_Tf.position = startPos;
        target_Tf.rotation = startRot;

        // Add a move tween from startPos to lastPos
        sequence.Append(target_Tf.DOMove(lastPos, duration)); // You can adjust the duration

        // Add a rotate tween from startRot to lastRot
        sequence.Join(target_Tf.DORotateQuaternion(lastRot, duration)); // You can adjust the duration

        // Add a callback when the tween is completed
        sequence.OnComplete(() =>
        {
            // Call the UnityEvent when the tween is completed
            onCompleted.Invoke();
        });

        // Start the tween sequence
        sequence.Play();
    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Vector3 lastPos, Quaternion lastRot,
        UnityEvent onCompleted, float duration = 0.7f)
    {
        TranslateGameObject(target_Tf, target_Tf.position, lastPos, target_Tf.rotation, lastRot, onCompleted, duration);
    }

    //------------------------------
    public static void TranslateObject(Transform target_Tf, Vector3 lastPos,
        UnityEvent onCompleted, float duration = 0.7f)
    {
        TranslateGameObject(target_Tf, target_Tf.position, lastPos, target_Tf.rotation,
            Quaternion.identity, onCompleted, duration);
    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Vector3 lastPos, float duration = 0.7f)
    {
        UnityEvent unityEvent = new UnityEvent();
        TranslateObject(target_Tf, lastPos, unityEvent);
    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Transform last_Tf,
        UnityEvent onCompleted, float duration = 0.7f)
    {
        Vector3 lastPos = last_Tf.position;
        Quaternion lastRot = last_Tf.rotation;

        // Create a sequence of tweens to move and rotate the target_Tf
        Sequence sequence = DOTween.Sequence();

        // Add a move tween from startPos to lastPos
        sequence.Append(target_Tf.DOMove(lastPos, duration)); // You can adjust the duration

        // Add a rotate tween from startRot to lastRot
        sequence.Join(target_Tf.DORotateQuaternion(lastRot, duration)); // You can adjust the duration

        // Add a callback when the tween is completed
        sequence.OnComplete(() =>
        {
            // Call the UnityEvent when the tween is completed
            onCompleted.Invoke();
        });

        // Start the tween sequence
        sequence.Play();
    }

    //------------------------------
    public static void TranslateGameObject(Transform target_Tf, Transform last_Tf, float duration = 0.7f)
    {
        UnityEvent unityEvent = new UnityEvent();
        TranslateGameObject(target_Tf, last_Tf, unityEvent);
    }

    //------------------------------
    public static void DoScaleTargetObject(Transform target_Tf, Vector3 lastScale, UnityEvent unityEvent,
        float duration = 1f)
    {
        // Tweening target scale
        target_Tf.DOScale(lastScale, duration)
            .OnComplete(() => unityEvent.Invoke());
    }

    //------------------------------
    public static void JumpObject(Transform target_Tf, Vector3 originPos, Vector3 lastPos,
        UnityEvent unityEvent, float dur = 1f)
    {
        // Calculate the jump power and duration
        float jumpPower = Vector3.Distance(originPos, lastPos) / 2f;

        // Use DOJump to move the object along a parabolic path
        target_Tf.DOJump(lastPos, jumpPower, 1, dur)
            .OnComplete(() => unityEvent.Invoke());
    }


    //--------------------------------------------------
    public static void DoFlipOverCard(Transform card_Tf_pr, UnityEvent unityEvent, float dur = 0.7f)
    {
        // Set the initial rotation
        Quaternion initialRotation = card_Tf_pr.rotation;

        // Rotate the card 180 degrees around the right axis (up direction)
        Quaternion targetRotation = initialRotation * Quaternion.Euler(0f, 0f, 180f);

        // Move the card up (transform.up) while rotating
        Vector3 oriPos = card_Tf_pr.position;
        Vector3 tarPos = card_Tf_pr.position + Vector3.up * 0.2f;

        // Use DOTween to smoothly animate both rotation and translation
        Sequence seqPos = DOTween.Sequence();
        seqPos.Append(card_Tf_pr.DOMove(tarPos, dur));
        seqPos.Append(card_Tf_pr.DOMove(oriPos, dur));

        Sequence seqRot = DOTween.Sequence();
        seqRot.Append(card_Tf_pr.DORotateQuaternion(targetRotation, dur * 2f));
        seqRot.OnComplete(() => unityEvent.Invoke());

        seqPos.Play();
        seqRot.Play();
    }
}
