using System;
using System.Collections.Generic;
using UnityEngine;

namespace RetroPlatform.Battle
{
    public class AnimatorView<T>
    {
        Animator animator;
        Dictionary<int, T> animatorStateHash = new Dictionary<int, T>();

        public AnimatorView(Animator animator)
        {
            this.animator = animator;
            GetAnimationStates();
        }

        void GetAnimationStates()
        {
            foreach (T state in (T[])Enum.GetValues(typeof(T)))
            {
                animatorStateHash.Add(Animator.StringToHash(state.ToString()), state);
            }
        }

        public T GetCurrentStatus()
        {
            var hashName = animator.GetCurrentAnimatorStateInfo(0).shortNameHash;

            if (animatorStateHash.ContainsKey(hashName))
                return animatorStateHash[hashName];

            return default(T);
        }
    }
}
