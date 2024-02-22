namespace ab5entSDK.Common.DesignPatterns.CreationalPatterns.Prototype
{
    public interface IPrototype<out T>
    {
        T Clone();
    }
}