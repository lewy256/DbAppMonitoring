using MonitoringApp.Models;

namespace MonitoringApp.ViewModels {
    public class EventsViewModel : BaseViewModel {

        private ChartValues<long> eventFg = new ChartValues<long>();
        public ChartValues<long> EventFg {
            get { return eventFg; }
            set {
                if (eventFg != value) {
                    eventFg = value;
                    OnPropertyChanged();
                }
            }
        }

        private ChartValues<long> eventBg = new ChartValues<long>();
        public ChartValues<long> EventBg {
            get { return eventBg; }
            set {
                if (eventBg != value) {
                    eventBg = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> avgEventFg = new ChartValues<double>();
        public ChartValues<double> AvgEventFg {
            get { return avgEventFg; }
            set {
                if (avgEventFg != value) {
                    avgEventFg = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> avgEventBg = new ChartValues<double>();
        public ChartValues<double> AvgEventBg {
            get { return avgEventBg; }
            set {
                if (avgEventBg != value) {
                    avgEventBg = value;
                    OnPropertyChanged();
                }
            }
        }

        private ChartValues<double> application = new ChartValues<double>();
        public ChartValues<double> Application {
            get { return application; }
            set {
                if (application != value) {
                    application = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> scheduler = new ChartValues<double>();
        public ChartValues<double> Scheduler {
            get { return scheduler; }
            set {
                if (scheduler != value) {
                    scheduler = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> systemIO = new ChartValues<double>();
        public ChartValues<double> SystemIO {
            get { return systemIO; }
            set {
                if (systemIO != value) {
                    systemIO = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> network = new ChartValues<double>();
        public ChartValues<double> Network {
            get { return network; }
            set {
                if (network != value) {
                    network = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> userIO = new ChartValues<double>();
        public ChartValues<double> UserIO {
            get { return userIO; }
            set {
                if (userIO != value) {
                    userIO = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> commit = new ChartValues<double>();
        public ChartValues<double> Commit {
            get { return commit; }
            set {
                if (commit != value) {
                    commit = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> administrative = new ChartValues<double>();
        public ChartValues<double> Administrative {
            get { return administrative; }
            set {
                if (administrative != value) {
                    administrative = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> other = new ChartValues<double>();
        public ChartValues<double> Other {
            get { return other; }
            set {
                if (other != value) {
                    other = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> concurrency = new ChartValues<double>();
        public ChartValues<double> Concurrency {
            get { return concurrency; }
            set {
                if (concurrency != value) {
                    concurrency = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> configuration = new ChartValues<double>();
        public ChartValues<double> Configuration {
            get { return configuration; }
            set {
                if (configuration != value) {
                    configuration = value;
                    OnPropertyChanged();
                }
            }
        }
        private ChartValues<double> cpu = new ChartValues<double>();
        public ChartValues<double> CPU {
            get { return cpu; }
            set {
                if (cpu != value) {
                    cpu = value;
                    OnPropertyChanged();
                }
            }
        }

        public ChartValues<string> labelsClassEvent = new ChartValues<string>();
        public ChartValues<string> LabelsClassEvent {
            get { return labelsClassEvent; }
            set {
                if (labelsClassEvent != value) {
                    labelsClassEvent = value;
                    OnPropertyChanged();
                }
            }
        }
        public ChartValues<string> labelsFgEvent = new ChartValues<string>();
        public ChartValues<string> LabelsFgEvent {
            get { return labelsFgEvent; }
            set {
                if (labelsFgEvent != value) {
                    labelsFgEvent = value;
                    OnPropertyChanged();
                }
            }
        }
        public ChartValues<string> labelsBgEvent = new ChartValues<string>();
        public ChartValues<string> LabelsBgEvent {
            get { return labelsBgEvent; }
            set {
                if (labelsBgEvent != value) {
                    labelsBgEvent = value;
                    OnPropertyChanged();
                }
            }
        }


        public ICommand RefreshCommand { get; set; }

        public EventsViewModel() {
            RefreshCommand = new BaseCommand(Refresh);
            TakeEventData();
            EventClassData();
            Formatter = val => val.ToString("P");
        }

        public Func<double, string> Formatter { get; set; }

        public void EventClassData() {
            try {
                int beginId = ((App)System.Windows.Application.Current).beginId;
                int endId = ((App)System.Windows.Application.Current).endId;

                using (var ctx = new Entities()) {
                    var listId = ctx.SNAPSHOT_PARMS.Where(x => x.SNAP_ID <= endId && x.SNAP_ID >= beginId).OrderBy(x => x.SNAP_ID).AsEnumerable()
                        .Select(x => x.SNAP_ID).ToList();

                    for (int i = 0; i < listId.Count; i++) {
                        var templist = listId.Skip(i).Take(2);

                        var begin = templist.FirstOrDefault();
                        var end = templist.LastOrDefault();

                        var sub = (
                            from b in ctx.WAIT_CLASS_PARMS
                            from e in ctx.WAIT_CLASS_PARMS
                            where b.SNAP_ID == begin && e.SNAP_ID == end && e.WAIT_CLASS == b.WAIT_CLASS
                            select new {
                                TIME_WAITED = e.TIME_WAITED - b.TIME_WAITED,
                                WAIT_CLASS = e.WAIT_CLASS
                            }).ToList();

                        var sum = sub.Sum(x => x.TIME_WAITED);


                        if (sum > 0) {
                            Application.AddRange(sub.Where(x => x.WAIT_CLASS == "Application")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Scheduler.AddRange(sub.Where(y => y.WAIT_CLASS == "Scheduler")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));

                            SystemIO.AddRange(sub.Where(x => x.WAIT_CLASS == "SystemIO")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Network.AddRange(sub.Where(x => x.WAIT_CLASS == "Network")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            UserIO.AddRange(sub.Where(x => x.WAIT_CLASS == "UserIO")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Commit.AddRange(sub.Where(x => x.WAIT_CLASS == "Commit")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Administrative.AddRange(sub.Where(x => x.WAIT_CLASS == "Administrative")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Other.AddRange(sub.Where(x => x.WAIT_CLASS == "Other")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Concurrency.AddRange(sub.Where(x => x.WAIT_CLASS == "Concurrency")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            Configuration.AddRange(sub.Where(x => x.WAIT_CLASS == "Configuration")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                            CPU.AddRange(sub.Where(x => x.WAIT_CLASS == "CPU")
                                .Select(x => (double)x.TIME_WAITED / sum * 100));
                        }

                        LabelsClassEvent.Add(ctx.SNAPSHOT_PARMS.Where(x => x.SNAP_ID == end).AsEnumerable()
                            .Select(x => x.SNAP_TIME.ToString("MM/dd/yyyy h:mm tt")).FirstOrDefault());


                    }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }


        public void TakeEventData() {
            int beginId = ((App)System.Windows.Application.Current).beginId;
            int endId = ((App)System.Windows.Application.Current).endId;
            try {

                using (var ctx = new Entities()) {
                    var resultFg = (
                        from b in ctx.SYSTEM_EVENT_FG_PARMS
                        from e in ctx.SYSTEM_EVENT_FG_PARMS
                        orderby e.TIME_WAITED_FG - b.TIME_WAITED_FG descending
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.EVENT == e.EVENT &&
                              e.TOTAL_WAITS_FG - b.TOTAL_WAITS_FG > 0
                        select new {
                            EVENT = e.EVENT,
                            TIME_WAITED_FG = e.TIME_WAITED_FG - b.TIME_WAITED_FG,
                            TOTAL_WAIT = e.TOTAL_WAITS_FG - b.TOTAL_WAITS_FG,
                            AVERAGE_WAIT = (double)(e.TIME_WAITED_FG - b.TIME_WAITED_FG) / (e.TOTAL_WAITS_FG - b.TOTAL_WAITS_FG)
                        }).Take(5).ToList();

                    var resultBg = (
                        from b in ctx.SYSTEM_EVENT_BG_PARMS
                        from e in ctx.SYSTEM_EVENT_BG_PARMS
                        orderby e.TIME_WAITED - b.TIME_WAITED descending
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.EVENT == e.EVENT &&
                              e.TOTAL_WAITS - b.TOTAL_WAITS > 0
                        select new {
                            EVENT = e.EVENT,
                            TIME_WAITED_BG = e.TIME_WAITED - b.TIME_WAITED,
                            TOTAL_WAIT = e.TOTAL_WAITS - b.TOTAL_WAITS,
                            AVERAGE_WAIT = (double)(e.TIME_WAITED - b.TIME_WAITED) / (e.TOTAL_WAITS - b.TOTAL_WAITS)
                        }).Take(5).ToList();

                    EventFg.AddRange(resultFg.Select(x => x.TIME_WAITED_FG));

                    EventBg.AddRange(resultBg.Select(x => x.TIME_WAITED_BG));

                    AvgEventFg.AddRange(resultFg.Select(x => x.AVERAGE_WAIT));
                    AvgEventBg.AddRange(resultBg.Select(x => x.AVERAGE_WAIT));

                    LabelsFgEvent.AddRange(resultFg.Select(x => x.EVENT));
                    LabelsBgEvent.AddRange(resultBg.Select(x => x.EVENT));
                }


            } catch (Exception e) {
                Console.WriteLine(e);
                throw;
            }
        }



        public void Refresh(object obj) {
            EventFg.Clear();
            EventBg.Clear();
            LabelsBgEvent.Clear();
            LabelsFgEvent.Clear();
            LabelsClassEvent.Clear();
            CPU.Clear();
            Application.Clear();
            Commit.Clear();
            Concurrency.Clear();
            Configuration.Clear();
            Administrative.Clear();
            Network.Clear();
            UserIO.Clear();
            Scheduler.Clear();
            SystemIO.Clear();
            Other.Clear();
            AvgEventFg.Clear();
            AvgEventBg.Clear();

            TakeEventData();
            EventClassData();
        }

    }


}

