using System.Windows;

namespace project // Имя проекта: project
{
    public partial class LoginWindow : Window
    {
        public LoginWindow()
        {
            InitializeComponent();
        }

        // Этот метод срабатывает при нажатии на кнопку "Войти"
        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            // 1. Берем то, что ввел пользователь
            string login = LoginTextBox.Text;
            string password = PasswordInput.Password;

            // 2. Сбрасываем старые ошибки (прячем красный текст)
            ErrorTextBlock.Visibility = Visibility.Hidden;
            ErrorTextBlock.Text = "";

            // 3. Проверяем: не пустые ли поля?
            if (string.IsNullOrWhiteSpace(login) || string.IsNullOrWhiteSpace(password))
            {
                ErrorTextBlock.Text = "Введите логин и пароль";
                ErrorTextBlock.Visibility = Visibility.Visible;
                return; // Выходим, дальше не проверяем
            }

            // 4. Проверяем правильность (admin / admin)
            if (login == "admin" && password == "admin")
            {
                // Если всё верно - открываем Главное окно
                MainWindow mainWindow = new MainWindow();
                mainWindow.Show();

                // И закрываем текущее окно авторизации
                this.Close();
            }
            else
            {
                // Если неверно - показываем ошибку
                ErrorTextBlock.Text = "Неверный логин или пароль";
                ErrorTextBlock.Visibility = Visibility.Visible;

                // Очищаем поле пароля для повторного ввода
                PasswordInput.Clear();
                PasswordInput.Focus(); // Ставим курсор обратно в поле пароля
            }
        }
    }
}