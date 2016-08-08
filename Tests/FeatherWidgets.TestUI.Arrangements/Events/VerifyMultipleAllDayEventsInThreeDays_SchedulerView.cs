﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using FeatherWidgets.TestUtilities.CommonOperations;
using Telerik.Sitefinity.Events.Model;
using Telerik.Sitefinity.Modules.Events;
using Telerik.Sitefinity.TestArrangementService.Attributes;
using Telerik.Sitefinity.TestUI.Arrangements.Framework.Server;
using Telerik.Sitefinity.TestUtilities.CommonOperations;

namespace FeatherWidgets.TestUI.Arrangements
{
    public class VerifyMultipleAllDayEventsInThreeDays_SchedulerView : TestArrangementBase
    {
        /// <summary>
        /// Creates an Event.
        /// Creates a Page with Events Widget.
        /// </summary>
        [ServerSetUp]
        public void OnBeforeTestsStarts()
        {
            var templateId = ServerOperations.Templates().GetTemplateIdByTitle(TemplateTitle);
            Guid pageId = ServerOperations.Pages().CreatePage(PageTitle, templateId);
            var pageNodeId = ServerOperations.Pages().GetPageNodeId(pageId);
            ServerOperationsFeather.Pages().AddEventsWidgetToPage(pageNodeId, PlaceHolderId);

            ServerOperations.Events().CreateAllDayEvent(Event1Title, string.Empty, this.event1Start, this.event1End);
            var event1Item = EventsManager.GetManager().GetEvents().Where<Event>(ni => ni.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live
                && ni.Title == Event1Title).FirstOrDefault();
            ServerArrangementContext.GetCurrent().Values.Add("event1Id", event1Item.Id.ToString());

            ServerOperations.Events().CreateAllDayEvent(Event2Title, string.Empty, this.event2Start, this.event2End);
            var event2Item = EventsManager.GetManager().GetEvents().Where<Event>(ni => ni.Status == Telerik.Sitefinity.GenericContent.Model.ContentLifecycleStatus.Live
                && ni.Title == Event2Title).FirstOrDefault();
            ServerArrangementContext.GetCurrent().Values.Add("event2Id", event2Item.Id.ToString());
        }

        /// <summary>
        /// Deletes all Events and Pages
        /// </summary>
        [ServerTearDown]
        public void OnAfterTestCompletes()
        {
            ServerOperations.Pages().DeleteAllPages();
            ServerOperations.Events().DeleteAllEvents();
        }

        private const string Event1Title = "Event1Title";
        private readonly DateTime event1Start = new DateTime(2016, 1, 10, 0, 0, 0, DateTimeKind.Utc);
        private readonly DateTime event1End = new DateTime(2016, 1, 13, 0, 0, 0, DateTimeKind.Utc);

        private const string Event2Title = "Event2Title";
        private readonly DateTime event2Start = new DateTime(2016, 4, 10, 0, 0, 0, DateTimeKind.Utc);
        private readonly DateTime event2End = new DateTime(2016, 4, 13, 0, 0, 0, DateTimeKind.Utc);

        private const string PageTitle = "EventsPage";
        private const string TemplateTitle = "default";
        private const string PlaceHolderId = "Contentplaceholder1";
    }
}
