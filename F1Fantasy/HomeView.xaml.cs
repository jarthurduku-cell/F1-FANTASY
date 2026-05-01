using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class HomeView : UserControl
    {
        // Agregar esta línea para declarar el campo LeaguesList si no está generado por XAML
        public ListView LeaguesList { get; set; }

        public HomeView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // TODO: replace with real DB call
            LeaguesList.ItemsSource = new List<LeagueItem>
            {
                new LeagueItem
                {
                    Icon     = "🏆",
                    Name     = "Office Champions League",
                    Members  = "24 members",
                    Position = "#3",
                    Points   = "1,847",
                    Trend    = "↑ ↑",
                    TrendColor = new SolidColorBrush(Color.FromRgb(76, 175, 80))
                },
                new LeagueItem
                {
                    Icon     = "👥",
                    Name     = "Family Racing Crew",
                    Members  = "8 members",
                    Position = "#1",
                    Points   = "2,103",
                    Trend    = "↑ ↑",
                    TrendColor = new SolidColorBrush(Color.FromRgb(76, 175, 80))
                },
                new LeagueItem
                {
                    Icon     = "🌍",
                    Name     = "F1 Fanatics Global",
                    Members  = "156 members",
                    Position = "#47",
                    Points   = "1,654",
                    Trend    = "↓",
                    TrendColor = new SolidColorBrush(Color.FromRgb(232, 0, 45))
                },
                new LeagueItem
                {
                    Icon     = "❌",
                    Name     = "Weekend Warriors",
                    Members  = "12 members",
                    Position = "#5",
                    Points   = "1,923",
                    Trend    = "↑ ↑",
                    TrendColor = new SolidColorBrush(Color.FromRgb(76, 175, 80))
                },
                new LeagueItem
                {
                    Icon     = "👾",
                    Name     = "Speed Demons",
                    Members  = "45 members",
                    Position = "#12",
                    Points   = "1,789",
                    Trend    = "—",
                    TrendColor = new SolidColorBrush(Color.FromRgb(136, 136, 152))
                },
                new LeagueItem
                {
                    Icon     = "🎯",
                    Name     = "Pole Position Hunters",
                    Members  = "32 members",
                    Position = "#8",
                    Points   = "1,876",
                    Trend    = "↑ ↑",
                    TrendColor = new SolidColorBrush(Color.FromRgb(76, 175, 80))
                },
            };
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
