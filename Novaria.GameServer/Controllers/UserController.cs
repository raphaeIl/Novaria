using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace Novaria.GameServer.Controllers
{
    [ApiController]
    [Route("/user")]
    public class UserController : ControllerBase
    {
        [Route("set")]
        public IResult PostSet()
        {
            Log.Information("post set received!");

            string jsonResponse = @"
{
    ""Code"": 200,
    ""Data"": {},
    ""Msg"": ""OK""
}
";
            return Results.Text(jsonResponse, "application/json");
        }

        [Route("login")]
        public IResult PostLogin()
        {
            Log.Information("post login received!");

            string jsonResponse = @"
{
    ""Code"": 200,
    ""Data"": {
        ""AgeVerifyMethod"": 0,
        ""IsNew"": 1,
        ""UserInfo"": {
            ""ID"": ""69"",
            ""UID2"": 0,
            ""PID"": ""US-NOVA-TEST"",
            ""Token"": ""seggs"",
            ""Birthday"": """",
            ""RegChannel"": ""pc"",
            ""TransCode"": """",
            ""State"": 1,
            ""DeviceID"": """",
            ""CreatedAt"": 1748359959
        },
        ""Yostar"": {
            ""ID"": ""Y69"",
            ""Country"": ""US"",
            ""Nickname"": """",
            ""Picture"": """",
            ""State"": 1,
            ""AgreeAd"": 0,
            ""CreatedAt"": 1654600174
        }
    },
    ""Msg"": ""OK""
}
";
            return Results.Text(jsonResponse, "application/json");
        }

        [Route("quick-login")]
        public IResult PostQuickLogin()
        {
            Log.Information("post login received!");

            string jsonResponse = @"
{
    ""Code"": 200,
    ""Data"": {
        ""AgeVerifyMethod"": 0,
        ""Destroy"": null,
        ""IsTestAccount"": false,
        ""Keys"": [
            {
                ""ID"": ""69"",
                ""Type"": ""yostar"",
                ""Key"": ""seggs@gmail.com"",
                ""NickName"": ""seggs@gmail.com"",
                ""CreatedAt"": 1654600175
            }
        ],
        ""ServerNowAt"": 1748359960,
        ""UserInfo"": {
            ""ID"": ""69"",
            ""UID2"": 0,
            ""PID"": ""US-NOVA-TEST"",
            ""Token"": ""seggs"",
            ""Birthday"": """",
            ""RegChannel"": ""pc"",
            ""TransCode"": """",
            ""State"": 1,
            ""DeviceID"": """",
            ""CreatedAt"": 1748359959
        },
        ""Yostar"": {
            ""ID"": ""Y69"",
            ""Country"": ""US"",
            ""Nickname"": """",
            ""Picture"": """",
            ""State"": 1,
            ""AgreeAd"": 0,
            ""CreatedAt"": 1654600174
        },
        ""YostarDestroy"": null

    },
    ""Msg"": ""OK""
}
";
            return Results.Text(jsonResponse, "application/json");

        }

        [Route("detail")]
        public IResult PostDetail()
        {
            string jsonResponse = @"
{
    ""Code"": 200,
    ""Data"": {
        ""AgeVerifyMethod"": 0,
        ""Destroy"": null,
        ""IsTestAccount"": false,
        ""Keys"": [
            {
                ""ID"": ""69"",
                ""Type"": ""yostar"",
                ""Key"": ""seggs@gmail.com"",
                ""NickName"": ""seggs@gmail.com"",
                ""CreatedAt"": 1654600175
            }
        ],
        ""ServerNowAt"": 1748359960,
        ""UserInfo"": {
            ""ID"": ""69"",
            ""UID2"": 0,
            ""PID"": ""US-NOVA-TEST"",
            ""Token"": ""seggs"",
            ""Birthday"": """",
            ""RegChannel"": ""pc"",
            ""TransCode"": """",
            ""State"": 1,
            ""DeviceID"": """",
            ""CreatedAt"": 1748359959
        },
        ""Yostar"": {
            ""ID"": ""Y69"",
            ""Country"": ""US"",
            ""Nickname"": """",
            ""Picture"": """",
            ""State"": 1,
            ""AgreeAd"": 0,
            ""CreatedAt"": 1654600174
        },
        ""YostarDestroy"": null
    },
    ""Msg"": ""OK""
}
";

            return Results.Text(jsonResponse, "application/json");
        }


    }
}
