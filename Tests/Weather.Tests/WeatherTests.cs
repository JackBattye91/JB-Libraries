namespace Weather.Tests {
    public class WeatherTests {
        [Test]
        public void GetTodaysForecast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.GetTodaysForecast("2643123");
            getTodaysForcastTask.Wait();
            var rc = getTodaysForcastTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void Get3DayForecast() {
            // Arrange
            JB.Weather.IWrapper wrapper = JB.Weather.Factory.CreateWeatherWrapper();

            // Act
            var getTodaysForcastTask = wrapper.Get3DayForecast("2643123");
            getTodaysForcastTask.Wait();
            var rc = getTodaysForcastTask.Result;

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
    }
}