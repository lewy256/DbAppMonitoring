using MonitoringApp.Models;


namespace MonitoringApp.ViewModels {

    public class InstanceEffieciencyViewModel : BaseViewModel {

        private double bufferRatio;
        private double latchRatio;
        private double libraryRatio;
        private double dictionaryRatio;
        private double sortRatio;
        private double rollbackRatio;

        private string bufferRatioAlert;
        private string latchRatioAlert;
        private string libraryRatioAlert;
        private string dictionaryRatioAlert;
        private string sortRatioAlert;
        private string rollbackAlert;

        public double RollbackRatio {
            get { return rollbackRatio; }
            set {
                if (rollbackRatio != value) {
                    rollbackRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public double BufferRatio {
            get { return bufferRatio; }
            set {
                if (bufferRatio != value) {
                    bufferRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public double LatchRatio {
            get { return latchRatio; }
            set {
                if (latchRatio != value) {
                    latchRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public double LibraryRatio {
            get { return libraryRatio; }
            set {
                if (libraryRatio != value) {
                    libraryRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public double DictionaryRatio {
            get { return dictionaryRatio; }
            set {
                if (dictionaryRatio != value) {
                    dictionaryRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public double SortRatio {
            get { return sortRatio; }
            set {
                if (sortRatio != value) {
                    sortRatio = value;
                    OnPropertyChanged();
                }
            }
        }

        public string RollbackRatioAlert {
            get { return rollbackAlert; }
            set {
                if (rollbackAlert != value) {
                    rollbackAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string BufferRatioAlert {
            get { return bufferRatioAlert; }
            set {
                if (bufferRatioAlert != value) {
                    bufferRatioAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LatchRatioAlert {
            get { return latchRatioAlert; }
            set {
                if (latchRatioAlert != value) {
                    latchRatioAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string LibraryRatioAlert {
            get { return libraryRatioAlert; }
            set {
                if (libraryRatioAlert != value) {
                    libraryRatioAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string DictionaryRatioAlert {
            get { return dictionaryRatioAlert; }
            set {
                if (dictionaryRatioAlert != value) {
                    dictionaryRatioAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public string SortRatioAlert {
            get { return sortRatioAlert; }
            set {
                if (sortRatioAlert != value) {
                    sortRatioAlert = value;
                    OnPropertyChanged();
                }
            }
        }

        public ICommand RefreshCommand { get; set; }

        public InstanceEffieciencyViewModel() {
            RefreshCommand = new BaseCommand(Refresh);
            TakeValues();
            DisplayAlert();

        }


        public void Refresh(object obj) {
            TakeValues();
            DisplayAlert();
        }

        public void TakeValues() {
            try {
                int beginId = ((App)Application.Current).beginId;
                int endId = ((App)Application.Current).endId;

                using (var ctx = new Entities()) {
                    var libraryRatio = (
                    from b in ctx.LIBRARY_CACHE_PARMS
                    from e in ctx.LIBRARY_CACHE_PARMS
                    where b.SNAP_ID == beginId && e.SNAP_ID == endId
                    select new {
                        LIBRARYRATIO = (double)((e.PINS - b.PINS) - (e.RELOADS - b.RELOADS)) / (e.PINS - b.PINS) * 100
                    }).ToList();

                    LibraryRatio = Math.Round(libraryRatio[0].LIBRARYRATIO, 2);


                    var dictionaryRatio = (
                        from b in ctx.ROW_CACHE_PARMS
                        from e in ctx.ROW_CACHE_PARMS
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId
                        select new {
                            DICTIONARYRATIO = (double)((e.GETS - b.GETS) - (e.GETMISSES - b.GETMISSES)) / (e.GETS - b.GETS) * 100
                        }).ToList();

                    DictionaryRatio = Math.Round(dictionaryRatio[0].DICTIONARYRATIO, 2);

                    var bufferRatio = (
                        from a1 in ctx.SYSSTAT_PARMS
                        from a2 in ctx.SYSSTAT_PARMS
                        from b1 in ctx.SYSSTAT_PARMS
                        from b2 in ctx.SYSSTAT_PARMS
                        from c1 in ctx.SYSSTAT_PARMS
                        from c2 in ctx.SYSSTAT_PARMS
                        from d1 in ctx.SYSSTAT_PARMS
                        from d2 in ctx.SYSSTAT_PARMS
                        where a1.SNAP_ID == beginId && b1.SNAP_ID == beginId && c1.SNAP_ID == beginId && d1.SNAP_ID == beginId && a2.SNAP_ID == endId && b2.SNAP_ID == endId && c2.SNAP_ID == endId && d2.SNAP_ID == endId
                              && a1.STAT_NAME == "physical reads" && a2.STAT_NAME == "physical reads" && b1.STAT_NAME == "physical reads direct (lob)" && b2.STAT_NAME == "physical reads direct (lob)" && c1.STAT_NAME == "physical reads direct" && c2.STAT_NAME == "physical reads direct"
                              && d1.STAT_NAME == "session logical reads" && d2.STAT_NAME == "session logical reads"
                        select new {
                            BUFFERRATIO = (double)(1 - ((a2.VAL - a1.VAL) - (b2.VAL - b1.VAL) - (c2.VAL - c1.VAL)) / (d2.VAL - d1.VAL)) * 100
                        }).ToList();

                    BufferRatio = Math.Round(bufferRatio[0].BUFFERRATIO, 2);

                    var sortRatio = (
                        from a1 in ctx.SYSSTAT_PARMS
                        from a2 in ctx.SYSSTAT_PARMS
                        from b1 in ctx.SYSSTAT_PARMS
                        from b2 in ctx.SYSSTAT_PARMS
                        where a1.SNAP_ID == beginId && b1.SNAP_ID == beginId && a2.SNAP_ID == endId && b2.SNAP_ID == endId && a1.STAT_NAME == "sorts (memory)" && b1.STAT_NAME == "sorts (disk)" && a2.STAT_NAME == "sorts (memory)" && b2.STAT_NAME == "sorts (disk)"
                        select new {
                            SORTRATIO = 100 * (double)((a2.VAL - a1.VAL) - (b2.VAL - b1.VAL)) / (a2.VAL - a1.VAL)
                        }).ToList();

                    SortRatio = Math.Round(sortRatio[0].SORTRATIO, 2);


                    var latchRatio = (
                        from b in ctx.LATCH_PARMS
                        from e in ctx.LATCH_PARMS
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId
                        select new {
                            LATCHRATIO = (double)(1 - (e.MISSES - b.MISSES) / (e.GETS - b.GETS)) * 100
                        }).ToList();

                    LatchRatio = Math.Round(latchRatio[0].LATCHRATIO, 2);


                    var rollbackRatio = (
                        from b in ctx.ROLLBACK_PARMS
                        from e in ctx.ROLLBACK_PARMS
                        where b.SNAP_ID == beginId && e.SNAP_ID == endId
                        select new {
                            ROLLBACKRATIO = (double)(e.WAITS - b.WAITS) / (e.GETS - b.GETS) * 100
                        }).ToList();

                    RollbackRatio = Math.Round(rollbackRatio[0].ROLLBACKRATIO, 2);
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public void DisplayAlert() {
            try {
                BufferRatioAlert = BufferRatio < 90 ? "Increase DB_BLOCK_BUFFERS" : "Correct value";
                LatchRatioAlert = LatchRatio < 99 ? "Increase number of latches" : "Correct value";
                DictionaryRatioAlert = DictionaryRatio < 85 ? "Increase SHARED_POOL_SIZE" : "Correct value";
                SortRatioAlert = sortRatio < 95 ? "Increase SORT_AREA_SIZE parameter" : "Correct value";
                LibraryRatioAlert = libraryRatio < 99 ? "Increase SHARED_POOL_SIZE" : "Correct value";
                RollbackRatioAlert = rollbackRatio > 5 ? "Increase number of Rollback Segments" : "Correct value";
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}

