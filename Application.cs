using System;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text.Json;
using SimApi.Communications;
using SimApi.Helpers;

namespace CoceApp;

public class Application
{
    private string Server { get; }

    public string AppId { get; }

    private string AppKey { get; }

    public bool Debug { get; set; }

    private JsonSerializerOptions JsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public Application(string server, string appId, string appKey, bool debug = false)
    {
        Debug = debug;
        Server = server + "/api/app";
        AppId = appId;
        AppKey = appKey;
    }

    private TRes Query<TReq, TRes>(string uri, TReq obj)
    {
        var json = SignRequest(obj);
        var wc = new HttpClient();
        if (Debug) Console.WriteLine($"请求的地址是: {Server + uri}");
        if (Debug) Console.WriteLine($"请求的内容: {SimApiUtil.Json(json)}");
        var resp = wc.PostAsJsonAsync(Server + uri, json).Result;
        if (Debug) Console.WriteLine($"请求的结果: {resp}");
        return resp.Content.ReadFromJsonAsync<TRes>().Result;
    }

    private T SignRequest<T>(T request)
    {
        (request as Request.BaseApiRequest).AppId = AppId;
        (request as Request.BaseApiRequest).Time = (int)SimApiUtil.TimestampNow;
        var requestJson = JsonSerializer.Serialize(request, JsonSerializerOptions);
        var parameters = JsonDocument.Parse(requestJson).RootElement.EnumerateObject();

        var dic = parameters.ToDictionary(x => x.Name, x => x.Value.ToString());
        dic.Remove("sign");
        var sorted = dic.OrderBy(x => x.Key);
        string signStr = "";
        foreach (var item in sorted)
        {
            signStr += $"{item.Key}={item.Value}&";
        }

        signStr = signStr.TrimEnd('&');
        if (Debug) Console.WriteLine($"签名的字符串: {signStr}");
        var sign = SimApiUtil.Md5(signStr + AppKey);
        if (Debug) Console.WriteLine($"签名: {sign}");
        (request as Request.BaseApiRequest).Sign = sign;
        return request;
    }

    /// <summary>
    /// 更新应用卡片
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="groupId"></param>
    /// <param name="type"></param>
    /// <param name="data"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public SimApiBaseResponse UpdateCard(string userId, string groupId, CardDataTypeEnum type, string data,
        string image = null)
    {
        var request = new Request.UpdateCardRequest
        {
            UserId = userId,
            GroupId = groupId,
            Type = type.ToString(),
            Data = data,
            Image = image
        };
        return Query<Request.UpdateCardRequest, SimApiBaseResponse>("/card", request);
    }


    /// <summary>
    /// 向用户发送信息
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="type"></param>
    /// <param name="text"></param>
    /// <param name="title"></param>
    /// <param name="image"></param>
    /// <returns></returns>
    public SimApiBaseResponse SendMessage(string userId, CardDataTypeEnum type, string text, string title = null,
        string image = null)
    {
        var request = new Request.SendMessageRequest
        {
            UserId = userId,
            Type = type.ToString(),
            Text = text,
            Title = title,
            Image = image
        };
        return Query<Request.SendMessageRequest, SimApiBaseResponse>("/message", request);
    }

    /// <summary>
    /// 使用Token获取用户信息
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public SimApiBaseResponse<Response.GetUserByTokenResponse> GetUserByToken(string token)
    {
        var request = new Request.GetUserByTokenRequest
        {
            Token = token
        };
        return Query<Request.GetUserByTokenRequest, SimApiBaseResponse<Response.GetUserByTokenResponse>>("/user/info",
            request);
    }

    /// <summary>
    /// 使用userId 获取用户组信息
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public SimApiBaseResponse<Response.GetUserGroupsResponseItem[]> GetUserGroups(string userId)
    {
        var request = new Request.GetUserGroupsRequest
        {
            UserId = userId
        };
        return Query<Request.GetUserGroupsRequest, SimApiBaseResponse<Response.GetUserGroupsResponseItem[]>>(
            "/user/groups", request);
    }

    /// <summary>
    /// 发起交易
    /// </summary>
    /// <param name="amount"></param>
    /// <param name="name"></param>
    /// <param name="ext"></param>
    /// <returns></returns>
    public SimApiBaseResponse<string> TradeCreate(int amount, string name, string ext)
    {
        var request = new Request.TradeCreateRequest
        {
            Amount = amount,
            Name = name,
            Ext = ext
        };
        return Query<Request.TradeCreateRequest, SimApiBaseResponse<string>>("/trade/create", request);
    }

    /// <summary>
    /// 查询交易
    /// </summary>
    /// <param name="tradeNo"></param>
    /// <returns></returns>
    public SimApiBaseResponse<Response.TradeCheckResponse> TradeCheck(string tradeNo)
    {
        var request = new Request.TradeCheckRequest
        {
            TradeNo = tradeNo
        };
        return Query<Request.TradeCheckRequest, SimApiBaseResponse<Response.TradeCheckResponse>>("/trade/result",
            request);
    }
}