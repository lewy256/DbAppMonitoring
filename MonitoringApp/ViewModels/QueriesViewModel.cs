using MonitoringApp.Models;


namespace MonitoringApp.ViewModels {
    public class QueriesViewModel : BaseViewModel {

        private ObservableCollection<QueryParms> queryParmsValues;
        private ObservableCollection<double> percentValues;
        private string sortOption;

        public SeriesCollection SeriesCollection1 { get; set; }
        public SeriesCollection SeriesCollection2 { get; set; }
        public SeriesCollection SeriesCollection3 { get; set; }
        public SeriesCollection SeriesCollection4 { get; set; }
        public SeriesCollection SeriesCollection5 { get; set; }

        public ICommand RefreshCommand { get; set; }

        public ICommand NextCommand { get; set; }

        public ICommand BackCommand { get; set; }


        public ObservableCollection<QueryParms> QueryParmsValues {
            get { return queryParmsValues; }
            set {
                if (queryParmsValues != value) {
                    queryParmsValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public ObservableCollection<double> PercentValues {
            get { return percentValues; }
            set {
                if (percentValues != value) {
                    percentValues = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SortOption {
            get { return sortOption; }
            set {
                if (sortOption != value) {
                    sortOption = value;
                    OnPropertyChanged();
                }
            }
        }


        public QueriesViewModel() {
            RefreshCommand = new BaseCommand(Refresh);
            NextCommand = new BaseCommand(Next);
            BackCommand = new BaseCommand(Back);
            SeriesCollection1 = new SeriesCollection();
            SeriesCollection2 = new SeriesCollection();
            SeriesCollection3 = new SeriesCollection();
            SeriesCollection4 = new SeriesCollection();
            SeriesCollection5 = new SeriesCollection();
            QueryParmsValues = new ObservableCollection<QueryParms>();
            PercentValues = new ObservableCollection<double>();

            DisplayValues(0);


        }


        private int count;

        public void Next(object obj) {

            if (count < 5) {
                count++;
                DisplayValues(count);
            }

        }

        public void Back(object obj) {
            if (count >= 0) {
                count--;
                DisplayValues(count);
            }
        }

        public void Refresh(object obj) {
            DisplayValues(count);
        }

        public void DisplayValues(int sortOption) {

            QueryParmsValues.Clear();

            int beginId = ((App)Application.Current).beginId;
            int endId = ((App)Application.Current).endId;

            using (var ctx = new Entities()) {
                try {
                    var result = new List<QueryParms>();

                    switch (sortOption) {
                        case 0: {
                                SortOption = "(Sort by Elapsed time)";
                                result = (
                                from b in ctx.SQL_INFO_PARMS
                                from e in ctx.SQL_INFO_PARMS
                                orderby (e.ELAPSED_TIME - b.ELAPSED_TIME) / (e.EXECUTIONS - b.EXECUTIONS) descending
                                where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == e.SQL_ID &&
                                      e.EXECUTIONS - b.EXECUTIONS > 0
                                select new QueryParms() {
                                    SQLid = e.SQL_ID,
                                    Executions = e.EXECUTIONS - b.EXECUTIONS,
                                    ElapsedTime = e.ELAPSED_TIME - b.ELAPSED_TIME,
                                    CpuTime = e.CPU_TIME - b.CPU_TIME,
                                    BufferGets = e.BUFFER_GETS - b.BUFFER_GETS,
                                    DiskReads = e.DISK_READS - b.DISK_READS
                                }).Take(5).ToList();
                                break;
                            }

                        case 1: {
                                SortOption = "(Sort by CPU time)";
                                result = (
                                   from b in ctx.SQL_INFO_PARMS
                                   from e in ctx.SQL_INFO_PARMS
                                   orderby (e.CPU_TIME - b.CPU_TIME) / (e.EXECUTIONS - b.EXECUTIONS) descending
                                   where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == e.SQL_ID &&
                                         e.EXECUTIONS - b.EXECUTIONS > 0
                                   select new QueryParms() {
                                       SQLid = e.SQL_ID,
                                       Executions = e.EXECUTIONS - b.EXECUTIONS,
                                       ElapsedTime = e.ELAPSED_TIME - b.ELAPSED_TIME,
                                       CpuTime = e.CPU_TIME - b.CPU_TIME,
                                       BufferGets = e.BUFFER_GETS - b.BUFFER_GETS,
                                       DiskReads = e.DISK_READS - b.DISK_READS
                                   }).Take(5).ToList();
                                break;
                            }
                        case 2: {
                                SortOption = "(Sort by Buffer gets)";
                                result = (
                                    from b in ctx.SQL_INFO_PARMS
                                    from e in ctx.SQL_INFO_PARMS
                                    orderby (e.BUFFER_GETS - b.BUFFER_GETS) / (e.EXECUTIONS - b.EXECUTIONS) descending
                                    where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == e.SQL_ID &&
                                          e.EXECUTIONS - b.EXECUTIONS > 0
                                    select new QueryParms() {
                                        SQLid = e.SQL_ID,
                                        Executions = e.EXECUTIONS - b.EXECUTIONS,
                                        ElapsedTime = e.ELAPSED_TIME - b.ELAPSED_TIME,
                                        CpuTime = e.CPU_TIME - b.CPU_TIME,
                                        BufferGets = e.BUFFER_GETS - b.BUFFER_GETS,
                                        DiskReads = e.DISK_READS - b.DISK_READS
                                    }).Take(5).ToList();
                                break;
                            }
                        case 3: {
                                SortOption = "(Sort by Executions)";
                                result = (
                                    from b in ctx.SQL_INFO_PARMS
                                    from e in ctx.SQL_INFO_PARMS
                                    orderby e.EXECUTIONS - b.EXECUTIONS descending
                                    where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == e.SQL_ID &&
                                          e.EXECUTIONS - b.EXECUTIONS > 0
                                    select new QueryParms() {
                                        SQLid = e.SQL_ID,
                                        Executions = e.EXECUTIONS - b.EXECUTIONS,
                                        ElapsedTime = e.ELAPSED_TIME - b.ELAPSED_TIME,
                                        CpuTime = e.CPU_TIME - b.CPU_TIME,
                                        BufferGets = e.BUFFER_GETS - b.BUFFER_GETS,
                                        DiskReads = e.DISK_READS - b.DISK_READS
                                    }).Take(5).ToList();
                                break;
                            }
                        case 4: {
                                SortOption = "(Sort by Disk reads)";
                                result = (
                                    from b in ctx.SQL_INFO_PARMS
                                    from e in ctx.SQL_INFO_PARMS
                                    orderby (e.DISK_READS - b.DISK_READS) / (e.EXECUTIONS - b.EXECUTIONS) descending
                                    where b.SNAP_ID == beginId && e.SNAP_ID == endId && b.SQL_ID == e.SQL_ID &&
                                          e.EXECUTIONS - b.EXECUTIONS > 0
                                    select new QueryParms() {
                                        SQLid = e.SQL_ID,
                                        Executions = e.EXECUTIONS - b.EXECUTIONS,
                                        ElapsedTime = e.ELAPSED_TIME - b.ELAPSED_TIME,
                                        CpuTime = e.CPU_TIME - b.CPU_TIME,
                                        BufferGets = e.BUFFER_GETS - b.BUFFER_GETS,
                                        DiskReads = e.DISK_READS - b.DISK_READS
                                    }).Take(5).ToList();
                                break;
                            }
                    }

                    foreach (var x in result) {
                        QueryParmsValues.Add(new QueryParms() {
                            SQLid = x.SQLid,
                            Executions = x.Executions,
                            ElapsedTime = x.ElapsedTime / x.Executions / 1000,
                            CpuTime = x.CpuTime / x.Executions / 1000,
                            BufferGets = x.BufferGets / x.Executions,
                            DiskReads = x.DiskReads / x.Executions,
                        });
                    }
                } catch (Exception e) {
                    Console.WriteLine(e);
                    throw;
                }



            }

            AddChartsValues();
            CountPercent();
        }


        public void AddChartsValues() {
            SeriesCollection1.Clear();
            SeriesCollection2.Clear();
            SeriesCollection3.Clear();
            SeriesCollection4.Clear();
            SeriesCollection5.Clear();

            try {
                SeriesCollection1.Add(new PieSeries { Title = "Elapsed time", Values = new ChartValues<double> { QueryParmsValues[0].ElapsedTime } });
                SeriesCollection1.Add(new PieSeries { Title = "CPU time", Values = new ChartValues<double> { QueryParmsValues[0].CpuTime } });

                SeriesCollection2.Add(new PieSeries { Title = "Elapsed time", Values = new ChartValues<double> { QueryParmsValues[1].ElapsedTime } });
                SeriesCollection2.Add(new PieSeries { Title = "CPU time", Values = new ChartValues<double> { QueryParmsValues[1].CpuTime } });

                SeriesCollection3.Add(new PieSeries { Title = "Elapsed time", Values = new ChartValues<double> { QueryParmsValues[2].ElapsedTime } });
                SeriesCollection3.Add(new PieSeries { Title = "CPU time", Values = new ChartValues<double> { QueryParmsValues[2].CpuTime } });

                SeriesCollection4.Add(new PieSeries { Title = "Elapsed time", Values = new ChartValues<double> { QueryParmsValues[3].ElapsedTime } });
                SeriesCollection4.Add(new PieSeries { Title = "CPU time", Values = new ChartValues<double> { QueryParmsValues[3].CpuTime } });

                SeriesCollection5.Add(new PieSeries { Title = "Elapsed time", Values = new ChartValues<double> { QueryParmsValues[4].ElapsedTime } });
                SeriesCollection5.Add(new PieSeries { Title = "CPU time", Values = new ChartValues<double> { QueryParmsValues[4].CpuTime } });
            } catch (Exception e) {
                Console.WriteLine(e);
            }

        }

        public void CountPercent() {
            try {
                PercentValues.Clear();

                double sum1 = QueryParmsValues[0].ElapsedTime + QueryParmsValues[0].CpuTime;
                double sum2 = QueryParmsValues[1].ElapsedTime + QueryParmsValues[1].CpuTime;
                double sum3 = QueryParmsValues[2].ElapsedTime + QueryParmsValues[2].CpuTime;
                double sum4 = QueryParmsValues[3].ElapsedTime + QueryParmsValues[3].CpuTime;
                double sum5 = QueryParmsValues[4].ElapsedTime + QueryParmsValues[4].CpuTime;

                PercentValues.Add(Math.Round((QueryParmsValues[0].ElapsedTime / sum1 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[0].CpuTime / sum1 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[1].ElapsedTime / sum2 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[1].CpuTime / sum2 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[2].ElapsedTime / sum3 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[2].CpuTime / sum3 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[3].ElapsedTime / sum4 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[3].CpuTime / sum4 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[4].ElapsedTime / sum5 * 100), 1));
                PercentValues.Add(Math.Round((QueryParmsValues[4].CpuTime / sum5 * 100), 1));

            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }

    public class QueryParms {
        public string SQLid { get; set; }
        public long Executions { get; set; }
        public long ElapsedTime { get; set; }
        public long CpuTime { get; set; }
        public long BufferGets { get; set; }
        public long DiskReads { get; set; }
    }
}

