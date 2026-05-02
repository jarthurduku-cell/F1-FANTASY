using System.Windows;
using System.Windows.Controls;

namespace WPF
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        private void EmailBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (EmailPlaceholder != null)
                EmailPlaceholder.Visibility = string.IsNullOrEmpty(EmailBox.Text)
                    ? Visibility.Visible : Visibility.Collapsed;
        }

        private void SignIn_Click(object sender, RoutedEventArgs e)
        {
            string email = EmailBox.Text.Trim();
            string password = PasswordBox.Password;

            // when clicked and everything is alright, then the homepage opens
           // would open hugo's screen



            if (string.IsNullOrEmpty(email) || string.IsNullOrEmpty(password))
            {
                MessageBox.Show("Please enter your email and password.", "Validation",
                    MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // TODO: Replace with real auth logic
            // var main = new MainWindow();
            // main.Show();
            // Close();
        }

        private void ForgotPassword_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Navigate to password reset
        }

        private void GoogleLogin_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Google OAuth flow
        }

        private void GitHubLogin_Click(object sender, RoutedEventArgs e)
        {
            // TODO: GitHub OAuth flow
        }

        private void SignUp_Click(object sender, RoutedEventArgs e)
        {
            // TODO: Open registration window
        }

        private void RememberMeCheck_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
