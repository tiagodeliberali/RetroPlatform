using System.Collections.Generic;
using RetroPlatform;

namespace RetroPlatformTest
{
    public class TestEnvironmentData : IEnvironmentData
    {
        public float DeltaTime { get; set; }
        public Stack<int> RandomResult { get; set; } = new Stack<int>();
        public Stack<bool> RandomBoolResults { get; set; } = new Stack<bool>();

        public TestEnvironmentData()
        {
            DeltaTime = 0.01f;
        }

        public float GetDeltaTime()
        {
            return DeltaTime;
        }

        public int GetRandom(int min, int max)
        {
            return RandomResult.Pop();
        }

        public bool RandomBool()
        {
            return RandomBoolResults.Pop();
        }
    }
}
