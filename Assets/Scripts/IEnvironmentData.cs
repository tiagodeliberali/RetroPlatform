namespace RetroPlatform
{
    public interface IEnvironmentData
    {
        float GetDeltaTime();
        int GetRandom(int min, int max);
        bool RandomBool();
    }
}
