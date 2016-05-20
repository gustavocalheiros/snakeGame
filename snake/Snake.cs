using System.Collections.Generic;
using System.Linq;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace snake
{
    class Snake : List<Line>
    {
        public Stack<Point> turns = new Stack<Point>();
        //public List<Line> snake = new List<Line>();
        public const int speed = 5;
        public const int snakeSize = 15 * speed;
        public const int snakeThickness = 5;
        public bool grow = false;
        Canvas myCanvas;

        public Snake(Canvas myCanvas)
        {
            this.myCanvas = myCanvas;
        }

        public Line addLine(VirtualKey key)
        {
            Line l = configureNewLine(key);
            this.Add(l);

            return l;
        }

        public Line configureNewLine(VirtualKey key)
        {
            Line line = new Line();
            line.Stroke = new SolidColorBrush(Colors.Black);
            line.StrokeThickness = Snake.snakeThickness;
            line.Tag = key;

            if (turns.Count != 0)
            {
                Point p = turns.Peek();

                line.X1 =
                line.X2 = p.X;
                line.Y1 =
                line.Y2 = p.Y;
            }
            else
            {
                line.X1 =
                line.Y1 =
                line.Y2 = Snake.snakeThickness;
                line.X2 = Snake.snakeSize;
                line.Tag = key = VirtualKey.Right;
            }

            return line;
        }

        public void moveTail()
        {
            switch ((int)this.First().Tag)
            {
                case (int)VirtualKey.Up:
                    this.First().Y1 -= speed;
                    checkRemoveTail(this.First().Y1, this.First().Y2);
                    break;

                case (int)VirtualKey.Down:
                    this.First().Y1 += speed;
                    checkRemoveTail(this.First().Y1, this.First().Y2);
                    break;

                case (int)VirtualKey.Left:
                    this.First().X1 -= speed;
                    checkRemoveTail(this.First().X1, this.First().X2);
                    break;

                case (int)VirtualKey.Right:
                    this.First().X1 += speed;
                    checkRemoveTail(this.First().X1, this.First().X2);
                    break;
            }
        }

        public bool checkRemoveTail(double v1, double v2)
        {
            if (v1 == v2)
            {
                myCanvas.Children.Remove(this[0]);
                this.RemoveAt(0);
                turns.Pop();

                return true;
            }

            return false;
        }

        public void snakeMove(VirtualKey key)
        {
            if (!grow)
                moveTail();
            else
                grow = false;

            moveHead(key);
        }

        public void moveHead(VirtualKey key)
        {
            switch ((int)key)
            {
                case (int)VirtualKey.Up:
                    this.Last().Y2 -= speed;
                    break;
                case (int)VirtualKey.Down:
                    this.Last().Y2 += speed;
                    break;
                case (int)VirtualKey.Left:
                    this.Last().X2 -= speed;
                    break;
                case (int)VirtualKey.Right:
                    this.Last().X2 += speed;
                    break;
            }
        }

        public bool checkColisionSnake(double horBoundary, double verBoundary)
        {
            Line head = this.Last();

            //colision boundary
            if (head.X2 <= 0 || head.X2 >= horBoundary ||
                head.Y2 <= 0 || head.Y2 >= verBoundary)
            {
                return true;
            }
            else
            {
                for (int i = 0; i < this.Count - 2; i++)
                {
                    // colision itself
                    if ((head.X2 <= this[i].X2 && head.X2 >= this[i].X1) &&
                        (head.Y2 <= this[i].Y2 && head.Y2 >= this[i].Y1))
                    {
                        return true;
                    }
                }
            }

            return false;
        }
    }
}
