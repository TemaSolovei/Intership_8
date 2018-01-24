using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;

namespace Intership_8
{
    class Program
    {
        public static List<int> tops = new List<int>(); // Список с проверенными вершинами

        static void Main(string[] args)
        {
            int choice;
            bool ok; // ok — проверка на принадлежность типу

            do
            {
                Console.Write("Каким образом будем задавать входные данные? \n 1. Ввод вручную  \n 2. Генератор тестов\n Введите номер выбранного варианта: ");
                ok = int.TryParse(Console.ReadLine(), out choice);
                if ((!ok) || (1 > choice) || (2 < choice)) Console.WriteLine("Введите 1 или 2!");
            } while (!ok);

            switch (choice)
            {
                case 1:
                    int lineFirst, columnFirst; // lineFirst - количество строк, columnFirst - количество столбцов, count - счётчик
                    int[,] matrix; // Введённая матрица

                    do
                    {
                        lineFirst = checkInt("Введите количество строк: ");
                        columnFirst = checkInt("Введите количество столбцов: ");
                        if (lineFirst > columnFirst) Console.WriteLine("Ошибка! Количество строк должно быть больше или равно количеству столбцов!");
                    } while (lineFirst > columnFirst);

                    matrix = new int[lineFirst, columnFirst];

                    for (int j = 0; j < columnFirst; j++)
                    {
           
                            for (int i = 0; i < lineFirst; i++)
                            {
                                Console.Write("Элемент по адресу {0} строка, {1} столбец. ", i + 1, j + 1);
                                
                                    do
                                    {
                                        Console.Write("Введите элемент: ");
                                        ok = int.TryParse(Console.ReadLine(), out matrix[i, j]);
                                        if ((!ok) || (0 > matrix[i, j]) || (1 < matrix[i, j])) Console.WriteLine("Введите 0 или 1!");
                                    } while (!ok);

                                Console.WriteLine();
                            }                       

                        Console.WriteLine();
                    }

                    writeMatrix(matrix, lineFirst, columnFirst); // Вывод матрицы

                    Console.WriteLine("В данном графе {0} компонент(а) связности", solution(matrix, 0));

                    break;

                case 2: 
                    int lineSecond, columnSecond; // line - количество строк, column - количество столбцов
                    int[,] testMatrix; // Сгенерированная тестовая матрица
                    int countOfTests = checkInt("Введите количество тестов: ");
                    

                    for (int i = 0; i < countOfTests; i++)
                    {
                        lineSecond = 0;
                        columnSecond = 0;

                        Console.WriteLine("Тест номер {0}:\n", i+1);

                        testMatrix = genTests(ref lineSecond, ref columnSecond); // Генерирование матрицы
                        writeMatrix(testMatrix, lineSecond, columnSecond); // Вывод матрицы

                        Console.WriteLine("В данном графе {0} компонент(а) связности", solution(testMatrix, 0));
                    }

                    break;

                default: Console.WriteLine("Произошла ошибка при вводе выбранного пункта!");
                    break;
            }


        }

        static int checkInt(string message)
        {
            int result; // result - проверенная переменная
            bool ok; // Проверка ввода

            do
            {
                Console.Write(message);
                ok = int.TryParse(Console.ReadLine(), out result);
                if ((!ok) || (result <= 0)) Console.WriteLine("Ошибка! Введите целое число больше нуля!");
            } while ((!ok) || (result <= 0));

            return result;
        }

        static int[,] genTests(ref int line, ref int column) // Генерирование матрицы. line - количество строк, column - количество столбцов
        {
            int count, firstPosition, secondPosition; // count — счётчик
            int[,] matrix; // Матрица инциденций
            Random rand = new Random();

            line = rand.Next(2, 11);
            Thread.Sleep(20);
            column = rand.Next(line, 11);
            Thread.Sleep(20);

            matrix = new int[line, column];

            for (int j = 0; j < column; j++)
            {
                firstPosition = rand.Next(0, line);
                Thread.Sleep(20);
                do
                {
                    secondPosition = rand.Next(0, line);
                    Thread.Sleep(20);
                } while (firstPosition == secondPosition);

                matrix[firstPosition, j] = 1;
                matrix[secondPosition, j] = 1;
            }

            return matrix;
        }

        static void writeMatrix(int[,] matrix, int line, int column) // Вывод матрицы
        {
            Console.WriteLine("Обрабатываемая матрица:\n");

            for (int i = 0; i < line; i++)
            {
                for (int j = 0; j < column; j++)
                {
                    Console.Write("{0} ", matrix[i,j]);
                }
                
                Console.WriteLine();
            }
            Console.WriteLine();
        }

        static int solution(int[,] matrix, int startTop) // Вычисление количества компонент связности
        {
            tops.Add(startTop);

            for (int i = 0; i < tops.Count; i++)
            {
                var checkTop = tops[i];

                for (int j = 0; j < matrix.GetLength(0); j++)
                {
                    if (matrix[checkTop, j] == 1)
                    {
                        for (int k = 0; k < matrix.GetLength(1); k++)
                        {
                            if (k == 1 && !tops.Contains(k)) tops.Add(k);
                        }
                    }
                }
            }

            var number = tops[0]; // Число, которого нет в списке

            for (int i = 0; i < matrix.GetLength(0) && number == tops[0]; i++)
            {
                if (!tops.Contains(i))
                {
                    number = i;
                }
            }

            if (number != tops[0]) return solution(matrix, number) + 1;
            return 1;
        }
    }
}