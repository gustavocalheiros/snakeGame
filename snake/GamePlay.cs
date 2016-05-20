using System;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI.Core;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Shapes;

namespace snake
{
    class GamePlay
    {
        MainPage mainPage;
        Canvas MyCanvas;
        private VirtualKey key = VirtualKey.Right;
        public Snake snake;
        private Random random = new Random();
        public Food food;
        private const int gameLoopInterval = 30; //milisec
        private DispatcherTimer gameTimer = new DispatcherTimer();
        public int points = 0;

        public GamePlay(MainPage mainPage, Canvas MyCanvas)
        {
            this.mainPage = mainPage;
            this.MyCanvas = MyCanvas;

            snake = new Snake(MyCanvas);
            food = new Food();

            gameTimer.Tick += gameMainLoop;
            gameTimer.Interval = new TimeSpan(TimeSpan.TicksPerMillisecond * gameLoopInterval);
            gameTimer.Start();

            mainPage.repositionElement(food.FoodRect, 
                random.Next(1, (int)MyCanvas.Width - Food.foodSize), 
                random.Next(1, (int)MyCanvas.Height - Food.foodSize));

            snake.addLine(key);
        }

        private bool isKeyValid(VirtualKey currentKey, VirtualKey newKey)
        {
            // is the key an arrow?
            if (newKey >= VirtualKey.Left && newKey <= VirtualKey.Down)
                // has the direction changed?
                if ((int)currentKey % 2 != (int)newKey % 2)
                    return true;

            return false;
        }

        public void gameMainLoop(object sender, object e)
        {
            snake.snakeMove(key);
            if (snake.checkColisionSnake(MyCanvas.Width, MyCanvas.Height))
            {
                gameTimer.Stop();
                mainPage.gameOver();
            }
                
            if (food.checkColisionFood(snake.Last(), Canvas.GetLeft(food.FoodRect), Canvas.GetTop(food.FoodRect)))
            {
                points++;

                mainPage.repositionElement(food.FoodRect, 
                    random.Next(1, (int)MyCanvas.Width - Food.foodSize), 
                    random.Next(1, (int)MyCanvas.Height - Food.foodSize));
                snake.grow = true;
            }

            mainPage.updateUI();
        }

        public void handleKeyPress(CoreWindow s, KeyEventArgs e)
        {
            if (isKeyValid(key, e.VirtualKey))
            {
                key = e.VirtualKey;
                snake.turns.Push(new Point(snake.Last().X2, snake.Last().Y2));

                Line l = snake.addLine(key);
                MyCanvas.Children.Add(l);
            }
        }
    }
}
