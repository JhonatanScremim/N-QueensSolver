using System;
using System.Diagnostics;
using System.Threading;

namespace N_QueensSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            for(int i = 0; i < 20; i++)
            {
                Console.WriteLine("Novo tabuleiro:");

                bool[,] board = InitializeBoard(8);

                DrawTheBoard(board);

                int time = ApplySimulatedAnnealing(board, 35, 1.03, 35, 1.005);

                DrawTheBoard(board);
                Console.WriteLine(") Tempo decorrido: " + time + " ms");

                Console.ReadKey();
            }
        }

        #region Iniciar tabuleiro
        static bool[,] InitializeBoard(int size)
        {
            bool[,] board = new bool[size, size];
            Random rnd = new Random();

            for (int i = 0; i < size; i++)
            {
                int randomSquare = rnd.Next(size); 
                for (int j = 0; j < size; j++)
                {
                    if (j != randomSquare)
                        board[i, j] = false;
                    else
                        board[i, j] = true;
                }
            }
            return board;
        }
        #endregion

        #region Desenhar tabuleiro
        static void DrawTheBoard(bool[,] board)
        {
            int size = Convert.ToInt32(Math.Sqrt(board.Length)); //Se a placa for 8x8. O comprimento será 64

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == true)
                        Console.Write("O  ");
                    else
                        Console.Write("-  ");
                }
                Console.WriteLine();
            }

            Console.WriteLine("\n");
        }

        #endregion

        #region Calcular colisões
        static int CalculateCollisions(bool[,] board)
        {
            int size = Convert.ToInt32(Math.Sqrt(board.Length)); //Se a placa for 8x8. O comprimento será 64
            int totalCollisions = 0;

            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    if (board[i, j] == true) // Se houver alguma rainha naquela casa
                    {
                        //for (int z = j + 1; z < size; z++) // Verifica a linha //Não será necessário, pois com essa inicialização já é inserido uma rainha por linha
                        //{
                        //    if (board[i, z] == true)
                        //    {
                        //        totalCollisions++;
                        //    }
                        //}

                        for (int z = i + 1; z < size; z++) // Verifica a coluna
                        {
                            if (board[z, j] == true)
                            {
                                totalCollisions++;
                            }
                        }

                        for (int z = 1; i + z < size && j + z < size; z++) // Verifica a diagonal inferior direita
                        {
                            if (board[i + z, j + z] == true)
                            {
                                totalCollisions++;
                            }
                        }

                        for (int z = 1; i - z >= 0 && j + z < size; z++) // Verfica a diagonal superior direita
                        {
                            if (board[i - z, j + z] == true)
                            {
                                totalCollisions++;
                            }
                        }
                    }
                }
            }
            return totalCollisions;
        }
        #endregion

        #region Algoritmo Simulated Annealing
        static int ApplySimulatedAnnealing(bool[,] board, double initialTemp, double coolingFactor, double currentStabilizer, double stabilizingFactor)
        {
            int size = Convert.ToInt32(Math.Sqrt(board.Length));

            Stopwatch stopwatch = new();

            stopwatch.Start();

            for (double temperature = initialTemp; (temperature > 0) && (CalculateCollisions(board) != 0); temperature -= coolingFactor)
            {
                Console.WriteLine("Temperatura : " + temperature + ", total de colisões: " + CalculateCollisions(board));
                //DrawTheBoard(board);
                for (int k = 0; k < currentStabilizer; k++)
                {
                    //bool[,] temporaryBoard = board.Clone() as bool[,];
                    bool[,] temporaryBoard = new bool[size, size];
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            temporaryBoard[i, j] = board[i, j];
                        }
                    }
                    Random rnd = new();

                    //Primeiro faça um movimento aleatório
                    int randomRowIndex;
                    int randomColumnIndex;
                    do
                    {
                        randomRowIndex = rnd.Next(8);
                        randomColumnIndex = rnd.Next(8);
                    }
                    while (temporaryBoard[randomRowIndex, randomColumnIndex] == true && randomRowIndex == randomColumnIndex);
                    for (int j = 0; j < size; j++)
                    {
                        if (temporaryBoard[randomRowIndex, j] == true)
                        {
                            temporaryBoard[randomRowIndex, j] = false;
                        }
                    }
                    temporaryBoard[randomRowIndex, randomColumnIndex] = true;

                    //Então faça o melhor movimento
                    int[,] successors = new int[size, size];

                    for (int i = 0; i < size; i++)
                    {
                        //Encontre a rainha da fileira
                        int indexOfQueen = -1;
                        for (int j = 0; j < size; j++)
                        {
                            if (temporaryBoard[i, j] == true)
                            {
                                indexOfQueen = j;
                                temporaryBoard[i, j] = false;
                            }
                        }
                        //Experimente todos os movimentos na linha e salve as colisões
                        for (int j = 0; j < size; j++)
                        {
                            if (j != indexOfQueen)
                            {
                                temporaryBoard[i, j] = true;
                                successors[i, j] = CalculateCollisions(temporaryBoard);
                                temporaryBoard[i, j] = false;
                            }
                            else
                            {
                                successors[i, j] = 999; //Para ignorar a posição antiga
                            }
                        }
                        temporaryBoard[i, indexOfQueen] = true; //Fixando a linha em sua primeira posição
                    }

                    //Selecione o menor valor de sucessores
                    int min = 998;
                    int indexI = -1;
                    int indexJ = -1;
                    for (int i = 0; i < size; i++)
                    {
                        for (int j = 0; j < size; j++)
                        {
                            if (successors[i, j] < min)
                            {
                                min = successors[i, j];
                                indexI = i;
                                indexJ = j;
                            }
                        }
                    }

                    //Faça o movimento, remova a rainha da linha primeiro e coloque a nova
                    for (int j = 0; j < size; j++)
                    {
                        if (temporaryBoard[indexI, j] == true)
                        {
                            temporaryBoard[indexI, j] = false;
                        }
                    }
                    temporaryBoard[indexI, indexJ] = true;

                    //DrawTheBoard(temporaryBoard);


                    double delta = CalculateCollisions(board) - CalculateCollisions(temporaryBoard);
                    double probability = Math.Exp(delta / temperature);

                    double rand = rnd.NextDouble();
                    //Console.Clear();

                    if (delta > 0)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                board[i, j] = temporaryBoard[i, j];

                            }
                        }
                    }
                    else if (rand <= probability)
                    {
                        for (int i = 0; i < size; i++)
                        {
                            for (int j = 0; j < size; j++)
                            {
                                board[i, j] = temporaryBoard[i, j];
                            }
                        }
                    }
                }
                currentStabilizer *= stabilizingFactor;
            }
            stopwatch.Stop();
            return stopwatch.Elapsed.Milliseconds;
        }
        #endregion
    }
}
