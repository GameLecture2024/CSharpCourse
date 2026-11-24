namespace PoopDodgeGame
{
    public class Player
    {
        public int X { get; private set; }
        public int Y { get; private set; }
        private readonly int _maxWidth;

        public Player(int startX, int y, int maxWidth)
        {
            X = startX;
            Y = y;
            _maxWidth = maxWidth;
        }

        public void MoveLeft()
        {
            // 왼쪽 벽(X = 0)을 넘지 않도록 제한
            if (X > 1)
            {
                X--;
            }
        }

        public void MoveRight()
        {
            // 오른쪽 벽(X = _maxWidth - 1)을 넘지 않도록 제한
            if (X < _maxWidth - 2)
            {
                X++;
            }
        }
    }
}
