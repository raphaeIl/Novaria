using Novaria.Common.Core;
using Novaria.Common.Crypto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Novaria.Common.Util;
using Google.Protobuf;
using System.Reflection;
using Proto;

namespace Novaria.PcapParser
{
    public class PcapParser : Singleton<PcapParser>
    {
        public int totalPacketsCount = 0;
        public List<NovaPacket> packets = new List<NovaPacket>();

        public void LoadAllPackets()
        {
            byte[] real_key = AeadTool.key3;
            ClientType real_client_type = AeadTool.clientType;

            // load from pcap
            AeadTool.clientType = ClientType.Mobile; // my pcap were from mobile so gotta switch it over
            AeadTool.InitAeadTool();

            // these are all the packets i have, should've done more but ran out of time :/
            PcapParser.Instance.Parse("all_mainmenu_packets.json");
            PcapParser.Instance.Parse("accidently_too_many.json");
            PcapParser.Instance.Parse("create_acc_packets.json");
            PcapParser.Instance.Parse("first_instant_join.json");
            PcapParser.Instance.Parse("full_everything.json");
            PcapParser.Instance.Parse("lvl30_tutorials.json");

            // after pcap parse switch it back
            AeadTool.clientType = real_client_type;
            AeadTool.InitAeadTool();
            AeadTool.key3 = real_key;
        }

