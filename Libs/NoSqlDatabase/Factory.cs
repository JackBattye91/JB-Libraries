
namespace JB.NoSqlDatabase {
    public class Factory {
        public static IWrapper CreateNoSqlDatabaseWrapper(string? pConnectionString = null) {
            return new Cosmos.Wrapper(pConnectionString);
        }
    }
}
