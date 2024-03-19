﻿using JB.Email.Interfaces;
using JB.Common;
using JB.Email;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Net.Http.Headers;
using SendGrid.Helpers.Mail.Model;

namespace JB.Email.SendGrid
{
    internal class Wrapper : IWrapper
    {
        private string? ApiKey;

        public Wrapper(string? pApiKey)
        {
            ApiKey = pApiKey;
        }

        public async Task<IReturnCode> Send(IEmailHeader pHeader, IEmailBody pBody)
        {
            IReturnCode rc = new ReturnCode();
            SendGridClient? client = null;
            SendGridMessage? emailMessage = null;

            try
            {
                if (rc.Success)
                {
                    if (string.IsNullOrEmpty(ApiKey))
                    {
                        ApiKey = Environment.GetEnvironmentVariable("Email__ApiKey");
                    }

                    if (string.IsNullOrEmpty(ApiKey))
                    {
                        rc.AddError(new Error("Unable to get API key"));
                    }
                }

                if (rc.Success)
                {
                    client = new SendGridClient(ApiKey);
                    EmailAddress from = new EmailAddress(pHeader.Sender);

                    if (pHeader.To.Count() >= 1)
                    {
                        List<EmailAddress> to = pHeader.To.Count() > 0 ? pHeader.To.Select(x => new EmailAddress(x)).ToList() : [];
                        List<EmailAddress> cc = pHeader.Cc.Count() > 0 ? pHeader.Cc.Select(x => new EmailAddress(x)).ToList() : [];
                        List<EmailAddress> bcc = pHeader.Bcc.Count() > 0 ? pHeader.Bcc.Select(x => new EmailAddress(x)).ToList() : [];
                        string subject = pHeader.Subject;
                        string body = pBody.Content;

                        emailMessage = new SendGridMessage();
                        emailMessage.SetFrom(from);
                        emailMessage.ReplyTo = from;
                        emailMessage.SetGlobalSubject(subject);

                        if (pBody.IsHtml)
                        {
                            emailMessage.AddContent(MimeType.Html, body);
                            emailMessage.HtmlContent = body;
                        }
                        else
                        {
                            emailMessage.AddContent(MimeType.Text, body);
                            emailMessage.PlainTextContent = body;
                        }
                        
                        if (to.Count > 0)
                        {
                            for (var i = 0; i < to.Count; i++)
                            {
                                emailMessage.AddTo(to[i], i);
                            }
                        }

                        if (cc.Count > 0)
                        {
                            for (var i = 0; i < cc.Count; i++)
                            {
                                emailMessage.AddCc(cc[i], i);
                            }
                        }

                        if (bcc.Count > 0)
                        {
                            for (var i = 0; i < bcc.Count; i++)
                            {
                                emailMessage.AddBcc(bcc[i], i);
                            }
                        }
                    }
                    else
                    {
                        rc.AddError(new Error("No recipients found"));
                    }
                }

                if (rc.Success) { 
                    Response response = await client!.SendEmailAsync(emailMessage!);

                    if (!response.IsSuccessStatusCode)
                    {
                        rc.AddError(new Error("Unable to send email"));
                    }
                }
            }
            catch (Exception ex) {
                rc.AddError(new Error(ex));
            }

            return rc;
        }
    }
}