using System;


/*
N×N 크기의 공간에 물고기 M마리와 아기 상어 1마리가 있다. 공간은 1×1 크기의 정사각형 칸으로 나누어져 있다. 한 칸에는 물고기가 최대 1마리 존재한다.

아기 상어와 물고기는 모두 크기를 가지고 있고, 이 크기는 자연수이다. 가장 처음에 아기 상어의 크기는 2이고, 아기 상어는 1초에 상하좌우로 인접한 한 칸씩 이동한다.

아기 상어는 자신의 크기보다 큰 물고기가 있는 칸은 지나갈 수 없고, 나머지 칸은 모두 지나갈 수 있다. 아기 상어는 자신의 크기보다 작은 물고기만 먹을 수 있다. 따라서, 크기가 같은 물고기는 먹을 수 없지만, 그 물고기가 있는 칸은 지나갈 수 있다.

아기 상어가 어디로 이동할지 결정하는 방법은 아래와 같다.

더 이상 먹을 수 있는 물고기가 공간에 없다면 아기 상어는 엄마 상어에게 도움을 요청한다.
먹을 수 있는 물고기가 1마리라면, 그 물고기를 먹으러 간다.
먹을 수 있는 물고기가 1마리보다 많다면, 거리가 가장 가까운 물고기를 먹으러 간다.
거리는 아기 상어가 있는 칸에서 물고기가 있는 칸으로 이동할 때, 지나야하는 칸의 개수의 최솟값이다.
거리가 가까운 물고기가 많다면, 가장 위에 있는 물고기, 그러한 물고기가 여러마리라면, 가장 왼쪽에 있는 물고기를 먹는다.
아기 상어의 이동은 1초 걸리고, 물고기를 먹는데 걸리는 시간은 없다고 가정한다. 즉, 아기 상어가 먹을 수 있는 물고기가 있는 칸으로 이동했다면, 이동과 동시에 물고기를 먹는다. 물고기를 먹으면, 그 칸은 빈 칸이 된다.

아기 상어는 자신의 크기와 같은 수의 물고기를 먹을 때 마다 크기가 1 증가한다. 예를 들어, 크기가 2인 아기 상어는 물고기를 2마리 먹으면 크기가 3이 된다.

공간의 상태가 주어졌을 때, 아기 상어가 몇 초 동안 엄마 상어에게 도움을 요청하지 않고 물고기를 잡아먹을 수 있는지 구하는 프로그램을 작성하시오.

입력
첫째 줄에 공간의 크기 N(2 ≤ N ≤ 20)이 주어진다.

둘째 줄부터 N개의 줄에 공간의 상태가 주어진다. 공간의 상태는 0, 1, 2, 3, 4, 5, 6, 9로 이루어져 있고, 아래와 같은 의미를 가진다.

0: 빈 칸
1, 2, 3, 4, 5, 6: 칸에 있는 물고기의 크기
9: 아기 상어의 위치
아기 상어는 공간에 한 마리 있다.

출력
첫째 줄에 아기 상어가 엄마 상어에게 도움을 요청하지 않고 물고기를 잡아먹을 수 있는 시간을 출력한다.

ex)
입력 예제
3
0 0 1
0 0 0
0 9 0

출력 예제
3
 */

using System.Collections.Generic;

// TODO 예제 2개 정답이 다름

namespace BackJoon_16236
{
    public class Direction
    {   // up, left, right, down (같은 거리일 경우, 가장 위 물고기 -> 가장 왼쪽 물고기 조건
        public static int[] X = { 0, -1, 1, 0 };
        public static int[] Y = { -1, 0, 0, 1 };
    }

    public struct Point
    {
        public int x, y;

        public Point(int _x, int _y)
        {
            x = _x; 
            y = _y;
        }
    }


