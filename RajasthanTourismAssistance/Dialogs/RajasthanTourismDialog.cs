using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;

namespace RajasthanTourismAssistance.Dialogs
{
    [Serializable]
    public class RajasthanTourismDialog : IDialog<object>
    {
        public Task StartAsync(IDialogContext context)
        {
            context.Wait(MessageReceivedAsync);

            return Task.CompletedTask;
        }

        private async Task MessageReceivedAsync(IDialogContext context, IAwaitable<object> result)
        {
            var activity = await result as Activity;

            // User options
            CardAction bestInRajasthan = new CardAction()
            {
                Value = "1",
                Type = "postBack",
                Title = "Search Best Places In Rajasthan",
            };
            CardAction bestInSpecificCity = new CardAction()
            {
                Value = "2",
                Type = "postBack",
                Title = "Search By Citi"
            };


            // Welcome messages with user actions
            Activity reply = activity.CreateReply($"Welcome in Rajasthan Chatbot!");
            reply.Recipient = activity.From;
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();

            var welcomeCard = new ThumbnailCard
            {

                Title = "Please select what you want to do?",
                Text = "Rajasthan bot provides you many services on your favorite chat messengers including Facebook, Skype etc.",
                Images = new List<CardImage> { new CardImage("https://despatchbay.com/assets/images/couriers/courier-van-dhl.png") },
                Buttons = new List<CardAction> { bestInRajasthan, bestInSpecificCity }
            };


            Attachment plAttachment = welcomeCard.ToAttachment();
            reply.Attachments.Add(plAttachment);
            await context.PostAsync(reply);
            context.Wait(HandleUserSelectedOption);
        }

        private async Task HandleUserSelectedOption(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var message = await result as Activity;
            int selectedValue = Convert.ToInt32(message.Value);
            if (selectedValue == 1)
            {

            }
        }
    }
}