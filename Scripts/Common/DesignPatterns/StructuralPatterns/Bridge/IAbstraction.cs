namespace ab5entSDK.Common.DesignPatterns.StructuralPatterns.Bridge
{
    public interface IAbstraction<T> where T : IImplementor
    {
        public void SetImplementor(in T implementor);
    }
}