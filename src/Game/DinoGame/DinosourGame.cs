public class DinosourGame
{
    // 게임 상태 및 화면 설정
    private bool isRunning = true;
    private bool isGameOver = false;
    private const int GroundY = 15;
    private const int ScreenWidth = 60;

    // 공룡(플레이어) 변수
    private int dinoX = 8;
    private float dinoY = GroundY;
    private float velocityY = 0f;
    private const float Gravity = 0.8f;
    private const float JumpForce = -3.8f;
    private bool isJumping = false;

    // 장애물(선인장) 및 점수 변수
    private int cactusX = ScreenWidth - 5;
    private int score = 0;
    private int gameSpeed = 40; // 밀리초 단위 프레임 지연 (낮을수록 빠름)

    public void Run()
    {
        Console.CursorVisible = false;
        Console.SetWindowSize(ScreenWidth + 5, GroundY + 5);

        // 메인 게임 루프
        while (isRunning)
        {
            Input(); // AI 니가 다해
            Update();
            Rendering(); // AI 니가 다해

            Thread.Sleep(gameSpeed);
        }
    }

    private void Input()
    {
        if (Console.KeyAvailable)
        {
            ConsoleKeyInfo keyInfo = Console.ReadKey(true);

            // 게임 중 점프 (스페이스바 또는 위쪽 방향키)
            if (!isGameOver && (keyInfo.Key == ConsoleKey.Spacebar || keyInfo.Key == ConsoleKey.UpArrow))
            {
                if (!isJumping)
                {
                    velocityY = JumpForce;
                    isJumping = true;
                }
            }
            // 게임 오버 시 재시작 (R키) 또는 종료 (ESC키)
            else if (isGameOver)
            {
                if (keyInfo.Key == ConsoleKey.R)
                {
                    ResetGame();
                }
                else if (keyInfo.Key == ConsoleKey.Escape)
                {
                    isRunning = false;
                }
            }
        }
    }

    private void Update()
    {
        if (isGameOver) return;

        // 1. 공룡 점프 및 중력 물리 연산
        if (isJumping)
        {
            dinoY += velocityY;
            velocityY += Gravity;

            // 바닥에 착지했을 때
            if (dinoY >= GroundY)
            {
                dinoY = GroundY;
                velocityY = 0f;
                isJumping = false;
            }
        }

        // 2. 선인장 이동
        cactusX--;
        if (cactusX < 1)
        {
            cactusX = ScreenWidth - 2;
            score += 10;

            // 점수가 오를수록 게임 속도 증가
            if (gameSpeed > 15)
            {
                gameSpeed -= 1;
            }
        }

        // 3. 충돌 판정 (공룡과 선인장의 좌표가 겹칠 때)
        if (Math.Abs(dinoX - cactusX) <= 1 && (int)Math.Round(dinoY) >= GroundY - 1)
        {
            isGameOver = true;
        }
    }

    private void Rendering()
    {
        // 화면 깜빡임 방지를 위해 Clear() 대신 커서 위치 초기화 사용
        Console.SetCursorPosition(0, 0);

        // 상단 UI (점수 및 조작법)
        Console.WriteLine($"Score: {score:D5}   [Jump: Space / Up Arrow]          ");

        // 게임 필드 그리기
        for (int y = 1; y <= GroundY + 1; y++)
        {
            char[] line = new char[ScreenWidth];
            Array.Fill(line, ' ');

            // 바닥 그리기
            if (y == GroundY + 1)
            {
                Array.Fill(line, '=');
            }

            // 공룡 그리기 (2칸 높이)
            int currentDinoY = (int)Math.Round(dinoY);
            if (y == currentDinoY - 1)
            {
                line[dinoX] = 'O'; // 공룡 머리
                line[dinoX + 1] = '>';
            }
            else if (y == currentDinoY)
            {
                line[dinoX] = '█'; // 공룡 몸통
            }

            // 선인장 그리기 (2칸 높이)
            if (cactusX >= 0 && cactusX < ScreenWidth)
            {
                if (y == GroundY - 1 || y == GroundY)
                {
                    line[cactusX] = '#'; // 콘솔 폰트에 따라 깨지면 'Y'로 변경하세요
                    if (line[cactusX] == '#') line[cactusX] = 'Y'; // 안전한 ASCII 문자 대체
                }
            }

            Console.WriteLine(new string(line));
        }

        // 게임 오버 메시지 출력
        if (isGameOver)
        {
            Console.SetCursorPosition(ScreenWidth / 2 - 10, GroundY / 2);
            Console.Write("=== GAME OVER ===");
            Console.SetCursorPosition(ScreenWidth / 2 - 14, GroundY / 2 + 1);
            Console.Write("Press 'R' to Restart / 'ESC' to Exit");
        }
    }

    private void ResetGame()
    {
        dinoY = GroundY;
        velocityY = 0f;
        isJumping = false;
        cactusX = ScreenWidth - 5;
        score = 0;
        gameSpeed = 40;
        isGameOver = false;
        Console.Clear();
    }
}