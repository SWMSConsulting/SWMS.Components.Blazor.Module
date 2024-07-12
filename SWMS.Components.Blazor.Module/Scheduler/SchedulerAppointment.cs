using DevExpress.ExpressApp.DC;
using DevExpress.ExpressApp.Filtering;
using DevExpress.ExpressApp.Model;
using DevExpress.ExpressApp.SystemModule.Notifications;
using DevExpress.ExpressApp;
using DevExpress.Persistent.Base;
using DevExpress.Persistent.Base.General;
using DevExpress.Persistent.Validation;
using DevExpress.Utils;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Xml;
using DevExpress.XtraScheduler.Xml;

namespace SWMS.Components.Blazor.Module.Scheduler
{
    public abstract class SchedulerAppointment: IXafEntityObject, IObjectSpaceLink, IEvent, IReminderEvent, ISupportNotifications //, IRecurrentEvent
    {
        [NotMapped]
        public abstract SchedulerResource? SchedulerResource { get; set; }

        [NotMapped]
        public abstract SchedulerActivity? SchedulerActivity { get; set; }


        #region IEvent
        [FieldSize(250)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        [NotMapped]
        public string Subject { get { return SchedulerActivity?.Summary ?? ""; } set { } }

        [FieldSize(-1)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual string Description { get; set; } = "";

        [ModelDefault("DisplayFormat", "{0:dd.MM.yyyy hh:mm}"), ModelDefault("EditMask", "dd.MM.yyyy  hh:mm")]
        public virtual DateTime? StartOn { get; set; }

        DateTime IEvent.StartOn
        {
            get
            {
                if (!StartOn.HasValue)
                {
                    return DateTime.MinValue;
                }

                return StartOn.Value;
            }
            set
            {
                StartOn = value;
            }
        }

        [ModelDefault("DisplayFormat", "{0:dd.MM.yyyy hh:mm}"), ModelDefault("EditMask", "dd.MM.yyyy  hh:mm")]
        public virtual DateTime? EndOn { get; set; }

        DateTime IEvent.EndOn
        {
            get
            {
                if (!EndOn.HasValue)
                {
                    return DateTime.MinValue;
                }

                return EndOn.Value;
            }
            set
            {
                EndOn = value;
            }
        }


        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual bool AllDay { get; set; }

        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual string Location { get; set; }

        [ImmediatePostData]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual int Label { get; set; }

        public virtual int Status { get; set; }

        [Browsable(false)]
        public virtual int Type { get; set; }


        private bool isUpdateResourcesDelayed;

        private string resourceId;

        [NotMapped]
        [Browsable(false)]
        public virtual string ResourceId
        {
            get
            {
                if (resourceId == null)
                {
                    UpdateResourceIds();
                }

                return resourceId;
            }
            set
            {
                if (resourceId != value)
                {
                    resourceId = value;
                    if (ObjectSpace != null)
                    {
                        UpdateResources();
                    }
                    else
                    {
                        isUpdateResourcesDelayed = true;
                    }
                }
            }
        }

        [NotMapped]
        [Browsable(false)]
        public object ResourceIdBlazor
        {
            get
            {
                return SchedulerResource.ID;
            }
            set
            {
                SchedulerResource = null;

                if (value != null)
                {
                    SchedulerResource = ObjectSpace.GetObjectByKey<SchedulerResource>(value);
                }
            }
        }
        private void UpdateResources()
        {
            SchedulerResource = null;

            if (string.IsNullOrEmpty(resourceId))
            {
                return;
            }

            XmlDocument xmlDocument = SafeXml.CreateDocument(resourceId);
            foreach (XmlNode childNode in xmlDocument.DocumentElement.ChildNodes)
            {
                AppointmentResourceIdXmlLoader appointmentResourceIdXmlLoader = new AppointmentResourceIdXmlLoader(childNode);
                object key = appointmentResourceIdXmlLoader.ObjectFromXml();
                object objectByKey = ObjectSpace.GetObjectByKey(typeof(SchedulerResource), key);
                if (objectByKey != null)
                {
                    SchedulerResource = (SchedulerResource)objectByKey;
                }
            }
        }

        public void UpdateResourceIds()
        {
            resourceId = "<ResourceIds>\r\n";
            if (SchedulerResource != null)
            {
                resourceId += $"<ResourceId Type=\"{SchedulerResource.ID.GetType().FullName}\" Value=\"{SchedulerResource.ID}\" />\r\n";
            }

            resourceId += "</ResourceIds>";
        }

        [Browsable(false)]
        public object AppointmentId => ID;

        [Browsable(false)]
        [RuleFromBoolProperty("IsIntervalValid", DefaultContexts.Save, "The start date must be less than the end date", SkipNullOrEmptyValues = false, UsedProperties = "StartOn, EndOn")]
        public bool IsIntervalValid => StartOn <= EndOn;
        #endregion

        #region IRecurrenceEvent
        /*
        [StringLength(300)]
        [NonCloneable]
        [DisplayName("Recurrence")]
        [FieldSize(-1)]
        [VisibleInDetailView(false), VisibleInListView(false), VisibleInLookupListView(false)]
        public virtual string RecurrenceInfoXml { get; set; }

        [Browsable(false)]
        public virtual SchedulerAppointment RecurrencePattern { get; set; }

        [Browsable(false)]
        public virtual IList<SchedulerAppointment> RecurrenceEvents { get; set; } = new ObservableCollection<SchedulerAppointment>();

        IRecurrentEvent IRecurrentEvent.RecurrencePattern
        {
            get
            {
                return RecurrencePattern;
            }
            set
            {
                RecurrencePattern = (SchedulerAppointment)value;
            }
        }

        [NotMapped]
        [Browsable(false)]
        public string RecurrenceInfoXmlBlazor
        {
            get
            {
                return RecurrenceInfoXml?.ToNewRecurrenceInfoXml();
            }
            set
            {
                RecurrenceInfoXml = value?.ToOldRecurrenceInfoXml();
            }
        }
        */
        #endregion

        #region IReminderEvent

        private IList<PostponeTime> postponeTimes;

        [Browsable(false)]
        [NotMapped]
        public IEnumerable<PostponeTime> PostponeTimeList
        {
            get
            {
                if (postponeTimes == null)
                {
                    postponeTimes = CreatePostponeTimes();
                }

                return postponeTimes;
            }
        }

        public event EventHandler<CustomizeNotificationsPostponeTimeListEventArgs> CustomizeReminderTimeLookup;
        private IList<PostponeTime> CreatePostponeTimes()
        {
            IList<PostponeTime> list = PostponeTime.CreateDefaultPostponeTimesList();
            list.Add(new PostponeTime("None", null, "None"));
            list.Add(new PostponeTime("AtStartTime", TimeSpan.Zero, "0 minutes"));
            CustomizeNotificationsPostponeTimeListEventArgs customizeNotificationsPostponeTimeListEventArgs = new CustomizeNotificationsPostponeTimeListEventArgs(list);
            if (this.CustomizeReminderTimeLookup != null)
            {
                this.CustomizeReminderTimeLookup(this, customizeNotificationsPostponeTimeListEventArgs);
            }

            PostponeTime.SortPostponeTimesList(customizeNotificationsPostponeTimeListEventArgs.PostponeTimesList);
            return customizeNotificationsPostponeTimeListEventArgs.PostponeTimesList;
        }

        [Browsable(false)]
        public virtual TimeSpan? RemindIn { get; set; }

        [ImmediatePostData]
        [NotMapped]
        [ModelDefault("AllowClear", "False")]
        [DataSourceProperty("PostponeTimeList", new string[] { })]
        [SearchMemberOptions(SearchMemberMode.Exclude)]
        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        public virtual PostponeTime ReminderTime
        {
            get
            {
                if (RemindIn.HasValue)
                {
                    return PostponeTimeList.Where((PostponeTime x) => x.RemindIn.HasValue && x.RemindIn.Value == RemindIn.Value).FirstOrDefault();
                }

                return PostponeTimeList.Where((PostponeTime x) => !x.RemindIn.HasValue).FirstOrDefault();
            }
            set
            {
                if (value != null)
                {
                    if (value.RemindIn.HasValue)
                    {
                        RemindIn = value.RemindIn.Value;
                    }
                    else
                    {
                        RemindIn = null;
                    }
                }
            }
        }

        private string reminderInfoXml;

        [NonCloneable]
        [Browsable(false)]
        [StringLength(200)]
        public virtual string ReminderInfoXml
        {
            get
            {
                return reminderInfoXml;
            }
            set
            {
                if (reminderInfoXml != value)
                {
                    reminderInfoXml = value;
                    if (ObjectSpace != null)
                    {
                        UpdateAlarmTime();
                    }
                }
            }
        }

        private void UpdateAlarmTime()
        {
            if (!string.IsNullOrEmpty(ReminderInfoXml))
            {
                AppointmentReminderInfo appointmentReminderInfo = new AppointmentReminderInfo();
                try
                {
                    appointmentReminderInfo.FromXml(ReminderInfoXml);
                    AlarmTime = appointmentReminderInfo.ReminderInfos[0].AlertTime;
                    return;
                }
                catch (XmlException exception)
                {
                    Tracing.Tracer.LogError(exception);
                    return;
                }
            }

            AlarmTime = null;
            RemindIn = null;
            IsPostponed = false;
        }
        #endregion

        #region ISupportNotifications


        [Browsable(false)]
        public object UniqueId => ID;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public string NotificationMessage => Subject;

        private DateTime? alarmTime;

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public virtual DateTime? AlarmTime
        {
            get
            {
                return alarmTime;
            }
            set
            {
                if (!(alarmTime != value))
                {
                    return;
                }

                alarmTime = value;
                if (ObjectSpace != null)
                {
                    if (!value.HasValue)
                    {
                        RemindIn = null;
                        IsPostponed = false;
                    }

                    UpdateRemindersInfoXml(UpdateAlarmTime: true);
                }
            }
        }
        private void UpdateRemindersInfoXml(bool UpdateAlarmTime)
        {
            if (RemindIn.HasValue && AlarmTime.HasValue)
            {
                AppointmentReminderInfo appointmentReminderInfo = new AppointmentReminderInfo();
                ReminderInfo reminderInfo = new ReminderInfo();
                reminderInfo.TimeBeforeStart = RemindIn.Value;
                DateTime dateTime = (StartOn.HasValue ? StartOn.Value : DateTime.MinValue);
                if (UpdateAlarmTime)
                {
                    reminderInfo.AlertTime = AlarmTime.Value;
                }
                else
                {
                    reminderInfo.AlertTime = dateTime - RemindIn.Value;
                }

                appointmentReminderInfo.ReminderInfos.Add(reminderInfo);
                ReminderInfoXml = appointmentReminderInfo.ToXml();
            }
            else
            {
                ReminderInfoXml = null;
            }
        }

        [VisibleInDetailView(false)]
        [VisibleInListView(false)]
        [VisibleInLookupListView(false)]
        public virtual bool IsPostponed { get; set; }
        #endregion


        #region IObjectSpaceLink
        protected IObjectSpace ObjectSpace;

        IObjectSpace IObjectSpaceLink.ObjectSpace
        {
            get
            {
                return ObjectSpace;
            }
            set
            {
                ObjectSpace = value;
            }
        }
        #endregion

        #region IXafEntityObject

        [Key]
        [VisibleInListView(false)]
        [VisibleInDetailView(false)]
        [VisibleInLookupListView(false)]
        public virtual Guid ID { get; set; }

        public virtual void OnCreated() { }
        public virtual void OnSaving() { }
        public virtual void OnLoaded() { }
        #endregion
    }
}