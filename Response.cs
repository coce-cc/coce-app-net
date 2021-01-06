using System;
using System.Text.Json.Serialization;
using SimApi.Communications;

namespace SimUcApp
{
    public class SimUcResponse
    {
        public class TradeCheckResponse : SimApiBaseResponse
        {
            public string TradeNo { get; set; }
            public int Amount { get; set; }
            public int Fee { get; set; }
            public string Name { get; set; }
            public string Ext { get; set; }
            public string Status { get; set; }
            public DateTime CreatedAt { get; set; }
            public DateTime FinishedAt { get; set; }
            public DateTime RefundAt { get; set; }
            public DateTime CloseAt { get; set; }
        }

        public class GetUserByTokenResponse : SimApiBaseResponse
        {
            public class GroupItem
            {
                public int Id { get; set; }
                public string Name { get; set; }
                public string Image { get; set; }
                public string Description { get; set; }
                public string Role { get; set; }
            }

            public int UserId { get; set; }
            public string Name { get; set; }
            public string Image { get; set; }
            public GroupItem[] Groups { get; set; }
            public bool Verified { get; set; }
        }

    }
}
