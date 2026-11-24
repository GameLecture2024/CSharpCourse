using System;

namespace PoopDodgeGame
{
    public class InputManager
    {
        // 입력 이벤트 정의
        public event Action? OnMoveLeft;
        public event Action? OnMoveRight;
        public event Action? OnQuit;

        public void PollInput()
        {
            // 키가 눌렸을 때만 논블로킹(Non-blocking)으로 처리
            while (Console.KeyAvailable)
            {
                ConsoleKeyInfo keyInfo = Console.ReadKey(intercept: true);

                switch (keyInfo.Key)
                {
                    case ConsoleKey.LeftArrow:
                        OnMoveLeft?.Invoke();
                        break;
                    case ConsoleKey.RightArrow:
                        OnMoveRight?.Invoke();
                        break;
                    case ConsoleKey.Escape:
                        OnQuit?.Invoke();
                        break;
                }
            }
        }
    }
}
