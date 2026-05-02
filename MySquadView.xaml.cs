using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class MySquadView : UserControl
    {
        public MySquadView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // ── Available drivers (left list) ─────────────────────────
            // TODO: replace with real DB call
            AvailableList.ItemsSource = new List<AvailableDriver>
            {
                new AvailableDriver("1",  "Max Verstappen",  "Red Bull Racing", "#1E41FF",
                    "456", "↗", new[]{25,25,18,25,25}, false),

                new AvailableDriver("16", "Charles Leclerc", "Ferrari",          "#DC0000",
                    "387", "↗", new[]{18,15,25,12,18}, true),

                new AvailableDriver("4",  "Lando Norris",    "McLaren",          "#FF8700",
                    "342", "↗", new[]{15,12,18,15,15}, true),

                new AvailableDriver("55", "Carlos Sainz",    "Ferrari",          "#DC0000",
                    "324", "↘", new[]{12,18,15,10,15}, false),

                new AvailableDriver("44", "Lewis Hamilton",  "Mercedes",         "#00D2BE",
                    "312", "↘", new[]{10, 8,12,10,10}, true),

                new AvailableDriver("11", "Sergio Perez",    "Red Bull Racing",  "#1E41FF",
                    "298", "→", new[]{10,15,12, 8,10}, false),

                new AvailableDriver("14", "Fernando Alonso", "Aston Martin",     "#358C75",
                    "276", "↗", new[]{ 8,12, 8,10,12}, false),

                new AvailableDriver("63", "George Russell",  "Mercedes",         "#00D2BE",
                    "265", "→", new[]{10, 8,12, 8,10}, false),
            };

            // ── Squad (right panel) ───────────────────────────────────
            SquadList.ItemsSource = new List<SquadDriver>
            {
                new SquadDriver("16", "Charles Leclerc", "Ferrari",  "#DC0000", "$28.0M", "387"),
                new SquadDriver("4",  "Lando Norris",    "McLaren",  "#FF8700", "$22.0M", "342"),
                new SquadDriver("81", "Oscar Piastri",   "McLaren",  "#FF8700", "$16.5M", "278"),
                new SquadDriver("44", "Lewis Hamilton",  "Mercedes", "#00D2BE", "$25.0M", "312"),
            };
        }
    }

    // ── AVAILABLE DRIVER MODEL ───────────────────────────────────────
    public class AvailableDriver
    {
        public string Number   { get; }
        public string Name     { get; }
        public string Team     { get; }
        public string Points   { get; }
        public string Trend    { get; }
        public bool   InSquad  { get; }

        private readonly string _teamHex;
        private readonly int[]  _form;

        public AvailableDriver(string number, string name, string team, string teamHex,
                               string points, string trend, int[] form, bool inSquad)
        {
            Number   = number;
            Name     = name;
            Team     = team;
            _teamHex = teamHex;
            Points   = points;
            Trend    = trend;
            _form    = form;
            InSquad  = inSquad;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        public SolidColorBrush TrendColor => Trend switch
        {
            "↗" => new SolidColorBrush(Color.FromRgb(76, 175, 80)),
            "↘" => new SolidColorBrush(Color.FromRgb(232, 0, 45)),
            _   => new SolidColorBrush(Color.FromRgb(136, 136, 152))
        };

        // Highlight row if driver is already in squad
        public SolidColorBrush RowBg
            => InSquad
                ? new SolidColorBrush(Color.FromArgb(20, 232, 0, 45))
                : new SolidColorBrush(Colors.Transparent);

        public SolidColorBrush RowBorder
            => InSquad
                ? new SolidColorBrush(Color.FromArgb(60, 232, 0, 45))
                : new SolidColorBrush(Color.FromRgb(26, 26, 30));

        public Visibility InSquadVis => InSquad ? Visibility.Visible : Visibility.Collapsed;

        // Form chips
        public List<FormChip> Form
        {
            get
            {
                var list = new List<FormChip>();
                foreach (var v in _form)
                {
                    var c = v >= 25 ? Color.FromRgb(46, 125, 50)
                          : v >= 18 ? Color.FromRgb(0, 131, 143)
                                    : Color.FromRgb(55, 55, 62);
                    list.Add(new FormChip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── SQUAD DRIVER MODEL ───────────────────────────────────────────
    public class SquadDriver
    {
        public string Number { get; }
        public string Name   { get; }
        public string Team   { get; }
        public string Price  { get; }
        public string Points { get; }

        private readonly string _teamHex;

        public SquadDriver(string number, string name, string team,
                           string teamHex, string price, string points)
        {
            Number   = number;
            Name     = name;
            Team     = team;
            _teamHex = teamHex;
            Price    = price;
            Points   = points;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;
    }

    // ── FORM CHIP ────────────────────────────────────────────────────
    public class FormChip
    {
        public string          Val { get; }
        public SolidColorBrush Bg  { get; }
        public FormChip(string val, SolidColorBrush bg) { Val = val; Bg = bg; }
    }
}
