using MonitoringApp.Models;

namespace MonitoringApp.ViewModels {
    public class SettingsViewModel : BaseViewModel {

        private ObservableCollection<SNAPSHOT_PARMS> snapshotParms;

        public ObservableCollection<SNAPSHOT_PARMS> SnapshotParms {
            get { return snapshotParms; }
            set {
                if (snapshotParms != value) {
                    snapshotParms = value;
                    OnPropertyChanged();
                }
            }
        }

        public int Time { get; set; } = 1;
        public string Freq { get; set; } = "HOURLY";
        public string[] ComboBoxValues { get; set; }

        public int firstSnap { get; set; }
        public int lastSnap { get; set; }

        public ICommand SaveCommand { get; set; }
        public ICommand DeleteCommand { get; set; }

        public SettingsViewModel() {
            SaveCommand = new BaseCommand(SaveSnap);
            DeleteCommand = new BaseCommand(DeleteSnap);

            ComboBoxValues = new[] { "HOURLY", "DAILY", "MINUTELY" };

            try {
                using (var ctx = new Entities()) {
                    SnapshotParms = new ObservableCollection<SNAPSHOT_PARMS>(ctx.SNAPSHOT_PARMS.OrderBy(x => x.SNAP_ID));
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public void SaveSnap(object obj) {
            try {
                using (var ctx = new Entities()) {
                    ctx.CHANGE_JOB_PRC("INSERT_JOB", "repeat_interval", $"freq={Freq}; interval={Time};");
                    MessageBox.Show("Changed");
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }

        public void DeleteSnap(object obj) {
            try {
                using (var ctx = new Entities()) {
                    ctx.DELETE_SNAPSHOTS_PRC(firstSnap, lastSnap);

                    SnapshotParms = new ObservableCollection<SNAPSHOT_PARMS>(ctx.SNAPSHOT_PARMS.OrderBy(x => x.SNAP_ID));
                    MessageBox.Show("Removed");
                }
            } catch (Exception e) {
                Console.WriteLine(e);
            }
        }
    }
}
