namespace ab5entSDK.Common.DesignPatterns.StructuralPatterns.Adapter
{
    public interface IAdapter<T> where T : IAdaptee
    {
        public void SetAdaptee(in T adaptee);
    }
}