        public void Parse(string pcapFileName, bool auto_key = true)
        {
            //string pcapJsonFile = File.ReadAllText($"E:\\documents\\Decompiling\\Extracted\\NOVA\\Novaria\\Novaria.PcapParser\\{pcapFileName}"); // disgusting pathing, but "not hardcoded" now ig
            string basePath = AppContext.BaseDirectory;
            string projectRoot = Path.GetFullPath(Path.Combine(basePath, @"..\..\..\..\"));
            string pcapPath = Path.Combine(projectRoot, "Novaria.PcapParser", pcapFileName);
            string pcapJsonFile = File.ReadAllText(pcapPath);
            var data = System.Text.Json.JsonSerializer.Deserialize<List<PcapPacket>>(pcapJsonFile);

            foreach (PcapPacket packet in data)
            {
                totalPacketsCount++;
                byte[] payload = ConvertStringToByteArray(packet.payload);

                if (packet.type == "KEY")
                {
                    AeadTool.key3 = payload;
                    Console.WriteLine("got key!");
                    Utils.PrintByteArray(AeadTool.key3);
                    continue;
                }

                if (AeadTool.key3 == null) // skip first 2 packets (ike req and resp)
                {
                    continue;
                }

                // parse packet and add to packet list here
                Packet parsedPacket = null;

                try
                {

                    if (packet.type == "REQUEST")
                    {
                        parsedPacket = HttpNetworkManager.Instance.ParseRequest(payload);
                    } else
                    {
                        parsedPacket = HttpNetworkManager.Instance.ParseResponse(payload);
                    }
                } catch (Exception e)
                {
                    Console.WriteLine($"something went wrong while parsing {packet.type} packet");
                    continue;
                }

                NetMsgId msgid = (NetMsgId)parsedPacket.msgId;
                Type requestType = GetRequestPacketTypeByProtocol(msgid);

                if (requestType == null)
                {
                    Console.WriteLine("invalid imessage type");
                    continue;
                }

                IMessage decodedPayload = HttpNetworkManager.Instance.DecodePacket(requestType, parsedPacket);

                Console.WriteLine($"Got Type: {packet.type}, MsgId: {(short)msgid}");

                packets.Add(new NovaPacket()
                {
                    Method = packet.type,
                    Packet = Convert.ChangeType(decodedPayload, requestType),
                    MsgId = msgid,
                    ClassType = NetMsgIdToNameMappings[(short)msgid],
                    MsgIdName = Enum.GetName(typeof(NetMsgId), msgid)
                });
            }
        }

        public void SavePackets(string saveFileName)
        {
            Console.WriteLine($"Got {packets.Count} packet(s) out a total of {totalPacketsCount}");
            File.WriteAllText($"../../../{saveFileName}", JsonConvert.SerializeObject(packets, Formatting.Indented));
        }

        public IMessage GetPcapPacket(NetMsgId msgId)
        {
            return (IMessage)packets.Where(p => p.MsgId == msgId).FirstOrDefault().Packet;
        }

        public static byte[] ConvertStringToByteArray(string input)
        {
            return input.Trim('[', ']').Split(',').Select(byte.Parse).ToArray();
        }

        public static Type? GetRequestPacketTypeByProtocol(NetMsgId msgId)
        {
            if (!NetMsgIdToNameMappings.ContainsKey((short)msgId))
            {
                return null;
            }

            string msgIdClassName = NetMsgIdToNameMappings[(short)msgId];

            Type packetClassType = Assembly.GetAssembly(typeof(LoginReq))!.GetTypes().Where(x => x.Name == msgIdClassName).SingleOrDefault();

            return packetClassType;
        }

        public static readonly Dictionary<short, string> NetMsgIdToNameMappings = new Dictionary<short, string>()
        {
            [-10034] = "GmCharUpNotify", // 角色数据变更
            [-10033] = "GmTowerGrowthNodesNotify", // 星塔养成节点变化通知
            [-10032] = "GmHarmonySkillNotify", // 共鸣技能新增
            [-10031] = "GmStory", // 一键通关故事并获得所有证据
            [-10030] = "GmStory", // 通关指定的故事并获取证据
            [-10029] = "InfinityTowerLevelInfo", // 无尽之塔跳层
            [-10028] = "GmAllInfinityTowerInfos", // 全通所有无尽之塔
            [-10027] = "GmClearAllEquipmentInstance", // 全通所有装备副本
            [-10026] = "StarTowerBuildInfo", // 返回星塔build导入的build信息
            [-10025] = "String", // 返回星塔build的json序列化字符串
            [-10024] = "StarTowerRoomCase", // 返回操作添加的新case
            [-10023] = "STAddTeamExpNtf", // 队伍等级经验最终值及当前未处理的所有case
            [-10022] = "StarTowerInfo", // 星塔跳层
            [-10021] = "GmSTClearAllStarTower", // 全通所有星塔(返回通关星塔ID列表)
            [-10020] = "ChangeInfo", // 星塔道具变化通知
            [-10019] = "CharAffinity", // 添加角色好感度
            [-10017] = "Disc", // 星盘信息通知
            [-10016] = "GmClearAllTravelerDue", // 全通所有旅人对决
            [-10015] = "GmClearAllRegionBossLevel", // 全通所有强敌讨伐
            [-10013] = "GmClearAllDailyInstance", // 一键通关所有日常副本
            [-10009] = "UI32", // 增加吸血鬼模式副本赛季积分 返回当前总分
            [-10008] = "GmClearAllVampireSurvivor", // 一键通关所有吸血鬼模式副本
            [-10007] = "RegionBossLevel", // 地区boss关卡最终数据通知
            [-10006] = "Chars", // 角色列表最终数据通知
            [-10005] = "GmWorldClass", // 世界等级最终值通知
            [-10004] = "Char", // 角色数据变化通知
            [-10003] = "Char", // 角色信息通知
            [-10001] = "ChangeInfo", // 道具变化通知
            [-3] = "Error", // 请求失败
            [-2] = "Nil", // 成功,响应会走notify附加数据流程
            [-1] = "SudoCommand", // 客户端内置GM命令请求
            [1] = "IKEReq", // internet key exchange
            [2] = "IKEResp", // 成功,返回服务器方的秘钥,之后每次请求都需要将Token写入header X-Token段,服务器以此识别用户
            [3] = "Error", // 失败
            [4] = "LoginReq", // 登录
            [5] = "LoginResp", // 成功,将返回的新token，替换之前ike阶段的token,放置于header X-Token段,服务器以此识别用户
            [6] = "Error", // 失败
            [1001] = "Nil", // 获取用户全量数据
            [1002] = "PlayerInfo", // 成功,如果是新账号，会返回player_new_notify协议
            [1003] = "Error", // 请求失败
            [1004] = "PlayerReg", // 注册角色
            [1005] = "Error", // 注册失败 返回错误，反之，成功会调用player_data_succeed_ack
            [1006] = "PlayerNameEditReq", // 请求修改昵称
            [1007] = "PlayerNameEditResp", // 请求修改昵称成功
            [1008] = "Error", // 请求修改昵称失败
            [1009] = "PlayerHeadIconSetReq", // 请求设置头像
            [1010] = "Nil", // 设置头像成功
            [1011] = "Error", // 设置头像失败
            [1012] = "Ping", // 心跳
            [1013] = "Pong", // 心跳回馈
            [1014] = "Error", // 不会返回失败
            [1015] = "NewbieInfo", // 新手教学,提交教学信息
            [1016] = "Nil", // 教学步骤记录成功
            [1017] = "Error", // 教学记录失败
            [1018] = "Nil", // 获取注销账号数据NotifyUrl
            [1019] = "PlayerDestroy", // 生成注销回调地址以提交到sdk server
            [1020] = "Error", // 获取失败
            [1021] = "PlayerBoardSetReq", // 请求设置看板
            [1022] = "Nil", // 设置看板成功
            [1023] = "Error", // 设置看板失败
            [1024] = "UI32", // 请求领取世界等级奖励
            [1025] = "ChangeInfo", // 请求领取世界等级奖励成功
            [1026] = "Error", // 请求领取世界等级奖励失败
            [1027] = "PlayerSignatureEditReq", // 请求修改签名
            [1028] = "Nil", // 请求修改签名成功
            [1029] = "Error", // 请求修改签名失败
            [1030] = "PlayerTitleEditReq", // 请求修改头衔
            [1031] = "Nil", // 请求修改头衔成功
            [1032] = "Error", // 请求修改头衔失败
            [1033] = "PlayerCharsShowReq", // 请求展示角色
            [1034] = "Nil", // 请求展示角色成功
            [1035] = "Error", // 请求展示角色失败
            [1036] = "PlayerSkinShowReq", // 请求展示皮肤
            [1037] = "Nil", // 请求展示皮肤成功
            [1038] = "Error", // 请求展示皮肤失败
            [1039] = "Nil", // 请求切换性别
            [1040] = "Nil", // 切换性别成功
            [1041] = "Error", // 切换性别失败
            [1042] = "PlayerSurveyReq", // 申请发起调查问卷,填写问卷ID
            [1043] = "PlayerSurveyResp", // 返回第三方问卷ID和回调通知地址
            [1044] = "Error", // 申请失败，比如过期，已经完成过
            [1045] = "Nil", // 退出游戏
            [1046] = "Nil", // 退出成功
            [1047] = "Error", // 退出失败
            [1048] = "PlayerHonorEditReq", // 荣誉称号最新的列表
            [1049] = "Nil", // 修改成功
            [1050] = "Error", // 修改失败
            [1101] = "ItemUseReq", // 道具使用
            [1102] = "ChangeInfo", // 道具使用成功，返回ChangeInfo
            [1103] = "Error", // 道具使用失败，返回错误信息
            [1104] = "UI32", // 传入钻石数量，兑换心相碎片，默认规则(免费钻不够使用付费钻)
            [1105] = "ChangeInfo", // 兑换成功
            [1106] = "Error", // 兑换失败
            [1107] = "ItemProductReq", // 材料合成
            [1108] = "ChangeInfo", // 材料合成成功
            [1109] = "Error", // 材料合成失败
            [1110] = "Nil", // 所有角色溢出碎片兑换
            [1111] = "ChangeInfo", // 兑换成功
            [1112] = "Error", // 兑换失败
            [1113] = "Nil", // 领取商店每日免费赠礼
            [1114] = "ChangeInfo", // 领取成功
            [1115] = "Error", // 领取失败
            [1201] = "Nil", // 请求获取好友/好友申请列表
            [1202] = "FriendListGetResp", // 获取好友/好友申请列表成功
            [1203] = "Error", // 获取好友/好友申请列表失败
            [1204] = "FriendUIdSearchReq", // 请求通过UId搜索用户信息
            [1205] = "FriendUIdSearchResp", // 通过UId搜索用户信息成功
            [1206] = "Error", // 通过UId搜索用户信息失败
            [1207] = "FriendNameSearchReq", // 请求通过用户昵称搜索用户信息
            [1208] = "FriendNameSearchResp", // 通过用户昵称搜索用户信息成功
            [1209] = "Error", // 通过用户昵称搜索用户信息失败
            [1210] = "FriendAddReq", // 请求申请添加好友
            [1211] = "Nil", // 申请添加好友成功
            [1212] = "Error", // 申请添加好友失败
            [1213] = "FriendAddAgreeReq", // 同意添加好友请求
            [1214] = "FriendAddAgreeResp", // 同意添加好友成功
            [1215] = "Error", // 同意添加好友失败
            [1216] = "Nil", // 请求一键添加好友
            [1217] = "FriendAllAgreeResp", // 一键添加好友成功
            [1218] = "Error", // 一键添加好友失败
            [1219] = "FriendDeleteReq", // 请求删除好友
            [1220] = "Nil", // 删除好友成功
            [1221] = "Error", // 删除好友失败
            [1222] = "FriendInvitesDeleteReq", // 请求删除好友申请
            [1223] = "Nil", // 删除好友申请成功
            [1224] = "Error", // 删除好友申请失败
            [1225] = "FriendStarSetReq", // 请求设置星级好友
            [1226] = "Nil", // 请求设置星级好友成功
            [1227] = "Error", // 请求设置星级好友失败
            [1228] = "FriendReceiveEnergyReq", // 请求领取好友赠送体力
            [1229] = "FriendReceiveEnergyResp", // 请求领取好友赠送体力成功
            [1230] = "Error", // 请求领取好友赠送体力失败
            [1231] = "FriendSendEnergyReq", // 请求赠送好友体力
            [1232] = "FriendSendEnergyResp", // 请求赠送好友体力成功
            [1233] = "Error", // 请求赠送好友体力失败
            [1234] = "Nil", // 请求好友推荐列表
            [1235] = "FriendRecommendationGetResp", // 请求好友推荐列表成功
            [1236] = "Error", // 请求好友推荐列表失败
            [1301] = "Nil", // 获取星塔养成详细信息
            [1302] = "TowerGrowthDetailResp", // 获取成功
            [1303] = "Error", // 获取信息失败，返回错误
            [1304] = "UI32", // 传入节点ID,解锁对应的养成节点
            [1305] = "ChangeInfo", // 解锁成功
            [1306] = "Error", // 解锁失败，返回错误信息
            [2001] = "PlayerFormationReq", // 设置编队
            [2002] = "Nil", // 设置编队成功
            [2003] = "Error", // 设置编队失败
            [2104] = "CharEquipmentChangeReq", // 装备穿上/替换/卸下
            [2105] = "CharEquipmentChangeResp", // 装备穿上/替换/卸下成功
            [2106] = "Error", // 装备穿上/替换/卸下失败
            [2301] = "CharUpgradeReq", // 角色升级
            [2302] = "CharUpgradeResp", // 角色升级成功
            [2303] = "Error", // 角色升级失败
            [2304] = "UI32", // 角色进阶,传入角色ID
            [2305] = "ChangeInfo", // 进阶成功
            [2306] = "Error", // 进阶失败
            [2307] = "CharSkillUpgradeReq", // 角色技能升级
            [2308] = "ChangeInfo", // 升级成功
            [2309] = "Error", // 升级失败
            [2313] = "CharAdvanceRewardReceiveReq", // 请求领取角色进阶奖励
            [2314] = "CharAdvanceRewardReceiveResp", // 请求领取角色进阶奖励成功
            [2315] = "Error", // 请求领取角色进阶奖励失败
            [2316] = "CharSkinSetReq", // 设置角色皮肤
            [2317] = "Nil", // 设置成功
            [2318] = "Error", // 设置失败
            [2322] = "CharAffinityQuestRewardReceiveReq", // 请求领取角色好感度任务奖励
            [2323] = "CharAffinityQuestRewardReceiveResp", // 请求领取角色好感度任务成功
            [2324] = "Error", // 请求领取角色好感度任务失败
            [2325] = "UI32", // 传入角色ID，招募角色
            [2326] = "ChangeInfo", // 招募成功
            [2327] = "Error", // 招募失败
            [2328] = "CharAffinityGiftSendReq", // 请求赠送好感度礼物
            [2329] = "CharAffinityGiftSendResp", // 请求赠送好感度礼物成功
            [2330] = "Error", // 请求赠送好感度礼物失败
            [2401] = "CharDatingLandmarkSelectReq", // 选择地点邀约角色
            [2402] = "CharDatingLandmarkSelectResp", // 选择地点邀约角色成功
            [2403] = "Error", // 选择地点邀约角色失败
            [2404] = "CharDatingGiftSendReq", // 请求邀约赠礼
            [2405] = "CharDatingGiftSendResp", // 请求邀约赠礼成功
            [2406] = "Error", // 请求邀约赠礼失败
            [2407] = "CharDatingEventRewardReceiveReq", // 请求领取特殊事件奖励
            [2408] = "ChangeInfo", // 请求领取特殊事件奖励成功
            [2409] = "Error", // 请求领取特殊事件奖励失败
            [3119] = "DiscStrengthenReq", // 星盘强化
            [3120] = "DiscStrengthenResp", // 星盘强化成功
            [3121] = "Error", // 星盘强化失败
            [3122] = "DiscPromoteReq", // 星盘升阶
            [3123] = "DiscPromoteResp", // 星盘升阶成功
            [3124] = "Error", // 星盘升阶失败
            [3125] = "DiscLimitBreakReq", // 星盘突破
            [3126] = "DiscLimitBreakResp", // 星盘突破成功
            [3127] = "Error", // 星盘突破失败
            [3128] = "DiscReadRewardReceiveReq", // 请求领取星盘阅读奖励
            [3129] = "ChangeInfo", // 请求领取星盘阅读奖励成功
            [3130] = "Error", // 请求领取星盘阅读奖励失败
            [3201] = "EquipmentUpgradeReq", // 装备强化
            [3202] = "EquipmentUpgradeResp", // 装备强化成功
            [3203] = "Error", // 装备强化失败
            [3204] = "EquipmentDismantleReq", // 装备分解
            [3205] = "ChangeInfo", // 装备分解成功
            [3206] = "Error", // 装备分解失败
            [3207] = "EquipmentLockUnlockReq", // 装备加解锁
            [3208] = "EquipmentInfo", // 装备加解锁成功
            [3209] = "Error", // 装备加解锁失败
            [3301] = "AgentApplyReq", // 请求派遣委托
            [3302] = "AgentApplyResp", // 请求派遣委托成功
            [3303] = "Error", // 请求派遣委托失败
            [3304] = "AgentGiveUpReq", // 请求放弃派遣委托
            [3305] = "AgentGiveUpResp", // 请求放弃派遣委托成功
            [3306] = "Error", // 请求放弃派遣委托失败
            [3307] = "AgentRewardReceiveReq", // 请求领取派遣委托奖励
            [3308] = "AgentRewardReceiveResp", // 请求领取派遣委托奖励成功
            [3309] = "Error", // 请求领取派遣委托奖励失败
            [4201] = "UI32", // 领取手册任务奖励 value表示任务ID，0表示一键领取
            [4202] = "TourGuideQuestRewardResp", // 获取成功
            [4203] = "Error", // 获取失败
            [4204] = "UI32", // 领取日常任务奖励 value表示任务ID，0表示一键领取
            [4205] = "ChangeInfo", // 获取成功
            [4206] = "Error", // 获取失败
            [4207] = "DictRewardReq", // 领取词条奖励
            [4208] = "ChangeInfo", // 获取成功
            [4209] = "Error", // 获取失败
            [4210] = "UI32", // 领取星塔任务奖励 value表示任务ID，0表示一键领取
            [4211] = "ChangeInfo", // 获取成功
            [4212] = "Error", // 获取失败
            [4213] = "Nil", // 领取日常任务活跃奖励
            [4214] = "QuestDailyActiveRewardReceiveResp", // 领取日常任务活跃奖励成功
            [4215] = "Error", // 领取日常任务活跃奖励失败
            [4216] = "Nil", // 领取任务组奖励
            [4217] = "TourGuideQuestGroupRewardResp", // 获取成功
            [4218] = "Error", // 获取失败
            [4401] = "AchievementRewardReq", // 领取成就奖励
            [4402] = "ChangeInfo", // 获取成功
            [4403] = "Error", // 获取失败
            [4404] = "Nil", // 获取成就数据
            [4405] = "Achievements", // 获取成功
            [4406] = "Error", // 获取失败
            [4501] = "Nil", // 申请无尽塔关卡数据
            [4502] = "InfinityTowerInfoResp", // 申请无尽塔关卡数据成功
            [4503] = "Error", // 申请无尽塔关卡数据失败
            [4504] = "InfinityTowerApplyReq", // 申请进入无尽塔关卡
            [4505] = "Nil", // 申请进入无尽塔关卡成功
            [4506] = "Error", // 申请进入无尽塔关卡失败
            [4507] = "UI32", // 申请结算无尽塔关卡 Value:1胜利,2失败,3退出(1,2重复挑战无需再发申请)
            [4508] = "InfinityTowerSettleResp", // 申请结算无尽塔成功 非0表示可以继续挑战关卡ID(失败当前关卡ID，成功下一个关卡ID),无需再发申请
            [4509] = "Error", // 申请结算无尽塔失败
            [4510] = "Nil", // 请求领取无尽塔每日奖励
            [4511] = "InfinityTowerDailyRewardReceiveResp", // 请求领取无尽塔每日奖励成功
            [4512] = "Error", // 请求领取无尽塔每日奖励失败
            [4513] = "UI32", // 请求领取无尽塔剧情奖励
            [4514] = "InfinityTowerPlotRewardReceiveResp", // 请求领取无尽塔剧情奖励成功
            [4515] = "Error", // 请求领取无尽塔剧情奖励失败
            [4601] = "StarTowerApplyReq", // 申请探索星塔
            [4602] = "StarTowerApplyResp", // 申请成功返回
            [4603] = "Error", // 申请失败
            [4607] = "StarTowerInteractReq", // 星塔交互请求
            [4608] = "StarTowerInteractResp", // 交互请求成功
            [4609] = "Error", // 申请失败
            [4610] = "Nil", // 获取星塔信息,用于重连
            [4611] = "StarTowerInfo", // 获取星塔信息成功
            [4612] = "Error", // 获取星塔信息失败
            [4613] = "Nil", // 放弃星塔
            [4614] = "StarTowerGiveUpResp", // 放弃星塔成功
            [4615] = "Error", // 放弃星塔失败
            [4701] = "StarTowerBuildWhetherSaveReq", // 请求是否保存星塔build
            [4702] = "StarTowerBuildWhetherSaveResp", // 请求是否保存星塔build返回
            [4703] = "Error", // 请求是否保存星塔build失败
            [4704] = "Nil", // 请求星塔build简要信息列表
            [4705] = "StarTowerBuildBriefListGetResp", // 请求星塔build简要信息列表返回
            [4706] = "Error", // 请求星塔build简要信息列表失败
            [4707] = "StarTowerBuildDetailGetReq", // 请求星塔build详细信息列表
            [4708] = "StarTowerBuildDetailGetResp", // 请求星塔build详细信息列表返回
            [4709] = "Error", // 请求遗迹build详细信息列表失败
            [4710] = "StarTowerBuildDeleteReq", // 请求解散星塔build
            [4711] = "ChangeInfo", // 请求解散星塔build返回
            [4712] = "Error", // 请求解散星塔build失败
            [4713] = "StarTowerBuildNameSetReq", // 请求设置星塔build名
            [4714] = "Nil", // 请求设置星塔build名返回
            [4715] = "Error", // 请求设置星塔build名失败
            [4716] = "StarTowerBuildLockUnlockReq", // 请求星塔build加解锁
            [4717] = "Nil", // 请求星塔build加解锁返回
            [4718] = "Error", // 请求星塔build加解锁失败
            [4719] = "StarTowerBuildPreferenceSetReq", // 请求设置星塔build偏好
            [4720] = "Nil", // 请求设置星塔build偏好返回
            [4721] = "Error", // 请求设置星塔build偏好失败
            [4801] = "Nil", // 请求星塔排行榜信息
            [4802] = "StarTowerRankInfo", // 请求星塔排行榜信息成功
            [4803] = "Error", // 请求星塔排行信息失败
            [4804] = "StarTowerRankApplyReq", // 申请探索星塔排行榜
            [4805] = "StarTowerRankApplyResp", // 申请成功返回
            [4806] = "Error", // 申请失败
            [4901] = "Nil", // 请求星塔图鉴角色潜能简要信息
            [4902] = "StarTowerBookPotentialBriefListResp", // 请求星塔图鉴角色潜能简要信息成功
            [4903] = "Error", // 请求星塔图鉴角色潜能简要信息失败
            [4904] = "UI32", // 请求星塔图鉴角色潜能信息
            [4905] = "StarTowerBookPotentialGetResp", // 请求星塔图鉴角色潜能信息成功
            [4906] = "Error", // 请求星塔图鉴角色潜能信息失败
            [4907] = "UI32", // 请求领取星塔图鉴角色潜能奖励
            [4908] = "StarTowerBookPotentialRewardReceiveResp", // 请求领取星塔图鉴角色潜能奖励成功
            [4909] = "Error", // 请求领取星塔图鉴角色潜能奖励失败
            [4910] = "Nil", // 请求星塔图鉴事件信息
            [4911] = "StarTowerBookEventGetResp", // 请求星塔图鉴事件信息成功
            [4912] = "Error", // 请求星塔图鉴事件信息失败
            [4913] = "UI32", // 请求领取星塔图鉴角色潜能奖励
            [4914] = "StarTowerBookEventRewardReceiveResp", // 请求领取星塔图鉴角色潜能奖励成功
            [4915] = "Error", // 请求领取星塔图鉴角色潜能奖励失败
            [5010] = "ResidentShopGetReq", // 请求常驻商店信息
            [5011] = "ResidentShopGetResp", // 请求常驻商店信息成功
            [5012] = "Error", // 请求常驻商店信息失败
            [5013] = "ResidentShopPurchaseReq", // 请求常驻商店购买物品
            [5014] = "ResidentShopPurchaseResp", // 请求常驻商店购买物品成功
            [5015] = "Error", // 请求常驻商店购买物品失败
            [5101] = "Nil", // 获取钻石商城产品列表
            [5102] = "MallGemList", // 获取成功的列表
            [5103] = "Error", // 获取失败
            [5104] = "String", // 下单购买商品
            [5105] = "OrderInfo", // 下单成功
            [5106] = "Error", // 下单失败
            [5107] = "UI64", // 取消某个尚未支付的订单
            [5108] = "UI64", // 取消成功
            [5109] = "Error", // 取消失败
            [5110] = "UI64", // 领取某个支付成功的订单的奖励
            [5111] = "CollectResp", // 返回成功，请根据具体状态处理
            [5112] = "CollectResp", // 领取失败
            [5113] = "Nil", // 获取月卡商城产品列表
            [5114] = "MallMonthlyCardList", // 获取成功的列表
            [5115] = "Error", // 获取失败
            [5116] = "String", // 购买月卡商城商品
            [5117] = "OrderInfo", // 下单成功
            [5118] = "Error", // 获取失败
            [5119] = "Nil", // 获取礼包商城商品列表
            [5120] = "MallPackageList", // 商品列表
            [5121] = "Error", // 获取失败
            [5122] = "String", // 购买礼包商城产品
            [5123] = "MallPackageOrder", // 购买成功结果
            [5124] = "Error", // 购买失败
            [5125] = "Nil", // 获取星尘兑换商城商品列表
            [5126] = "MallShopProductList", // 商品列表
            [5127] = "Error", // 获取失败
            [5128] = "MallShopOrderReq", // 购买星尘兑换商店产品
            [5129] = "ChangeInfo", // 购买成功结果
            [5130] = "Error", // 购买失败
            [6001] = "GachaSpinReq", // 根据卡池ID以及卡池模式在当前卡池抽卡
            [6002] = "GachaSpinResp", // 成功，返回掉落道具以及ChangeInfo
            [6003] = "Error", // 错误，返回错误信息
            [6004] = "Nil", // 获取所有卡池数据
            [6005] = "GachaInformationResp", // 成功，返回所有的卡池数据
            [6006] = "Error", // 错误，返回错误信息
            [6007] = "UI32", // 根据存盘ID获取抽卡的历史记录
            [6008] = "GachaHistories", // 成功，返回抽卡的历史数据
            [6009] = "Error", // 错误，返回错误
            [6101] = "Nil", // 获取已经拥有的命运卡图鉴
            [6102] = "TowerBookFateCardDetailResp", // 成功，返回已经拥有的命运卡和已经领取的任务
            [6103] = "Error", // 错误，返回错误信息
            [6104] = "TowerBookFateCardRewardReq", // 领取命运卡任务奖励
            [6105] = "ChangeInfo", // 成功，返回任务奖励
            [6106] = "Error", // 错误，返回错误信息
            [7013] = "UI32", // 传入剧情ID,领取剧情奖励
            [7014] = "ChangeInfo", // 领取成功
            [7015] = "Error", // 领取失败
            [7016] = "DailyInstanceApplyReq", // 日常副本申请
            [7017] = "Nil", // 日常副本申请成功
            [7018] = "Error", // 错误，返回错误信息
            [7019] = "DailyInstanceSettleReq", // 日常副本结算请求
            [7020] = "DailyInstanceSettleResp", // 日常副本结算成功
            [7021] = "Error", // 错误，返回错误信息
            [7022] = "DailyInstanceRaidReq", // 日常副本扫荡请求
            [7023] = "DailyInstanceRaidResp", // 日常副本扫荡成功
            [7024] = "Error", // 错误，返回错误信息
            [7028] = "DailyEquipmentApplyReq", // 日常装备副本申请
            [7029] = "Nil", // 日常装备副本申请成功
            [7030] = "Error", // 错误，返回错误信息
            [7031] = "DailyEquipmentSettleReq", // 日常装备副本结算请求
            [7032] = "DailyEquipmentSettleResp", // 日常装备副本结算成功
            [7033] = "Error", // 错误，返回错误信息
            [7034] = "DailyEquipmentSweepReq", // 日常装备副本扫荡请求
            [7035] = "DailyEquipmentSweepResp", // 日常装备副本扫荡成功
            [7036] = "Error", // 错误，返回错误信息
            [7101] = "RegionBossLevelApplyReq", // 请求进入地区boss关卡
            [7102] = "ChangeInfo", // 请求进入地区boss关成功
            [7103] = "Error", // 请求进入地区boss关卡失败
            [7104] = "RegionBossLevelSettleReq", // 请求结算地区boss关卡
            [7105] = "RegionBossLevelSettleResp", // 请求结算地区boss关成功
            [7106] = "Error", // 请求结算地区boss关卡失败
            [7107] = "RegionBossLevelSweepReq", // 请求扫荡地区boss关卡
            [7108] = "RegionBossLevelSweepResp", // 请求扫荡地区boss关成功
            [7109] = "Error", // 请求结算地区boss关卡失败
            [7201] = "TravelerDuelLevelApplyReq", // 请求进入旅人对决关卡
            [7202] = "Nil", // 请求进入旅人对决关卡成功
            [7203] = "Error", // 请求进入旅人对决关卡失败
            [7204] = "TravelerDuelLevelSettleReq", // 请求结算旅人对决关卡
            [7205] = "TravelerDuelLevelSettleResp", // 请求结算旅人对决关卡成功
            [7206] = "Error", // 请求结算旅人对决关卡失败
            [7207] = "Nil", // 请求旅人对决信息
            [7208] = "TravelerDuelInfo", // 请求旅人对决信息成功
            [7209] = "Error", // 请求旅人对决信息失败
            [7210] = "TravelerDuelQuestRewardReceiveReq", // 领取旅人对决任务奖励
            [7211] = "TravelerDuelQuestRewardReceiveResp", // 获取成功
            [7212] = "Error", // 获取失败
            [7213] = "Nil", // 请求旅人对决排行榜信息
            [7214] = "TravelerDuelRankInfo", // 请求旅人对决排行榜信息成功
            [7215] = "Error", // 请求旅人对决排行信息失败
            [7216] = "TravelerDuelBattleSamples", // 请求旅人对决上传分数、附带本次战斗统计数据
            [7217] = "RankChange", // 请求旅人对决上传分数成功
            [7218] = "Error", // 请求旅人对决上传分数失败
            [7301] = "StoryApplyReq", // 关卡申请
            [7302] = "Nil", // 申请成功，返回 Nil
            [7303] = "Error", // 错误，返回错误信息
            [7304] = "StorySettleReq", // 关卡结算
            [7305] = "ChangeInfo", // 结算成功，发放通关奖励
            [7306] = "Error", // 错误，返回错误信息
            [8001] = "Nil", // 购买体力请求
            [8002] = "EnergyBuyResp", // 购买成功，返回当日的购买次数，以及ChangeInfo
            [8003] = "Error", // 购买失败，返回错误信息
            [8101] = "Events", // 客户端事件上报
            [8102] = "Nil", // 客户端事件上报成功
            [8103] = "Error", // 客户端事件上报失败
            [8201] = "VampireSurvivorApplyReq", // 灾变防线副本申请
            [8202] = "VampireSurvivorApplyResp", // 灾变防线副本申请成功
            [8203] = "Error", // 灾变防线副本申请失败
            [8204] = "VampireSurvivorAreaChangeReq", // 灾变防线阶段转化
            [8205] = "Nil", // 灾变防线副本申请成功
            [8206] = "Error", // 灾变防线副本申请失败
            [8207] = "VampireSurvivorSettleReq", // 灾变防线副本结算申请 [失败也发这个]
            [8208] = "VampireSurvivorSettleResp", // 灾变防线副本结算申请成功
            [8209] = "Error", // 灾变防线副本结算申请失败
            [8210] = "VampireSurvivorRewardSelectReq", // 灾变防线副本升级申请
            [8211] = "VampireSurvivorRewardSelectResp", // 灾变防线副本结算申请成功
            [8212] = "Error", // 灾变防线副本结算申请失败
            [8213] = "VampireSurvivorRewardChestReq", // 灾变防线开宝箱申请
            [8214] = "VampireSurvivorRewardChestResp", // 灾变防线开宝箱申请成功
            [8215] = "Error", // 灾变防线开宝箱申请失败
            [8216] = "VampireSurvivorQuestRewardReceiveReq", // 灾变防线领取任务奖励申请
            [8217] = "ChangeInfo", // 灾变防线领取任务奖励成功
            [8218] = "Error", // 灾变防线领取任务奖励失败
            [8219] = "Nil", // 请求吸血鬼模式排行榜信息
            [8220] = "VampireRankInfo", // 请求吸血鬼模式排行榜信息成功
            [8221] = "Error", // 请求吸血鬼模式排行榜信息失败
            [8301] = "Nil", // 获取节点信息
            [8302] = "VampireTalentDetailResp", // 获取节点信息成功
            [8303] = "Error", // 获取失败，返回错误信息
            [8304] = "Nil", // 重置所有的天赋节点
            [8305] = "UI32", // 重置成功,返回最终天赋点数
            [8306] = "Error", // 重置失败，返回错误信息
            [8307] = "UI32", // 传入节点ID,解锁对应的养成节点
            [8308] = "Nil", // 解锁成功
            [8309] = "Error", // 解锁失败，返回错误信息
            [9001] = "Nil", // 获取邮件列表
            [9002] = "Mails", // 邮件列表
            [9003] = "Error", // 获取失败
            [9004] = "MailRequest", // 标记邮件已读
            [9005] = "UI32", // 返回已设置为已读的邮件ID
            [9006] = "Error", // 设置失败
            [9007] = "MailRequest", // 领取奖励 一键领取发送0，单独领取发送对应邮件ID上来
            [9008] = "MailRecvResp", // 领取成功
            [9009] = "Error", // 领取失败
            [9010] = "MailRequest", // 删除邮件 一键删除所有已读已领发送0，单独删除发送对应邮件ID上来
            [9011] = "MailRemoveResp", // 删除成功
            [9012] = "Error", // 删除失败
            [9101] = "Nil", // 获取所有的活动数据
            [9102] = "ActivityResp", // 获取成功
            [9103] = "Error", // 获取失败
            [9104] = "ActivityPeriodicRewardReq", // 领取活动奖励
            [9105] = "ChangeInfo", // 领取成功
            [9106] = "Error", // 领取失败
            [9107] = "UI32", // 仪式感，领取当前活动的最终奖励
            [9108] = "ChangeInfo", // 领取成功
            [9109] = "Error", // 领取失败
            [9110] = "UI32", // 领取活动奖励
            [9111] = "ChangeInfo", // 领取成功
            [9112] = "Error", // 领取失败
            [9201] = "Nil", // 获取手机所有联系人的数据
            [9202] = "PhoneContactsInfoResp", // 获取联系人数据成功
            [9203] = "Error", // 获取联系人数据
            [9204] = "PhoneContactsReportReq", // 联系人聊天上报
            [9205] = "ChangeInfo", // 上报成功
            [9206] = "Error", // 上报失败
            [9301] = "UI32", // 传入天赋ID,解锁相关天赋
            [9302] = "TalentUnlockResp", // 解锁成功
            [9303] = "Error", // 解锁失败，返回错误信息
            [9304] = "TalentResetReq", // 传入角色ID,重置角色所有天赋
            [9305] = "ChangeInfo", // 重置成功
            [9306] = "Error", // 重置失败，返回错误信息
            [9307] = "UI32", // 天赋普通节点ID
            [9308] = "TalentNodeResetResp", // 重置成功
            [9309] = "Error", // 重置失败，返回错误信息
            [9801] = "Nil", // 获取当前战令信息
            [9802] = "BattlePassInfo", // 战令信息
            [9803] = "Error", // 获取失败
            [9804] = "BattlePassRewardReceiveReq", // 领取战令奖励,传入战令等级和版本,全部领等级取传0
            [9805] = "BattlePassRewardReceiveResp", // 领取战令奖励成功
            [9806] = "Error", // 领取战令奖励失败
            [9807] = "UI32V", // 战令等级购买,传入需要购买的级数和版本
            [9808] = "BattlePassLevelBuyResp", // 购买成功
            [9809] = "Error", // 购买成功
            [9810] = "BattlePassOrderReq", // 战令进阶下单
            [9811] = "OrderInfo", // 战令进阶下单成功
            [9812] = "Error", // 领取失败
            [9813] = "UI64", // 战令进阶订单收取
            [9814] = "BattlePassOrderCollectResp", // 返回成功，请根据具体状态处理
            [9815] = "CollectResp", // 战令进阶失败
            [9816] = "UI32", // 战令任务一键领取 value表示任务ID，0表示一键领取
            [9817] = "BattlePassQuestRewardResp", // 获取成功
            [9818] = "Error", // 获取失败
            [9901] = "String", // 兑换码兑换
            [9902] = "RedeemCodeResp", // 兑换成功，返回兑换后德道具以及ChangeInfo
            [9903] = "Error", // 兑换失败，返回错误信息
            [10000] = "Error", // 系统级失败,主要用于http模式下，强制失败返回
            [10001] = "Nil", // 新用户
            [10002] = "MailState", // 邮件状态变更
            [10003] = "Error", // 在其他地方登录
            [10004] = "Error", // token过期
            [10005] = "Error", // 用户被ban
            [10006] = "Quest", // 任务进度变更
            [10007] = "ChangeInfo", // 星塔排行榜门票
            [10008] = "NewAgent", // 每周刷新新委托ID列表
            [10009] = "WorldClassUpdate", // 世界等级变化
            [10010] = "FriendEnergyState", // 好友赠送体力状态变更
            [10011] = "SigninRewardUpdate", // 登陆奖励更新
            [10012] = "FriendState", // 好友状态变更
            [10013] = "UI64", // 订单已完成支付通知，可以发起领取
            [10014] = "ChangeInfo", // 订单道具被撤回，主要用于恶意退款
            [10015] = "StarTowerBookPotentialChange", // 星塔潜能图鉴状态变更
            [10016] = "StarTowerBookEventChange", // 星塔潜能图鉴状态变更
            [10017] = "BattlePassState", // 战令状态变更
            [10018] = "WorldClassRewardState", // 世界等级奖励状态变更
            [10019] = "CharAdvanceRewardState", // 角色进阶奖励状态变更
            [10020] = "Achievement", // 成就进度变更
            [10021] = "AchievementState", // 成就待领取红点提示
            [10022] = "Skin", // 角色获得新皮肤，如果是重复获取，将发送转换数据
            [10023] = "SkinChange", // 角色装备的皮肤发生改变
            [10024] = "HandbookInfo", // 图鉴数据发生变化
            [10025] = "MonthlyCardRewards", // 月卡奖励通知
            [10026] = "QuestState", // 任务红点奖励notify
            [10027] = "MallPackageState", // 礼包商城免费商品红点notify
            [10028] = "Dictionary", // 字典数据变更
            [10029] = "Activity", // 活动数据变化
            [10030] = "ActivityQuest", // 活动任务数据变化
            [10031] = "CharAffinityRewardState", // 角色好感度奖励最终值
            [10032] = "Nil", // 道具超发进邮件
            [10033] = "InfinityTowerRewards", // 无尽塔是否有奖励可领最终值
            [10034] = "UI32", // 手机新聊天变化
            [10035] = "ChangeInfo", // 角色碎片溢出
            [10036] = "ActivityLogin", // 七日登录活动通知
            [10037] = "TowerBookFateCardCollectNotify", // 新获得的命运卡数据
            [10038] = "TowerBookFateCardRewardChangeNotify", // 命运卡图鉴奖励变化
            [10039] = "ChangeInfo", // 区域boss挑战模式门票变更通知最终值
            [10040] = "HonorChangeNotify", // 荣誉称号变更通知(最终值,多个Notify以最后一个为准)
        };

    }

    public class PcapPacket
    {
        public string payload { get; set; }
        public string type { get; set; }
    }

    public class NovaPacket
    {
        public string Method { get; set; }
        public object Packet { get; set; }
        public string ClassType { get; set; }
        public NetMsgId MsgId { get; set; }
        public string MsgIdName { get; set; }
    }

}
