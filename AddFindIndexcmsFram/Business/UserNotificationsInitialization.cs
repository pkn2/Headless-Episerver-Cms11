using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Security;
using AddFindIndexcmsFram.Models.Pages;
using EPiServer;
using EPiServer.Core;
using EPiServer.Core.Internal;
using EPiServer.Editor;
using EPiServer.Framework;
using EPiServer.Framework.Initialization;
using EPiServer.Logging;
using EPiServer.Notification;
using EPiServer.ServiceLocation;

namespace AddFindIndexcmsFram.Business
{
    [InitializableModule]
    [ModuleDependency(typeof(EPiServer.Web.InitializationModule))]
    public class UserNotificationsInitialization : IInitializableModule
    {
        private static readonly ILogger logger = LogManager.GetLogger(typeof(UserNotificationsInitialization));

        /// <summary>
        /// Name of the channel this notification demo uses.
        /// </summary>
        public const string NotificationChannelName = "Sample.MasterLanguage.Published.Test";

        private IContentEvents contentEvents = null;
        private IContentLoader contentLoader = null;
        private IContentVersionRepository contentVersionRepository = null;
        private INotifier notifier = null;

        private static readonly INotificationUser NotificationSender = new NotificationUser("SystemNotificationSender");

        private bool isInitialized = false;

        public void Initialize(InitializationEngine context)
        {
            if (!isInitialized)
            {
                // this is just personal preference to get the reference to local variable
                //ServiceLocationHelper serviceLocationHelper = context.Locate;

                contentEvents = ServiceLocator.Current.GetInstance<IContentEvents>();
                contentLoader = ServiceLocator.Current.GetInstance<IContentLoader>();
                contentVersionRepository = ServiceLocator.Current.GetInstance<IContentVersionRepository>();


                notifier = ServiceLocator.Current.GetInstance<INotifier>();

                // register the channel with immediate delivery
                RegisterChannel(ServiceLocator.Current.GetInstance<INotificationChannelOptionsRegistry>());

                contentEvents.PublishedContent += ContentEventsPublishedContent;

                isInitialized = true;
            }
        }

        private void RegisterChannel(INotificationChannelOptionsRegistry notificationChannelOptionsRegistry)
        {
            if (notificationChannelOptionsRegistry == null)
            {
                throw new ArgumentNullException(nameof(notificationChannelOptionsRegistry));
            }

            // is the channel already registered
            var channelOptions = notificationChannelOptionsRegistry.Get(NotificationChannelName);

            if (channelOptions == null)
            {
                // register the channel to immediatly notify user
                //NOTE: the channel option seems not to be persisted currently (EPiServer.CMS NuGet, version 11.12.0)
                //so this code is executed everytime initialization happens
                notificationChannelOptionsRegistry.Add(NotificationChannelName, new NotificationChannelOptions(true));

                logger.Debug($"Registered notification channel '{NotificationChannelName}' with immediate delivery option.");
            }
            else
            {
                logger.Debug($"Notification channel '{NotificationChannelName}' already registered.");
            }
        }

