using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace TheWorld.Services
{
    public class DebugMailService : IMailService
    {
        public void SendMail(string from, string to, string subject, string body)
        {
            Debug.WriteLine($"Mail from: {from}, to: {to}, subj: {subject}, body: {body}");
        }
    }
}
