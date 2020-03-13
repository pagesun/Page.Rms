using System;
using NUnit.Framework;

namespace Page.Rms.Test
{
    public class Tests
    {
        [SetUp]
        public void Setup()
        {
        }

        [Test]
        public void NoticeAsync()
        {
            //var rms=new Rms("http://www.rms110.com/api-source?project_code=M1708008", "M1708008", "WDsnRmhD2JhLbFlIDcss");

            


            //RmsNotice data = new RmsNotice
            //{
            //    Content = new Content("≤‚ ‘ «Î∫ˆ¬‘£°"),
            //    ErrorCode= "AR1002",
            //    IsTest=0,
            //    Level=2,
            //    NoticeTimeStamp= GetTimestamp(),
            //    PointCode= "SJC52606",
            //    ServerIp="10.35.5.32",
            //    ServerName= "ÀÔ≈ÙæŸ",
            //    ServiceType= "Page"
            //};
            //var res= rms.NoticeAsync(data).Result;
            Assert.Pass();
        }

        [Test]
        public void RestoreAsync()
        {
            //var rms = new Rms("http://www.rms110.com/api-notice?project_code=M1708008", "M1708008", "WDsnRmhD2JhLbFlIDcss");

            //var data = new RmsRestore
            //{
            //    Content = new Content("≤‚ ‘ «Î∫ˆ¬‘£°"),
            //    ErrorCode = "AR1002",
            //    PointCode = "SJC52606",
            //    ServerIp = "10.35.5.32",
            //    ServerName = "ÀÔ≈ÙæŸ",
            //    NoticeTime=DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"),
            //    ResultStatus=0,
            //    ResultExtra="auto"
            //};
            //var res = rms.RestoreAsync(data).Result;
            Assert.Pass();
        }

        private  long GetTimestamp()
        {
            var ts = DateTime.Now - new DateTime(1970, 1, 1, 8, 0, 0, 0);
            return Convert.ToInt64(ts.TotalSeconds);
        }
    }
}