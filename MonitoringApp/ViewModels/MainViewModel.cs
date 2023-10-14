namespace MonitoringApp.ViewModels {
    public class MainViewModel : BaseViewModel {
        public ICommand EventsCommand { get; set; }
        public ICommand QueriesCommand { get; set; }
        public ICommand StatisticsCommand { get; set; }
        public ICommand InstanceEffieciencyCommand { get; set; }
        public ICommand SettingsCommand { get; set; }
        public ICommand AccountCommand { get; set; }
        public ICommand SqlInfoCommand { get; set; }

        public MainViewModel() {
            EventsCommand = new BaseCommand(OpenEvents);
            QueriesCommand = new BaseCommand(OpenQueries);
            StatisticsCommand = new BaseCommand(OpenStatistics);
            InstanceEffieciencyCommand = new BaseCommand(InstanceEfficiency);
            SettingsCommand = new BaseCommand(OpenSettings);
            AccountCommand = new BaseCommand(OpenAccount);
            SqlInfoCommand = new BaseCommand(SqlInfo);
        }

        private object selectedViewModel;

        public object SelectedViewModel {
            get { return selectedViewModel; }
            set {
                selectedViewModel = value;
                OnPropertyChanged();
            }
        }


        private void OpenEvents(object obj) {
            SelectedViewModel = new EventsViewModel();
        }


        private void OpenQueries(object obj) {
            SelectedViewModel = new QueriesViewModel();
        }


        private void OpenStatistics(object obj) {
            SelectedViewModel = new StatisticsViewModel();
        }

        private void InstanceEfficiency(object obj) {
            SelectedViewModel = new InstanceEffieciencyViewModel();
        }

        private void OpenSettings(object obj) {
            SelectedViewModel = new SettingsViewModel();
        }

        private void OpenAccount(object obj) {
            SelectedViewModel = new AccountViewModel();
        }

        private void SqlInfo(object obj) {
            SelectedViewModel = new SqlInfoViewModel();
        }
    }

}

