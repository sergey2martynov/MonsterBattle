using System;
using UnityEngine;

namespace Pokemon.Animations
{
    [Serializable]
    public class BaseAnimation
    {
        [SerializeField] protected AnimationClip _animationClip;
        [SerializeField] private float _actionTime;

        public string Name => _animationClip.name;
        public float Duration => _animationClip.length;
        public float FrameRate => _animationClip.frameRate;
        public float ActionTime => _actionTime;
    }
}