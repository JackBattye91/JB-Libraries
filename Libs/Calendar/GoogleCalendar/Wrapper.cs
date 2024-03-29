﻿using JB.Common;
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
            IList<string> scopes = new List<string>(new string[] { CalendarService.Scope.Calendar });
            IList<Interfaces.ICalendarEvent> calendarEvents = new List<Interfaces.ICalendarEvent>();

            if (rc.Success) {
                IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                if (getCredentialRc.Success == false) {
                    ErrorWorker.CopyErrors(getCredentialRc, rc);
                }
            }

            if (rc.Success) {
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
                                Id = eventItem.Id,
                                Description = eventItem.Summary,
                                Start = eventItem.Start.DateTime,
                                Finish = eventItem.End.DateTime
                            });
                        }
                    }
                }
                catch (Exception ex) {
                    rc.AddError(new JB.Common.Error(ErrorCodes.UNABLE_TO_GET_ITEMS, ex));
                }
            }

            if (rc.Success) {
                rc.Data = calendarEvents;
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> AddEvent(Interfaces.ICalendarEvent pEvent, string pCalenderId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            IList<string> scopes = new List<string>(new string[] { CalendarService.Scope.CalendarEvents });

            if (rc.Success) {
                IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                if (getCredentialRc.Success == false) {
                    ErrorWorker.CopyErrors(getCredentialRc, rc);
                }
            }

            try {
                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = userCredential,
                    ApplicationName = Consts.APPLICATION_NAME
                });

                Event newEvent = Worker.Convert(pEvent);
             
                var request = service.Events.Insert(newEvent, pCalenderId);
                newEvent = await request.ExecuteAsync();
            }
            catch (Exception ex) {
                rc.AddError(new JB.Common.Error(ErrorCodes.UNABLE_TO_INSERT_NEW_ITEM, ex));
            }

            return rc;
        }
        public async Task<IReturnCode<bool>> UpdateEvent(Interfaces.ICalendarEvent pEvent, string pCalendarID) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            IList<string> scopes = new List<string>();

            if (rc.Success) {
                IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                if (getCredentialRc.Success == false) {
                    ErrorWorker.CopyErrors(getCredentialRc, rc);
                }
            }


            if (rc.Success) {
                try {
                    var service = new CalendarService(new BaseClientService.Initializer() {
                        HttpClientInitializer = userCredential,
                        ApplicationName = Consts.APPLICATION_NAME
                    });

                    Event updatedEvent = Worker.Convert(pEvent);

                    var request = service.Events.Patch(updatedEvent, pCalendarID, pEvent.Id);
                    updatedEvent = await request.ExecuteAsync();
                }
                catch (Exception ex) {
                    rc.AddError(new JB.Common.Error(ErrorCodes.UNABLE_TO_UPDATE_ITEM, ex));
                }
            }

            return rc;
        }

        public async Task<IReturnCode<bool>> CancelEvent(string pEventId, string pCalendarId) {
            IReturnCode<bool> rc = new ReturnCode<bool>();
            IList<string> scopes = new List<string>();

            if (rc.Success) {
                IReturnCode<bool> getCredentialRc = await GetUserCredentials(scopes);

                if (getCredentialRc.Success == false) {
                    ErrorWorker.CopyErrors(getCredentialRc, rc);
                }
            }

            try {
                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = userCredential,
                    ApplicationName = Consts.APPLICATION_NAME
                });

                var request = service.Events.Delete(pCalendarId, pEventId);
                string response = await request.ExecuteAsync();                
            }
            catch (Exception ex) {
                rc.AddError(new JB.Common.Error(ErrorCodes.UNABLE_TO_DELETE_ITEM, ex));
            }

            return rc;
        }

        protected async Task<IReturnCode<bool>> GetUserCredentials(IList<string> pScopes) {
            IReturnCode<bool> rc = new ReturnCode<bool>();

            if (rc.Success) {
                try {
                    string? clientId = Environment.GetEnvironmentVariable("clientId");
                    string? clientSecret = Environment.GetEnvironmentVariable("clientSecret");

                    ClientSecrets clientSecrets = new ClientSecrets() {
                        ClientId = clientId,
                        ClientSecret = clientSecret
                    };

                    userCredential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, pScopes, "user", CancellationToken.None);
                }
                catch (Exception ex) {
                    rc.AddError(new JB.Common.Error(ErrorCodes.UNABLE_TO_GET_USER_CREDENTIALS, ex));
                }
            }
            
            return rc;
        }
    }
}
