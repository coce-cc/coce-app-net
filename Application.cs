using System;
using System.Linq;
using System.Net;
using System.Text.Json;
using SimApi.Communications;
using Newtonsoft.Json;
using SimApi.Helpers;

namespace CoceApp;

public class Application
{
    private string Server { get; }

    public string AppId { get; }

    private string AppKey { get; }

    public Application(string server, string appId, string appKey)
    {
        Server = server + "/api/application";
        AppId = appId;
        AppKey = appKey;
    }

    private string Query(string uri, string body)
    {
        var wc = new WebClient();
        wc.Headers.Add(HttpRequestHeader.ContentType, "application/json");
        Console.WriteLine($"请求的地址是: {Server + uri}");
        Console.WriteLine($"请求的内容: {body}");
        var resp = wc.UploadString(Server + uri, body);
        Console.WriteLine($"请求的结果: {resp}");
        return resp;
    }

    private string SignRequest<T>(T request)
    {
        (request as SimUcRequest.BaseApiRequest).AppId = AppId;
        (request as SimUcRequest.BaseApiRequest).Time =
            (int)(DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0, 0)).TotalSeconds;
        var requestJson = JsonConvert.SerializeObject(request);
        var parameters = JsonDocument.Parse(requestJson).RootElement.EnumerateObject();

        var dic = parameters.ToDictionary(x => x.Name, x => x.Value.ToString());
        dic.Remove("Sign");
        var sorted = dic.OrderBy(x => x.Key);
        string signStr = "";
        foreach (var item in sorted)
        {
            signStr += $"{item.Key}={item.Value}&";
        }

        signStr = signStr.TrimEnd('&');
        Console.WriteLine($"签名的字符串: {signStr}");
        var sign = SimApiUtil.Md5(signStr + AppKey);
        Console.WriteLine($"签名: {sign}");
        (request as SimUcRequest.BaseApiRequest).Sign = sign;
        return JsonConvert.SerializeObject(request);
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
        var request = new SimUcRequest.UpdateCardRequest
        {
            UserId = userId,
            GroupId = groupId,
            Type = type.ToString(),
            Data = data,
            Image = image
        };
        var body = SignRequest(request);
        Console.WriteLine(body);
        var resp = Query("/card", body);
        return JsonConvert.DeserializeObject<SimApiBaseResponse>(resp);
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
        var request = new SimUcRequest.SendMessageRequest
        {
            UserId = userId,
            Type = type.ToString(),
            Text = text,
            Title = title,
            Image = image
        };
        var body = SignRequest(request);
        var resp = Query("/message", body);
        return JsonConvert.DeserializeObject<SimApiBaseResponse>(resp);
    }

    /// <summary>
    /// 使用Token获取用户信息
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public Response.GetUserByTokenResponse GetUserByToken(string token)
    {
        var request = new SimUcRequest.GetUserByTokenRequest
        {
            Token = token
        };
        var body = SignRequest(request);
        var resp = Query("/user/info", body);
        return JsonConvert.DeserializeObject<Response.GetUserByTokenResponse>(resp);
    }

    /// <summary>
    /// 使用userId 获取用户组信息
    /// </summary>
    /// <param name="token"></param>
    /// <returns></returns>
    public SimApiBaseResponse<Response.GetUserGroupsResponseItem[]> GetUserGroups(string userId)
    {
        var request = new SimUcRequest.GetUserGroupsRequest
        {
            UserId = userId
        };
        var body = SignRequest(request);
        var resp = Query("/user/groups", body);
        return JsonConvert.DeserializeObject<SimApiBaseResponse<Response.GetUserGroupsResponseItem[]>>(resp);
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
        var request = new SimUcRequest.TradeCreateRequest
        {
            Amount = amount,
            Name = name,
            Ext = ext
        };
        var body = SignRequest(request);
        var resp = Query("/trade/create", body);
        return JsonConvert.DeserializeObject<SimApiBaseResponse<string>>(resp);
    }

    /// <summary>
    /// 查询交易
    /// </summary>
    /// <param name="tradeNo"></param>
    /// <returns></returns>
    public Response.TradeCheckResponse TradeCheck(string tradeNo)
    {
        var request = new SimUcRequest.TradeCheckRequest
        {
            TradeNo = tradeNo
        };
        var body = SignRequest(request);
        var resp = Query("/trade/result", body);
        return JsonConvert.DeserializeObject<Response.TradeCheckResponse>(resp);
    }
}