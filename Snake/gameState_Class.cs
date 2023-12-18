using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing.Reader;
using System.Linq;
using System.Printing;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Snake
{
    public class gameState_Class
    {
        public int Rows { get; }
        public int Cols { get; }
        public GridValue[,] Grid {  get; }
        public Direction Dir { get; private set; }
        public int Score { get; private set; }
        public bool GameOver { get; private set; }

        private readonly LinkedList<Direction> dirChanges = new LinkedList<Direction>();
        private readonly LinkedList<Position> snakePositions = new LinkedList<Position>();
        private readonly Random random = new Random();

        public static int MovementSpeed = 100;
        public int FlashesActive = 0;

        public gameState_Class(int rows, int cols)
        {
            Rows = rows;
            Cols = cols;
            Grid = new GridValue[rows, cols];
            Dir = Direction.Right;

            AddSnake();
            AddFood();
        }

        private void AddSnake()
        {
            int r = Rows / 2;
            
            for (int c = 1; c <= 3; c++)
            {
                Grid[r, c] = GridValue.Snake;
                snakePositions.AddFirst(new Position(r, c));
            }
        }

        private IEnumerable<Position> EmptyPositions()
        {
            for (int r = 0; r < Rows; r++)
            {
                for (int c = 0; c < Cols; c++)
                {
                    if (Grid[r, c] == GridValue.Empty)
                    {
                        yield return new Position(r, c);
                    }
                }
            }
        }

        private void AddFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Food;
        }

        private void AddBigFood()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.BigFood;
        }

        private void AddFlash()
        {
            List<Position> empty = new List<Position>(EmptyPositions());

            if (empty.Count == 0)
            {
                return;
            }

            Position pos = empty[random.Next(empty.Count)];
            Grid[pos.Row, pos.Col] = GridValue.Flash;
        }

        public Position HeadPosition()
        {
            return snakePositions.First.Value;
        }

        public Position TailPosition() 
        {
            return snakePositions.Last.Value;
        }

        public IEnumerable<Position> SnakePositions()
        {
            return snakePositions;
        }

        private void AddHead(Position pos)
        {
            snakePositions.AddFirst(pos);
            Grid[pos.Row, pos.Col] = GridValue.Snake;
        }

        private void RemoveTail()
        {
            Position tail = snakePositions.Last.Value;
                Grid[tail.Row, tail.Col] = GridValue.Empty;
            snakePositions.RemoveLast();
        }

        private void RemoveTail(string replaceWith)
        {
            Position tail = snakePositions.Last.Value;
            if (replaceWith == null)
            Grid[tail.Row, tail.Col] = GridValue.Empty;
            else if (replaceWith == "Poo")
            Grid[tail.Row, tail.Col] = GridValue.Ew;
            snakePositions.RemoveLast();
        }

        private Direction GetLastDirection()
        {
            if (dirChanges.Count == 0)
            {
                return Dir;
            }

            return dirChanges.Last.Value;
        }

        public static int GetMovementSpeed()
        {
            return MovementSpeed;
        }

        private bool CanChangeDirection(Direction newDir)
        {
            if (dirChanges.Count == 2)
            {
                return false;
            }

            Direction lastDir = GetLastDirection();
            return newDir != lastDir && newDir != lastDir.Opposite();
        }

        public void ChangeDirection(Direction dir)
        {
            if (CanChangeDirection(dir)) { dirChanges.AddLast(dir); }
           
        }

        private bool OutsideGrid(Position pos)
        {
            return pos.Row < 0 || pos.Row >= Rows || pos.Col < 0 || pos.Col >= Cols;
        }

        private GridValue WillHit(Position newHeadPos)
        {
            if (OutsideGrid(newHeadPos))
            {
                return GridValue.Outside;
            }

            if (newHeadPos == TailPosition())
            {
                return GridValue.Empty;
            }

            return Grid[newHeadPos.Row, newHeadPos.Col];
        }

        public async void Move()
        {
            if (dirChanges.Count > 0) 
            {
                Dir = dirChanges.First.Value;
                dirChanges.RemoveFirst();
            }

            Position newHeadPos = HeadPosition().Translate(Dir);
            GridValue hit = WillHit(newHeadPos);
             
            if (hit == GridValue.Outside || hit == GridValue.Snake)
            {
                GameOver = true;
            }
            else if (hit == GridValue.Empty)
            {
                RemoveTail();
                AddHead(newHeadPos);

                int chance = random.Next(0, 1001);
                if (chance == 1 & Score >= 5)
                {
                    RemoveTail("Poo");
                    Score--;
                }
                if (chance < 10)
                {
                    AddFlash();
                }
            }
            else if (hit == GridValue.Food)
            {
                AddHead(newHeadPos);
                Score++;

                for (int i = 0; i < 5; i++)
                {
                    AddFood();
                }
                Random random = new Random();
                if (random.Next(0, 101) <= 10)
                {
                    AddFlash();
                }
                if (random.Next(0, 51) <= 10)
                {
                    AddBigFood();
                }
            }
            else if (hit == GridValue.Flash)
            {
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                Score += 3;
                if (MovementSpeed > 10)
                {
                    MovementSpeed -= 25;
                    if (MovementSpeed < 10)
                    {
                        MovementSpeed = 10;
                    }
                }
            }
            else if (hit == GridValue.Ew)
            {
                AddHead(newHeadPos);
                if (Score > 30)
                { Score -= 30;
                    for (int i = 0; i < 30; i++)
                    {
                        RemoveTail();
                    }
                }
                else GameOver = true;
                    
            }
            else if (hit == GridValue.BigFood)
            {
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                AddHead(newHeadPos);
                MovementSpeed += 10;
                Score += 5;
            }
        }

        internal async void BeginWatchforKonami(bool KonamiRunning)
        {
            if (KonamiRunning) 
            {
            //if ()
            }
        }

        internal void ResetMovementSpeed()
        {
            MovementSpeed = 100;
        }
    }
}
