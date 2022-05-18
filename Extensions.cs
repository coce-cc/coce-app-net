using System;
using Microsoft.Extensions.DependencyInjection;

namespace CoceApp;

public static class Extensions
{
    //**********快捷添加**************

    /// <summary>
    /// asp.net core 注入
    /// </summary>
    /// <param name="builder"></param>
    /// <param name="server"></param>
    /// <param name="appId"></param>
    /// <param name="appKey"></param>
    /// <returns></returns>
    public static IServiceCollection AddCoceApp(this IServiceCollection builder, string server,
        string appId, string appKey, bool debug = false)
    {
        return builder.AddSingleton(new Application(server, appId, appKey, debug));
    }
}