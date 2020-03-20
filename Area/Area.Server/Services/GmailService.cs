using Area.Server.Database.Models;
using Area.Server.Database.Tables;
using Area.Shared.Entities;
using Area.Shared.Protocol.Actions;
using Area.Shared.Protocol.Other;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Net.Mail;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace Area.Server.Services
{
    public class GmailServ : IService
    {

        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail API .NET Quickstart";

        static void Main(string[] args)
        {
            UserCredential credential;

            using (var stream =
                new FileStream("credentials.json", FileMode.Open, FileAccess.Read))
            {

                string credPath = "token.json";
                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                    GoogleClientSecrets.Load(stream).Secrets,
                    Scopes,
                    "user",
                    CancellationToken.None,
                    new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }


            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });
        }


        public static object Gmail(ServiceModel service, AccountModel account, ActionRequestMessage msg)
        {
            switch (msg.ActionId)
            {
                case (int)ActionEnum.SendMail:
                    string[] parts = msg.Params.Split("|", StringSplitOptions.RemoveEmptyEntries);
                    string r = "";

                    if (parts.Length < 3)
                        return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.BadParams, service.Id, msg.ActionId, r, msg.Params));
                    r = GmailServ.Send(account.Username, account.Password, parts[0], parts[1], parts[2]).ToString();
                    return (new ActionResultMessage(Shared.Protocol.Actions.Enums.ActionResultEnum.Success, service.Id, msg.ActionId, r, msg.Params));
                default:
                    return new UnknowBehaviourMessage();
            }
        }


        public static bool Send(string subject, string msg)
        {
            AccountModel account = AccountTable.GetModelByServiceId((int)ServiceEnum.Gmail);
            if (account == null)
                return (false);
            return (Send(account.Username, account.Password, account.Username, subject, msg));
        }

        public static bool Send(string username, string password, string receiver, string subject, string msg)
        {
            try
            {
                using (MailMessage mail = new MailMessage())
                {
                    mail.From = new MailAddress(username);
                    mail.To.Add(receiver);
                    mail.Subject = subject;
                    mail.Body = "<h1>" + msg + "</h1>";
                    mail.IsBodyHtml = true;

                    using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                    {
                        smtp.UseDefaultCredentials = false;
                        smtp.Credentials = new NetworkCredential(username, password);
                        smtp.EnableSsl = true;
                        smtp.Send(mail);
                    }
                }
                return (true);
            }
            catch (Exception ex)
            {
                return (false);
            }
        }

    }
}
