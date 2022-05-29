using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using JB.Common;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Calendar.v3;
using Google.Apis.Calendar.v3.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;


namespace JB.Calendar.GoogleCalendar {
    internal class Worker {
        public static async  Task<IReturnCode<UserCredential>> GetCredentials(IList<string> pScopes) {
            IReturnCode<UserCredential> rc = new ReturnCode<UserCredential>();
            UserCredential? credential = null;

            try {
                ClientSecrets clientSecrets = new ClientSecrets() {
                    ClientId = "",
                    ClientSecret = ""
                };

                credential = await GoogleWebAuthorizationBroker.AuthorizeAsync(clientSecrets, pScopes, "", CancellationToken.None);


                var service = new CalendarService(new BaseClientService.Initializer() {
                    HttpClientInitializer = credential,
                    ApplicationName = Consts.APPLICATION_NAME
                });


            }
            catch (Exception e) {

            }

            // https://developers.google.com/calendar/api/quickstart/dotnet?hl=en_GB

            return rc;
        }
    }
}
