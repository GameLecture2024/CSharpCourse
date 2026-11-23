class Game
{
    private static bool isRunning = true;
    private static int width = 20;
    private static int height = 15;

    private static int playerX = 10;
    private static int enemyX = 5;
    private static int enemyY = 0;
    private static int score = 0;
    private static Random rand = new Random();

    // Game 클래스. 시작 중. 게임 영역 크기, 플레이어 좌표, 몬스터좌표, 점수

    // Run()핵심. 게임이다.
    public static void Run()
    {
        Console.CursorVisible = false;

        while (isRunning)
        {
            Input();
            Update();
            Rendering();

            Thread.Sleep(80);
        }

        Console.Clear();
        Console.WriteLine("==================");
        Console.WriteLine("    GAME OVER!    ");
        Console.WriteLine($"   최종 점수: {score}");
        Console.WriteLine("==================");
    }

    // 1. 입력 기능
    private static void Input()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo key = Console.ReadKey(true);
            if (key.Key == ConsoleKey.LeftArrow && playerX > 1)
            {
                playerX--;
            }
            else if (key.Key == ConsoleKey.RightArrow && playerX < width - 2)
            {
                playerX++;
            }
            else if (key.Key == ConsoleKey.Escape)
            {
                isRunning = false;
            }
        }
    }

    // 2. 업데이트 기능
    private static void Update()
    {
        enemyY++;

        if (enemyY >= height - 1)
        {
            enemyY = 0;
            enemyX = rand.Next(1, width - 1);
            score += 10;
        }

        if (enemyY == height - 2 && enemyX == playerX)
        {
            isRunning = false;
        }
    }

    // 3. 렌더링 기능
    private static void Rendering()
    {
        Console.SetCursorPosition(0, 0);
        Console.WriteLine($"점수: {score}  (이동: ← → / 종료: ESC)");

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                if (x == 0 || x == width - 1 || y == height - 1)
                {
                    Console.Write("#");
                }
                else if (y == height - 2 && x == playerX)
                {
                    Console.Write("▲");
                }
                else if (y == enemyY && x == enemyX)
                {
                    Console.Write("▼");
                }
                else
                {
                    Console.Write(" ");
                }
            }
            Console.WriteLine();
        }
    }
}