namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns
{
    public interface IBuilder<out T>
    {
        T Build();
    }
}