using System;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Page.Rms
{
    /// <summary>
    /// RMS监控服务
    /// </summary>
    public class Rms
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly RmsOptions _options;

        /// <summary>
        ///     RMS监控
        /// </summary>
        /// <param name="options"></param>
        /// <param name="clientFactory"></param>
        public Rms(RmsOptions options, IHttpClientFactory clientFactory)
        {
            _options = options ?? throw new ArgumentNullException(nameof(options));
            _clientFactory = clientFactory ?? throw new ArgumentNullException(nameof(clientFactory));
        }

        /// <summary>
        ///     发送告警信息
        /// </summary>
        /// <param name="noticeMessage"></param>
        /// <returns></returns>
        public async Task<string> NoticeAsync(RmsNotice noticeMessage)
        {
            Check(noticeMessage);
            return await RmsRequest(noticeMessage, _options.NoticeUrl);
        }

        /// <summary>
        ///     发送告警信息
        /// </summary>
        /// <param name="pointCode">监控点简码。通过监控点管理->监控点列表->监控点简码</param>
        /// <param name="errorCode">错误简码。通过监控点管理->监控点列表->监控点简码->查看</param>
        /// <param name="level">级别(自定义的，0至6之间，数字越小级别越高)</param>
        /// <param name="content">内容,可以是数组也可以是字符串，目前暂定数组，内容4万个字符以内</param>
        /// <returns></returns>
        public async Task<string> NoticeAsync(string pointCode, string errorCode, int level, string content)
        {
            var notice = new RmsNotice
            {
                Content = new Content(content),
                ErrorCode = errorCode,
                Level = level,
                PointCode = pointCode,
                ServiceType = _options.ProjectName
            };

            Check(notice);
            return await RmsRequest(notice, _options.NoticeUrl);
        }

        /// <summary>
        ///     自动恢复告警
        /// </summary>
        /// <param name="restoreMessage"></param>
        /// <returns></returns>
        public async Task<string> RestoreAsync(RmsRestore restoreMessage)
        {
            Check(restoreMessage);
            return await RmsRequest(restoreMessage, _options.RestoreUrl);
        }

        /// <summary>
        ///     自动恢复告警
        /// </summary>
        /// <param name="pointCode">监控点简码。通过监控点管理->监控点列表->监控点简码</param>
        /// <param name="errorCode">错误简码。通过监控点管理->监控点列表->监控点简码->查看</param>
        /// <param name="resultStatus">级别(自定义的，0至6之间，数字越小级别越高)</param>
        /// <param name="content">内容,可以是数组也可以是字符串，目前暂定数组，内容4万个字符以内</param>
        /// <returns></returns>
        public async Task<string> RestoreAsync(string pointCode, string errorCode, int resultStatus, string content)
        {
            var restore = new RmsRestore
            {
                Content = new Content(content),
                ErrorCode = errorCode,
                PointCode = pointCode,
                ResultStatus = resultStatus
            };
            Check(restore);
            return await RmsRequest(restore, _options.RestoreUrl);
        }

        /// <summary>
        ///     请求RMS API
        /// </summary>
        /// <param name="message"></param>
        /// <param name="requestUrl"></param>
        /// <returns></returns>
        private async Task<string> RmsRequest(RmsData message, string requestUrl)
        {
            var source = GetRequestParameter(message);
            var strData = JsonConvert.SerializeObject(source);

            var stringContent = new StringContent(strData, Encoding.UTF8, "application/json");

            var client = _clientFactory.CreateClient();
            var response = client.PostAsync(requestUrl, stringContent);

            return await response.Result.Content.ReadAsStringAsync();
        }

        /// <summary>
        ///     构造请求参数
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private RmsRequestData GetRequestParameter(RmsData data)
        {
            var str = JsonConvert.SerializeObject(data);
            return new RmsRequestData
            {
                Token = CreateToken(str),
                Data = Convert.ToBase64String(Encoding.UTF8.GetBytes(str))
            };
        }

        /// <summary>
        ///     计算token
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        private string CreateToken(string data)
        {
            var inputValue = $"{_options.Key}{data}";
            using (var md5 = MD5.Create())
            {
                var bytesHash = md5.ComputeHash(Encoding.UTF8.GetBytes(inputValue));
                var strResult = BitConverter.ToString(bytesHash);
                return strResult.Replace("-", "").ToLower();
            }
        }


        #region 完善本机信息

        /// <summary>
        ///     补全服务器信息
        /// </summary>
        /// <param name="notice"></param>
        private void Check(RmsNotice notice)
        {
            if (notice.NoticeTimeStamp <= 0)
                notice.NoticeTimeStamp = GetTimesTamp();
            if (string.IsNullOrEmpty(notice.ServerName))
                notice.ServerName = GetServerName();
            if (string.IsNullOrEmpty(notice.ServerIp))
                notice.ServerIp = GetLocalAddress();
        }

        /// <summary>
        ///     补全服务器信息
        /// </summary>
        /// <param name="notice"></param>
        private void Check(RmsRestore notice)
        {
            if (string.IsNullOrEmpty(notice.NoticeTime))
                notice.NoticeTime = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
            if (string.IsNullOrEmpty(notice.ServerName))
                notice.ServerName = GetServerName();
            if (string.IsNullOrEmpty(notice.ServerIp))
                notice.ServerIp = GetLocalAddress();
        }

        /// <summary>
        ///     获取时间戳
        /// </summary>
        /// <returns></returns>
        private long GetTimesTamp()
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }

        /// <summary>
        ///     获取本地ip地址
        /// </summary>
        /// <returns></returns>
        private string GetLocalAddress()
        {
            var address = Dns.GetHostEntry(Dns.GetHostName()).AddressList.FirstOrDefault(f =>
                f.AddressFamily == AddressFamily.InterNetwork);
            return address == null ? "" : address.ToString();
        }

        /// <summary>
        ///     获取服务器名
        /// </summary>
        /// <returns></returns>
        public string GetServerName()
        {
            return Environment.MachineName;
        }

        #endregion
    }
}