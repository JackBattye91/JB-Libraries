using JB.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace JB.Calendar.GoogleCalendar {
    internal class Wrapper : IWrapper {
        UserCredential? userCredential;

        public Wrapper() {
            userCredential = null;
        }

        public IReturnCode<bool> GetCalendarEvents() {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            UserCredential credential;

            var service = new CalendarService(new BaseClientService.Initializer() {
                HttpClientInitializer = credential,
                ApplicationName = Consts.APPLICATION_NAME
            });

            return rc;
        }
    }
}
