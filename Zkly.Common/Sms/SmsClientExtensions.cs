using System;
using System.Threading.Tasks;
using Zkly.Common.Extension;

namespace Zkly.Common.SMS
{
    public static class SmsClientExtensions
    {
        public static SmsResult Send(this SmsClient client, string message, params string[] numbers)
        {
            Func<Task<SmsResult>> func = () => client.SendAsync(message, numbers);
            return func.RunSync();
        }

        public static SmsResult Send(this SmsClient client, DateTime? schedule, string message, params string[] numbers)
        {
            Func<Task<SmsResult>> func = () => client.SendAsync(schedule, message, numbers);
            return func.RunSync();
        }

        public static SmsResult UpdatePassword(this SmsClient client, string password)
        {
            Func<Task<SmsResult>> func = () => client.UpdatePasswordAsync(password);
            return func.RunSync();
        }

        /// <summary>
        /// Account接口（查余额接口）
        /// </summary>
        /// <returns>POST提交之后，用户名密码认证失败返回错误码，成功返回短信剩余条数记录</returns>
        public static SmsResult QueryAccount(this SmsClient client)
        {
            Func<Task<SmsResult>> func = client.QueryAccountAsync;
            return func.RunSync();
        }

        public static SmsResult Register(this SmsClient client, string userName, string mobile, string phone, string fax, string email, string postcode, string company, string address)
        {
            Func<Task<SmsResult>> func = () => client.RegisterAsync(userName, mobile, phone, fax, email, postcode, company, address);
            return func.RunSync();
        }

        public static SmsResult QueryReport(this SmsClient client)
        {
            Func<Task<SmsResult>> func = client.QueryReportAsync;
            return func.RunSync();
        }
    }
}
