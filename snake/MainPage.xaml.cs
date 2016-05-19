using System;
using System.Linq;
using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Core;

namespace snake
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        VirtualKey key;
        DispatcherTimer gameTimer = new DispatcherTimer();
        List<Line> snake = new List<Line>();
        Rectangle food = new Rectangle();
        Stack<Point> turns = new Stack<Point>();
        Random random = new Random();
        const int speed = 5;
        const int snakeSize = 15 * speed;
        const int gameLoopInterval = 50; //milisec
        int points = 0;

        private Line configureNewLine()
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = 3;
            line.Tag = key;

            if (turns.Count != 0)
            {
                Point p = turns.Peek();

                line.X1 = 
                line.X2 = p.X;
                line.Y1 = 
                line.Y2 = p.Y;
            } else
            {
                line.X1 = 
                line.Y1 = 
                line.Y2 = 0;
                line.X2 = snakeSize;
                line.Tag = key = VirtualKey.Right;
            }

            return line;
        }

        public MainPage()
        {
            this.InitializeComponent();
            food.Stroke = new SolidColorBrush(Colors.Black);
            food.StrokeThickness = 10;

            gameTimer.Tick += gameMainLoop;
            gameTimer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * gameLoopInterval);
            gameTimer.Start();

            snake.Add(configureNewLine());

            repositionFood();

            MyCanvas.Children.Add(snake.First());
            MyCanvas.Children.Add(food);

            Window.Current.CoreWindow.KeyDown += handleKeyPress;            
        }

        private void gameMainLoop(object sender, object e)
        {
            snakeMove();
            checkColisionSnake();
            checkColisionFood();
        }


        private void snakeMove()
        {
            //Move TAIL
            switch ((int)snake.First().Tag)
            {
                case (int)VirtualKey.Up:
                    snake.First().Y1 -= speed;
                    checkRemoveTail(snake.First().Y1, snake.First().Y2);
                    break;

                case (int)VirtualKey.Down:
                    snake.First().Y1 += speed;
                    checkRemoveTail(snake.First().Y1, snake.First().Y2);
                    break;

                case (int)VirtualKey.Left:
                    snake.First().X1 -= speed;
                    checkRemoveTail(snake.First().X1, snake.First().X2);
                    break;

                case (int)VirtualKey.Right:
                    snake.First().X1 += speed;
                    checkRemoveTail(snake.First().X1, snake.First().X2);
                    break;
            }

            //Move HEAD
            switch ((int)key)
            {
                case (int)VirtualKey.Up:
                    snake.Last().Y2 -= speed;
                    break;
                case (int)VirtualKey.Down:
                    snake.Last().Y2 += speed;
                    break;
                case (int)VirtualKey.Left:
                    snake.Last().X2 -= speed;
                    break;
                case (int)VirtualKey.Right:
                    snake.Last().X2 += speed;
                    break;
            }
            
        }

        private void checkColisionFood()
        {
            Line head = snake.Last();

            double x1 = Canvas.GetLeft(food);
            double y1 = Canvas.GetTop(food);

            double x2 = x1 + food.Width;
            double y2 = y1 + food.Height;

            if ((head.X2 <= x2 && head.X2 >= x1) &&
                (head.Y2 <= y2 && head.Y2 >= y1))
            {
                tbPoints.Text = "Points: " + ++points;
                repositionFood();
            }
        }

        private void checkColisionSnake()
        {
            Line head = snake.Last();

            for (int i = 0; i < snake.Count - 2; i++)
            {
                if((head.X2 <= snake[i].X2 && head.X2 >= snake[i].X1) &&
                    (head.Y2 <= snake[i].Y2 && head.Y2 >= snake[i].Y1))
                {
                    tbGameOver.Visibility = Visibility.Visible;
                    gameTimer.Stop();
                }
            }
        }

        private void checkRemoveTail(double v1, double v2)
        {
            if (v1 == v2)
            {
                snake.RemoveAt(0);
                MyCanvas.Children.RemoveAt(0);
                //TODO remove turnss
                turns.Pop();
            }
        }

        private void repositionFood()
        {
            Canvas.SetTop(food, random.Next(1, (int)MyCanvas.Height));
            Canvas.SetLeft(food, random.Next(1, (int)MyCanvas.Width));
        }

        private void handleKeyPress(CoreWindow s, KeyEventArgs e)
        {
            {
                if (e.VirtualKey >= VirtualKey.Left && e.VirtualKey <= VirtualKey.Down)
                {
                    key = e.VirtualKey;
                    turns.Push(new Point(snake.Last().X2, snake.Last().Y2));

                    Line l = configureNewLine();
                    snake.Add(l);
                    MyCanvas.Children.Add(l);
                }
            };
        }
    }
}
