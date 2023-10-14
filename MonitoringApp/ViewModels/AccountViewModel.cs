using MonitoringApp.Models;


namespace MonitoringApp.ViewModels {
    public class AccountViewModel {

        public ICommand LoginCommand { get; set; }
        public ICommand LogoutCommand { get; set; }

        public AccountViewModel() {
            LoginCommand = new BaseCommand(Login);
            LogoutCommand = new BaseCommand(Logout);
        }


        public string LoginBox { get; set; }
        public string HostBox { get; set; }
        public string PortBox { get; set; }
        public string SidBox { get; set; }

        public void Login(object obj) {
            var passwordBox = obj as PasswordBox;

            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["Entities"].ConnectionString =
                $"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=Oracle.ManagedDataAccess.Client;provider connection string=\";DATA SOURCE={HostBox}:{PortBox}/{SidBox};PASSWORD={passwordBox.Password};USER ID={LoginBox}\";";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");

            try {
                using (var ctx = new Entities()) {
                    string result = ctx.Database.SqlQuery<string>("select 'checked' from dual").First();
                    MessageBox.Show("Logged in");

                }
            } catch {
                MessageBox.Show("Incorrect data");
            }
        }

        public void Logout(object obj) {
            var config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            var connectionStringsSection = (ConnectionStringsSection)config.GetSection("connectionStrings");
            connectionStringsSection.ConnectionStrings["Entities"].ConnectionString =
                $"metadata=res://*/Models.Model.csdl|res://*/Models.Model.ssdl|res://*/Models.Model.msl;provider=Oracle.ManagedDataAccess.Client;provider connection string=\";DATA SOURCE=:/;PASSWORD=;USER ID=\";";
            config.Save();
            ConfigurationManager.RefreshSection("connectionStrings");
            MessageBox.Show("Logged out");
        }
    }
}
