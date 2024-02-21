using FiveRound;
using UnityEngine;

//////////////////////////////////////////////////////////////////////
/// <summary>
/// Animator functionalities
/// </summary>
//////////////////////////////////////////////////////////////////////
#region Animator functionalities

public class AnimatorFunc : MonoBehaviour
{
    public static AnimationClip GetClip(Animator anim_Cp, string name, int layer = 0)
    {
        AnimationClip clip = null;

        foreach (AnimationClip clip_tp in anim_Cp.runtimeAnimatorController.animationClips)
        {
            if (string.Equals(clip_tp.name, name))
            {
                clip = clip_tp;
                break;
            }
        }

        return clip;
    }

    public static float GetClipLength(Animator anim_Cp, string name, int layer = 0)
    {
        return GetClip(anim_Cp, name, layer).length;
    }

    public static void AddEvent(Animator anim_Cp, string name, Hash128 hash, int layer = 0)
    {
        AnimationClip clip = GetClip(anim_Cp, name, layer);

        GameObject anim_GO = anim_Cp.gameObject;
        if (anim_GO.GetComponent<AnimatorDynamicFunc>() == null)
        {
            anim_GO.AddComponent<AnimatorDynamicFunc>();
        }

        AnimationEvent ev = new AnimationEvent();
        ev.time = clip.length;
        ev.functionName = "OnAnimationFinish";
        ev.stringParameter = hash.ToString();

        clip.AddEvent(ev);
    }

    public static void RemoveEvent(Animator anim_Cp, string name, int layer = 0)
    {
        AnimationClip clip = GetClip(anim_Cp, name, layer);

        clip.events = null;
    }
}

#endregion
