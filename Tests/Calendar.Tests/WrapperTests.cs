namespace Calendar.Tests {
    public class WrapperTests {

        [Test]
        public void GetEvents() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();

            var getEventsTask = wrapper.GetEvents();
            getEventsTask.Wait();

            // Assert
            Assert.That(getEventsTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void AddEvents() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();

            JB.Calendar.Interfaces.ICalendarEvent newEvent = new Models.CalendarEvent() {
                Start = DateTime.Now,
                Finish = DateTime.Now.AddHours(2),
                Description = "Test event"
            };

            var getEventsTask = wrapper.AddEvent(newEvent, "primary");
            getEventsTask.Wait();

            // Assert
            Assert.That(getEventsTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void UpdateEvents() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();

            JB.Calendar.Interfaces.ICalendarEvent newEvent = new Models.CalendarEvent() {
                Start = DateTime.Now,
                Finish = DateTime.Now.AddHours(2),
                Description = "Test event"
            };

            var getEventsTask = wrapper.UpdateEvent(newEvent, "primary");
            getEventsTask.Wait();

            // Assert
            Assert.That(getEventsTask.Result.ErrorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
    }
}