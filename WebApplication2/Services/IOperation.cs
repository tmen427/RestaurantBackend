namespace WebApplication2.Services
{
    public interface IOperation
    {

        string OperationId { get; set; }
    }


    public interface IOperationTransient: IOperation { }
    public interface IOperationSingleton: IOperation { }

    public interface IOperationScoped: IOperation { }



}
