using System;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SnakeGame
{
    class GamePlay
    {
        private Point startPointClick;
        private MainPage mainPage;
        private Canvas MyCanvas;
        private VirtualKey key = VirtualKey.Right;
        public Snake snake;

        private Random random = new Random();
        private DispatcherTimer gameTimer = new DispatcherTimer();

        public int points = 0;
        private const int GAME_LOOP_INTERVAL = 60; //milisec

        //food
        public Rectangle food { get; set; }
        public const int FOOD_SIZE = 8;

        public GamePlay(MainPage mainPage, Canvas MyCanvas)
        {
            this.mainPage = mainPage;
            this.MyCanvas = MyCanvas;

            snake = new Snake();

            food = new Rectangle();
            food.Fill = new SolidColorBrush(Colors.Black);
            food.StrokeThickness = 1;
            food.Height =
            food.Width = FOOD_SIZE;

            gameTimer.Tick += gameMainLoop;
            gameTimer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * GAME_LOOP_INTERVAL);
            gameTimer.Start();

            mainPage.repositionElement(food, 
                random.Next(1, (int)MyCanvas.Width - FOOD_SIZE), 
                random.Next(1, (int)MyCanvas.Height - FOOD_SIZE));

            snake.setUp(key);
        }

        private bool isKeyValid(VirtualKey newKey)
        {
            // is the key an arrow?
            if (newKey >= VirtualKey.Left && newKey <= VirtualKey.Down)
                return hasDirectionChanged(newKey);

            return false;
        }

        // Vertical to Horizontal or vice-versa
        private bool hasDirectionChanged(VirtualKey newKey)
        {
            return (int)key % 2 != (int)newKey % 2;
        }

        public void gameMainLoop(object sender, object e)
        {
            snakeMove(key);
            if (checkColisionSnake())
            {
                gameTimer.Stop();
                mainPage.gameOver();
            }
                
            if (checkColisionFood())
            {
                points++;

                mainPage.repositionElement(food, 
                    random.Next(1, (int)MyCanvas.Width - FOOD_SIZE), 
                    random.Next(1, (int)MyCanvas.Height - FOOD_SIZE));
                snake.grow = true;
            }

            mainPage.updateUI();
        }

        // keyboard handling
        public void handleKeyPress(CoreWindow s, KeyEventArgs e)
        {
            //is arrow?
            if (isKeyValid(e.VirtualKey))
            {
                key = e.VirtualKey;
                snake.turns.Push(new Point(snake.Last().X2, snake.Last().Y2));

                Line l = snake.changeDirection(key);
                MyCanvas.Children.Add(l);
            }
        }

        public void moveTail()
        {
            switch ((int)snake.First().Tag)
            {
                case (int)VirtualKey.Up:
                    snake.First().Y1 -= Snake.speed;
                    checkRemoveTail(snake.First().Y1, snake.First().Y2);
                    break;

                case (int)VirtualKey.Down:
                    snake.First().Y1 += Snake.speed;
                    checkRemoveTail(snake.First().Y1, snake.First().Y2);
                    break;

                case (int)VirtualKey.Left:
                    snake.First().X1 -= Snake.speed;
                    checkRemoveTail(snake.First().X1, snake.First().X2);
                    break;

                case (int)VirtualKey.Right:
                    snake.First().X1 += Snake.speed;
                    checkRemoveTail(snake.First().X1, snake.First().X2);
                    break;
            }
        }

        public bool checkRemoveTail(double v1, double v2)
        {
            if (v1 == v2)
            {
                MyCanvas.Children.Remove(snake[0]);
                snake.RemoveAt(0);
                snake.turns.Pop();

                return true;
            }

            return false;
        }

        // Moves head and tail. 
        // OBS: if has eaten food (snake.grow == true), tail does is not moved.
        public void snakeMove(VirtualKey key)
        {
            if (!snake.grow)
                moveTail();
            else
                snake.grow = false;

            moveHead(key);
        }

        public void moveHead(VirtualKey key)
        {
            switch ((int)key)
            {
                case (int)VirtualKey.Up:
                    snake.Last().Y2 -= Snake.speed;
                    break;
                case (int)VirtualKey.Down:
                    snake.Last().Y2 += Snake.speed;
                    break;
                case (int)VirtualKey.Left:
                    snake.Last().X2 -= Snake.speed;
                    break;
                case (int)VirtualKey.Right:
                    snake.Last().X2 += Snake.speed;
                    break;
            }
        }

        //check snake colision on boundary and on itself, for game over.
        public bool checkColisionSnake()
        {
            Line head = snake.Last();
            
            //colision boundary
            if (head.X2 <= 0 || head.X2 >= MyCanvas.Width ||
                head.Y2 <= 0 || head.Y2 >= MyCanvas.Height)
            {
                return true;
            }
            else
            {
                double xLarge, xSmall, yLarge, ySmall;

                // colision itself
                for (int i = 0; i < snake.Count - 2; i++)
                {
                    //adjust values for comparison regardless of the snake's direction in the axis.
                    // X axis
                    if(snake[i].X2 > snake[i].X1)
                    {
                        xLarge = snake[i].X2;
                        xSmall = snake[i].X1;
                    }
                    else
                    {
                        xSmall = snake[i].X2;
                        xLarge = snake[i].X1;
                    }

                    // Y axis
                    if (snake[i].Y2 > snake[i].Y1)
                    {
                        yLarge = snake[i].Y2;
                        ySmall = snake[i].Y1;
                    }
                    else
                    {
                        ySmall = snake[i].Y2;
                        yLarge = snake[i].Y1;
                    }

                    //comparison
                    if ((head.X2 <= xLarge && head.X2 >= xSmall) &&
                        (head.Y2 <= yLarge && head.Y2 >= ySmall))                        
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        public bool checkColisionFood()
        {
            Line head = snake.Last(); 
            double x1 = Canvas.GetLeft(food);
            double y1 = Canvas.GetTop(food);

            double x2 = x1 + food.Width;
            double y2 = y1 + food.Height;

            return ((head.X2 <= x2 && head.X2 >= x1) &&
                (head.Y2 <= y2 && head.Y2 >= y1));
        }

        // touch released! compare with first position to calculate new direction.
        public void pointerReleased(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            Point endPointClick = e.GetCurrentPoint(null).Position;

            double xDiff = startPointClick.X - endPointClick.X;
            double yDiff = startPointClick.Y - endPointClick.Y;

            VirtualKey newKey;
            snake.turns.Push(new Point(snake.Last().X2, snake.Last().Y2));

            if (Math.Abs(xDiff) > Math.Abs(yDiff))
                newKey = startPointClick.X > endPointClick.X ? VirtualKey.Left : VirtualKey.Right;
            else
                newKey = startPointClick.Y > endPointClick.Y ? VirtualKey.Up : VirtualKey.Down;

            if (hasDirectionChanged(newKey))
            {
                key = newKey;
                Line l = snake.changeDirection(key);
                MyCanvas.Children.Add(l);
                mainPage.startAnimation(key);
            }
        }

        // touch pressed! get position and save
        public void pointerPressed(object sender, Windows.UI.Xaml.Input.PointerRoutedEventArgs e)
        {
            startPointClick = e.GetCurrentPoint(null).Position;
        }
    }
}
