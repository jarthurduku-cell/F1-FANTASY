using System.Collections.Generic;
using System.Windows.Controls;
using System.Windows.Media;

namespace F1Fantasy.Views
{
    public partial class MyTeamView : UserControl
    {
        public MyTeamView()
        {
            InitializeComponent();
            LoadData();
        }

        private void LoadData()
        {
            // TODO: Replace with real DB call
            DriversList.ItemsSource = new List<DriverItem>
            {
                new DriverItem { Number="1",  Name="Max Verstappen",  Team="Red Bull Racing", Price="$32.5M", TotalPoints="456", LastRace="+25", TeamColor=new SolidColorBrush(Color.FromRgb(30,65,255))  },
                new DriverItem { Number="16", Name="Charles Leclerc", Team="Ferrari",          Price="$28.0M", TotalPoints="387", LastRace="+18", TeamColor=new SolidColorBrush(Color.FromRgb(220,0,0))    },
                new DriverItem { Number="4",  Name="Lando Norris",    Team="McLaren",          Price="$22.0M", TotalPoints="342", LastRace="+15", TeamColor=new SolidColorBrush(Color.FromRgb(255,135,0))  },
                new DriverItem { Number="55", Name="Carlos Sainz",    Team="Ferrari",          Price="$25.5M", TotalPoints="324", LastRace="+12", TeamColor=new SolidColorBrush(Color.FromRgb(220,0,0))    },
                new DriverItem { Number="44", Name="Lewis Hamilton",  Team="Mercedes",         Price="$25.0M", TotalPoints="312", LastRace="+10", TeamColor=new SolidColorBrush(Color.FromRgb(0,210,190))  },
            };
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
