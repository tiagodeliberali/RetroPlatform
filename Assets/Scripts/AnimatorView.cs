using System;
using System.Collections.Generic;
using UnityEngine;

namespace RetroPlatform.Battle
{
    public class AnimatorView<T>
    {
        Animator animator;
        Dictionary<int, T> animatorStateHash = new Dictionary<int, T>();
        T currentStatus;

        public event Action<T> OnStatusChanged;

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
            T newStatus;

            if (animatorStateHash.ContainsKey(hashName))
                newStatus = animatorStateHash[hashName];
            else
                newStatus = default(T);

            if (!newStatus.Equals(currentStatus))
            {
                currentStatus = newStatus;
                if (OnStatusChanged != null) OnStatusChanged(currentStatus);
            }

            return currentStatus;
        }
    }
}
