using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LuzInga.Application.Services
{
    public interface IEmailProvider
    {
        void SendEmail(string to, string subject, string body);
    }
}