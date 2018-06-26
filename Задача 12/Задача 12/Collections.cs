using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;


namespace Задача_12
{
    public class Collections
    {
        static int countChange = 0;
        static int countEqual = 0;
        // Массивы - упорядоченный по возрастанию, по убыванию, не упорядоченный
        int[] SortedInc;
        int[] SortedDec;
        int[] Unsorted;

        #region Generation
        // ДСЧ, используемый при генерации массивов
        static Random rnd = new Random();

        // Конструктор
        public Collections(int Size = 100)
        {
            SortedInc = GenerateSortedArray(Size, 1);
            SortedDec = GenerateSortedArray(Size, -1);
            Unsorted = GenerateArray(Size);
        }

        // Метод для получения массивов
        // Переменная Order определяет порядок упорядочивания элементов массива: 
        // 1 - по возрастанию, -1 - по убыванию, 0 или любое другое число - не упорядоченный
        public int[] GetArray(int Order)
        {
            switch (Order)
            {
                case 1:
                    return SortedInc.ToArray();
                case -1:
                    return SortedDec.ToArray();
                default:
                    return Unsorted.ToArray();
            }
        }

        // Генерация упорядоченного массива
        // Переменная Order определяет порядок упорядочивания элементов массива: 1 - по возрастанию, -1 - по убыванию
        int[] GenerateSortedArray(int Size, int Order)
        {
            int[] Array = new int[Size];
            Array[0] = rnd.Next(10);
            for (ushort i = 1; i < Size; i++)
                Array[i] = Array[i - 1] + (rnd.Next(10) + 1) * Order;
            return Array;
        }

        // Генерация неупорядоченного массива
        int[] GenerateArray(int Size)
        {
            int[] Array = new int[Size];
            for (ushort i = 0; i < Size; i++)
                Array[i] = rnd.Next(-1000, 1000);
            return Array;
        }
        #endregion Generation

        #region Sort
        // Длины промежутков для сортировки
        // Использована последовательность Марцина Циура, считающаяся лучшим вариантом для массивов длиной менее 4000
        static int[] Increment = new int[] { 1, 4, 10, 23, 57, 132, 301, 701, 1750 };

        // Поразрядная сортировка
        public static void RadixSort(ref int[] arr)
        {
            var time = Stopwatch.StartNew();                    // подготовка: 
            countChange = 0;                                    // обнуление счетчиков операций
            countEqual = 0;                                     // и времени

            int i, j;
            int[] tmp = new int[arr.Length];
            for (int shift = 31; shift > -1; --shift)
            {
                j = 0;
                for (i = 0; i < arr.Length; ++i)
                {
                    bool move = (arr[i] << shift) >= 0;
                    if (shift == 0 ? !move : move)
                        arr[i - j] = arr[i];
                    else
                        tmp[j++] = arr[i];
                }
                Array.Copy(tmp, 0, arr, arr.Length - j, j);
            }

            Console.WriteLine("Результат: " + String.Join(", ", arr));
            Console.WriteLine("Затрачено {0} тиков, {1} сравнений, {2} перессылок",
                time.ElapsedTicks, countEqual, countChange);
            time.Reset();
        }

        // Быстрая сортировка
        // Для реализации было выбрано разбиение Хоара
        public static void QuickSort(ref int[] Array, int IndexFirst, int IndexLast, ref int ComparesCount, ref int ReplacesCount)
        {
            if (IndexFirst > IndexLast)
                return;
            // Опорный элемент, в данном случае - средний
            int Pivot = Array[(IndexLast - IndexFirst) / 2];
            // Вспомогательные переменные для прохода по массиву
            int i = IndexFirst, j = IndexLast;
            // Проход по массиву
            while (i < j)
            {
                // Поиск опорного элемента
                while (Array[i] < Pivot)
                {
                    ComparesCount++;
                    i++;
                }
                while (Array[j] > Pivot)
                {
                    ComparesCount++;
                    j--;
                }

                // Если не дошли до опорного элемента
                if (i < j)
                {
                    // Вспомогательная переменная для перестановки элементов
                    int Temp = Array[i];
                    Array[i++] = Array[j];
                    Array[j--] = Temp;
                    ReplacesCount++;
                }

                // Сортировка частей, на которые был разделен массив
                QuickSort(ref Array, IndexFirst, j, ref ComparesCount, ref ReplacesCount);
                QuickSort(ref Array, i + 1, IndexLast, ref ComparesCount, ref ReplacesCount);
            }
        }
        #endregion Sort

        // Печать массива
        public static void PrintArray(int[] Array)
        {
            foreach (int Item in Array)
                Console.Write("{0} ", Item);
            Console.WriteLine("\n");
        }
    }
}
