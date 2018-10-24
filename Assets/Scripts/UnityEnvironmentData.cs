using UnityEngine;

namespace RetroPlatform
{
    public class UnityEnvironmentData : MonoBehaviour, IEnvironmentData
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}
