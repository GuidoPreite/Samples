namespace DI_Example1.Interfaces
{
    public interface IRandomNumberService
    {
        int GetNumber();
    }
    public interface ITransientRandomNumberService : IRandomNumberService { }
    public interface IScopedRandomNumberService : IRandomNumberService { }
    public interface ISingletonRandomNumberService : IRandomNumberService { }

}
