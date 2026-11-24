namespace PoopDodgeGame
{
    public class Poop
    {
        public float X { get; private set; }
        public float Y { get; private set; }
        public int Size { get; private set; } // 1이면 1x1(*), 2면 2x2(**)
        private readonly float _speed;

        public Poop(float x, float y, int size, float speed)
        {
            X = x;
            Y = y;
            Size = size;
            _speed = speed;
        }

        public void Update(float deltaTime)
        {
            // Delta Time을 곱해 프레임 속도와 무관하게 일정한 속도로 낙하
            Y += _speed * deltaTime;
        }

        public bool CheckCollision(Player player)
        {
            int poopLeft = (int)X;
            int poopRight = (int)X + Size - 1;
            int poopTop = (int)Y;
            int poopBottom = (int)Y + Size - 1;

            // AABB 충돌 판정 (플레이어의 좌표가 별의 1x1 또는 2x2 영역 안에 들어왔는지 확인)
            bool hitX = player.X >= poopLeft && player.X <= poopRight;
            bool hitY = player.Y >= poopTop && player.Y <= poopBottom;

            return hitX && hitY;
        }
    }
}
