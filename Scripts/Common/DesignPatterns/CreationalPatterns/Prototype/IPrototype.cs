namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns
{
    public interface IPrototype<out T>
    {
        T Clone();
    }
}