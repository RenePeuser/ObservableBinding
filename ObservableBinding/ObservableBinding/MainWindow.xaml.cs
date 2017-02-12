using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;

namespace ObservableBinding
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        private int counter = 0;

        public MainWindow()
        {
            InitializeComponent();
            MyStrings = new ObservableCollection<string>();
            this.DataContext = this;
        }

        private ObservableCollection<string> myStrings;

        public ObservableCollection<string> MyStrings
        {
            get
            {
                return myStrings;
            }
            set
            {
                myStrings = value;
                OnPropertyChanged();
            }
        }

        private void ButtonBase_OnClick(object sender, RoutedEventArgs e)
        {
            MyStrings.Add(counter.ToString());
            counter++;
        }

        private void ChangeCollection(object sender, RoutedEventArgs e)
        {
            counter = 0;
            MyStrings = new ObservableCollection<string>();
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
