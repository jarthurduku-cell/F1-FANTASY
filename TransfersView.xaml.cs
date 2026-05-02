using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class TransfersView : UserControl
    {
        public TransfersView()
        {
            InitializeComponent();
            LoadData();
        }

        // ── TAB SWITCHING ─────────────────────────────────────────────
        private void BtnBuy_Click(object sender, RoutedEventArgs e)
        {
            BuyPanel.Visibility     = Visibility.Visible;
            StealPanel.Visibility   = Visibility.Collapsed;
            MySquadPanel.Visibility = Visibility.Collapsed;
            BtnBuy.Style     = (Style)FindResource("TabActive");
            BtnSteal.Style   = (Style)FindResource("TabInactive");
            BtnMySquad.Style = (Style)FindResource("TabInactive");
        }

        private void BtnSteal_Click(object sender, RoutedEventArgs e)
        {
            BuyPanel.Visibility     = Visibility.Collapsed;
            StealPanel.Visibility   = Visibility.Visible;
            MySquadPanel.Visibility = Visibility.Collapsed;
            BtnSteal.Style   = (Style)FindResource("TabActive");
            BtnBuy.Style     = (Style)FindResource("TabInactive");
            BtnMySquad.Style = (Style)FindResource("TabInactive");
        }

        private void BtnMySquad_Click(object sender, RoutedEventArgs e)
        {
            BuyPanel.Visibility     = Visibility.Collapsed;
            StealPanel.Visibility   = Visibility.Collapsed;
            MySquadPanel.Visibility = Visibility.Visible;
            BtnMySquad.Style = (Style)FindResource("TabActive");
            BtnBuy.Style     = (Style)FindResource("TabInactive");
            BtnSteal.Style   = (Style)FindResource("TabInactive");
        }

        // ── DATA ──────────────────────────────────────────────────────
        private void LoadData()
        {
            // TODO: replace with real DB calls

            // BUY list — free agents
            BuyList.ItemsSource = new List<TransferDriver>
            {
                new TransferDriver("1",  "Max Verstappen",  "Red Bull Racing", "#1E41FF", "456", "↗", "$32.5M", new[]{25,25,18,25,25}),
                new TransferDriver("4",  "Lando Norris",    "McLaren",         "#FF8700", "342", "↗", "$22.0M", new[]{15,12,18,15,15}),
                new TransferDriver("55", "Carlos Sainz",    "Ferrari",         "#DC0000", "324", "↘", "$25.5M", new[]{12,18,15,10,15}),
                new TransferDriver("44", "Lewis Hamilton",  "Mercedes",        "#00D2BE", "312", "↘", "$25.0M", new[]{10, 8,12,10,10}),
                new TransferDriver("14", "Fernando Alonso", "Aston Martin",    "#358C75", "245", "→", "$18.0M", new[]{ 8,10, 6, 8,10}),
            };

            // STEAL list — owned by others
            StealList.ItemsSource = new List<StealDriver>
            {
                new StealDriver("16", "Charles Leclerc", "Ferrari",         "#DC0000", "Owned by Mike Chen",     "387", "$42.0M", new[]{18,15,25,12,18}),
                new StealDriver("11", "Sergio Perez",    "Red Bull Racing", "#1E41FF", "Owned by Sarah Johnson", "298", "$36.0M", new[]{10,15,12, 8,10}),
                new StealDriver("63", "George Russell",  "Mercedes",        "#00D2BE", "Owned by James Taylor",  "289", "$30.0M", new[]{12,12,10,15,12}),
                new StealDriver("81", "Oscar Piastri",   "McLaren",         "#FF8700", "Owned by Emma Wilson",   "278", "$24.8M", new[]{12,10,15,12,12}),
                new StealDriver("18", "Lance Stroll",    "Aston Martin",    "#358C75", "Owned by David Kim",     "167", "$16.5M", new[]{ 8, 6, 8, 8, 6}),
            };

            // MY SQUAD list — with clauses
            MySquadList.ItemsSource = new List<MySquadDriver>
            {
                new MySquadDriver("10", "Pierre Gasly",   "Alpine",   "#0090FF", "198", "$14.0M", "$21.0M", "Cost: $1.4M/wk", new[]{6,8,6,8,6},  true),
                new MySquadDriver("31", "Esteban Ocon",   "Alpine",   "#0090FF", "178", "$13.5M", "$20.3M", "Cost: $1.4M/wk", new[]{6,4,8,6,4},  false),
                new MySquadDriver("22", "Yuki Tsunoda",   "AlphaTauri","#5E8FAA","145", "$11.0M", "$16.5M", "Cost: $1.1M/wk", new[]{4,6,4,6,4},  true),
            };
        }
    }

    // ── BUY DRIVER ───────────────────────────────────────────────────
    public class TransferDriver
    {
        public string Number { get; }
        public string Name   { get; }
        public string Team   { get; }
        public string Points { get; }
        public string Trend  { get; }
        public string Price  { get; }

        private readonly string _teamHex;
        private readonly int[]  _form;

        public TransferDriver(string number, string name, string team, string teamHex,
                              string points, string trend, string price, int[] form)
        {
            Number   = number; Name = name; Team = team;
            _teamHex = teamHex; Points = points; Trend = trend;
            Price = price; _form = form;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        public SolidColorBrush TrendColor => Trend switch
        {
            "↗" => new SolidColorBrush(Color.FromRgb(76,175,80)),
            "↘" => new SolidColorBrush(Color.FromRgb(232,0,45)),
            _   => new SolidColorBrush(Color.FromRgb(136,136,152))
        };

        public List<TChip> Form
        {
            get
            {
                var list = new List<TChip>();
                foreach (var v in _form)
                {
                    var c = v >= 25 ? Color.FromRgb(46,125,50)
                          : v >= 15 ? Color.FromRgb(0,131,143)
                                    : Color.FromRgb(55,55,62);
                    list.Add(new TChip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── STEAL DRIVER ─────────────────────────────────────────────────
    public class StealDriver
    {
        public string Number { get; }
        public string Name   { get; }
        public string Team   { get; }
        public string Owner  { get; }
        public string Points { get; }
        public string Clause { get; }

        private readonly string _teamHex;
        private readonly int[]  _form;

        public StealDriver(string number, string name, string team, string teamHex,
                           string owner, string points, string clause, int[] form)
        {
            Number = number; Name = name; Team = team;
            _teamHex = teamHex; Owner = owner; Points = points;
            Clause = clause; _form = form;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        public List<TChip> Form
        {
            get
            {
                var list = new List<TChip>();
                foreach (var v in _form)
                {
                    var c = v >= 18 ? Color.FromRgb(46,125,50)
                          : v >= 12 ? Color.FromRgb(0,131,143)
                                    : Color.FromRgb(55,55,62);
                    list.Add(new TChip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── MY SQUAD DRIVER ──────────────────────────────────────────────
    public class MySquadDriver
    {
        public string Number     { get; }
        public string Name       { get; }
        public string Team       { get; }
        public string Points     { get; }
        public string Value      { get; }
        public string Clause     { get; }
        public string ClauseCost { get; }
        public bool   IsProtected{ get; }

        private readonly string _teamHex;
        private readonly int[]  _form;

        public MySquadDriver(string number, string name, string team, string teamHex,
                             string points, string value, string clause, string clauseCost,
                             int[] form, bool isProtected)
        {
            Number = number; Name = name; Team = team;
            _teamHex = teamHex; Points = points; Value = value;
            Clause = clause; ClauseCost = clauseCost;
            _form = form; IsProtected = isProtected;
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        public Visibility ProtectedVis
            => IsProtected ? Visibility.Visible : Visibility.Collapsed;

        public List<TChip> Form
        {
            get
            {
                var list = new List<TChip>();
                foreach (var v in _form)
                {
                    var c = v >= 8  ? Color.FromRgb(46,125,50)
                          : v >= 6  ? Color.FromRgb(0,131,143)
                                    : Color.FromRgb(55,55,62);
                    list.Add(new TChip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── CHIP ─────────────────────────────────────────────────────────
    public class TChip
    {
        public string          Val { get; }
        public SolidColorBrush Bg  { get; }
        public TChip(string val, SolidColorBrush bg) { Val = val; Bg = bg; }
    }
}
