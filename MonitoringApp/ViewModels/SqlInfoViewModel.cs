using MonitoringApp.Models;

namespace MonitoringApp.ViewModels {
    public class SqlInfoViewModel : BaseViewModel {

        private ObservableCollection<ExecutionStats> execStatsValues = new ObservableCollection<ExecutionStats>();
        private ObservableCollection<OtherStats> otherStatsValues = new ObservableCollection<OtherStats>();

        public string[] BreakdownLabels { get; set; }

        public ChartValues<decimal> BreakdownStats { get; set; } = new ChartValues<decimal>();

        public SeriesCollection SeriesCollection { get; set; } = new SeriesCollection();

        public ObservableCollection<ExecutionStats> ExecStatsValues {
            get { return execStatsValues; }
            set {
                if (execStatsValues != value) {
                    execStatsValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<OtherStats> OtherStatsValues {
            get { return otherStatsValues; }
            set {
                if (otherStatsValues != value) {
                    otherStatsValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SqlidBox { get; set; }

        public ICommand RefreshCommand { get; set; }

        public void Refresh(object obj) {
            TakeData();
        }

        public SqlInfoViewModel() {
            RefreshCommand = new BaseCommand(Refresh);

            BreakdownLabels = new[] {
                "SQL Time", "PL / SQL Time", "Java Time"
            };
        }

        public void TakeData() {
            try {
                BreakdownStats.Clear();
                SeriesCollection.Clear();
                otherStatsValues.Clear();
                execStatsValues.Clear();

                int beginId = ((App)Application.Current).beginId;
                int endId = ((App)Application.Current).endId;

                using (var ctx = new Entities()) {
                    var result = (
                        from b in ctx.SQL_INFO_PARMS
                        from e in ctx.SQL_INFO_PARMS
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == SqlidBox && e.SQL_ID == SqlidBox
                        select new {
                            ELAPSED_TIME = e.ELAPSED_TIME - b.ELAPSED_TIME,
                            CPU_TIME = e.CPU_TIME - b.CPU_TIME,
                            EXECUTIONS = e.EXECUTIONS - b.EXECUTIONS,
                            ROWS_PROCESSED = e.ROWS_PROCESSED - b.ROWS_PROCESSED,
                            APPLICATION_WAIT_TIME = e.APPLICATION_WAIT_TIME - b.APPLICATION_WAIT_TIME,
                            CONCURRENCY_WAIT_TIME = e.CONCURRENCY_WAIT_TIME - b.CONCURRENCY_WAIT_TIME,
                            FETCHES = e.FETCHES - b.FETCHES,
                            BUFFER_GETS = e.BUFFER_GETS - b.BUFFER_GETS,
                            DIRECT_WRITES = e.DIRECT_WRITES - b.DIRECT_WRITES,
                            DISK_READS = e.DISK_READS - b.DISK_READS,
                            USER_IO_WAIT_TIME = e.USER_IO_WAIT_TIME - b.USER_IO_WAIT_TIME,
                            PLSQL_EXEC_TIME = e.PLSQL_EXEC_TIME - b.PLSQL_EXEC_TIME,
                            JAVA_EXEC_TIME = e.JAVA_EXEC_TIME - b.JAVA_EXEC_TIME,
                            SERIALIZABLE_ABORTS = e.SERIALIZABLE_ABORTS - b.SERIALIZABLE_ABORTS,
                            PARSE_CALLS = e.PARSE_CALLS - b.PARSE_CALLS,
                            LOADS = e.LOADS - b.LOADS,
                            INVALIDATIONS = e.INVALIDATIONS - b.INVALIDATIONS,
                            X1 = b.ELAPSED_TIME - b.CPU_TIME - b.APPLICATION_WAIT_TIME - b.CONCURRENCY_WAIT_TIME - b.USER_IO_WAIT_TIME,
                            X2 = e.ELAPSED_TIME - e.CPU_TIME - e.APPLICATION_WAIT_TIME - e.CONCURRENCY_WAIT_TIME - e.USER_IO_WAIT_TIME
                        }).ToList();

                    if (result[0].EXECUTIONS > 0) {
                        ExecStatsValues.Add(new ExecutionStats {
                            Per = "Executions",
                            ElapsedTime = (result[0].ELAPSED_TIME / result[0].EXECUTIONS) / 1000,
                            CpuTime = (result[0].CPU_TIME / result[0].EXECUTIONS) / 1000,
                            BufferGets = result[0].BUFFER_GETS / result[0].EXECUTIONS,
                            DiskReads = result[0].DISK_READS / result[0].EXECUTIONS,
                            DirectWrites = result[0].DIRECT_WRITES / result[0].EXECUTIONS,
                            Fetches = result[0].FETCHES / result[0].EXECUTIONS,
                            Executions = result[0].EXECUTIONS / result[0].EXECUTIONS,
                            Rows = result[0].ROWS_PROCESSED / result[0].EXECUTIONS
                        });

                        SeriesCollection.Add(new PieSeries { Title = "Remaining Waits", Values = new ChartValues<double> { (double)(result[0].X2 - result[0].X1) / result[0].ELAPSED_TIME } });
                        SeriesCollection.Add(new PieSeries { Title = "CPU", Values = new ChartValues<double> { (double)result[0].CPU_TIME / result[0].ELAPSED_TIME } });
                        SeriesCollection.Add(new PieSeries { Title = "User I/O Waits", Values = new ChartValues<double> { (double)result[0].USER_IO_WAIT_TIME / result[0].ELAPSED_TIME } });
                        SeriesCollection.Add(new PieSeries { Title = "Application Waits", Values = new ChartValues<double> { (double)result[0].APPLICATION_WAIT_TIME / result[0].ELAPSED_TIME } });
                        SeriesCollection.Add(new PieSeries { Title = "Concurrency Waits", Values = new ChartValues<double> { (double)result[0].CONCURRENCY_WAIT_TIME / result[0].ELAPSED_TIME } });
                    }

                    if (result[0].ROWS_PROCESSED > 0) {
                        ExecStatsValues.Add(new ExecutionStats {
                            Per = "Row",
                            ElapsedTime = (result[0].ELAPSED_TIME / result[0].ROWS_PROCESSED) / 1000,
                            CpuTime = (result[0].CPU_TIME / result[0].ROWS_PROCESSED) / 1000,
                            BufferGets = result[0].BUFFER_GETS / result[0].ROWS_PROCESSED,
                            DiskReads = result[0].DISK_READS / result[0].ROWS_PROCESSED,
                            DirectWrites = result[0].DIRECT_WRITES / result[0].ROWS_PROCESSED,
                            Fetches = result[0].FETCHES / result[0].ROWS_PROCESSED,
                            Executions = result[0].EXECUTIONS / result[0].ROWS_PROCESSED,
                            Rows = result[0].ROWS_PROCESSED / result[0].ROWS_PROCESSED
                        });
                    }


                    OtherStatsValues.Add(new OtherStats {
                        TotalParse = result[0].PARSE_CALLS,
                        LoadsHardParses = result[0].LOADS,
                        Invalidations = result[0].INVALIDATIONS,
                        SerializableAborts = result[0].SERIALIZABLE_ABORTS
                    });


                    BreakdownStats.Add((result[0].ELAPSED_TIME - result[0].PLSQL_EXEC_TIME - result[0].JAVA_EXEC_TIME) / 1000);
                    BreakdownStats.Add(result[0].PLSQL_EXEC_TIME / 1000);
                    BreakdownStats.Add(result[0].JAVA_EXEC_TIME / 1000);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }

        }
    }


    public class ExecutionStats {
        public string Per { get; set; }
        public long ElapsedTime { get; set; }
        public long CpuTime { get; set; }
        public long BufferGets { get; set; }
        public long DiskReads { get; set; }
        public long DirectWrites { get; set; }
        public long Fetches { get; set; }
        public long Executions { get; set; }
        public long Rows { get; set; }

    }

    public class OtherStats {
        public long TotalParse { get; set; }
        public long LoadsHardParses { get; set; }
        public long Invalidations { get; set; }
        public long SerializableAborts { get; set; }
    }
}

