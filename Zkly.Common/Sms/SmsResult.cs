using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web;

namespace Zkly.Common.SMS
{
    public class SmsResult
    {
        private static readonly IDictionary<string, string> Map = new Dictionary<string, string>
        {
            { "0", "成功" },
            { "500", "程序异常" },
            { "-1", "参数格式错误" },
            { "-2", "注册号或者密码错误" }, 
            { "-3", "密码连续三次错误" }, 
            { "-4", "管理员名称或密码错误" }, 
            { "-5", "Unknown Error" }, 
            { "-6", "没有输入注册号" }, 
            { "-7", "没有输入密码" }, 
            { "-8", "没有输入手机号" }, 
            { "-9", "没有输入短信内容" }, 
            { "-10", "短信内容超长，大于560个字符" }, 
            { "-11", "余额不足" }, 
            { "-12", "短信内容中有非法词" }, 
            { "-13", "短信包中的手机号码数量超过1000条" }, 
            { "-14", "定时时间不能为空" }, 
            { "-15", "非本平台的注册号" }, 
            { "-16", "注册号不可用" }, 
            { "-17", "充值金额必须为数字" }, 
            { "-18", "扣费金额必须为数字" }, 
            { "-19", "线程过多，大于4" }
        };

        private readonly NameValueCollection _data;

        internal SmsResult(string queryString)
        {
            if (string.IsNullOrEmpty(queryString))
            {
                throw new ArgumentNullException("queryString");
            }

            _data = HttpUtility.ParseQueryString(queryString);
        }

        public bool Success
        {
            get { return Code == "0"; }
        }

        public string ErrorMessage
        {
            get
            {
                if (Success)
                {
                    return null;
                }

                string msg;
                return Map.TryGetValue(Code, out msg) ? msg : "未知代码";
            }
        }

        public string Code
        {
            get { return _data["result"]; }
        }

        public string Message
        {
            get { return _data["message"] ?? "成功"; }
        }

        /// <summary>
        /// 短信剩余条数记
        /// </summary>
        public string Balance
        {
            get { return _data["balance"]; }
        }

        /// <summary>
        /// 当天提交总数
        /// </summary>
        public string DailySubmited
        {
            get { return _data["total"]; }
        }

        /// <summary>
        /// 等待发送数量
        /// </summary>
        public string WaitNumber
        {
            get { return _data["waitnum"]; }
        }

        /// <summary>
        /// 正在发送数量
        /// </summary>
        public string SendingNumber
        {
            get { return _data["sendingnum"]; }
        }

        /// <summary>
        /// 成功数量
        /// </summary>
        public string SuccessNumber
        {
            get { return _data["sucessnum"]; }
        }

        /// <summary>
        /// 失败数量
        /// </summary>
        public string FailNumber
        {
            get { return _data["failnum"]; }
        }

        /// <summary>
        /// 短信包ID
        /// </summary>
        public string SmsId
        {
            get { return _data["smsid"]; }
        }
    }
}