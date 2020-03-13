using Microsoft.Extensions.Options;

namespace Page.Rms
{
    public class RmsOptions : IOptions<RmsOptions>
    {
        /// <summary>
        ///     项目名称
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        ///     接口密钥
        /// </summary>
        public string Key { get; set; }

        /// <summary>
        ///     项目简码
        /// </summary>
        public string ProjectCode { get; set; }

        /// <summary>
        ///     告警API
        /// </summary>
        public string NoticeUrl { get; set; }

        /// <summary>
        ///     自动恢复API
        /// </summary>
        public string RestoreUrl { get; set; }

        public RmsOptions Value => this;
    }
}