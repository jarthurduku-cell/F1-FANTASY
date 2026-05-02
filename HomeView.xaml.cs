using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media;
using F1Fantasy.Services;

namespace F1Fantasy.Views
{
    public partial class HomeView : UserControl
    {
        public HomeView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (!DatabaseService.Instance.IsConnected) return;

            var leagues = await DatabaseService.Instance.GetLeaguesForUserAsync(AppSession.UserId);
            var icons   = new[] { "🏆", "👥", "🌍", "🎯", "👾", "❌" };
            var items   = new List<LeagueItem>();
            int i = 0;

            foreach (var l in leagues)
            {
                items.Add(new LeagueItem
                {
                    Icon       = icons[i % icons.Length],
                    Name       = l.Name,
                    Members    = $"{l.MemberCount} members",
                    Position   = $"#{l.Position}",
                    Points     = l.TotalPoints.ToString("N0"),
                    Trend      = l.LastRacePoints >= 0 ? "↑" : "↓",
                    TrendColor = l.LastRacePoints >= 0
                        ? new SolidColorBrush(Color.FromRgb(76, 175, 80))
                        : new SolidColorBrush(Color.FromRgb(232, 0, 45)),
                });
                i++;
            }

            LeaguesList.ItemsSource = items;
        }
    }

    public class LeagueItem
    {
        public string           Icon       { get; set; }
        public string           Name       { get; set; }
        public string           Members    { get; set; }
        public string           Position   { get; set; }
        public string           Points     { get; set; }
        public string           Trend      { get; set; }
        public SolidColorBrush  TrendColor { get; set; }
    }
}
