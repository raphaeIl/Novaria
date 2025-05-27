using Novaria.Common.Core;
using Proto;

namespace Novaria.GameServer.Controllers.Api.ProtocolHandlers
{
    public class Gacha : ProtocolHandlerBase
    {
        public Gacha(IProtocolHandlerFactory protocolHandlerFactory) : base(protocolHandlerFactory)
        {

        }

        [ProtocolHandler(NetMsgId.gacha_information_req)]
        public Packet GachaInfoHandler(Nil req)
        {
            GachaInformationResp gachaInfoResp = new GachaInformationResp()
            {
                Information =
                {
                  new GachaInfo()
                  {
                      Id = 1,
                      DaysCount = 31,
                      AupMissTimes = 1,
                      TotalTimes = 123,
                      AupGuaranteeTimes = 10,
                  }
                },
            };

            return Packet.Create(NetMsgId.gacha_information_succeed_ack, gachaInfoResp);
        }

        [ProtocolHandler(NetMsgId.gacha_spin_req)]
        public Packet GachaInfoHandler(GachaSpinReq req)
        {
            GachaSpinResp gachaSpinResp = new GachaSpinResp()
            {
                Change = new ChangeInfo()
                {
                },
                Time = 10,
                TotalTimes = 123,
                AupGuaranteeTimes = 1,
                AupMissTimes = 1,
                DaysCount = 2,
            };

            for (int i = 0; i < 10; i++)
            {
                gachaSpinResp.Cards.Add(new GachaCard()
                {
                    Card = new ItemTpl()
                    {
                        Qty = 1,
                        Tid = 132,
                    },
                });
            }

            return Packet.Create(NetMsgId.gacha_spin_succeed_ack, gachaSpinResp);
        }

    }
}
