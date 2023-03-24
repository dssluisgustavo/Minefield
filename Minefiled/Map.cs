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
                    }
                    if (cell.hasBomb == true)
                    {
                        openCells += 1;

                        int lineMin = Math.Clamp(i - 1, 0, cells.GetLength(0) - 1);
                        int lineMax = Math.Clamp(i + 1, 0, cells.GetLength(0) - 1);

                        int columnMin = Math.Clamp(j - 1, 0, cells.GetLength(1) - 1);
                        int columnMax = Math.Clamp(j + 1, 0, cells.GetLength(1) - 1);

                        for (int k = lineMin; k <= lineMax; k++)
                        {
                            for (int m = columnMin; m <= columnMax; m++)
                            {
                                Cell tempCell = cells[k, m];

                                if (tempCell.hasBomb == false)
                                {
                                    tempCell.bombsNear += 1;
                                }
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
            Random randomNum = new Random();
            Cell cell = new Cell();

            switch (difficulty)
            {
                case 1:
                    return 10;

                case 2:
                    return 30;

                case 3:
                    return 40;

            }

            return 30;
        }

        public void SetCellAsVisible(int line, int column)
        {
            Queue <Cell> cellsToBeVisible = new Queue <Cell>();

            cellsToBeVisible.Enqueue(cells[line,column]);

            while (cellsToBeVisible.Count > 0)
            {
                var spreedCell = cellsToBeVisible.Dequeue();
                spreedCell.isVisible = true;

                int lineMin = Math.Clamp(line - 1, 0, cells.GetLength(0) - 1);
                int lineMax = Math.Clamp(line + 1, 0, cells.GetLength(0) - 1);

                int columnMin = Math.Clamp(column - 1, 0, cells.GetLength(1) - 1);
                int columnMax = Math.Clamp(column + 1, 0, cells.GetLength(1) - 1);

                for (int k = lineMin; k <= lineMax; k++)
                {
                    for (int m = columnMin; m <= columnMax; m++)
                    {
                        Cell tempCell = cells[k, m];

                        if (tempCell == cells[line, column] || tempCell.isVisible)
                        {
                            continue;
                        }

                        if (tempCell.hasBomb == false)
                        {
                            cellsToBeVisible.Enqueue(tempCell);
                        }
                    }
                }
            }
        }
    }
}
