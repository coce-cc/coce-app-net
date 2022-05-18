using System;
using SimApi.Communications;

namespace CoceApp;

public class Response
{
    public class TradeCheckResponse
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

    public class GetUserByTokenResponse
    {
        public string UserId { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public bool Verified { get; set; }
    }

    public class GetUserGroupsResponseItem
    {
        public string Id { get; set; }

        public string Name { get; set; }

        public string Image { get; set; }

        public string Description { get; set; }

        public string Role { get; set; }
    }
}