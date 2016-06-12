using System.Collections.Generic;
using Windows.Foundation;
using Windows.System;
using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace SnakeGame
{
    class Snake : List<Line>
    {
        public Stack<Point> turns = new Stack<Point>();
        public const int speed = 5;
        public const int size = 15 * speed;
        public const int thickness = 5;
        public bool grow = false;
        private SolidColorBrush color = new SolidColorBrush(Colors.Black);

        private Line newLine(VirtualKey key)
        {
            Line line = new Line();
            line.Stroke = color;
            line.StrokeThickness = thickness;
            line.Tag = key;
            return line;
        }

        public Line changeDirection(VirtualKey key)
        {
            Line line = newLine(key);
            
            Point p = turns.Peek();

            line.X1 =
            line.X2 = p.X;
            line.Y1 =
            line.Y2 = p.Y;

            Add(line);
            return line;
        }

        public Line setUp(VirtualKey key)
        {
            Line line = newLine(key);
            
            line.X1 =
            line.Y1 =
            line.Y2 = thickness;
            line.X2 = size;

            Add(line);
            return line;
        }
    }
}
