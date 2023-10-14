using MonitoringApp.Models;

namespace MonitoringApp.ViewModels {
    public class StatisticsViewModel {

        public ObservableCollection<FileIO> FileIoValues { get; set; }
        public ObservableCollection<SessionActivity> SessionActivityValues { get; set; }
        public ObservableCollection<TimeModel> TimeModeValues { get; set; }

        public ICommand RefreshCommand { get; set; }

        public StatisticsViewModel() {
            FileIoValues = new ObservableCollection<FileIO>();
            SessionActivityValues = new ObservableCollection<SessionActivity>();
            TimeModeValues = new ObservableCollection<TimeModel>();
            RefreshCommand = new BaseCommand(Refresh);

            TakeParms();
        }

        public void Refresh(object obj) {
            FileIoValues.Clear();
            SessionActivityValues.Clear();
            TimeModeValues.Clear();
            TakeParms();
        }

        public void TakeParms() {
            try {
                int beginId = ((App)Application.Current).beginId;
                int endId = ((App)Application.Current).endId;

                using (var ctx = new Entities()) {
                    var dbTimeSess = (
                        from a1 in ctx.SESS_PARMS
                        from a2 in ctx.SESS_PARMS
                        orderby a2.VAL - a1.VAL descending
                        where a1.SNAP_ID == beginId && a2.SNAP_ID == endId && a2.SID_NUM == a1.SID_NUM
                        select new {
                            SID = a2.SID_NUM,
                            USERNAME = a2.USERNAME,
                            PROGRAM = a2.PROGRAM_NAME,
                            DBTIME_SESS = a2.VAL - a1.VAL
                        }).Take(10).ToList();

                    var totalDbTime = (
                        from a1 in ctx.SYS_PARMS
                        from a2 in ctx.SYS_PARMS
                        from b1 in ctx.SYS_PARMS
                        from b2 in ctx.SYS_PARMS
                        where a1.SNAP_ID == beginId && a2.SNAP_ID == endId && b1.SNAP_ID == beginId &&
                              b2.SNAP_ID == endId
                              && b1.STAT_NAME == "DB_TIME" && b2.STAT_NAME == "DB_TIME"
                        select new {
                            TOTAL_DBTIME = b2.VAL - b1.VAL
                        }).ToList();

                    var result = dbTimeSess
                        .Select(x => new SessionActivity() {
                            Activity = Math.Round((double)x.DBTIME_SESS / totalDbTime[0].TOTAL_DBTIME * 100, 2), Sid = x.SID,
                            Username = x.USERNAME, Program = x.PROGRAM
                        }).ToList();

                    foreach (var x in result) { SessionActivityValues.Add(x); }

                    var fileIO = (
                        from b in ctx.IO_PARMS
                        from e in ctx.IO_PARMS
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId && e.FILE_NAME == b.FILE_NAME
                        select new FileIO() {
                            FileName = e.FILE_NAME,
                            BlockReads = e.BLOCK_READS - b.BLOCK_READS,
                            BlockWrites = e.BLOCK_WRITES - b.BLOCK_WRITES,
                            WriteTime = e.WRITE_TIME - b.WRITE_TIME,
                            ReadTime = e.READ_TIME - b.READ_TIME,
                            TotalIO = e.TOTAL_IO - b.TOTAL_IO
                        }).ToList();

                    foreach (var x in fileIO) { FileIoValues.Add(x); }


                    var timeModel = (
                        from b in ctx.TIME_MODEL_PARMS
                        from e in ctx.TIME_MODEL_PARMS
                        orderby e.VAL - b.VAL descending
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.STAT_NAME == e.STAT_NAME
                        select new TimeModel() {
                            TimeValue = (e.VAL - b.VAL),
                            StatName = e.STAT_NAME
                        }).ToList();

                    var temp = timeModel.Select(x => new TimeModel() {
                        TimeAcitivty = Math.Round((double)x.TimeValue / timeModel.Sum(y => y.TimeValue) * 100, 2),
                        TimeValue = x.TimeValue,
                        StatName = x.StatName
                    }).ToList();

                    foreach (var x in temp) { TimeModeValues.Add(x); }
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    public class SessionActivity {
        public double Activity { get; set; }
        public long Sid { get; set; }
        public string Username { get; set; }
        public string Program { get; set; }
    }

    public class TimeModel {
        public string StatName { get; set; }
        public long TimeValue { get; set; }
        public double TimeAcitivty { get; set; }
    }

    public class FileIO {
        public string FileName { get; set; }
        public long BlockReads { get; set; }
        public long BlockWrites { get; set; }
        public long WriteTime { get; set; }
        public long ReadTime { get; set; }
        public long TotalIO { get; set; }
    }

}








