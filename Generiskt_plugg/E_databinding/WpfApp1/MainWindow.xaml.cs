using System.ComponentModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

public class User : INotifyPropertyChanged
{
    private string firstName = string.Empty;
    public event PropertyChangedEventHandler? PropertyChanged;
    public string FirstName
    {
        get { return firstName; } 
        set 
        { 
            firstName = value;
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(FirstName)));
        }
    }
    public string LastName { get; set; } = string.Empty;
    public int Age { get; set; }
}
namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {

        public MainWindow()
        {
            InitializeComponent();
            DataContext = new User
            {
                FirstName = "Furqan",
                LastName = "Farooqi",
                Age = 25
            };
            
        }

        private void MyButton_Click(object sender, RoutedEventArgs e)
        {
            User user = (User)DataContext;
            user.FirstName = "Erik";
        }
    }
}