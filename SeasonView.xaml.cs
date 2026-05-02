using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class SeasonView : UserControl
    {
        private bool _driversTabActive = true;

        public SeasonView()
        {
            InitializeComponent();
            LoadData();
        }

        // ── TAB SWITCHING ─────────────────────────────────────────────
        private void BtnDrivers_Click(object sender, RoutedEventArgs e)
        {
            _driversTabActive = true;
            BtnDrivers.Style      = (Style)FindResource("TabActive");
            BtnConstructors.Style = (Style)FindResource("TabInactive");
            DriversScroll.Visibility      = Visibility.Visible;
            ConstructorsScroll.Visibility = Visibility.Collapsed;
            PredTitle.Text    = "Top 10 Drivers Prediction";
            CounterText.Text  = "0/10 Drivers";
        }

        private void BtnConstructors_Click(object sender, RoutedEventArgs e)
        {
            _driversTabActive = false;
            BtnConstructors.Style = (Style)FindResource("TabActive");
            BtnDrivers.Style      = (Style)FindResource("TabInactive");
            ConstructorsScroll.Visibility = Visibility.Visible;
            DriversScroll.Visibility      = Visibility.Collapsed;
            PredTitle.Text   = "Top 10 Constructors Prediction";
            CounterText.Text = "0/10 Constructors";
        }

        // ── DATA ──────────────────────────────────────────────────────
        private void LoadData()
        {
            // Drivers prediction list (empty by default — user fills it)
            DriverPredList.ItemsSource = new List<SeasonPredRow>();

            // Constructor prediction list (empty by default)
            ConstructorPredList.ItemsSource = new List<SeasonPredRow>();

            // Available drivers (right panel)
            // TODO: replace with real DB call
            AvailableList.ItemsSource = new List<SeasonAvailItem>
            {
                new SeasonAvailItem("1",  "Max Verstappen",  "Red Bull Racing", "#1E41FF", "P1"),
                new SeasonAvailItem("16", "Charles Leclerc", "Ferrari",         "#DC0000", "P3"),
                new SeasonAvailItem("4",  "Lando Norris",    "McLaren",         "#FF8700", "P4"),
                new SeasonAvailItem("11", "Sergio Perez",    "Red Bull Racing", "#1E41FF", "P2"),
                new SeasonAvailItem("44", "Lewis Hamilton",  "Mercedes",        "#00D2BE", "P6"),
                new SeasonAvailItem("55", "Carlos Sainz",    "Ferrari",         "#DC0000", "P5"),
                new SeasonAvailItem("81", "Oscar Piastri",   "McLaren",         "#FF8700", "P7"),
                new SeasonAvailItem("63", "George Russell",  "Mercedes",        "#00D2BE", "P8"),
                new SeasonAvailItem("14", "Fernando Alonso", "Aston Martin",    "#358C75", "P9"),
                new SeasonAvailItem("18", "Lance Stroll",    "Aston Martin",    "#358C75", "P11"),
            };
        }
    }

    // ── SEASON PREDICTION ROW ─────────────────────────────────────────
    public class SeasonPredRow
    {
        public string Rank   { get; set; }
        public string Number { get; set; }
        public string Name   { get; set; }
        public string Team   { get; set; }

        private readonly string _teamHex;

        public SeasonPredRow(int rank, string number, string name,
                             string team, string teamHex)
        {
            Rank     = rank.ToString();
            Number   = number;
            Name     = name;
            Team     = team;
            _teamHex = teamHex;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        public SolidColorBrush RankColor => Rank switch
        {
            "1" => new SolidColorBrush(Color.FromRgb(201, 168, 76)),
            "2" => new SolidColorBrush(Color.FromRgb(192, 192, 192)),
            "3" => new SolidColorBrush(Color.FromRgb(205, 127, 50)),
            _   => new SolidColorBrush(Color.FromRgb(136, 136, 152))
        };
    }

    // ── AVAILABLE ITEM (right panel) ──────────────────────────────────
    public class SeasonAvailItem
    {
        public string Number     { get; }
        public string Name       { get; }
        public string Team       { get; }
        public string CurrentPos { get; }

        private readonly string _teamHex;

        public SeasonAvailItem(string number, string name, string team,
                               string teamHex, string currentPos)
        {
            Number     = number;
            Name       = name;
            Team       = team;
            _teamHex   = teamHex;
            CurrentPos = currentPos;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;
    }
}
