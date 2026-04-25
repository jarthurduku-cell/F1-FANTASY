using System.Windows;
using System.Windows.Controls;
using F1Fantasy.Views;

namespace F1Fantasy
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Navigate("MyTeam");
        }

        private void Nav_Click(object sender, RoutedEventArgs e)
        {
            var tag = (sender as Button)?.Tag?.ToString();
            if (tag != null) Navigate(tag);
        }

        private void Navigate(string view)
        {
            var inactive = (Style)FindResource("NavBtn");
            var active = (Style)FindResource("NavBtnActive");

            btnHome.Style = inactive;
            btnStandings.Style = inactive;
            btnMyTeam.Style = inactive;
            btnMySquad.Style = inactive;
            btnTransfers.Style = inactive;
            btnWeekly.Style = inactive;
            btnSeason.Style = inactive;

            switch (view)
            {
                case "Home":
                    btnHome.Style = active;
                    MainContent.Content = new HomeView();
                    txtSubtitle.Text = "2026 SEASON";
                    break;
                case "Standings":
                    btnStandings.Style = active;
                    MainContent.Content = new StandingsView();
                    txtSubtitle.Text = "LEAGUE DASHBOARD";
                    break;
                case "MyTeam":
                    btnMyTeam.Style = active;
                    MainContent.Content = new MyTeamView();
                    txtSubtitle.Text = "LEAGUE DASHBOARD";
                    break;
                case "MySquad":
                    btnMySquad.Style = active;
                    MainContent.Content = new MySquadView();
                    txtSubtitle.Text = "SQUAD SELECTION";
                    break;
                case "Transfers":
                    btnTransfers.Style = active;
                    MainContent.Content = new TransfersView();
                    txtSubtitle.Text = "TRANSFER MARKET";
                    break;
                case "Weekly":
                    btnWeekly.Style = active;
                    MainContent.Content = new WeeklyView();
                    txtSubtitle.Text = "WEEKLY PREDICTIONS";
                    break;
                case "Season":
                    btnSeason.Style = active;
                    MainContent.Content = new SeasonView();
                    txtSubtitle.Text = "SEASON PREDICTIONS";
                    break;
            }
        }
    }
}
