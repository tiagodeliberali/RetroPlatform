using UnityEngine;


namespace RetroPlatform
{
    public class UnityEnvironmentData : IEnvironmentData
    {
        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }
    }
}
