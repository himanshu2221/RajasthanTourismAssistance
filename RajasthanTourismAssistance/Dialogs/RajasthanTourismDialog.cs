using System;
using System.Threading.Tasks;
using Microsoft.Bot.Builder.Dialogs;
using Microsoft.Bot.Connector;
using System.Collections.Generic;
using System.Collections;
using RajasthanTourismAssistance.Utility;
using RajasthanTourismAssistance.Model;

namespace RajasthanTourismAssitance.Dialogs
{
    [Serializable]
    public class RajasthanTourismDialogs : IDialog<object>
    {
        protected int cityID = 0;

        public Task StartAsync(IDialogContext context)
        {
            context.Wait(StartConversationAsync);

            return Task.CompletedTask;
        }

        //Cards
        private async Task StartConversationAsync(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;

            // User options shown via Adaptive Card
            CardAction bestTouristPlaces = new CardAction()
            {
                Value = "1",
                Type = "postBack",
                Title = "Best Places",
            };
            CardAction cityWiseTourism = new CardAction()
            {
                Value = "2",
                Type = "postBack",
                Title = "City Wise Tourism"
            };


            // Welcome messages with user actions
            Activity reply = activity.CreateReply($"Welcome in Rajasthan Tourism!");
            reply.Recipient = activity.From;
            reply.Type = "message";
            reply.Attachments = new List<Attachment>();

            var welcomeCard = new ThumbnailCard
            {
                Title = "How may I help you?",
                Text = "Rajasthan Tourism bot provides many services like searching destinations, booking accommodation and transportation on your favorite chat messengers including Facebook, Skype etc.",
                Images = new List<CardImage> { new CardImage("https://image.ibb.co/iAgtzb/Rajasthan_Bot.png") },
                Buttons = new List<CardAction> { bestTouristPlaces, cityWiseTourism }
            };


            Attachment plAttachment = welcomeCard.ToAttachment();
            reply.Attachments.Add(plAttachment);
            await context.PostAsync(reply);
            context.Wait(SelectCity);
        }


        private async Task SelectCity(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            Activity reply = activity.CreateReply($"Please let me know city name");
            await context.PostAsync(reply);
            context.Wait(ShowCategories);
        }

        private async Task ShowCategories(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            String cityName = activity.Text;

            cityID = DBHelper.Instance.GetCityID(cityName.ToUpper());

            List<Category> categories = DBHelper.Instance.GetCategories();

            Activity reply = activity.CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (Category category in categories)
            {
                reply.Attachments.Add(CreateCard(category.imageUrl, category.categoryName));
            }

            await context.PostAsync(reply);

            context.Wait(ShowSubCategories);
        }


        private Attachment CreateCard(String imageUrl, String value)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(imageUrl));

            CardAction plButton = new CardAction()
            {
                Value = value,
                Type = ActionTypes.PostBack,
                Title = value
            };

            cardButtons.Add(plButton);

            HeroCard plCard = new HeroCard()
            {
                // Title = "FORTS",
                // Subtitle = $"{cardContent.Key} Wikipedia Page",
                Images = cardImages,
                Buttons = cardButtons
            };


            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = plCard
            };

            return attachment;
        }


        private async Task ShowSubCategories(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            String categoryName = activity.Text;

            //int categoryID = DBHelper.Instance.GetCityID(categoryName);


            List<SubCategory> categories = DBHelper.Instance.GetSubCategories();

            Activity reply = activity.CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            foreach (SubCategory subCategory in categories)
            {
                reply.Attachments.Add(CreateCard(subCategory.imageUrl, subCategory.subCategoryName));
            }

            await context.PostAsync(reply);

            context.Wait(ShowTouristPlaces);

        }

        private async Task ShowTouristPlaces(IDialogContext context, IAwaitable<IMessageActivity> result)
        {
            var activity = await result as Activity;
            String subCategoryName = activity.Text;
            Activity reply = activity.CreateReply();
            reply.AttachmentLayout = AttachmentLayoutTypes.Carousel;

            int subCategoryID = DBHelper.Instance.GetSubCategoryID(subCategoryName);

            List<TouristPlace> allPlaces = DBHelper.Instance.GetTouristPlaces(subCategoryID, cityID);

            foreach (TouristPlace ob in allPlaces)
            {
                reply.Attachments.Add(CreateCardInDetailText(ob.imageUrl, ob.place, ob.description,ob.hyperlink));
            }

            await context.PostAsync(reply);

            context.Wait(StartConversationAsync);
        }

        private Attachment CreateCardInDetailText(String imageUrl, String value, String description, string url)
        {
            List<CardAction> cardButtons = new List<CardAction>();
            List<CardImage> cardImages = new List<CardImage>();
            cardImages.Add(new CardImage(imageUrl));

            CardAction plButton = new CardAction()
            {
                Value = url,
                Type = ActionTypes.OpenUrl,
                Title = "more..." 
            };

            cardButtons.Add(plButton);

            HeroCard plCard = new HeroCard()
            {
                Title = value,
                Subtitle = description,
                Images = cardImages,
                Buttons = cardButtons
            };


            Attachment attachment = new Attachment()
            {
                ContentType = HeroCard.ContentType,
                Content = plCard
            };

            return attachment;
        }

    }
}

