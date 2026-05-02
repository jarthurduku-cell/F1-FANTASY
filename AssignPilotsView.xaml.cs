using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class AssignPilotsView : UserControl
    {
        // All 20 F1 2025 drivers
        private static readonly List<(string Number, string Name, string Team, string Hex)> AllDrivers = new()
        {
            ("1",  "Max Verstappen",   "Red Bull Racing", "#1E41FF"),
            ("11", "Sergio Perez",     "Red Bull Racing", "#1E41FF"),
            ("16", "Charles Leclerc",  "Ferrari",         "#DC0000"),
            ("55", "Carlos Sainz",     "Ferrari",         "#DC0000"),
            ("4",  "Lando Norris",     "McLaren",         "#FF8700"),
            ("81", "Oscar Piastri",    "McLaren",         "#FF8700"),
            ("44", "Lewis Hamilton",   "Mercedes",        "#00D2BE"),
            ("63", "George Russell",   "Mercedes",        "#00D2BE"),
            ("14", "Fernando Alonso",  "Aston Martin",    "#358C75"),
            ("18", "Lance Stroll",     "Aston Martin",    "#358C75"),
            ("10", "Pierre Gasly",     "Alpine",          "#0090FF"),
            ("31", "Esteban Ocon",     "Alpine",          "#0090FF"),
            ("23", "Alexander Albon",  "Williams",        "#005AFF"),
            ("2",  "Logan Sargeant",   "Williams",        "#005AFF"),
            ("22", "Yuki Tsunoda",     "AlphaTauri",      "#5E8FAA"),
            ("3",  "Daniel Ricciardo", "AlphaTauri",      "#5E8FAA"),
            ("20", "Kevin Magnussen",  "Haas",            "#B6BABD"),
            ("27", "Nico Hulkenberg",  "Haas",            "#B6BABD"),
            ("77", "Valtteri Bottas",  "Alfa Romeo",      "#900000"),
            ("24", "Zhou Guanyu",      "Alfa Romeo",      "#900000"),
        };

        private List<AssignmentRow> _rows = new();
        private readonly Random _rng = new();

        // Tag thresholds — shown on driver if they are top performer
        private static string? GetTag(string name) => name switch
        {
            "Max Verstappen"  => null,
            "Lando Norris"    => "Top 5",
            "Carlos Sainz"    => "Top 5",
            "Lewis Hamilton"  => "Top 5",
            "Fernando Alonso" => "Top 10",
            _ => null
        };

        private static Color TagColorFor(string tag) => tag switch
        {
            "Top 5"  => Color.FromRgb(46, 125, 50),
            "Top 10" => Color.FromRgb(0,  131, 143),
            _        => Color.FromRgb(55,  55,  62)
        };

        public AssignPilotsView()
        {
            InitializeComponent();
            LoadMembers();
        }

        // ── LOAD MEMBERS ─────────────────────────────────────────────
        private void LoadMembers()
        {
            // TODO: replace with real DB call — load actual league members
            var members = new List<(string Initials, string Name, string League, string AvatarHex)>
            {
                ("JP", "Juan Pérez",      "Tu ams", "#C62828"),
                ("SG", "Sara Gómez",      "Tu ams", "#AD1457"),
                ("LM", "Luis Martinez",   "Tu ams", "#6A1B9A"),
                ("AR", "Ana Rodriguez",   "Tu ams", "#00695C"),
                ("DT", "David Torres",    "Tu ams", "#E65100"),
            };

            // Assign a driver to each member (random, no repeats)
            var shuffled = AllDrivers.OrderBy(_ => _rng.Next()).ToList();

            _rows = members.Select((m, i) =>
            {
                var d = shuffled[i % shuffled.Count];
                var tag = GetTag(d.Name);
                return new AssignmentRow
                {
                    Initials          = m.Initials,
                    UserName          = m.Name,
                    LeagueName        = m.League,
                    AvatarColor       = (SolidColorBrush)new BrushConverter().ConvertFrom(m.AvatarHex)!,
                    DriverNumber      = d.Number,
                    DriverName        = d.Name,
                    TeamName          = d.Team,
                    DriverTeamColor   = (SolidColorBrush)new BrushConverter().ConvertFrom(d.Hex)!,
                    Tag               = tag ?? "",
                    TagVis            = tag != null ? Visibility.Visible : Visibility.Collapsed,
                    TagBrush          = tag != null
                                            ? new SolidColorBrush(TagColorFor(tag))
                                            : new SolidColorBrush(Colors.Transparent),
                    RandomizeBtnColor = (SolidColorBrush)new BrushConverter().ConvertFrom(d.Hex)!,
                };
            }).ToList();

            AssignmentList.ItemsSource = _rows;
        }

        // ── ASSIGN ALL ───────────────────────────────────────────────
        private void BtnAssignNow_Click(object sender, RoutedEventArgs e)
        {
            var shuffled = AllDrivers.OrderBy(_ => _rng.Next()).ToList();
            for (int i = 0; i < _rows.Count; i++)
            {
                var d = shuffled[i % shuffled.Count];
                ApplyDriver(_rows[i], d);
            }
            AssignmentList.ItemsSource = null;
            AssignmentList.ItemsSource = _rows;
        }

        // ── RANDOMIZE SINGLE ROW ─────────────────────────────────────
        private void BtnRandomizeSingle_Click(object sender, RoutedEventArgs e)
        {
            var btn = sender as Button;
            var userName = btn?.Tag?.ToString();
            var row = _rows.FirstOrDefault(r => r.UserName == userName);
            if (row == null) return;

            // Pick a random driver not already assigned to another member
            var usedNumbers = _rows.Where(r => r != row).Select(r => r.DriverNumber).ToHashSet();
            var available   = AllDrivers.Where(d => !usedNumbers.Contains(d.Number))
                                        .OrderBy(_ => _rng.Next())
                                        .ToList();
            if (available.Count == 0) available = AllDrivers.OrderBy(_ => _rng.Next()).ToList();

            ApplyDriver(row, available[0]);
            AssignmentList.ItemsSource = null;
            AssignmentList.ItemsSource = _rows;
        }

        // ── CONFIRM ──────────────────────────────────────────────────
        private void BtnConfirm_Click(object sender, RoutedEventArgs e)
        {
            // TODO: save assignments to DB
            // DbService.SavePilotAssignments(_rows);
            MessageBox.Show(
                "Pilots assigned successfully!\nThe season has started.",
                "Assignment Confirmed",
                MessageBoxButton.OK,
                MessageBoxImage.Information);
        }

        // ── HELPER ───────────────────────────────────────────────────
        private static void ApplyDriver(AssignmentRow row,
            (string Number, string Name, string Team, string Hex) d)
        {
            var tag = GetTag(d.Name);
            row.DriverNumber    = d.Number;
            row.DriverName      = d.Name;
            row.TeamName        = d.Team;
            var brush = (SolidColorBrush)new BrushConverter().ConvertFrom(d.Hex)!;
            row.DriverTeamColor   = brush;
            row.RandomizeBtnColor = brush;
            row.Tag     = tag ?? "";
            row.TagVis  = tag != null ? Visibility.Visible : Visibility.Collapsed;
            row.TagBrush = tag != null
                ? new SolidColorBrush(TagColorFor(tag))
                : new SolidColorBrush(Colors.Transparent);
        }
    }

    // ── ROW MODEL ────────────────────────────────────────────────────
    public class AssignmentRow
    {
        public string          Initials          { get; set; }
        public string          UserName          { get; set; }
        public string          LeagueName        { get; set; }
        public SolidColorBrush AvatarColor       { get; set; }
        public string          DriverNumber      { get; set; }
        public string          DriverName        { get; set; }
        public string          TeamName          { get; set; }
        public SolidColorBrush DriverTeamColor   { get; set; }
        public string          Tag               { get; set; }
        public Visibility      TagVis            { get; set; }
        public SolidColorBrush TagBrush          { get; set; }  // alias for binding
        public SolidColorBrush TagColor          => TagBrush;
        public SolidColorBrush RandomizeBtnColor { get; set; }
    }
}
