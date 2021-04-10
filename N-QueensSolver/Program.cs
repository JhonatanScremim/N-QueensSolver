using System;

namespace N_QueensSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            umnomeai ue = new umnomeai();
            //bool[,] board = InitializeBoard(8);
            //DrawTheBoard(board);
            //Console.WriteLine("Total de colisões: " + CalculateCollisions(board));
            //Console.ReadKey();
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

    }
}
