using RetroPlatform;

namespace RetroPlatformTest
{
    public class TestEnvironmentData : IEnvironmentData
    {
        public float DeltaTime { get; set; }

        public TestEnvironmentData()
        {
            DeltaTime = 0.01f;
        }

        public float GetDeltaTime()
        {
            return DeltaTime;
        }
    }
}
