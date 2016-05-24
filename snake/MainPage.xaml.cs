using System.Linq;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace snake
{
    public sealed partial class MainPage : Page
    {
        private GamePlay game;        

        public MainPage()
        {
            this.InitializeComponent();
            setUpCanvasAndEvents();
        }
        
        public void updateUI()
        {
            children.Text = "children: " + MyCanvas.Children.Count + " turns: " + game.snake.turns.Count;
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

        private void onClick(object sender, Windows.UI.Xaml.Input.TappedRoutedEventArgs e)
        {
            cleanCanvasAndEvents();
            setUpCanvasAndEvents();
        }

        private void cleanCanvasAndEvents()
        {
            tbGameOver.Visibility = Visibility.Collapsed;

            foreach (Line l in game.snake)
            {
                MyCanvas.Children.Remove(l);
            }
            MyCanvas.Children.Remove(game.food.FoodRect);
            Window.Current.CoreWindow.KeyDown -= game.handleKeyPress;
        }

        private void setUpCanvasAndEvents()
        {
            game = new GamePlay(this, MyCanvas);

            MyCanvas.Children.Add(game.snake.First());
            MyCanvas.Children.Add(game.food.FoodRect);

            Window.Current.CoreWindow.KeyDown += game.handleKeyPress;
        }
    }
}
