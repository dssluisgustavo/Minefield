using System;
using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Reflection.PortableExecutable;
using System.Text;
using System.Threading.Tasks;

namespace Minefiled
{
    public class Map
    {
        public Cell[,] cells;
        public int openCells;

        public void MapConstructor(int line, int column, int probability)
        {
            cells = new Cell[line, column];
            Random randomNum = new Random();

            var fatorDePropabilidade = GetDifficultyFactor(probability);

            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Cell newCell = new Cell();

                    cells[i, j] = newCell;
                }
            }
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    Cell cell = cells[i, j];

                    int num = randomNum.Next(0, 100);

                    if (num < fatorDePropabilidade)
                    {
                        cell.hasBomb = true;

                        openCells += 1;

                        var nearbyCells = GetNearbyCells(cell);
                        for (int k = 0; k < nearbyCells.Length; k++)
                        {
                            Cell tempCell = nearbyCells[k];

                            if (tempCell.hasBomb == false)
                            {
                                tempCell.bombsNear += 1;
                            }
                        }
                    }
                }
            }
        }

        public bool HasBomb(int line, int column)
        {
            Cell cell = cells[line, column];

            return cell.hasBomb;
        }

        public int GetDifficultyFactor(int difficulty)
        {
            switch (difficulty)
            {
                case 1: return 10;
                case 2: return 30;
                case 3: return 40;
            }

            return 30;
        }

        public void SetCellAsVisible(int line, int column)
        {
            Queue<Cell> cellsToBeVisible = new Queue<Cell>();

            cellsToBeVisible.Enqueue(cells[line, column]);

            while (cellsToBeVisible.Count > 0)
            {
                var spreedCell = cellsToBeVisible.Dequeue();

                spreedCell.isVisible = true;
                if (spreedCell.bombsNear != 0) continue;

                var nearbyCells = GetNearbyCells(spreedCell);

                for (int i = 0; i < nearbyCells.Length; i++)
                {
                    var cell = nearbyCells[i];
                    if (cell.isVisible) continue;

                    var alreadyInQueue = cellsToBeVisible.Any(c => c == cell);
                    if (alreadyInQueue == true) continue;

                    cellsToBeVisible.Enqueue(cell);
                }
            }
        }

        public Cell[] GetNearbyCells(Cell cell)
        {
            var cellPosition = GetCellPosition(cell);
            if (!cellPosition.HasValue)
            {
                return Array.Empty<Cell>();
            }

            var line = cellPosition.Value.x;
            var column = cellPosition.Value.y;

            var list = new List<Cell>();

            int lineMin = Math.Clamp(line - 1, 0, cells.GetLength(0) - 1);
            int lineMax = Math.Clamp(line + 1, 0, cells.GetLength(0) - 1);

            int columnMin = Math.Clamp(column - 1, 0, cells.GetLength(1) - 1);
            int columnMax = Math.Clamp(column + 1, 0, cells.GetLength(1) - 1);

            for (int i = lineMin; i <= lineMax; i++)
            {
                for (int j = columnMin; j <= columnMax; j++)
                {
                    if (i == line && j == column) continue;

                    list.Add(cells[i, j]);
                }
            }

            return list.ToArray();
        }

        public (int x, int y)? GetCellPosition(Cell cell)
        {
            for (int i = 0; i < cells.GetLength(0); i++)
            {
                for (int j = 0; j < cells.GetLength(1); j++)
                {
                    if (cells[i, j] == cell)
                    {
                        return (i, j);
                    }
                }
            }

            return null;
        }
    }
}
