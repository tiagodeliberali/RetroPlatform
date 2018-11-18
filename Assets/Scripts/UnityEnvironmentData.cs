using UnityEngine;

namespace RetroPlatform
{
    public class UnityEnvironmentData : IEnvironmentData
    {
        System.Random systemRandom = new System.Random();

        public float GetDeltaTime()
        {
            return Time.deltaTime;
        }

        public int GetRandom(int min, int max)
        {
            return Random.Range(min, max);
        }

        public bool RandomBool()
        {
            return systemRandom.NextDouble() >= 0.5;
        }
    }
}
