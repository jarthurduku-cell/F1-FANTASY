using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class StandingsView : UserControl
    {
        public StandingsView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // TODO: replace with real DB call
            StandingsList.ItemsSource = new List<StandingRow>
            {
                new StandingRow("1",  "👑", "Sarah Johnson", "SJ", "#C9A84C",
                    "2,847","156","↑ +1", true,  false,
                    new[]{145,132,167,142,156,189,134,156}),

                new StandingRow("2",  "👑", "Mike Chen",     "MC", "#808090",
                    "2,821","134","↓ -1", false, false,
                    new[]{178,156,145,167,134,156,151,134}),

                new StandingRow("3",  "👑", "Alex Rodriguez","AR", "#E8002D",
                    "2,756","167","↑ +1", true,  true,
                    new[]{134,145,156,151,167,145,134,167}),

                new StandingRow("4",  "",   "Emma Wilson",   "EW", "#646472",
                    "2,698","128","↓ -1", false, false,
                    new[]{156,167,134,145,128,167,156,128}),

                new StandingRow("5",  "",   "James Taylor",  "JT", "#646472",
                    "2,645","145","—",    null,  false,
                    new[]{145,134,156,134,145,134,167,145}),

                new StandingRow("6",  "",   "Lisa Anderson", "LA", "#646472",
                    "2,612","156","↑ +1", true,  false,
                    new[]{134,145,134,156,156,145,134,156}),

                new StandingRow("7",  "",   "David Kim",     "DK", "#646472",
                    "2,589","123","↓ -1", false, false,
                    new[]{156,134,145,134,123,156,145,123}),
            };
        }
    }

    // ── ROW MODEL ────────────────────────────────────────────────────
    public class StandingRow
    {
        public string Pos      { get; }
        public string Medal    { get; }
        public string Name     { get; }
        public string Initials { get; }
        public string TotalPts { get; }
        public string LastRace { get; }
        public string Trend    { get; }
        public bool   IsMe     { get; }

        private readonly string  _avatarHex;
        private readonly bool?   _trendUp;
        private readonly int[]   _history;

        public StandingRow(string pos, string medal, string name, string initials,
                           string avatarHex, string totalPts, string lastRace,
                           string trend, bool? trendUp, bool isMe, int[] history)
        {
            Pos         = pos;
            Medal       = medal;
            Name        = name;
            Initials    = initials;
            _avatarHex  = avatarHex;
            TotalPts    = totalPts;
            LastRace    = lastRace;
            Trend       = trend;
            _trendUp    = trendUp;
            IsMe        = isMe;
            _history    = history;
        }

        // ── Computed brushes ─────────────────────────────────────────
        public SolidColorBrush PosColor => Pos switch
        {
            "1" => new SolidColorBrush(Color.FromRgb(201,168,76)),
            "2" => new SolidColorBrush(Color.FromRgb(192,192,192)),
            "3" => new SolidColorBrush(Color.FromRgb(205,127,50)),
            _   => new SolidColorBrush(Color.FromRgb(136,136,152))
        };

        public SolidColorBrush AvatarBrush
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_avatarHex)!;

        public SolidColorBrush TrendBrush => _trendUp switch
        {
            true  => new SolidColorBrush(Color.FromRgb(76,175,80)),
            false => new SolidColorBrush(Color.FromRgb(232,0,45)),
            null  => new SolidColorBrush(Color.FromRgb(136,136,152))
        };

        public SolidColorBrush RowBg
            => IsMe
                ? new SolidColorBrush(Color.FromArgb(22,232,0,45))
                : new SolidColorBrush(Colors.Transparent);

        public Visibility YouVis => IsMe ? Visibility.Visible : Visibility.Collapsed;

        // ── History chips ─────────────────────────────────────────────
        public List<Chip> Chips
        {
            get
            {
                var list = new List<Chip>();
                foreach (var v in _history)
                {
                    // green ≥167 · teal ≥145 · dark otherwise
                    var c = v >= 167 ? Color.FromRgb(46,125,50)
                          : v >= 145 ? Color.FromRgb(0,131,143)
                                     : Color.FromRgb(55,55,62);
                    list.Add(new Chip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── CHIP MODEL ───────────────────────────────────────────────────
    public class Chip
    {
        public string          Val { get; }
        public SolidColorBrush Bg  { get; }
        public Chip(string val, SolidColorBrush bg) { Val = val; Bg = bg; }
    }
}
