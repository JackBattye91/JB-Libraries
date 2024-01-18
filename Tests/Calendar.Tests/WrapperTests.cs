namespace Calendar.Tests {
    public class WrapperTests {

        [Test]
        public void GetEvents() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();

            Task<JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>> getEventsTask = wrapper.GetEvents();
            getEventsTask.Wait();
            JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>> getEventRc = getEventsTask.Result;

            // Assert
            Assert.That(getEventRc.Success, Is.EqualTo(true));
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

            Task<JB.Common.IReturnCode<bool>> getEventsTask = wrapper.AddEvent(newEvent, "primary");
            getEventsTask.Wait();
            var getEventsRc = getEventsTask.Result;

            // Assert
            Assert.That(getEventsRc.Success, Is.EqualTo(true));
        }

        [Test]
        public void UpdateEvents() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");


            // Arrange
            JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>> rc = new JB.Common.ReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>();
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();
            JB.Calendar.Interfaces.ICalendarEvent? calEvent = null;

            if (rc.Success) {
                Task<JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>> getEventTask = wrapper.GetEvents();
                getEventTask.Wait();
                var getEventRc = getEventTask.Result;

                if (getEventRc.Success) {
                    calEvent = rc.Data?.Where(x => x.Description == "Test event").FirstOrDefault();
                }
                else {
                    JB.Common.ErrorWorker.CopyErrors(getEventRc, rc);
                }
            }
            
            if (rc.Success) {
                if (calEvent != null) {
                    calEvent.Finish = DateTime.Now.AddDays(2);
                    Task<JB.Common.IReturnCode<bool>> updateEvenntTask = wrapper.UpdateEvent(calEvent, "primary");
                    updateEvenntTask.Wait();
                    var updateEvenntRc = updateEvenntTask.Result;

                    if (updateEvenntRc.Success == false) {
                        JB.Common.ErrorWorker.CopyErrors(updateEvenntRc, rc);
                    }
                }
                
            }

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }

        [Test]
        public void CancelEvent() {
            Environment.SetEnvironmentVariable("clientId", "892355022973-sr7kl51qh74fl9p9n4jad50062q1bgvi.apps.googleusercontent.com");
            Environment.SetEnvironmentVariable("clientSecret", "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");

            // Arrange
            JB.Common.IReturnCode<bool> rc = new JB.Common.ReturnCode<bool>();
            JB.Calendar.IWrapper wrapper = JB.Calendar.Factory.CreateCalendarWrapper();

            Task<JB.Common.IReturnCode<IList<JB.Calendar.Interfaces.ICalendarEvent>>>? getEventsTask = wrapper.GetEvents();
            getEventsTask.Wait();
            var getEventsRc = getEventsTask.Result;


            if (getEventsRc.Success) {
                JB.Calendar.Interfaces.ICalendarEvent? newEvent = getEventsTask?.Result?.Data?.Where(x => x.Description == "Test event").FirstOrDefault();

                if (newEvent != null) {
                    Task<JB.Common.IReturnCode<bool>>? cancelEventTask = wrapper.CancelEvent(newEvent.Id, "primary");
                    cancelEventTask.Wait();
                    var cancelEventRc = cancelEventTask.Result;

                    if (cancelEventRc.Success == false) {
                        JB.Common.ErrorWorker.CopyErrors(cancelEventRc, rc);
                    }
                }
            }

            // Assert
            Assert.That(rc.Success, Is.EqualTo(true));
        }
    }
}