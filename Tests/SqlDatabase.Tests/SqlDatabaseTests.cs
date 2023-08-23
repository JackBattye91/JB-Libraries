namespace JB.SqlDatabase_Tests {
    public class SqlDatabaseTests {
        [Fact]
        public void CreateDatabase_Test() {
            Mock<IWrapper> mockWrapper = new Mock<IWrapper>();
            mockWrapper.Setup(x => x.CreateDatabase(It.IsAny<string>()));


        }
    }
}