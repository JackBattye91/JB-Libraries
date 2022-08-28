
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
            var rc = database.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void CreateContainer() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var database = wrapper.CreateContainer("testDatabase", "testContainer", "name");
            database.Wait();
            var rc = database.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
        [Test]
        public void GetContainer() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var database = wrapper.GetContainer("testDatabase", "testContainer");
            database.Wait();
            var rc = database.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
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
            var rc = itemTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
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
            var rc = itemTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
        [Test]
        public void GetItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var itemTask = wrapper.GetItem<Dictionary<string, object>>("testDatabase", "testContainer", "b0aa32aa-cec9-41b8-9bf6-3f29097e635d");
            itemTask.Wait();
            var rc = itemTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void UpdateItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            Dictionary<string, object> updatedItem = new Dictionary<string, object>();
            updatedItem.Add("id", "b0aa32aa-cec9-41b8-9bf6-3f29097e635d");
            updatedItem.Add("name", "Jack Battye");
            updatedItem.Add("testItems", new List<string>(new string[] { "Hello", "World", "John" }));

            // Act
            var itemTask = wrapper.UpdateItem("testDatabase", "testContainer", updatedItem, "b3cb6ebd-5278-4c84-8476-a50d7305b27d", "Jack Battye");
            itemTask.Wait();
            var rc = itemTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void DeleteItem() {
            // Arrange
            JB.NoSqlDatabase.IWrapper wrapper = JB.NoSqlDatabase.Factory.CreateNoSqlDatabaseWrapper();

            // Act
            var itemTask = wrapper.DeleteItem<Dictionary<string, object>>("testDatabase", "testContainer", "e87d175e-2203-462f-92e0-c9696d64ccb1", "Jack Battye");
            itemTask.Wait();
            var rc = itemTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
    }
}