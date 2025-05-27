using Google.Protobuf;
using Novaria.Common.Core;
using Proto;
using Serilog;
using System.Text.Json;
using Novaria.PcapParser;
using Novaria.Common.Crypto;
using Novaria.GameServer.Services;
using Nova.Client;

namespace Novaria.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Login : ProtocolHandlerBase
    {
        public Login(IProtocolHandlerFactory protocolHandlerFactory, TableService tableService) : base(protocolHandlerFactory)
        {
            //table_Achievement achievements = (table_Achievement)tableService.GetTable<Nova.Client.Achievement>();
            table_Character characters = (table_Character)tableService.GetTable<Nova.Client.Character>();

            Log.Information(Newtonsoft.Json.JsonConvert.SerializeObject(characters, Newtonsoft.Json.Formatting.Indented));
        }

        [ProtocolHandler(NetMsgId.player_login_req)] // req id goes here
        public Packet PlayerLoginHandler(LoginReq req)
        {
            //Log.Information("login_req received, contents: " + JsonSerializer.Serialize(req));

            //Log.Information("Building login resp...");

            LoginResp loginResp = new LoginResp()
            {
                Token = "seggstoken",
            };

            //Log.Information("Sending login_resp packet: " + JsonSerializer.Serialize(loginResp));
            return Packet.Create(NetMsgId.player_login_succeed_ack, loginResp);
        }

        [ProtocolHandler(NetMsgId.player_data_req)]
        public Packet PlayerDataHandler(Nil req)
        {
            // example: different netmsgid returned, if new player player_new_notify, other wise player_data_ack
        
            PlayerInfo pcapPlayerInfo = (PlayerInfo)PcapParser.PcapParser.Instance.GetPcapPacket(NetMsgId.player_data_succeed_ack);

            AccInfo accountInfo = new AccInfo()
            {
                Id = 1,
                Hashtag = 4562,
                HeadIcon = 100101,
                NickName = "ArkanDash",
                Gender = false,
                Signature = "",
                TitlePrefix = 1,
                TitleSuffix = 2,
                SkinId = 10301,
                CreateTime = pcapPlayerInfo.Acc.CreateTime,
            };
                
            accountInfo.Newbies.Add(pcapPlayerInfo.Acc.Newbies);
            accountInfo.NextPackage = pcapPlayerInfo.Acc.NextPackage;

            //accountInfo.Newbies.Add(new NewbieInfo() { GroupId = 101, StepId = -1 });
            //accountInfo.Newbies.Add(new NewbieInfo() { GroupId = 102, StepId = -1 });

            PlayerInfo playerInfoResponse = new PlayerInfo()
            {
                Acc = accountInfo
            };
            //playerInfoResponse.Chars.AddRange(
                //[
                //    new Proto.Char()
                //    {
                //        Tid = 111,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11101,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[0].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 117,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11701,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[1].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 108,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 10801,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[2].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 123,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 12301,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[3].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 127,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 12701,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[4].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 107,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 10701,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[5].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 132,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 13201,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[6].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 135,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 13501,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[7].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 126,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 12601,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[8].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 118,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11801,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[9].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 142,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 14201,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[10].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 119,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11901,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[11].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 103,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 10301,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[12].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 125,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 12501,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[13].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 120,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 12001,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[14].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 112,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11201,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[15].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //    new Proto.Char()
                //    {
                //        Tid = 113,
                //        Exp = 0,
                //        DatingLandmarkIds = [],
                //        DatingEventIds = [],
                //        DatingEventRewardIds = [],
                //        EquipmentIds = [
                //            0,
                //            0,
                //            0
                //        ],
                //        Level = 90,
                //        SkillLvs = [
                //            10,
                //            10,
                //            10,
                //            10,
                //            1
                //        ],
                //        Skin = 11301,
                //        AffinityLevel = 20,
                //        AffinityExp = 0,
                //        Advance = 8,
                //        Plots = [],
                //        AffinityQuests = pcapPlayerInfo.Chars[16].AffinityQuests,
                //        TalentNodes = ByteString.Empty,
                //        CreateTime = 1736745112,
                //        NextPackage = ByteString.Empty
                //    },
                //]);
            playerInfoResponse.Res.AddRange(pcapPlayerInfo.Res);
            playerInfoResponse.Items.AddRange(pcapPlayerInfo.Items);

            playerInfoResponse.Res.Where(i => i.Tid == 2).SingleOrDefault().Qty = 777777771; // gacha stone

            playerInfoResponse.Formation = pcapPlayerInfo.Formation;
            //playerInfoResponse.StarTowerRankTicket = 3;
            playerInfoResponse.Energy = pcapPlayerInfo.Energy;
            playerInfoResponse.WorldClass = pcapPlayerInfo.WorldClass;
            playerInfoResponse.Agent = pcapPlayerInfo.Agent;
            playerInfoResponse.RglPassedIds.AddRange(pcapPlayerInfo.RglPassedIds);
            playerInfoResponse.Equipments.AddRange(pcapPlayerInfo.Equipments);
            playerInfoResponse.RegionBossLevels.AddRange(pcapPlayerInfo.RegionBossLevels);
            playerInfoResponse.Quests = pcapPlayerInfo.Quests;
            playerInfoResponse.State = pcapPlayerInfo.State;
            playerInfoResponse.SendGiftCnt = 0;
            playerInfoResponse.Board.AddRange(pcapPlayerInfo.Board);
            playerInfoResponse.DatingCharIds.AddRange(pcapPlayerInfo.DatingCharIds);
            playerInfoResponse.Achievements = pcapPlayerInfo.Achievements;
            playerInfoResponse.Handbook.AddRange(pcapPlayerInfo.Handbook);
            playerInfoResponse.SigninIndex = 0;
            playerInfoResponse.Titles.AddRange(pcapPlayerInfo.Titles);
            playerInfoResponse.DailyInstances.AddRange(pcapPlayerInfo.DailyInstances);
            playerInfoResponse.Dictionaries.AddRange(pcapPlayerInfo.Dictionaries);
            playerInfoResponse.Activities.AddRange(pcapPlayerInfo.Activities);
            playerInfoResponse.Phone = pcapPlayerInfo.Phone;
            playerInfoResponse.TalentResetTime = 0;
            playerInfoResponse.EquipmentDoubleCount = 0;
            playerInfoResponse.Discs.AddRange(pcapPlayerInfo.Discs);
            playerInfoResponse.Story = pcapPlayerInfo.Story;
            playerInfoResponse.VampireSurvivorRecord = pcapPlayerInfo.VampireSurvivorRecord;
            playerInfoResponse.DailyActiveIds.AddRange(pcapPlayerInfo.DailyActiveIds);
            playerInfoResponse.TourGuideQuestGroup = 0; 
            playerInfoResponse.HonorList.AddRange(pcapPlayerInfo.HonorList); 
            playerInfoResponse.Honors.AddRange(pcapPlayerInfo.Honors); 
            playerInfoResponse.DailyShopRewardStatus = true; 
            playerInfoResponse.TowerTicket = 0; 
            playerInfoResponse.ServerTs = pcapPlayerInfo.ServerTs; 
            playerInfoResponse.NextPackage = pcapPlayerInfo.NextPackage;

            Log.Information("Sending player_new_notify packet =  " + JsonSerializer.Serialize(playerInfoResponse));
            return Packet.Create(NetMsgId.player_data_succeed_ack, playerInfoResponse);
        }

        [ProtocolHandler(NetMsgId.player_ping_req)]
        public Packet PlayerRegHandler(Ping req)
        {
            return Packet.Create(NetMsgId.player_ping_succeed_ack, new Pong()
            {
                ServerTs = 1736745112,
            });
        }

        [ProtocolHandler(NetMsgId.player_learn_req)]
        public Packet PlayerLearnHandler(NewbieInfo newbieInfo)
        {
            return Packet.Create(NetMsgId.player_learn_succeed_ack, new Nil()
            {

            });
        }

    }
}