    internal class Program
    {
        static void Main(string[] args)
        {
            int mapSize = int.Parse(Console.ReadLine());

            int[,] inputMap = new int[mapSize, mapSize];
            

            Queue<Point> nextPoints = new Queue<Point>();


            // Read Console Map & Find Shark Position
            for (int y = 0; y < mapSize; y++)
            {
                var inputValues = Console.ReadLine().Split(' ');
                
                for (int x = 0; x < inputValues.Length; x++)
                {
                    inputMap[y, x] = int.Parse(inputValues[x]);

                    if (inputMap[y, x] == 9)
                    {
                        nextPoints.Enqueue(new Point(x, y));
                        inputMap[y, x] = 0;
                    }
                }
            }

            
            // Visited Map
                // -1 : 방문한 적 없음 / 0 ~ n : 이동 거리
                    // 이동 거리 계산 : 상하좌우 탐색 시 부모 좌표의 값(이동 거리) ++;
            int[,] visitedMap = new int[mapSize, mapSize];
            ResetVisitedMap(ref visitedMap);


            int moveCount = 0;
            int sharkSize = 2;
            int eatCount = 0;

            Point sharkPos;
            Point neighborPos;

            bool isStartPoint = true;


            while (nextPoints.Count != 0)
            {
                sharkPos = nextPoints.Dequeue();

                // mark start point
                if (isStartPoint)
                {
                    visitedMap[sharkPos.y, sharkPos.x] = 0;
                    isStartPoint = false;
                }

                // search neighbor points
                for (int i = 0; i < 4; i++)
                {
                    neighborPos = new Point(sharkPos.x + Direction.X[i], sharkPos.y + Direction.Y[i]);

                    // 맵 벗어난 경우
                    if (neighborPos.x < 0 || neighborPos.x >= mapSize ||
                        neighborPos.y < 0 || neighborPos.y >= mapSize)
                        continue;

                    // 큰 물고기인 경우
                    if (inputMap[neighborPos.y, neighborPos.x] > sharkSize)
                        continue;

                    // 이미 방문한 경우
                    if (visitedMap[neighborPos.y, neighborPos.x] != -1)
                        continue;

                    // 이동 거리
                    visitedMap[neighborPos.y, neighborPos.x] = visitedMap[sharkPos.y, sharkPos.x] + 1;
                    //DrawMap(ref visitedMap);

                    // Eat
                    if (inputMap[neighborPos.y, neighborPos.x] < sharkSize && inputMap[neighborPos.y, neighborPos.x] > 0)
                    {
                        //Console.WriteLine($"Eat !! moveCount += {visitedMap[neighborPos.y, neighborPos.x]}");
                        //Console.WriteLine();
                        moveCount += visitedMap[neighborPos.y, neighborPos.x];

                        // reset queue & map
                        nextPoints.Clear();
                        nextPoints.Enqueue(neighborPos);
                        ResetVisitedMap(ref visitedMap);

                        // remove fish
                        inputMap[neighborPos.y, neighborPos.x] = 0;


                        // level up
                        if (++eatCount >= sharkSize)
                        {
                            eatCount = 0;
                            ++sharkSize;
                        }

                        isStartPoint = true;
                        break;
                    }
                    //
                    else
                    {
                        nextPoints.Enqueue(neighborPos);
                    }
                }
            }



            Console.WriteLine($"Answer : {moveCount}");
        }


        static void ResetVisitedMap(ref int[,] map)
        {
            for (int i = 0; i < map.GetLength(0); i++)
            {   
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    map[i, j] = -1;
            
                }
            }
        }


        static void DrawMap(ref int[,] map)
        {
            Console.WriteLine("--visited map--");

            for (int i = 0; i < map.GetLength(0); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    if (map[i,j] < 0)
                    {
                        Console.Write($" {map[i, j]}");
                    }
                    else
                    {
                        Console.Write($"  {map[i, j]}");
                    }
                    
                }
                Console.WriteLine();
            }

            Console.WriteLine();
        }
    }
}
