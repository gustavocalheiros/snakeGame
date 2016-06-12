using System;
using System.Linq;
using Windows.Foundation;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Animation;
using Windows.UI.Xaml.Shapes;

namespace SnakeGame
{
    public sealed partial class MainPage : Page
    {
        private GamePlay game;

        public MainPage()
        {
            this.InitializeComponent();

            textBox.Text = "\u25BC \u25BC CONTROL AREA \u25BC \u25BC";

            
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
            //children.Text = "children: " + MyCanvas.Children.Count + " turns: " + game.snake.turns.Count;
            tbPoints.Text = "Points: " + game.points;
        }

        public void repositionElement(Shape element, double x, double y)
        {
            Canvas.SetTop(element, y);
            Canvas.SetLeft(element, x);
        }
        
        public void gameOver()
        {
            tbGameOver.Visibility = Visibility.Visible;
        }

        public void startAnimation()
        {
            //image.Visibility = Visibility.Collapsed;
            //image.Visibility = Visibility.Visible;
            //image.Opacity = 1;

            //Storyboard storyboard = new Storyboard();
            //Duration duration = new Duration(TimeSpan.FromSeconds(2));

            //storyboard.Duration = duration;
            ////Duration duration = new Duration(TimeSpan.FromSeconds(2));

            //FadeOutThemeAnimation fadeOut = new FadeOutThemeAnimation();
            //DoubleAnimation scalex = new DoubleAnimation()
            //{
            //    From = 0,
            //    To = 8,
            //    AutoReverse = true,
            //    Duration = TimeSpan.FromSeconds(2)
            //};

            //fadeOut.Duration = duration;
            //storyboard.Children.Add(fadeOut);
            //Storyboard.SetTarget(fadeOut, image);
            //Storyboard.SetTargetProperty(fadeOut, "Opacity");

            //storyboard.Seek(TimeSpan.Zero);
            //storyboard.Begin();
            Storyboard.SetTargetName("image");
            fade.Begin();            
        }

        private void onClickRestart(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            cleanUp();
            setUp();
        }
    }
}
