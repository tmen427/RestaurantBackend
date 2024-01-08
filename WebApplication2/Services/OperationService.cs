using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace WebApplication2.Services
{
    public class OperationService
    {
        public IOperationScoped scoped { get; set; }
        public IOperationSingleton singleton { get; set; }

        public IOperationTransient transient { get; set; }


        public OperationService(IOperationScoped sc, IOperationSingleton si, IOperationTransient tr)
        {
            scoped = sc; 
            singleton = si;  
            transient = tr;
                
        }
    }
}
