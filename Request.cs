using System;
using System.ComponentModel.DataAnnotations;

namespace CoceApp;

public class Request
{
    public class BaseApiRequest
    {
        public string AppId { get; set; }

        public int Time { get; set; }

        public string Sign { get; set; }
    }

    public class UpdateCardRequest : BaseApiRequest
    {
        public string GroupId { get; set; }

        public string UserId { get; set; }

        /// <summary>
        /// 数据类型，只可以为 image，text，html
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// type为image的时候，这里是配文，为text的时候，这里是文字，为html的时候，这里是html链接
        /// </summary>
        public string Data { get; set; }

        /// <summary>
        /// 只有type为image的时候，这里是图片链接
        /// </summary>
        public string Image { get; set; }
    }


    public class SendMessageRequest : BaseApiRequest
    {
        public string UserId { get; set; }

        /// <summary>
        /// 数据类型，只可以为 image，text，html
        /// </summary>
        public string Type { get; set; }

        /// <summary>
        /// 可不填写
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// type为image的时候，这里是配文，为text的时候，这里是文字，为html的时候，这里是html链接
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 只有type为image的时候，这里是图片链接
        /// </summary>
        public string Image { get; set; }
    }

    public class GetUserByTokenRequest : BaseApiRequest
    {
        public string Token { get; set; }
    }

    public class GetUserGroupsRequest : BaseApiRequest
    {
        public string UserId { get; set; }
    }

    public class TradeCreateRequest : BaseApiRequest
    {
        public int Amount { get; set; }

        public string Name { get; set; }

        public string Ext { get; set; }
    }

    public class TradeCheckRequest : BaseApiRequest
    {
        public string TradeNo { get; set; }
    }
    
    public class TransferRequest : BaseApiRequest
    {
        public string Type { get; set; }

        public string TargetId { get; set; }

        public int Coin { get; set; }

        public string Marks { get; set; }
    }
    
    public class RefundTradeRequest : BaseApiRequest
    {
        [Required] public string TradeNo { get; set; }
    }
}

public enum CardDataTypeEnum
{
    Text,

    Image,

    Html
}