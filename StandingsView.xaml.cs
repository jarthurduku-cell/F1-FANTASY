using System.Collections.Generic;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using F1Fantasy.Services;

namespace F1Fantasy.Views
{
    public partial class StandingsView : UserControl
    {
        public StandingsView()
        {
            InitializeComponent();
            _ = LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            if (!DatabaseService.Instance.IsConnected) return;

            var standings = await DatabaseService.Instance.GetLeagueStandingsAsync(AppSession.LeagueId);
            var medals    = new[] { "👑", "👑", "👑" };
            var avatarHex = new[] { "#C9A84C", "#808090", "#E8002D", "#646472" };
            var items     = new List<StandingRow>();

            foreach (var s in standings)
            {
                int pos     = s.Position;
                string init = s.Username.Length >= 2
                    ? s.Username.Substring(0, 2).ToUpper()
                    : s.Username.ToUpper();
                bool isMe   = s.Username == AppSession.UserId.ToString();

                items.Add(new StandingRow(
                    pos.ToString(),
                    pos <= 3 ? medals[pos - 1] : "",
                    s.Username,
                    init,
                    avatarHex[Math.Min(pos - 1, avatarHex.Length - 1)],
                    s.TotalPoints.ToString("N0"),
                    s.LastRacePoints.ToString(),
                    s.LastRacePoints > 0 ? "↑" : "—",
                    s.LastRacePoints > 0,
                    isMe,
                    new[] { s.LastRacePoints }
                ));
            }

            StandingsList.ItemsSource = items;
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
