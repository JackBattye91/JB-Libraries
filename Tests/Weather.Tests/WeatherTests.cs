namespace Weather.Tests {
    public class WeatherTests {
        [Test]
        public void GetTodaysForcast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.GetTodaysForcast("2643123");
            getTodaysForcastTask.Wait();
            var rc = getTodaysForcastTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void Get3DayForcast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.Get3DayForcast("2643123");
            getTodaysForcastTask.Wait();
            var rc = getTodaysForcastTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
    }
}