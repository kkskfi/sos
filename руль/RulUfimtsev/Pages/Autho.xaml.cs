using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Text;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Windows.Threading;
using RulUfimtsev.Entities;
using Brush = System.Drawing.Brush;
using Brushes = System.Drawing.Brushes;
using Color = System.Drawing.Color;
using FontStyle = System.Drawing.FontStyle;
using Image = System.Windows.Controls.Image;
using MessageBox = System.Windows.MessageBox;
using Pen = System.Drawing.Pen;
using Point = System.Drawing.Point;

namespace RulUfimtsev.Pages
{

    public partial class Autho : Page
    {
        private int countUnsuccessful = 0;
        public Autho()
        {
            InitializeComponent();
            imgCaptcha.Visibility = Visibility.Hidden;
            textBlockCaptcha.Visibility = Visibility.Hidden;
            btnRepeat.Visibility = Visibility.Hidden;
            txtPassword.Visibility = Visibility.Hidden;
        }

        private void btnEnterGuest_Click(object sender, RoutedEventArgs e)
        {
            NavigationService.Navigate(new Client(null),null);
        }
        private string text;

        private void btnEnter_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim(); 
            string password = txtPassword.Text.Trim();
            

            User user = new User();
            user = RulEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).FirstOrDefault();
            int usercount = RulEntities.GetContext().User.Where(p => p.UserLogin == login && p.UserPassword == password).Count();



            if (countUnsuccessful < 1)
            {
                if (usercount > 0)
                {

                    MessageBox.Show("Вы вошли под: " + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(),user);
                }

                else
                {
                    MessageBox.Show("Вы ввели неверно логин или пароль!");
                    countUnsuccessful++;
                    text = randomText();
                    clearFields();
                    imgCaptcha.Source = BitmapToImageSource(GenerateCaptcha(text));
                    imgCaptcha.Visibility = Visibility.Visible;
                    textBlockCaptcha.Visibility = Visibility.Visible;
                    btnRepeat.Visibility = Visibility.Visible;
                }
            }

