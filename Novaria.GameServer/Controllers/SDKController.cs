using Microsoft.AspNetCore.Mvc;
using Serilog;

// rest of the random as packets during sdk
namespace Novaria.GameServer.Controllers
{
    [ApiController]
    [Route("/")]
    public class SDKController : ControllerBase
    {
        [HttpPost("yostar/send-code")]
        public IResult SendCode()
        {
            return Results.Content(
@"
{
    ""Code"": 200,
    ""Data"": {},
    ""Msg"": ""OK""
}
"
        , "application/json");
        }

        [HttpPost("yostar/get-auth")]
        public IResult GetAuth()
        {
            return Results.Content(
@"
{
    ""Code"": 200,
    ""Data"": {
        ""Account"": ""seggs@gmail.com"",
        ""Token"": ""sdfsdfsdfsdf"",
        ""UID"": ""seggs@gmail.com""
    },
    ""Msg"": ""OK""
}
"
        , "application/json");
        }


        [HttpPost("user/oauth2/v2/grant")]
        public IResult OAuthGrant()
        {
            return Results.Content(
@"
{
    ""msg"": ""sss"",
    ""status"": 106,
    ""type"": ""A""
}",
"application/json"
);
        }

        [Route("")]
        public IResult GetNothing()
        {
            return Results.Text(@"
{
    ""Code"": 200,
    ""Data"": ""Hello world"",
    ""Msg"": ""OK""
}
");
        }

        [Route("health-game/identity-auth")]
        public IResult PostIdentityAuth()
        {
            Log.Information("post login received!");

            string jsonResponse = @"
{
    ""Code"": 200,
    ""Data"": {
        ""Identity"": {
            ""BirthDate"": """",
            ""IDCard"": ""123*********34567*"",
            ""PI"": """",
            ""RealName"": ""**"",
            ""State"": 1,
            ""Type"": 0,
            ""Underage"": false
        }
    },
    ""Msg"": ""OK""
}
";
            return Results.Text(jsonResponse, "application/json");
        }

        [Route("common/config")]
        public IResult PostCommonConfig()
        {
            return Results.Text(@"
{
    ""Code"": 200,
    ""Data"": {
        ""AppConfig"": {
            ""AppropriateAge"": {
                ""Level"": ""18+"",
                ""Msg"": ""《星塔旅人》游戏适龄提示\n1、本游戏是一款玩法简单的角色扮演游戏，适用于年满18周岁及以上的用户。\n2、本游戏基于架空的故事背景和幻想世界观，剧情简单且积极向上，没有基于真实历史和现实事件的改编内容。游戏玩法基于肢体操作，鼓励玩家通过训练达成目标。游戏中有少量自定义文字系统，该社交系统遵循相关法律法规进行管理。\n3、本游戏中有用户实名认证系统，对年满18周岁及以上的用户开放，使用18周岁以下的身份信息认证账号将无法进入游戏。""
            },
            ""Captcha"": {
                ""AppID"": 191947906,
                ""Enable"": true
            },
            ""DestroyUser"": {
                ""Days"": 15,
                ""Enable"": true
            },
            ""DetectionAddress"": {
                ""Auto"": {
                    ""DNS"": null,
                    ""HTTP"": [
                        ""https://static-stellasora.yostar.net"",
                        ""https://nova.yostar.cn"",
                        ""https://sdk-api.yostar.cn""
                    ],
                    ""MTR"": [
                        ""https://static-stellasora.yostar.net"",
                        ""https://nova.yostar.cn"",
                        ""https://sdk-api.yostar.cn""
                    ],
                    ""PING"": null,
                    ""TCP"": null
                },
                ""Enable"": true,
                ""Internet"": ""https://www.baidu.com""
            },
            ""EnableTextReview"": true,
            ""NicknameReg"": ""^[A-Za-z0-9一-龥]{2,16}$"",
            ""Passport"": {
                ""DestroyDays"": 0,
                ""ModifyEmailDays"": 30,
                ""ModifyMobileDays"": 30,
                ""Prefix"": ""YS""
            },
            ""PassportPopup"": {
                ""Enable"": false,
                ""Text"": """"
            },
            ""SLS"": {
                ""AccessKeyID"": ""7b5d0ffd0943f26704fc547a871c68b1b5d56b5c9caeb354205b81f445d7af59"",
                ""AccessKeySecret"": ""4a5e9cc8a50819290c9bfa1fedc79da7c50e85189a05eb462a3d28a7688eabb0"",
                ""ENABLE"": true
            },
            ""Share"": {
                ""CaptureScreen"": {
                    ""AutoCloseDelay"": 0,
                    ""Enabled"": false
                },
                ""PengYouQuan"": {
                    ""AppID"": """",
                    ""Enabled"": false,
                    ""UniversalLink"": """"
                },
                ""QQ"": {
                    ""AppID"": """",
                    ""Enabled"": false,
                    ""UniversalLink"": """"
                },
                ""Qzone"": {
                    ""AppID"": """",
                    ""Enabled"": false,
                    ""UniversalLink"": """"
                },
                ""Sort"": null,
                ""WeiXin"": {
                    ""AppID"": """",
                    ""Enabled"": false,
                    ""UniversalLink"": """"
                },
                ""Weibo"": {
                    ""AppKey"": """",
                    ""Enabled"": false,
                    ""RedirectURL"": """",
                    ""UniversalLink"": """"
                }
            },
            ""ThirdInfoShareList"": ""https://account.yostar.cn/cn-nova/shared_list"",
            ""Version"": {
                ""ChildPrivacyAgreement"": ""0.1"",
                ""ErrorCode"": ""4.3"",
                ""PrivacyAgreement"": ""0.1"",
                ""UserAgreement"": ""0.1"",
                ""UserDestroy"": ""0.1""
            },
            ""WechatAppID"": """"
        },
        ""ChannelConfig"": {
            ""Adjust"": {
                ""AppID"": ""tdzg1orirlkw"",
                ""Debug"": false,
                ""Enable"": true,
                ""EventTokens"": {
                    ""#app_crash"": ""z9mk0z"",
                    ""#app_second_retention"": ""joj939"",
                    ""#app_seventh_retention"": ""cccp5a"",
                    ""#asa_attribution"": ""c9w7x5"",
                    ""#overwrite_install"": ""nnlh0v"",
                    ""chapter0_1_complete"": ""i0b43c"",
                    ""chapter0_2_complete"": ""iby4fp"",
                    ""daily_mission_complete"": ""e57suj"",
                    ""newbie_tutorial"": ""fx0u1h"",
                    ""normal_tutorial"": ""1vebid"",
                    ""role_create"": ""xiwizs"",
                    ""role_login"": ""msxnvi"",
                    ""setting_choice"": ""tdiya8"",
                    ""ysdk_account_bind"": ""yimfis"",
                    ""ysdk_account_create"": ""yimfis"",
                    ""ysdk_clear_cache"": ""blu5v6"",
                    ""ysdk_del_account"": ""dlbhch"",
                    ""ysdk_del_account_intention"": ""zakuij"",
                    ""ysdk_err"": ""qo4nog"",
                    ""ysdk_init"": ""v32wb9"",
                    ""ysdk_pay_illegal_currency"": ""jz0jhj"",
                    ""ysdk_role_info_upload"": ""vr4gzs"",
                    ""ysdk_user_login"": ""psim9y"",
                    ""ysdk_user_open_review"": ""htiqns"",
                    ""ysdk_user_open_survey"": ""wvbzk1"",
                    ""ysdk_user_paid_level"": ""p52k29"",
                    ""ysdk_user_pay_canceled"": ""qnta18"",
                    ""ysdk_user_pay_checkout"": ""8x4g6e"",
                    ""ysdk_user_pay_failed"": ""n99xq6"",
                    ""ysdk_user_pay_intention"": ""ockxw2"",
                    ""ysdk_user_share"": ""2tbkrf"",
                    ""ysdk_user_share_intention"": ""gh5qc8"",
                    ""ysdk_user_switch_account"": ""e23y7q""
                }
            },
            ""AiHelp"": {
                ""DisplayType"": ""Browser"",
                ""ServiceInterfaceURL"": """",
                ""ServiceURL"": ""https://account.yostar.cn/contact""
            },
            ""Debug"": 0,
            ""JPush"": {
                ""Debug"": false,
                ""Key"": """"
            },
            ""Login"": {
                ""Default"": ""onekey"",
                ""EnableList"": [
                    ""mobile"",
                    ""onekey""
                ]
            },
            ""OneKeyLoginSecret"": ""2c6d738eaea2a59ac405bfbd484f7aeac770ff4ed89a5a03d6493d84b2dc265ec5364dbf270e11980252c80ed5de5dfd8fda1c0ad4fc1cb2c8c08b41083bea50033925af6c4e833eaa7ede488ae974ef829e9bd08d19b6f42399d597485d8b2fb7d7ebbd73c79045ff6f7bf141b7a3322944916fa00401a522862e554f414658189ed0e843e4d6a24b0a6384932d5058c8106f54599b2679674fd542aaba0f63b09710a5757d03d79bcbcc49ee523198ca6d136ea5129d98f9345039385b8744452a089b5341cb85a3dd21396e4f4ac5808ea75f07c6134c3632947adb1f643508090aa6ac418b4f714834685cda9f40718234272b6bd7d1617a304bb47601f82c690ea0dff12cf24cccab982a45557452b7096f422862a506cbfc6cb0024a1a0367e387ab58d27e87af47eb1c6d8a9bdf377a00afe40282ac58e4a4c4b90d489a5c4590fe6f78ed5ffbaa67efa00e98"",
            ""Udata"": {
                ""Enable"": true,
                ""MSUAKey"": ""-----BEGIN CERTIFICATE-----\nMIIFmzCCA4OgAwIBAgIDAP1zMA0GCSqGSIb3DQEBCwUAMIGAMQswCQYDVQQGEwJD\nTjEQMA4GA1UECAwHQmVpamluZzEMMAoGA1UECgwDTVNBMREwDwYDVQQLDAhPQUlE\nX1NESzEeMBwGA1UEAwwVY29tLmJ1bi5taWl0bWRpZC5zaWduMR4wHAYJKoZIhvcN\nAQkBFg9tc2FAY2FpY3QuYWMuY24wHhcNMjQxMTEyMTY1NDA2WhcNMjUxMTEzMTY1\nNDA2WjCBjDELMAkGA1UEBhMCQ04xEDAOBgNVBAgMB0JlaWppbmcxEDAOBgNVBAcM\nB0JlaWppbmcxDzANBgNVBAoMBnlvc3RhcjEjMCEGA1UEAwwaY29tLlJvYW1pbmdT\ndGFyLlN0ZWxsYVNvcmExIzAhBgkqhkiG9w0BCQEWFHNvbmcubGl1QHlvLXN0YXIu\nY29tMIICIjANBgkqhkiG9w0BAQEFAAOCAg8AMIICCgKCAgEAshgvU9xluAUrL51T\nam/Ph5Z4yQEL0z3ex9d5tX/eOblLFGGDGmmbiAmI2+Knn2VMkIMsQVE2HwTk6RO3\nHHXDaGYAqdOUU54N9M75HOKY2sCSJDXiAtjRpTe0arjuCmv9Oi0rquTfm3y+/BND\nMGQIp6MUrMH/8jWOUdSydm9yOwZ1u61Um+ymwdEOM+SpridOIT1ZewbUnX1Z750V\nUwQ0Zv3DIMrqRJfgghlvB1Ho8zx3R3h0Sta8e37sECPuY4DJg2ETV2Q+AJidLwpb\nq0EeWrzdsTrsQfB27QfpUIrbDZ0Ar2GNV6eLOxbX5k+icwzUP/70YF8WKzmlRX6G\nP7oKzvkn2bano/pnSkdyoDmgAO+bfM3CXzOlhq2zyZd8R3Nm4Q+JBsEZEwHBye1k\nDiK1s/OQjwnyKGIfJ2ZQZbAjWpiSYej5dpwLZp3zLQ7mGSaP9kCuifyhbcsLGCHp\nFOn4kbCWmrpwX+bA/SMW7yXEUQfh0QrMkimBIPM+15YfdHx4pn1IE9BreOP6Pp8z\n4ewqvQMunVrTMWBce/GdXqWqTiXVDaWViBE/MEB64iZmydfE5YnRkQFvOZFtpGtn\nK+LuhHevCUGvpzfpG0tHQ3rrzgRjrIqqgsuWngNKM/PB0VliIGtNjy02WxLOD3WK\nzcx1giEo55N+CqcMMWCVJ+mactcCAwEAAaMQMA4wDAYDVR0TAQH/BAIwADANBgkq\nhkiG9w0BAQsFAAOCAgEACgZXL1JPsGlr1lVwbLTun3I8nZGAVXr3yhE3kcUYYX/F\n+dgyDlJmC8ZRNzhvAUPcUrsaic8s7JbMuTZzCLdw2fAkQWEs3nuN8zhZ3Lh1o2t9\n6t0V7ZnrahJn3OyY0XFfbLAuDtFWedxeNwSPL0hEVo13luCC/1N8dSZje/lEaTty\ngRTagbkMpHIDryOR5mPEob4UtSLdkcyWWNiuhvNFhY7TwXbPH6Mu/I9V79wwIkrw\nb45SdFgWYDPaNbBhWnvmKzXOZP1A+6x7wHonbvWf3oEPL+d+xyUXMpAEVnCpQS+A\nUgst9HuQcgEbyi6waQ2I21wkXJ4ajMM0TeHUfJDI/S/1mur8ijFQ6BToPUwPZ158\nxQNaeQalPNHiFuCuJvQlbkrlJVZPU0VKo+tTh2xxD0h7CyqwERV05h3RyS5auDf2\nQHvijp55H9mej8/K79rvwlMLvNI93FCjnWCgmy5/4AiFsm8R+gnEfVSpiSOzKYqS\ntLZ1HBaHFIgvAvW0dN1Qz8PXnYMRt7N6/fg0ZeXRiUbmGwuTxdfCBP5OyyWpIqtN\n91cpzIH8lYx19oEV5fNWMBbUjbZeQNmcSaz2NSKoM3PpFBkwW/bedqeUCuhj/uT+\nYNjvej3TMTXAi9Ln4eajGBIkpZlmtbnPAA4L4Kq2BedHbebKDVg+Ga5Eyy6Q/Ng=\n-----END CERTIFICATE-----""
            }
        }
    },
    ""Msg"": ""OK""
}
");
        }

        [Route("heartbeat/pulse")]
        public IResult PostsHeartbeatPulse()
        {
            return Results.Text(@"
{
    ""Code"": 200,
    ""Data"": {
        ""Delay"": 3600
    },
    ""Msg"": ""OK""
}
");
        }

        [Route("common/geo-ip")]
        public IResult PostGeoIP()
        {
            return Results.Text(@"
{
    ""Code"": 200,
    ""Data"": {
        ""Prov"": ""悠星总部""
    },
    ""Msg"": ""OK""
}
");
        }

        [HttpPost("daq/track/v2")]
        public IResult PostDaqTrackV2()
        {
            return Results.Text(@"
{
    ""code"": 200,
    ""data"": {
        ""ts"": 1736783123685
    },
    ""msg"": ""OK""
}
");
        }

        [HttpGet("daq/init/v2")]
        public IResult GetDaqInitV2([FromQuery] int os, [FromQuery] string? code = null)
        {
            Console.WriteLine("os", os);
            return Results.Text(@"
{
    ""code"": 200,
    ""data"": {
        ""ts"": 1736783033783
    },
    ""msg"": ""OK""
}
");
        }

        [Route("common/client-code")]
        public IResult PostClientCode()
        {
            return Results.Text(@"
{
    ""Code"": 200,
    ""Msg"": ""OK""
}
");
        }


    }
}
