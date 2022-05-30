
namespace NoSqlDatabase.Tests {
    public class WrapperTests {
        [SetUp]
        public void Setup() {
            Environment.SetEnvironmentVariable("cosmos-connection-string", "AccountEndpoint=https://chorepoints.documents.azure.com:443/;AccountKey=5MMRvx7Vw7AD45ojKQMni17DSXW4Xt6AShbE7WVpLq0h4uWKdngBa4tMkORd0PRNRXtOyMYsLgLjTJS5G7Z2RA==;");
        }

        [Test]
        public void CreateDatabase() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();
            
            // Act
            var database = wrapper.CreateDatabase("testDatabase");
            database.Wait();


            // Assert
            Assert.That(database.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void CreateContainer() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var database = wrapper.CreateContainer("testDatabase", "testContainer", "name");
            database.Wait();

            // Assert
            Assert.That(database.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
        [Test]
        public void GetContainer() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var database = wrapper.GetContainer("testDatabase", "testContainer");
            database.Wait();

            // Assert
            Assert.That(database.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void AddItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            Dictionary<string, object> item = new Dictionary<string, object>();
            item.Add("id", Guid.NewGuid().ToString());
            item.Add("name", "Jack Battye");
            item.Add("testItems", new List<string>(new string[] { "Hello", "World" }));

            // Act
            var itemTask = wrapper.AddItem("testDatabase", "testContainer", item);
            itemTask.Wait();

            // Assert
            Assert.That(itemTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void GetItems() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            Dictionary<string, object> item = new Dictionary<string, object>();
            item.Add("id", Guid.NewGuid().ToString());
            item.Add("name", "Jack Battye");
            item.Add("testItems", new List<string>(new string[] { "Hello", "World" }));

            // Act
            var itemTask = wrapper.GetItems<Dictionary<string, object>>("testDatabase", "testContainer");
            itemTask.Wait();

            // Assert
            Assert.That(itemTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
        [Test]
        public void GetItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var itemTask = wrapper.GetItem<Dictionary<string, object>>("testDatabase", "testContainer", "c7d0192e-a58d-4583-aae5-9fe6302d2ab7");
            itemTask.Wait();

            // Assert
            Assert.That(itemTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void UpdateItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            Dictionary<string, object> updatedItem = new Dictionary<string, object>();
            updatedItem.Add("name", "Sarah");

            // Act
            var itemTask = wrapper.UpdateItem("testDatabase", "testContainer", updatedItem, "c7d0192e-a58d-4583-aae5-9fe6302d2ab7");
            itemTask.Wait();

            // Assert
            Assert.That(itemTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
    }
}