using System;
using System.Collections.Specialized;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Zkly.Common.Config;
using Zkly.Common.Extension;

namespace Zkly.Common.SMS
{
    public class SmsClient
    {
        #region API Url
        private const string RegisterUrl = "http://www.stongnet.com/sdkhttp/reg.aspx";

        private const string SendUrl = "http://www.stongnet.com/sdkhttp/sendsms.aspx";

        private const string SendScheduleUrl = "http://www.stongnet.com/sdkhttp/sendschsms.aspx";

        private const string QueryAccountUrl = "http://www.stongnet.com/sdkhttp/getbalance.aspx";

        private const string QueryReportUrl = "http://www.stongnet.com/sdkhttp/getmtreport.aspx";

        private const string UpdatePasswordUrl = "http://www.stongnet.com/sdkhttp/uptpwd.aspx";
        #endregion

        private readonly string _signature;

        public SmsClient(string signature = null)
        {
            if (string.IsNullOrEmpty(signature))
            {
                signature = SmsSettings.Signature;
            }

            _signature = signature;
            SignatureInEnd = true;
        }

        /// <summary>
        /// 设置是否将签名放在消息的后面，默认为true
        /// </summary>
        public bool SignatureInEnd { get; set; }

        /// <summary>
        /// 异步发送短信
        /// </summary>
        /// <param name="message">消息内容</param>
        /// <param name="numbers">手机号码</param>
        public async Task<SmsResult> SendAsync(string message, params string[] numbers)
        {
            return await SendAsync(null, message, numbers);
        }

        /// <summary>
        /// 异步发送短信
        /// </summary>
        /// <param name="schedule">定时发送</param>
        /// <param name="message">消息内容</param>
        /// <param name="numbers">手机号码</param>
        public async Task<SmsResult> SendAsync(DateTime? schedule, string message, params string[] numbers)
        {
            if (string.IsNullOrEmpty(message))
            {
                throw new ArgumentNullException("message");
            }

            if (numbers == null || numbers.Length == 0)
            {
                throw new ArgumentNullException("numbers");
            }

            var url = SendUrl;
            var data = new NameValueCollection
            {
                { "reg", SmsSettings.RegisterId },
                { "pwd", SmsSettings.Password },
                { "sourceadd", string.Empty },
                { "phone", numbers.Join(",") },
                { "content", SignatureInEnd ? message + _signature : _signature + message }
            };

            if (schedule.HasValue)
            {
                if (schedule.Value < DateTime.Now)
                {
                    throw new InvalidOperationException("定时发送的时间不能小于当前系统时间。");
                }

                url = SendScheduleUrl;
                data.Add("tim", schedule.Value.ToString("yyyy-mm-dd HH:MM:SS"));
            }

            return await PostAsync(url, data);
        }

        public async Task<SmsResult> UpdatePasswordAsync(string password)
        {
            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentNullException("password");
            }

            var data = new NameValueCollection
            {
                { "reg", SmsSettings.RegisterId },
                { "pwd", SmsSettings.Password },
                { "newpwd", password }
            };

            return await PostAsync(UpdatePasswordUrl, data);
        }

        public async Task<SmsResult> RegisterAsync(string userName, string mobile, string phone, string fax, string email, string postcode, string company, string address)
        {
            var data = new NameValueCollection
            {
                { "reg", SmsSettings.RegisterId },
                { "pwd", SmsSettings.Password },
                { "uname", userName },
                { "mobile", mobile },
                { "phone", phone },
                { "fax", fax },
                { "email", email },
                { "postcode", postcode },
                { "company", company },
                { "address", address }
            };

            return await PostAsync(RegisterUrl, data);
        }

        /// <summary>
        /// Account接口（查余额接口）
        /// </summary>
        /// <returns>POST提交之后，用户名密码认证失败返回错误码，成功返回短信剩余条数记录</returns>
        public async Task<SmsResult> QueryAccountAsync()
        {
            var data = new NameValueCollection
            {
                { "reg", SmsSettings.RegisterId },
                { "pwd", SmsSettings.Password },
            };

            return await PostAsync(QueryAccountUrl, data);
        }

        public async Task<SmsResult> QueryReportAsync()
        {
            var data = new NameValueCollection
            {
                { "reg", SmsSettings.RegisterId },
                { "pwd", SmsSettings.Password }
            };

            return await PostAsync(QueryReportUrl, data);
        }

        private async Task<SmsResult> PostAsync(string url, NameValueCollection data)
        {
            if (string.IsNullOrEmpty(url))
            {
                throw new ArgumentNullException("url");
            }

            if (data == null || data.Count == 0)
            {
                throw new ArgumentNullException("data");
            }

            var client = new WebClient { Encoding = Encoding.UTF8 };
            client.Headers.Add("Content-Type", "application/x-www-form-urlencoded");
            byte[] responsebytes = await client.UploadValuesTaskAsync(url, "POST", data);
            var responsebody = Encoding.UTF8.GetString(responsebytes);
            var result = new SmsResult(responsebody);
            return result;
        }
    }
}
