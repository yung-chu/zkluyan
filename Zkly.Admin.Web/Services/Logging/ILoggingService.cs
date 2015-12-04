namespace Zkly.Admin.Web.Services
{
    using System;

    public interface ILoggingService
    {
        void Log(Exception exception);
    }
}
