namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.Builder
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}