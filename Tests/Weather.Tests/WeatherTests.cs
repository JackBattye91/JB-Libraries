namespace Weather.Tests {
    public class WeatherTests {
        [Test]
        public void GetTodaysForcast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.GetTodaysForcast("2643123");
            getTodaysForcastTask.Wait();


            // Assert
            Assert.That(getTodaysForcastTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void Get3DayForcast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.Get3DayForcast("2643123");
            getTodaysForcastTask.Wait();

            // Assert
            Assert.That(getTodaysForcastTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
    }
}