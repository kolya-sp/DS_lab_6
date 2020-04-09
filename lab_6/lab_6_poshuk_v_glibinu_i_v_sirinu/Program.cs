using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;

namespace lab_5_poshuk_v_glibinu_i_v_sirinu
{
    class Program
    {
        static List<v> poshuk_v_glubinu(int first, ref int[,] matr_sum)
        {
            List<v> V = new List<v>();
            int n = matr_sum.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                V.Add(new v { name = i, selected = false, prev = null });
            }
            Stack<v> T1 = new Stack<v>();
            v s = V[first];
            s.prev = null;
            int N = 1;
            T1.Push(s);
            s.selected = true;
            v w;
            s.num = N;
            while (T1.Count != 0)
            {
                v u = T1.Peek();
                for (int i = 0; i < n; i++)
                    if (matr_sum[u.name, i] == 1)
                    {
                        w = V[i];
                        if (w.selected == false)
                        {
                            T1.Push(w);
                            N = N + 1;
                            w.num = N;
                            w.selected = true;
                            w.prev = u;
                            u = w;
                            i = -1;
                        }
                    }
                T1.Pop();
            }
            return V;
        }
        static List<v> poshuk_v_sirinu(int first, ref int[,] matr_sum)
        {
            List<v> V = new List<v>();
            int n = matr_sum.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                V.Add(new v { name = i, selected = false, prev = null });
            }
            Queue<v> T2 = new Queue<v>();
            v s = V[first];
            s.prev = null;
            int N = 1;
            T2.Enqueue(s);
            s.selected = true;
            v w;
            s.num = N;
            while (T2.Count != 0)
            {
                v u = T2.Dequeue();
                for (int i = 0; i < n; i++)
                    if (matr_sum[u.name, i] == 1)
                    {
                        w = V[i];
                        if (w.selected == false)
                        {
                            T2.Enqueue(w);
                            N = N + 1;
                            w.num = N;
                            w.selected = true;
                            w.prev = u;
                        }
                    }
            }
            return V;
        }
        public class v
        {
            public int name;
            public bool selected;
            public v prev;
            public int num;
        }
        static int[,] gen_matr_sum(int n)
        {
            int[,] matr_sum = new int[n, n];
            Random r = new Random();
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (i < j)
                    {
                        matr_sum[i, j] = r.Next(0, 2);
                        matr_sum[j, i] = matr_sum[i, j];
                    }
                }
            }
            List<int> t = new List<int>();
            for (int i = 1; i < n; i++) t.Add(i);
            int k = 0;
            for (int i = 1; i < n; i++)
            {
                int l = r.Next(0, t.Count);
                int j = t[l];
                matr_sum[k, j] = 1;
                matr_sum[j, k] = 1;
                k = j;
                t.RemoveAt(l);
            }
            return matr_sum;
        }
        static void vivod_obhodu(ref List<v> V)
        {
            for (int i = 0; i < V.Count; i++)
            {
                Console.WriteLine("вершина " + (char)('a'+i) + " черговiсть обходу " + V[i].num);
            }
        }
        static void vivod_matr_sum(ref int[,] matr_sum)
        {
            int n = matr_sum.GetLength(1);
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    Console.Write(matr_sum[i, j] + " ");
                }
                Console.WriteLine();
            }
        }
        static void Main(string[] args)
        {
            string f_name = "data1.csv";
            FileStream file = new FileStream(f_name, FileMode.OpenOrCreate);
            file.Close();
            string t1, t2;
            Console.WriteLine("тест Обхiд графа: пошук в глибину, пошук в ширину  ");
            StreamWriter swr = new StreamWriter(f_name);
            swr.WriteLine("test obhid grafa: \n data; DFS(v glubinu); BFS(vshirinu)");
            Stopwatch sw = new Stopwatch();
            Random r = new Random();
            int[,] matr_sum;
            List<v> V=new List<v>();
            for (int j = 1; j <= 5; j++)
            {
                int n = 1000 * j;
                matr_sum = gen_matr_sum(n); // генерація однозвязної матриці суміжності 
                sw.Start();
                V = poshuk_v_glubinu(0, ref matr_sum); // ми шукаємо в глубину всі вершини і нумеруємо їх
                sw.Stop();
                t1 = sw.ElapsedMilliseconds.ToString();
                sw.Restart();
                V = poshuk_v_sirinu(0, ref matr_sum); // ми шукаємо в ширину всі вершини і нумеруємо їх
                sw.Stop();
                t2 = sw.ElapsedMilliseconds.ToString();
                sw.Reset();
                Console.WriteLine("однозвязний граф з кiлькiстю вершин: " + n + " обiйдено пошуком в глубину за " + t1 + " мс " + " пошуком в ширинуину за " + t2 + " мс ");
                swr.WriteLine(n + ";" + t1 + ";" + t2);
            }
            swr.Close();
            Console.WriteLine("тестовий файл з невеликою кiлькiсю даних для перевiрки роботи алгоритмiв сортування lab_6_..\\bin\\Debug\\data2.csv його вмiст:");
            string Pyt = "data2.csv";
            StreamReader sr = new StreamReader(Pyt);
            string[] mass = (sr.ReadToEnd()).Split(';', '\n');
            sr.Close();
            int n2 = (int)Math.Round(Math.Sqrt(mass.Length));
            int k = 0;
            matr_sum = new int[n2, n2];
            for (int i = 0; i < n2; i++)
                for (int j = 0; j < n2; j++)
                {
                    matr_sum[i, j] = int.Parse(mass[k]);
                    k++;
                }
            Console.WriteLine("матриця сумiжностi");
            vivod_matr_sum(ref matr_sum);
            Console.WriteLine("обхiд в глубину");
            V = poshuk_v_glubinu(1, ref matr_sum);
            vivod_obhodu(ref V);
            V = poshuk_v_sirinu(1, ref matr_sum);
            Console.WriteLine("обхiд в ширину");
            vivod_obhodu(ref V);
            Console.ReadKey();
        }
    }
}
