using System.Collections.Generic;
using UnityEngine;

namespace Pokemon.Animations
{
    public class AnimationEventTranslator : MonoBehaviour
    {
        [SerializeField] private List<BaseAnimation> _animations;

        public BaseAnimation GetAnimationInfo(string animName)
        {
            return _animations.Find(anim => anim.Name == animName);
        }
    }
}