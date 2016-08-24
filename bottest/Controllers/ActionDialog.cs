using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System;
using System.Threading.Tasks;
using AuthBot;
using AuthBot.Dialogs;
using AuthBot.Models;

namespace bottest.Controllers
{
    [Serializable]
    public class ActionDialog : IDialog<string>
        //[LUISModel(Constants.putinluisappid, Constants.putLuissubsctiponid)]
        //public class ActionDialog: LUISDialog<object>
        //make task and tag it with [LUISIntent("task")]
    {
        public async Task StartAsync(IDialogContext context)
        {
            // Just ignore this, all it does is guide the bot to the next method
            context.Wait(MessageReceivedAsync);
        }

        public virtual async Task MessageReceivedAsync(IDialogContext context, IAwaitable<IMessageActivity> msg)
        {
            try
            {
                // Here, we wait for the message. So that things can happen asynchronously, we have to wait for the user's input
                IMessageActivity message = await msg;

                // message.Text accesses what the user just said
                // "await context.PostAsync" sends a string to the user asynchronously
                await context.PostAsync(string.Format("You said {0}", message.Text));

                // this just drives the dialog. when you're done with the current dialog instance (aka this ActionDialog's lifespan),
                // you have to "Wait" for the method to finish.
                context.Wait(MessageReceivedAsync); //end everything with this
            }
            catch (Exception e)
            {
                // Tell user something broke, and end this dialog

                await context.PostAsync(string.Format("Crashed, {0}", e.Message));

                context.Wait(MessageReceivedAsync);
            }
        }
    }
}