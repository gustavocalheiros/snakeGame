using Windows.UI;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Shapes;

namespace snake
{
    class Food
    {
        private Rectangle foodRect = new Rectangle();
        public const int foodSize = 8;

        public Rectangle FoodRect
        {
            get
            {
                return foodRect;
            }

            set
            {
                foodRect = value;
            }
        }

        public Food()
        {
            FoodRect.Fill = new SolidColorBrush(Colors.Black);
            foodRect.StrokeThickness = 1;
            foodRect.Height =
            foodRect.Width = foodSize;
        }
        
        public bool checkColisionFood(Line head, double x1, double y1)
        {
            double x2 = x1 + foodRect.Width;
            double y2 = y1 + foodRect.Height;

            return ((head.X2 <= x2 && head.X2 >= x1) &&
                (head.Y2 <= y2 && head.Y2 >= y1));
        }
    }
}
