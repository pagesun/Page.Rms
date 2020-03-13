using Newtonsoft.Json;

namespace Page.Rms
{
    /// <summary>
    /// RMS API请求参数
    /// </summary>
    public class RmsRequestData
    {
        /// <summary>
        ///     访问令牌(key+json格式化的data后进行md5加密)key由rms系统提供
        /// </summary>
        [JsonProperty("token")]
        public string Token { get; set; }

        /// <summary>
        ///     是否进行base64编码
        /// </summary>
        [JsonProperty("encode")]
        public int Encode { get; set; } = 1;

        /// <summary>
        ///     请求参数
        /// </summary>
        [JsonProperty("data")]
        public string Data { get; set; }
    }

    /// <summary>
    /// rms api请求参数主体
    /// </summary>
    public abstract class RmsData
    {
        /// <summary>
        ///     监控点简码。通过监控点管理->监控点列表->监控点简码
        /// </summary>
        [JsonProperty("point_code")]
        public string PointCode { get; set; }

        /// <summary>
        ///     错误简码。通过监控点管理->监控点列表->监控点简码->查看
        /// </summary>
        [JsonProperty("error_code")]
        public string ErrorCode { get; set; }

        /// <summary>
        ///     内容,可以是数组也可以是字符串，目前暂定数组，内容4万个字符以内
        /// </summary>
        [JsonProperty("content")]
        public Content Content { get; set; }

        /// <summary>
        ///     服务器唯一标识，目前使用ip地址(如：10.37.1.1),默认值本地ip
        /// </summary>
        [JsonProperty("server_ip")]
        public string ServerIp { get; set; }

        /// <summary>
        ///     服务器标识名,默认值本机名
        /// </summary>
        [JsonProperty("server_name")]
        public string ServerName { get; set; }
    }

    /// <summary>
    /// 告警
    /// </summary>
    public class RmsNotice : RmsData
    {
        /// <summary>
        ///     发生时间(时间戳),默认当前时间
        /// </summary>
        [JsonProperty("notice_time_stamp")]
        public long NoticeTimeStamp { get; set; }

        /// <summary>
        ///     级别(网站自定义的，0至6之间，数字越小级别越高)
        /// </summary>
        [JsonProperty("level")]
        public int Level { get; set; }

        /// <summary>
        ///     是否是自检信息，1是 0否
        /// </summary>
        [JsonProperty("is_test")]
        public int IsTest { get; set; } = 0;

        /// <summary>
        ///     服务组合,运维和DB监控点必填
        /// </summary>
        [JsonProperty("service_type")]
        public string ServiceType { get; set; }
    }

    /// <summary>
    /// 告警恢复
    /// </summary>
    public class RmsRestore : RmsData
    {
        /// <summary>
        ///     发生时间(时间戳),默认当前时间
        /// </summary>
        [JsonProperty("notice_time")]
        public string NoticeTime { get; set; }

        /// <summary>
        ///     错误类型 0 误报 1 代码问题 2 SQL语句问题 3 服务器问题 4网络问题  5 DB服务问题  6 缓存服务问题 9其他问题 10 阈值设置问题
        /// </summary>
        [JsonProperty("result_status")]
        public int ResultStatus { get; set; }

        /// <summary>
        ///     处理方案  最多255个字符
        /// </summary>
        [JsonProperty("result_extra")]
        public string ResultExtra { get; set; } = "机器自动恢复,无需处理";
    }

    /// <summary>
    ///     消息内容
    /// </summary>
    public class Content
    {
        /// <summary>
        /// 消息内容
        /// </summary>
        /// <param name="info"></param>
        public Content(string info)
        {
            //消息内容
            Info = info;
        }

        /// <summary>
        ///     消息内容
        /// </summary>
        [JsonProperty("info")]
        public string Info { get; set; }
    }
}