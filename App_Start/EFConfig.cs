using System.Data.Entity;
using fintech.Data;

namespace fintech.App_Start
{
    public class EFConfig
    {
        public static void Initialize()
        {
            // Use CreateDatabaseIfNotExists so your scripts can also be used.
            //Database.SetInitializer<FintechDbContext>(new CreateDatabaseIfNotExists<FintechDbContext>());
        }
    }
}
