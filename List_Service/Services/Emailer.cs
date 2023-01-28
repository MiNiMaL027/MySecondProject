using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;

namespace AcumaticaExternalAppServer
{
    public static class Emailer
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.GmailSend };
        static string ApplicationName = "ToDoListServer";
        static UserCredential credential;
        public static GmailService service;

        static Emailer()
        {
            using (var stream =
                new FileStream("GmailApiCredentials.json", FileMode.Open, FileAccess.Read))
            {
                // The file token.json stores the user's access and refresh tokens, and is created
                // automatically when the authorization flow completes for the first time.
                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.FromStream(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
            }

            // Create Gmail API service.
            service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
            service.Users.Settings.SendAs.Get("me", "ToDoListServer");
        }
        public static string Base64UrlEncode(string input)
        {
            var inputBytes = System.Text.Encoding.UTF8.GetBytes(input);
            return Convert.ToBase64String(inputBytes).Replace("+", "-").Replace("/", "_").Replace("=", "");
        }
        public static bool SendEmail(string receiver, string title, string messageText)
        {

            string plainText = $"From: ToDoListServer <minimal027027@gmail.com>\r\n" +
                               $"To: {receiver}\r\n" +
                               $"Subject: {title}\r\n" +
                               "Content-Type: text/html; charset=utf-8\r\n\r\n" +
                               $"<h1>{messageText}</h1>";
            var newMsg = new Google.Apis.Gmail.v1.Data.Message();
            newMsg.Raw = Base64UrlEncode(plainText.ToString());
            try
            {
                service.Users.Messages.Send(newMsg, "me").Execute();
            }
            catch
            {
                return false;
            }
            return true;
        }
    }
}