            else if(countUnsuccessful == 1)
            {
                if(textBlockCaptcha.Text.ToLower() == text.ToLower() && usercount>0)
                {
                    MessageBox.Show("Вы вошли под: " + user.Role.RoleName.ToString());
                    LoadForm(user.Role.RoleName.ToString(),user);
                }

                else
                {
                    imgCaptcha.Visibility = Visibility.Hidden;
                    textBlockCaptcha.Visibility = Visibility.Hidden;
                    clearFields();
                    MessageBox.Show("Подождите 10 секунд");
                    countUnsuccessful = 0;

                    DispatcherTimer timer = new DispatcherTimer();
                    timer.Interval = new TimeSpan(0, 0, 10);
                    timer.Start();
                    btnEnter.IsEnabled = false;
                    timer.Tick += new EventHandler(Timer_Tick);
                }
            }

        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            btnEnter.IsEnabled = true;
            MessageBox.Show("Вам доступна авторизация");
        }

        BitmapImage BitmapToImageSource(Bitmap bitmap)
        {
            using (MemoryStream memory = new MemoryStream())
            {
                bitmap.Save(memory, System.Drawing.Imaging.ImageFormat.Bmp);
                memory.Position = 0;
                BitmapImage bitmapimage = new BitmapImage();
                bitmapimage.BeginInit();
                bitmapimage.StreamSource = memory;
                bitmapimage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapimage.EndInit();

                return bitmapimage;
            }
        }

        private string randomText()
        {
            Random rnd = new Random();
            string ALF = "1234567890QWERTYUIOPASDFGHJKLZXCVBNM";
            string text = "";

            for(int i = 0; i < 5; i++)
            {
                text += ALF[rnd.Next(ALF.Count()-1)];
            }
            return text;
        }


        private Bitmap GenerateCaptcha(string text)
        {
            int iHeight = 100;
            int iWidth = 400;
            Random oRandom = new Random();

            int[] aFontEmSizes = new int[] { 15, 20, 25, 30, 35 };

            string[] aFontNames = new string[]
            {
                "Comic Sans MS",
                 "Arial",
                 "Times New Roman",
                 "Georgia",
                 "Verdana",
                 "Geneva"
            };
            FontStyle[] aFontStyles = new FontStyle[]
            {
                 FontStyle.Bold,
                 FontStyle.Italic,
                 FontStyle.Regular,
                 FontStyle.Strikeout,
                 FontStyle.Underline
            };
            
            Pen[] colorpens = { Pens.Black, Pens.Red, Pens.RoyalBlue, Pens.Green, Pens.Yellow, Pens.White, Pens.Tomato, Pens.Sienna, Pens.Pink };

            string sCaptchaText = text;

            Bitmap oOutputBitmap = new Bitmap(iWidth, iHeight);
            Graphics oGraphics = Graphics.FromImage(oOutputBitmap);
            oGraphics.TextRenderingHint = TextRenderingHint.AntiAlias;

            oGraphics.Clear(Color.Gray);

            System.Drawing.Drawing2D.Matrix oMatrix = new System.Drawing.Drawing2D.Matrix();
            int i = 0;
            for (i = 0; i <= sCaptchaText.Length - 1; i++)
            {
                oMatrix.Reset();
                int iChars = sCaptchaText.Length;
                int x = iWidth / (iChars + 1) * i;
                int y = iHeight / 2;

                oMatrix.RotateAt(oRandom.Next(-40, 40), new PointF(x, y));
                oGraphics.Transform = oMatrix;

                oGraphics.DrawString
                (
                sCaptchaText.Substring(i, 1),
                new Font(aFontNames[oRandom.Next(aFontNames.Length - 1)],
                   aFontEmSizes[oRandom.Next(aFontEmSizes.Length - 1)],
                   aFontStyles[oRandom.Next(aFontStyles.Length - 1)]),
                new SolidBrush(Color.FromArgb(oRandom.Next(0, 100),
                   oRandom.Next(0, 100), oRandom.Next(0, 100))),
                x,
                oRandom.Next(10, 40)
                );
                oGraphics.ResetTransform();
            }

            oGraphics.DrawLine(colorpens[oRandom.Next(colorpens.Length)], new Point(0, 0), new Point(iWidth - 1, iHeight - 1));
            oGraphics.DrawLine(colorpens[oRandom.Next(colorpens.Length)], new Point(0, iHeight - 1), new Point(iWidth - 1, 0));

            for (int j = 0; j < Width; ++j)
                for (int k = 0; k < Height; ++k)
                    if (oRandom.Next() % 20 == 0)
                        oOutputBitmap.SetPixel(j, k, Color.White);
            return oOutputBitmap;
            
        }

        public void clearFields()
        {
            newCaptcha(1);
            textBlockCaptcha.Clear();
            txtPassword.Clear();
        }

        private void LoadForm(string _role, User user)
        {
            switch (_role)
            {
                case "Клиент":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Менеджер":
                    NavigationService.Navigate(new Client(user));
                    break;
                case "Администратор":
                    NavigationService.Navigate(new Admin(user));
                    break; 
            }

            
        }
        private void newCaptcha(int mode)
        {
            string newText = randomText();
            text = newText;
            imgCaptcha.Source = BitmapToImageSource(GenerateCaptcha(text));
            if (mode == 0)
            {
                imgCaptcha.Visibility = Visibility.Visible;
                textBlockCaptcha.Visibility = Visibility.Visible;
                btnRepeat.Visibility = Visibility.Visible;
                return;
            }

            imgCaptcha.Visibility = Visibility.Hidden;
            textBlockCaptcha.Visibility = Visibility.Hidden;
            btnRepeat.Visibility = Visibility.Hidden;


        }

        private void btnRepeat_Click(object sender, RoutedEventArgs e)
        {
            newCaptcha(0);
        }
        private bool Ischecked = false;
        private void btnSee_Click(object sender, RoutedEventArgs e)
        {
            if (Ischecked)
            {
                imgSee.Source = new BitmapImage(new Uri("D:\\VSProjects\\RulUfimtsev\\RulUfimtsev\\Resources\\See.png"));
                txtPassword.Visibility = Visibility.Hidden;
                Pswbox.Visibility = Visibility.Visible; 
                Ischecked = false;
            }

            else
            {
                imgSee.Source = new BitmapImage(new Uri("D:\\VSProjects\\RulUfimtsev\\RulUfimtsev\\Resources\\Close.png"));
                txtPassword.Visibility = Visibility.Visible;
                Pswbox.Visibility = Visibility.Hidden;
                Ischecked = true;
            }
            
        }

        

        private void txtPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            Pswbox.Password = txtPassword.Text;
        }

        private void Pswbox_PasswordChanged(object sender, RoutedEventArgs e)
        {
            txtPassword.Text = Pswbox.Password;
        }
    }
}