        private void ContentEventsPublishedContent(object sender, ContentEventArgs e)
        {
            // we are only interested of pages inherited from StandardPage in this demo
            // not using SitePageData because container page inherits it - but you get the idea
            if (e?.Content is HomePage page)
            {
                //logger.Debug(page, (state) =>
                //{
                //    // if debug is not on, this will never get executed
                //    return $"PublishedContent, IsMasterLanguage: '{state.IsMasterLanguageBranch}', has other languages: '{state.ExistingLanguages.Count() > 1}'.";
                //});

                // we are only interested about pages that are published in master language and have other language versions
                // if a language version exists it doesn't mean that the language version is published (we don't check that for now)

                //if (page.IsMasterLanguageBranch && page.ExistingLanguages.Count() > 1 && TryGetReceivers(page, out List<NotificationUser> receivers))
                if (TryGetReceivers(page, out List<NotificationUser> receivers))
                {
                    // reference without version information
                    //ContentReference contentWithoutVersion = page.ContentLink.ToReferenceWithoutVersion();

                    // get the master lanuage edit link
                    //string masterCultureName = page.MasterLanguage.Name;
                    //string masterPageEditUrl = PageEditing.GetEditUrlForLanguage(contentWithoutVersion, masterCultureName);

                    // with PageEditing.GetEditUrlForLanguage we get a link like this:
                    //http://localhost:12345/EPiServer/CMS//?language=en#context=epi.cms.contentdata:///5

                    // create a list of "urls" for the other languages
                    //var otherLanguages = page.ExistingLanguages
                    //    .Where(ci => !ci.Name.Equals(masterCultureName, StringComparison.OrdinalIgnoreCase))
                    //    .Select(ci => CreateLink(PageEditing.GetEditUrlForLanguage(contentWithoutVersion, ci.Name), ci.Name));

                    NotificationMessage notification = new NotificationMessage()
                    {
                        ChannelName = NotificationChannelName,
                        Content = $"Master language page",
                        Recipients = receivers,
                        Sender = NotificationSender,
                        Subject = $"Test Message",
                        TypeName = "PagePublished"
                    };

                    try
                    {
                        // always nice to call async code from synchronous code
                        var task = notifier.PostNotificationAsync(notification);
                        // blocking hack: https://msdn.microsoft.com/en-us/magazine/mt238404.aspx
                        // PostNotificationAsync uses ConfigureAwait(false) so this shouldn't deadlock
                        task.GetAwaiter().GetResult();
                    }
                    catch (Exception ex)
                    {
                        logger.Error($"Failed to send notification messages to users about published master language page (id: {page.ContentLink?.ID}) with language versions.", ex);
                    }
                }
            }
        }

        //private static string CreateLink(string url, string linkText)
        //{
        //    return $"<a style=\"color:#0000FF;text-decoration:underline;\" href=\"{url}\">{linkText}</a>";
        //}

        /// <summary>
        /// Tries to get the recipients from the page changed by or created by and who published tha page.
        /// </summary>
        /// <param name="page">the page instance</param>
        /// <param name="recipients">recipients if the method returns true otherwise empty list</param>
        /// <param name="getPublisher">should the username of the published also fetched and included to the recipients</param>
        /// <returns>true if a username was taken from page ChangedBy or CreatedBy property or false if no username was found</returns>
        private bool TryGetReceivers(PageData page, out List<NotificationUser> recipients, bool getPublisher = true)
        {
            bool success = false;
            recipients = new List<NotificationUser>();

            if (page != null)
            {
                string editorUser = null;


                string[] usersInRole = Roles.GetUsersInRole("WebAdmins");
                foreach (string sUser in usersInRole)
                {
                    editorUser = Membership.GetUser(sUser).ToString();
                    recipients.Add(new NotificationUser(editorUser));
                    success = true;
                }

                //if (!string.IsNullOrWhiteSpace(page.ChangedBy))
                //{
                //    editorUser = page.ChangedBy;
                //    recipients.Add(new NotificationUser(editorUser));
                //    success = true;
                //}
                

                if (getPublisher)
                {
                    // NOTE: ChangedBy/CreatedBy user is not the same thing as who published the content but it can be
                    // publisher info is not stored to the page data, the publish state change is stored to 'tblWorkContent'
                    // so you could view that info in SQL query like this: select * from tblWorkContent where fkContentID = 138
                    // where the fkContentID is your content id

                    // get version info
                    string publisherUsername = contentVersionRepository.LoadPublished(page.ContentLink)?.StatusChangedBy;

                    // also check that the publisher is not the same user as the editor user
                    if (!string.IsNullOrWhiteSpace(publisherUsername) && !publisherUsername.Equals(editorUser, StringComparison.OrdinalIgnoreCase))
                    {
                        recipients.Add(new NotificationUser(publisherUsername));
                        success = true;
                    }
                }

                if (!success)
                {
                    logger.Warning($"Failed to get notification users for page id: {page.ContentLink?.ID}.");
                }
            }

            return success;
        }

        public void Uninitialize(InitializationEngine context)
        {
            if (isInitialized && contentEvents != null)
            {
                contentEvents.PublishedContent -= ContentEventsPublishedContent;
                isInitialized = false;
                // on purpose not setting the service references to null
            }
        }
    }
}