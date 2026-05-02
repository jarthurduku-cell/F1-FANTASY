using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class WeeklyView : UserControl
    {
        public WeeklyView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // ── Prediction list (left) ────────────────────────────────
            // TODO: replace with real DB call
            PredictionList.ItemsSource = new List<PredRow>
            {
                new PredRow(1,  "11", "Sergio Perez",    "Red Bull Racing", "#1E41FF", "P2",  "P5",  new[]{5,2,6,4,5},  298),
                new PredRow(2,  "63", "George Russell",  "Mercedes",        "#00D2BE", "P8",  "P8",  new[]{8,8,8,8,8},  289),
                new PredRow(3,  "10", "Pierre Gasly",    "Alpine",          "#0090FF", "P10", "P10", new[]{10,11,9,10,11}, 198),
                new PredRow(4,  "18", "Lance Stroll",    "Aston Martin",    "#358C75", "P11", "P11", new[]{11,10,11,11,10}, 167),
                new PredRow(5,  "14", "Fernando Alonso", "Aston Martin",    "#358C75", "P9",  "P9",  new[]{8,9,10,8,9},  145),
            };

            // ── Available drivers (right) ─────────────────────────────
            AvailableList.ItemsSource = new List<AvailDriver>
            {
                new AvailDriver("1",  "Max Verstappen",  "Red Bull Racing", "#1E41FF", "P1"),
                new AvailDriver("16", "Charles Leclerc", "Ferrari",         "#DC0000", "P3"),
                new AvailDriver("4",  "Lando Norris",    "McLaren",         "#FF8700", "P4"),
                new AvailDriver("55", "Carlos Sainz",    "Ferrari",         "#DC0000", "P5"),
                new AvailDriver("44", "Lewis Hamilton",  "Mercedes",        "#00D2BE", "P6"),
                new AvailDriver("81", "Oscar Piastri",   "McLaren",         "#FF8700", "P7"),
                new AvailDriver("31", "Esteban Ocon",    "Alpine",          "#0090FF", "P12"),
            };

            // ── Quick remove chips ────────────────────────────────────
            var removes = new[]
            {
                ("P1", "Perez"), ("P2", "Russell"), ("P3", "Gasly"),
                ("P4", "Stroll"), ("P5", "Alonso")
            };
            foreach (var (pos, name) in removes)
            {
                var btn = new Button
                {
                    Content = $"{pos}  {name}",
                    Background = new SolidColorBrush(Color.FromRgb(26, 26, 28)),
                    Foreground = new SolidColorBrush(Color.FromRgb(240, 238, 234)),
                    BorderBrush = new SolidColorBrush(Color.FromRgb(50, 50, 55)),
                    BorderThickness = new Thickness(1),
                    FontFamily = new FontFamily("Segoe UI Semibold"),
                    FontSize = 11,
                    Padding = new Thickness(10, 5, 10, 5),
                    Margin = new Thickness(0, 0, 6, 6),
                    Cursor = System.Windows.Input.Cursors.Hand
                };
                var template = new ControlTemplate(typeof(Button));
                var border = new FrameworkElementFactory(typeof(Border));
                border.SetBinding(Border.BackgroundProperty,
                    new System.Windows.Data.Binding("Background")
                    { RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.TemplatedParent) });
                border.SetValue(Border.CornerRadiusProperty, new CornerRadius(4));
                border.SetBinding(Border.PaddingProperty,
                    new System.Windows.Data.Binding("Padding")
                    { RelativeSource = new System.Windows.Data.RelativeSource(System.Windows.Data.RelativeSourceMode.TemplatedParent) });
                var cp = new FrameworkElementFactory(typeof(ContentPresenter));
                cp.SetValue(ContentPresenter.HorizontalAlignmentProperty, HorizontalAlignment.Center);
                cp.SetValue(ContentPresenter.VerticalAlignmentProperty, VerticalAlignment.Center);
                border.AppendChild(cp);
                template.VisualTree = border;
                btn.Template = template;
                QuickRemovePanel.Children.Add(btn);
            }
        }
    }

    // ── PREDICTION ROW ───────────────────────────────────────────────
    public class PredRow
    {
        public string PredPos    { get; }
        public string Number     { get; }
        public string Name       { get; }
        public string Team       { get; }
        public string CurrentPos { get; }
        public string LastRace   { get; }
        public string Points     { get; }

        private readonly string _teamHex;
        private readonly int[]  _form;

        public PredRow(int predPos, string number, string name, string team,
                       string teamHex, string currentPos, string lastRace,
                       int[] form, int points)
        {
            PredPos    = predPos.ToString();
            Number     = number;
            Name       = name;
            Team       = team;
            _teamHex   = teamHex;
            CurrentPos = currentPos;
            LastRace   = lastRace;
            _form      = form;
            Points     = points.ToString();
        }

        public SolidColorBrush TeamColor
            => (SolidColorBrush)new BrushConverter().ConvertFrom(_teamHex)!;

        // Top 3 predicted positions get coloured
        public SolidColorBrush PosColor => PredPos switch
        {
            "1" => new SolidColorBrush(Color.FromRgb(201, 168, 76)),
            "2" => new SolidColorBrush(Color.FromRgb(192, 192, 192)),
            "3" => new SolidColorBrush(Color.FromRgb(205, 127, 50)),
            _   => new SolidColorBrush(Color.FromRgb(136, 136, 152))
        };

        public List<FormChip> Form
        {
            get
            {
                var list = new List<FormChip>();
                foreach (var v in _form)
                {
                    var c = v >= 10 ? Color.FromRgb(46, 125, 50)
                          : v >= 6  ? Color.FromRgb(0, 131, 143)
                                    : Color.FromRgb(55, 55, 62);
                    list.Add(new FormChip(v.ToString(), new SolidColorBrush(c)));
                }
                return list;
            }
        }
    }

    // ── AVAILABLE DRIVER ─────────────────────────────────────────────
    public class AvailDriver
    {
        public string Number     { get; }
        public string Name       { get; }
        public string Team       { get; }
        public string CurrentPos { get; }

        private readonly string _teamHex;

        public AvailDriver(string number, string name, string team,
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
