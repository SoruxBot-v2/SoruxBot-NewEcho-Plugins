using SoruxBot.SDK.Plugins.Ability;
using SoruxBot.SDK.Plugins.Basic;

namespace SoruxBot.Echo.Plugins;

/// <summary>
/// 插件注册
/// </summary>
public class Register : SoruxBotPlugin, ICommandPrefix
{
    public override string GetPluginName() => "Echo Plugin";

    public override string GetPluginVersion() => "1.0.0";

    public override string GetPluginAuthorName() => "Open SoruxBot Project";

    public override string GetPluginDescription() => "这是一个回声插件";
    
    public string GetPluginPrefix() => "#";
}