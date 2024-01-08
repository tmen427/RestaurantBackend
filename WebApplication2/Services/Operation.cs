namespace WebApplication2.Services
{
    public class Operation : IOperationTransient, IOperationSingleton, IOperationScoped
    {
        
        public string OperationId { get; set; }

        public ILogger<Operation> Logger { get; set; }
        public Operation(ILogger<Operation> logger)
        {
                OperationId = Guid.NewGuid().ToString();
                Logger = logger;
                Logger.LogInformation(OperationId);
        }

        




    }
}
