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

        public async Task<IReturnCode<IList<Interfaces.ICalendarEvent>>> GetEvents() {
            IReturnCode<IList<Interfaces.ICalendarEvent>> rc = new ReturnCode<IList<Interfaces.ICalendarEvent>>();
            IList<string> scopes = new List<string>();
            IList<Interfaces.ICalendarEvent> calendarEvents = new List<Interfaces.ICalendarEvent>();


            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                if (ErrorCodes.SUCCESS != getCredentialRc.ErrorCode) {
                    ErrorWorker.CopyErrorCode(getCredentialRc, rc);
                }
            }

            try {
                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = userCredential,
                    ApplicationName = Consts.APPLICATION_NAME
                });

                // Define parameters of request.
                EventsResource.ListRequest request = service.Events.List("primary");
                request.TimeMin = DateTime.Now;
                request.ShowDeleted = false;
                request.SingleEvents = true;
                request.MaxResults = 10;
                request.OrderBy = EventsResource.ListRequest.OrderByEnum.StartTime;

                // List events.
                Events events = request.Execute();
                if (events.Items != null && events.Items.Count > 0) {
                    foreach (var eventItem in events.Items) {
                        calendarEvents.Add(new Models.CalendarEvent() { 
                            Start = eventItem.Start.DateTime, 
                            Finish = eventItem.End.DateTime
                        });
                    }
                }
            }
            catch (Exception e) {
                rc.ErrorCode = 5;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> AddEvent(Interfaces.ICalendarEvent pEvent, string pCalenderId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            IList<string> scopes = new List<string>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                    IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                    if (ErrorCodes.SUCCESS != getCredentialRc.ErrorCode) {
                        ErrorWorker.CopyErrorCode(getCredentialRc, rc);
                    }
                }
            }

            try {
                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = userCredential,
                    ApplicationName = Consts.APPLICATION_NAME
                });

                Google.Apis.Calendar.v3.Data.Event newEvent = new Google.Apis.Calendar.v3.Data.Event();
                newEvent.Start = new EventDateTime() { DateTime = pEvent.Start };
                newEvent.End = new EventDateTime() { DateTime = pEvent.Finish };
                
                var newItem = service.Events.Insert(newEvent, pCalenderId);
            }
            catch (Exception e) {
                rc.ErrorCode = 6;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> UpdateEvent(Interfaces.ICalendarEvent pEvent, string pCalendarID) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            IList<string> scopes = new List<string>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                    IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                    if (ErrorCodes.SUCCESS != getCredentialRc.ErrorCode) {
                        ErrorWorker.CopyErrorCode(getCredentialRc, rc);
                    }
                }
            }

            try {
                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = userCredential,
                    ApplicationName = Consts.APPLICATION_NAME
                });

                Event updatedEvent = new Event() {
                    Start = new EventDateTime() { DateTime = pEvent.Start },
                    End = new EventDateTime() { DateTime = pEvent.Finish }
                };

                var newItem = service.Events.Patch(updatedEvent, pCalendarID, pEvent.Id);
            }
            catch (Exception e) {
                rc.ErrorCode = 6;
                ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
            }

            return rc;
        }
        
        protected async Task<IReturnCode<bool>> GetUserCredentials(IList<string> pScopes) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            if (JB.Common.ErrorCodes.SUCCESS == rc.ErrorCode) {
                try {
                    string? clientId = Environment.GetEnvironmentVariable("clientId");
                    string? clientSecret = Environment.GetEnvironmentVariable("clientSecret");

                    ClientSecrets clientSecrets = new ClientSecrets() {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    };

                    userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, pScopes, "user", CancellationToken.None);
                }
                catch (Exception e) {
                    rc.ErrorCode = 3;
                    ErrorWorker.AddError(rc, rc.ErrorCode, e.Message, e.StackTrace);
                }
            }
            
            return rc;
        }
    }
}
