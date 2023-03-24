using Minefiled;
using System;
using System.Data.Common;
using System.Net.Mail;

internal class Program
{
    public static int points;
    public static void ShowMap(Map map)
    {
        for (int i = 0; i < map.cells.GetLength(0); i++)
        {
            for (int j = 0; j < map.cells.GetLength(1); j++)
            {
                if (map.cells[i, j].isVisible == false)
                {
                    Console.Write("| |");
                }
                else
                {
                    if (map.HasBomb(i, j) == true)
                    {
                        Console.Write("|X|");
                    }
                    else
                    {
                        Console.Write("|{0}|", map.cells[i, j].bombsNear);
                    }
                }
            }
            Console.WriteLine();
        }
        Console.WriteLine();
    }

    private static void Main(string[] args)
    {
        //Defina a dificuldade;

        Console.WriteLine("Digite a quantidade de linhas do Campo Minado: ");
        int line = Convert.ToInt32(Console.ReadLine());

        Console.WriteLine("Digite a quantidade de colunas do Campo Minado: ");
        int column = Convert.ToInt32(Console.ReadLine());


        Console.WriteLine("Digite a dificuldade: ");
        int difficulty = Convert.ToInt32(Console.ReadLine());

        Map mapSize = new Map();

        mapSize.MapConstructor(line, column, difficulty);


        while (true)
        {
            Console.Clear();

            Console.WriteLine("Pontos: {0}", points);

            Console.WriteLine();

            ShowMap(mapSize);

            Console.WriteLine("Digite o espaço desejado:\nExemplo: nª da linha e nª da coluna");

            Console.WriteLine();

            Console.WriteLine("Número da Linha");
            int coordenateLine = Convert.ToInt32(Console.ReadLine());
            Console.WriteLine("Número da Coluna");
            int coordenateColumn = Convert.ToInt32(Console.ReadLine());

            if (coordenateLine < 0
                || coordenateLine > line - 1
                || coordenateColumn < 0
                || coordenateColumn > column - 1)
            {
                Console.WriteLine("Coordenada não econtrada");
                continue;

            }

            mapSize.cells[coordenateLine, coordenateColumn].isVisible = true;

            /*if (mapSize.cells[coordenateLine, coordenateColumn].isVisible == false)
            {
                //mapSize.SetCellAsVisible(coordenateLine,coordenateColumn);
            }*/

            if (mapSize.HasBomb(coordenateLine, coordenateColumn) == false)
            {
                points += 20;
                mapSize.openCells--;

                if (mapSize.openCells == 0)
                {
                    Console.WriteLine("Parabéns!!! Você Venceu!");
                    break;
                }
            }
            else if (mapSize.HasBomb(coordenateLine, coordenateColumn))
            {
                Console.WriteLine("Se Fudeu!");
                break;
            }
        }

    }
}