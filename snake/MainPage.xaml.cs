using System;
using System.Linq;
using Windows.Storage;
using Windows.System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Shapes;

namespace SnakeGame
{
    public sealed partial class MainPage : Page
    {
        private GamePlay game;

        private const string ARROW_LEFT = "ms-appx:///Assets/setaesquerda.jpg";
        private const string ARROW_UP = "ms-appx:///Assets/setacima.jpg";
        private const string ARROW_DOWN = "ms-appx:///Assets/setabaixo.jpg";
        private const string ARROW_RIGHT = "ms-appx:///Assets/setadireita.jpg";

        public MainPage()
        {
            this.InitializeComponent();

            textBox.Text = "\u25BC \u25BC CONTROL AREA \u25BC \u25BC";

            showInstructionsAndStart();
        }

        private async void showInstructionsAndStart()
        {
            MessageDialog alert = new MessageDialog(
                "Hi! \n" + 
                "Use the \"CONTROL AREA\" (in light blue) to control the snake.  \n" + 
                "You should tap (or click) and drag in the direction you want the snake to to go.\n" + 
                "e.g.: Tap on the top of the CONTROL AREA and drag your finger (or mouse) until the bottom of it will make the snake go down. \n"+
                "You can still use the arrows to play.\n" +
                "GOOD LUCK!!!", "Game Instructions");

            await alert.ShowAsync();
            setUp();
        }

        private void setUp()
        {
            game = new GamePlay(this, gameCanvas);

            gameCanvas.Children.Add(game.snake.First());
            gameCanvas.Children.Add(game.food);

            Window.Current.CoreWindow.KeyDown += game.handleKeyPress;
            tapArea.PointerPressed += game.pointerPressed;
            tapArea.PointerReleased += game.pointerReleased;

            var localSettings = ApplicationData.Current.LocalSettings;

            if (localSettings.Values["record"] == null)
                localSettings.Values["record"] = 0;

            tbRecord.Text = "Record: " + localSettings.Values["record"];
        }
        

        private void cleanUp()
        {
            tbGameOver.Visibility = Visibility.Collapsed;

            foreach (Line l in game.snake)
            {
                gameCanvas.Children.Remove(l);
            }
            gameCanvas.Children.Remove(game.food);

            Window.Current.CoreWindow.KeyDown -= game.handleKeyPress;
            tapArea.PointerPressed -= game.pointerPressed;
            tapArea.PointerReleased -= game.pointerReleased;
        }        

        public void updateUI()
        {
            tbPoints.Text = "Points: " + game.points;

            var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
            string str = localSettings.Values["record"].ToString();

            //update record
            if (Int32.Parse(str) < game.points)
            { 
                localSettings.Values["record"] = game.points;
            }
        }

        public void repositionElement(Shape element, double x, double y)
        {
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
        }
        
        public void gameOver()
        {
            //cleanUp();
            tbGameOver.Visibility = Visibility.Visible;
        }

        // Arrow fade animation.
        public void startAnimation(VirtualKey key)
        {
            switch(key)
            {
                case VirtualKey.Up:
                    image.Source = new BitmapImage(new Uri(ARROW_UP));
                    break;
                case VirtualKey.Down:
                    image.Source = new BitmapImage(new Uri(ARROW_DOWN));
                    break;
                case VirtualKey.Left:
                    image.Source = new BitmapImage(new Uri(ARROW_LEFT));
                    break;
                case VirtualKey.Right:
                    image.Source = new BitmapImage(new Uri(ARROW_RIGHT));
                    break;
            }            
            
            fade.Begin();            
        }

        private void onClickRestart(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            cleanUp();
            setUp();
        }
    }
}
