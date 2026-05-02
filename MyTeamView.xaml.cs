using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using F1Fantasy.Services;

namespace F1Fantasy.Views
{
    public partial class MyTeamView : UserControl
    {
        public MyTeamView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (!DatabaseService.Instance.IsConnected) return;

            var squad = await DatabaseService.Instance.GetSquadAsync(AppSession.LeagueMemberId);
            var items = new List<DriverItem>();

            foreach (var p in squad)
                items.Add(new DriverItem
                {
                    Number      = p.CarNumber.ToString(),
                    Name        = p.FullName,
                    Team        = p.ConstructorName,
                    Price       = $"${p.CurrentValue:N1}M",
                    TotalPoints = p.LastRacePoints.ToString(),
                    LastRace    = $"+{p.LastRacePoints}",
                    TeamColor   = new SolidColorBrush(Color.FromRgb(136, 136, 152)),
                });

            DriversList.ItemsSource = items;
        }
    }

    public class DriverItem
    {
        public string Number      { get; set; }
        public string Name        { get; set; }
        public string Team        { get; set; }
        public string Price       { get; set; }
        public string TotalPoints { get; set; }
        public string LastRace    { get; set; }
        public SolidColorBrush TeamColor { get; set; }
    }
}
