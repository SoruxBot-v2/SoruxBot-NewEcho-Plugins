using SoruxBot.SDK.Attribute;
using SoruxBot.SDK.Model.Attribute;
using SoruxBot.SDK.Model.Message;
using SoruxBot.SDK.Model.Message.Entity;
using SoruxBot.SDK.Plugins.Basic;
using SoruxBot.SDK.Plugins.Model;
using SoruxBot.SDK.Plugins.Service;
using SoruxBot.SDK.QQ;
using SoruxBot.SDK.QQ.Entity;
using System.Collections.Concurrent;
using System.Reflection;
using System.Security.Principal;
using System.Text;

namespace SoruxBot.Echo.Plugins;

public class EchoController(ILoggerService loggerService, ICommonApi bot) : PluginController
{
    [MessageEvent(MessageType.PrivateMessage)]
    [Command(CommandPrefixType.Single, "echo <content>")]
    public PluginFlag EchoInPrivate(MessageContext ctx)
    {
        loggerService.Info("EchoInPrivate",$"Receive a message from framework, echo it");
        var msgBuilder = QqMessageBuilder.PrivateMessage(ctx.TriggerId);
        int count = ctx.MessageChain.Messages.Count;
        for (int i = 0; i < count; i++)
        {
            string type = ctx.MessageChain.Messages[i].Type;
            switch (type)
            {
                case "text":
                    {
                        var message = (TextMessage)ctx.MessageChain.Messages[i];
                        string text = message.Content;
                        if (i == 0)//截取指令部分
                        {
                            if (text.Length == 5|| text.Length == 6)
                            {
                                break;
                            }
                            else
                            {
                                text = text.Substring(6, text.Length - 6);
                            }
                        }
                        msgBuilder = msgBuilder.Text(text);
                        break;
                    }
                case "face":
                    {
                        var message = (FaceMessage)ctx.MessageChain.Messages[i];
                        msgBuilder = msgBuilder.Face(message.FaceId, message.IsLargeFace);
                        if (message.IsLargeFace)
                        {
                            i++;
                        }//去除超级表情附带的文字
                        break;
                    }
                case "image":
                    {
                        var message = (ImageMessage)ctx.MessageChain.Messages[i];
                        msgBuilder = msgBuilder.ImageOfUrl(message.ImageUrl);
                        break;
                    }
                case "record":
                    {
                        var message = (RecordMessage)ctx.MessageChain.Messages[i];
                        msgBuilder = msgBuilder.RecordOfUrl(message.AudioUrl);
                        break;
                    }
                default:
                    {
                        var message = (TextMessage)ctx.MessageChain.Messages[i];
                        msgBuilder = msgBuilder.Text("此消息暂时无法识别");
                        break;
                    }
            }
        }
        var msgChain = msgBuilder.Build();
        MessageContext newctx = MessageContextHelper.WithNewMessageChain(ctx, msgChain);
        bot.SendMessage(newctx);
        return PluginFlag.MsgIntercepted;
    }

    [MessageEvent(MessageType.GroupMessage)]
    [Command(CommandPrefixType.Single, "echo [content]")]
    public PluginFlag EchoInGroup(MessageContext ctx, string? content)
    {
        if (content is null)
        {
            var msgChain = QqMessageBuilder.GroupMessage(ctx.TriggerPlatformId)
                .Text("Echo Content is null")
                .Build();
            MessageContext newctx = MessageContextHelper.WithNewMessageChain(ctx, msgChain);
            bot.SendMessage(newctx);
            return PluginFlag.MsgIntercepted;
        }
      
        {
            loggerService.Info("EchoInPrivate",
                $"Receive a message from framework, echo it");
            var msgBuilder = QqMessageBuilder.GroupMessage(ctx.TriggerPlatformId);
            int count = ctx.MessageChain.Messages.Count;
            for (int i = 0; i < count; i++)
            {
                string type = ctx.MessageChain.Messages[i].Type;
                switch (type)
                {
                    case "text":
                        {
                            var message = (TextMessage)ctx.MessageChain.Messages[i];
                            string text = message.Content;
                            if (i == 0)//截取指令部分
                            {
                                if (text.Length == 5 || text.Length == 6)
                                {
                                    break;
                                }
                                else
                                {
                                    text = text.Substring(6, text.Length - 6);
                                }
                            }
                            msgBuilder = msgBuilder.Text(text);
                            break;
                        }
                    case "face":
                        {
                            var message = (FaceMessage)ctx.MessageChain.Messages[i];
                            msgBuilder = msgBuilder.Face(message.FaceId, message.IsLargeFace);
                            if (message.IsLargeFace)
                            {
                                i++;
                            }//去除超级表情的文字
                            break;
                        }
                    case "image":
                        {
                            var message = (ImageMessage)ctx.MessageChain.Messages[i];
                            msgBuilder = msgBuilder.ImageOfUrl(message.ImageUrl);
                            break;
                        }
                    case "record":
                        {
                            var message = (RecordMessage)ctx.MessageChain.Messages[i];
                            msgBuilder = msgBuilder.RecordOfUrl(message.AudioUrl);
                            break;
                        }
                    default:
                        {
                            var message = (TextMessage)ctx.MessageChain.Messages[i];
                            msgBuilder = msgBuilder.Text("此消息暂时无法识别");
                            break;
                        }
                }
            }
            var msgChain = msgBuilder.Build();
            MessageContext newctx = MessageContextHelper.WithNewMessageChain(ctx, msgChain);
            bot.SendMessage(newctx);
        }
        return PluginFlag.MsgIntercepted;
    }
}