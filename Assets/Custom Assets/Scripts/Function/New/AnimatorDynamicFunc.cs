using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace FiveRound
{
    public class AnimatorDynamicFunc : MonoBehaviour
    {
        public void OnAnimationFinish(string hashS)
        {
            HashHandler.RemoveHash(Hash128.Parse(hashS));
        }
    }
}
