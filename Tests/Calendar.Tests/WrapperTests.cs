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
            long errorCode = 0;

            Task<JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>>? eventEventsTask = wrapper.GetEvents();
            eventEventsTask.Wait();
            if (eventEventsTask.Result.ErrorCode == 0) {
                JB.Calendar.Interfaces.ICalendarEvent? newEvent = eventEventsTask?.Result?.Data?.Where(x => x.Description == "Test event").FirstOrDefault();

                if (newEvent != null) {
                    newEvent.Finish = DateTime.Now.AddDays(2);
                    Task<JB.Common.IReturnCode<bool>>? getEventsTask = wrapper.UpdateEvent(newEvent, "primary");
                    getEventsTask.Wait();
                    errorCode = getEventsTask.Result.ErrorCode;
                }
            }
            

            // Assert
            Assert.That(errorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }

        [Test]
        public void CancelEvent() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();
            long errorCode = 0;

            Task<JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>>? eventEventsTask = wrapper.GetEvents();
            eventEventsTask.Wait();
            if (eventEventsTask.Result.ErrorCode == 0) {
                JB.Calendar.Interfaces.ICalendarEvent? newEvent = eventEventsTask?.Result?.Data?.Where(x => x.Description == "Test event").FirstOrDefault();

                if (newEvent != null) {
                    Task<JB.Common.IReturnCode<bool>>? getEventsTask = wrapper.CancelEvent(newEvent.Id, "primary");
                    getEventsTask.Wait();
                    errorCode = getEventsTask.Result.ErrorCode;
                }
            }

            // Assert
            Assert.That(errorCode, Is.EqualTo(JB.Common.ErrorCodes.SUCCESS));
        }
    }
}