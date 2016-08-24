using Autofac;
using bottest.Controllers;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Builder.Dialogs.Internals;
using Microsoft.Bot.Connector;
using System;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace bottest
{
    [BotAuthentication]
    public class MessagesController : ApiController
    {
        /// <summary>
        /// POST: api/Messages
        /// Receive a message from a user and reply to it
        /// </summary>
        [BotAuthentication]
        public async Task<HttpResponseMessage> Post([FromBody]Activity activity)
        {

            // TODOL tuyrn custo errors off or block all of them from output

            if (activity != null && activity.GetActivityType() == ActivityTypes.Message)
            {
                // calculate something for us to return
                // TODO: generate response to message
                try
                {
                    await Conversation.SendAsync(activity, () => new ActionDialog());
                }
                catch (Exception e)
                {

                }
            }

            else if (activity != null)
            {
                using (var scope = DialogModule.BeginLifetimeScope(Conversation.Container, activity))
                {
                    var client = scope.Resolve<IConnectorClient>();

                    await client.Conversations.ReplyToActivityAsync(HandleSystemMessage(activity));
                }
            }

            return new HttpResponseMessage(System.Net.HttpStatusCode.Accepted);
        }
        
        private Activity HandleSystemMessage(Activity message)

        {

            if (message.Type == ActivityTypes.Ping)

            {

                Activity reply = message.CreateReply();

                reply.Type = ActivityTypes.Ping;

                return reply;

            }

            else if (message.Type == ActivityTypes.ConversationUpdate && message.MembersAdded.Count != 0)

            {

                Activity reply = message.CreateReply();

                reply.Type = ActivityTypes.Message;

                reply.Text = string.Format("help message");

                return reply;

            }



            return null;

        }

    }
}