using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace SchoolTakeout.Model.FormatModel
{
    public class WeChatRequestModel
    {
        public string signature { get; set; }
        public string timestamp { get; set; }
        public string nonce { get; set; }

        public string echostr { get; set; }
    }

}
