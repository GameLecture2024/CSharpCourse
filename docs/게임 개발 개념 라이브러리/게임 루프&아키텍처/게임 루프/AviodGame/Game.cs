using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Threading;

namespace PoopDodgeGame
{
    public class Game
    {
        private readonly int _width;
        private readonly int _height;
        private bool _isRunning;
        private float _score;

        private readonly InputManager _inputManager;
        private readonly Player _player;
        private readonly List<Poop> _poops;
        private readonly Random _random;

        // 스폰 타이머 (Delta Time 누적용)
        private float _spawnTimer;
        private float _spawnInterval = 0.25f;

        // 화면 깜빡임 방지를 위한 렌더링 버퍼
        private readonly char[,] _screenBuffer;

        public Game(int width, int height)
        {
            _width = width;
            _height = height;
            _screenBuffer = new char[height, width];

            _inputManager = new InputManager();
            _player = new Player(startX: width / 2, y: height - 2, maxWidth: width);
            _poops = new List<Poop>();
            _random = new Random();

            // 입력 이벤트 구독
            _inputManager.OnMoveLeft += () => _player.MoveLeft();
            _inputManager.OnMoveRight += () => _player.MoveRight();
            _inputManager.OnQuit += () => _isRunning = false;
        }

        public void Run()
        {
            InitializeConsole();
            _isRunning = true;

            Stopwatch stopwatch = new Stopwatch();
            stopwatch.Start();
            double previousTime = stopwatch.Elapsed.TotalSeconds;

            // [핵심 게임 루프] Input -> Update -> Render
            while (_isRunning)
            {
                // Delta Time 계산 (초 단위)
                double currentTime = stopwatch.Elapsed.TotalSeconds;
                float deltaTime = (float)(currentTime - previousTime);
                previousTime = currentTime;

                // 1. 입력 (Input - 이벤트 발생)
                ProcessInput();

                // 2. 갱신 (Update - Delta Time 기반)
                Update(deltaTime);

                // 3. 렌더링 (Render - 화면 그리기)
                Render();

                // CPU 과점유 방지 및 프레임 안정화 (약 60 FPS 목표)
                Thread.Sleep(16);
            }

            ShowGameOver();
        }

        private void InitializeConsole()
        {
            Console.CursorVisible = false;
            Console.Clear();
        }

        private void ProcessInput()
        {
            _inputManager.PollInput();
        }

        private void Update(float deltaTime)
        {
            _score += deltaTime * 10f; // 생존 시간에 비례해 점수 증가
            _spawnTimer += deltaTime;

            // 일정 시간마다 새로운 별(똥) 생성
            if (_spawnTimer >= _spawnInterval)
            {
                _spawnTimer = 0f;
                SpawnPoop();
            }

            // 모든 별 이동 및 충돌 체크
            for (int i = _poops.Count - 1; i >= 0; i--)
            {
                Poop poop = _poops[i];
                poop.Update(deltaTime);

                // 플레이어와 충돌 검사
                if (poop.CheckCollision(_player))
                {
                    _isRunning = false;
                    return;
                }

                // 화면 바닥을 벗어나면 리스트에서 제거
                if (poop.Y >= _height - 1)
                {
                    _poops.RemoveAt(i);
                }
            }
        }

        private void SpawnPoop()
        {
            // 20% 확률로 2x2 큰 별 생성, 80% 확률로 1x1 일반 별 생성
            bool isBig = _random.NextDouble() < 0.2;
            int size = isBig ? 2 : 1;

            // 벽 안쪽에서 생성되도록 X 좌표 계산
            int spawnX = _random.Next(1, _width - size);
            // 별마다 떨어지는 속도를 약간 다르게 설정 (초당 8~16칸)
            float speed = (float)(_random.NextDouble() * 8.0 + 8.0);

            _poops.Add(new Poop(spawnX, 1, size, speed));
        }

        private void Render()
        {
            // 1. 버퍼 초기화 및 테두리 그리기
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    if (y == 0 || y == _height - 1 || x == 0 || x == _width - 1)
                        _screenBuffer[y, x] = '#';
                    else
                        _screenBuffer[y, x] = ' ';
                }
            }

            // 2. 별(똥) 버퍼에 그리기
            foreach (var poop in _poops)
            {
                int drawX = (int)poop.X;
                int drawY = (int)poop.Y;

                for (int dy = 0; dy < poop.Size; dy++)
                {
                    for (int dx = 0; dx < poop.Size; dx++)
                    {
                        int targetX = drawX + dx;
                        int targetY = drawY + dy;

                        if (targetX > 0 && targetX < _width - 1 && targetY > 0 && targetY < _height - 1)
                        {
                            _screenBuffer[targetY, targetX] = '*';
                        }
                    }
                }
            }

            // 3. 플레이어 버퍼에 그리기
            _screenBuffer[_player.Y, _player.X] = '▲';

            // 4. 버퍼 내용을 한 번에 콘솔에 출력 (커서를 0,0으로 이동하여 덮어쓰기)
            Console.SetCursorPosition(0, 0);
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            for (int y = 0; y < _height; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    sb.Append(_screenBuffer[y, x]);
                }
                sb.AppendLine();
            }
            sb.AppendLine($"Score: {(int)_score}  |  Move: <- ->  |  Quit: ESC");
            Console.Write(sb.ToString());
        }

        private void ShowGameOver()
        {
            Console.SetCursorPosition(0, _height + 2);
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine("========================================");
            Console.WriteLine($"   GAME OVER! 최종 점수: {(int)_score}점");
            Console.WriteLine("========================================");
            Console.ResetColor();
            Console.CursorVisible = true;
        }
    }
}
