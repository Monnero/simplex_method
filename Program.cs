using System;
using System.Collections.Generic;


namespace Simplex
{
    public class Simplex
    {

        double[,] simplex_table;

        int m, n;

        List<int> basis;

        public Simplex(double[,] source)
        {
            m = source.GetLength(0);
            n = source.GetLength(1);
            simplex_table = new double[m, n + m - 1];
            basis = new List<int>();

            for (int i = 0; i < m; i++)
            {
                for (int j = 0; j < simplex_table.GetLength(1); j++)
                {
                    if (j < n)
                        simplex_table[i, j] = source[i, j];
                    else
                        simplex_table[i, j] = 0;
                }

                if ((n + i) < simplex_table.GetLength(1))
                {
                    simplex_table[i, n + i] = 1;
                    basis.Add(n + i);
                }
            }

            n = simplex_table.GetLength(1);
        }

        public double[,] Calculate(double[] result)
        {
            int mainCol, mainRow;

            while (!IsItEnd())
            {
                mainCol = findMainCol();
                mainRow = findMainRow(mainCol);
                basis[mainRow] = mainCol;

                double[,] new_table = new double[m, n];

                for (int j = 0; j < n; j++)
                    new_table[mainRow, j] = simplex_table[mainRow, j] / simplex_table[mainRow, mainCol];

                for (int i = 0; i < m; i++)
                {
                    if (i == mainRow)
                        continue;

                    for (int j = 0; j < n; j++)
                        new_table[i, j] = simplex_table[i, j] - simplex_table[i, mainCol] * new_table[mainRow, j];
                }
                simplex_table = new_table;
            }

            for (int i = 0; i < result.Length; i++)
            {
                int k = basis.IndexOf(i + 1);
                if (k != -1)
                    result[i] = simplex_table[k, 0];
                else
                    result[i] = 0;
            }

            return simplex_table;
        }

        private bool IsItEnd()
        {
            bool flag = true;

            for (int j = 1; j < n; j++)
            {
                if (simplex_table[m - 1, j] < 0)
                {
                    flag = false;
                    break;
                }
            }

            return flag;
        }

        private int findMainCol()
        {
            int mainCol = 1;

            for (int j = 2; j < n; j++)
                if (simplex_table[m - 1, j] < simplex_table[m - 1, mainCol])
                    mainCol = j;

            return mainCol;
        }

        private int findMainRow(int mainCol)
        {
            int mainRow = 0;

            for (int i = 0; i < m - 1; i++)
                if (simplex_table[i, mainCol] > 0)
                {
                    mainRow = i;
                    break;
                }

            for (int i = mainRow + 1; i < m - 1; i++)
                if ((simplex_table[i, mainCol] > 0) && ((simplex_table[i, 0] / simplex_table[i, mainCol]) < (simplex_table[mainRow, 0] / simplex_table[mainRow, mainCol])))
                    mainRow = i;

            return mainRow;
        }

    }

    class Program
    {
        static void Main(string[] args)
        {
            double[,] table = { {900, 3, 2, 0},
                                {640, 2, 1, 5},
                                {1000,4, 8, 4},
                                { 0,-60,-40,-50}};

            double[] result = new double[3];
            double[,] table_result;
            Simplex S = new Simplex(table);
            table_result = S.Calculate(result);

            Console.WriteLine("Решенная симплекс-таблица:");
            for (int i = 0; i < table_result.GetLength(0); i++)
            {
                for (int j = 0; j < table_result.GetLength(1); j++)
                    Console.Write("{0}\t", table_result[i, j]);
                Console.WriteLine();
            }

            Console.WriteLine();
            Console.WriteLine("Решение:");
            Console.WriteLine("x1 = " + result[0] + "\nx2 = " + result[1] + "\nx3 = " + result[2]);
            
            Console.WriteLine("F(x) = " + table_result[3, 0]);
            Console.ReadLine();
        }
    }
}