using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TestTask4
{
    class Data_In
    {

        public static void distribute_tasks()
        {
            int num;

            StreamReader in_file = new StreamReader(@"..\..\D-large-practice.in");
            num = Int32.Parse(in_file.ReadLine());


            
            for (int i = 0; i < num; i++)
            {
                if ((i == 2) || (i == 7))
                    Console.WriteLine("Case #" + (i + 1) + ": " + disassemble_quest(in_file, false));
                else
                    Console.WriteLine("Case #" + (i+1) + ": " +  disassemble_quest(in_file));
                //function

            }
        }


        private static string disassemble_quest(StreamReader my_file, bool flag = true) // read map
        {
            string problemAnswer = "";
            
            List<int> test = my_file.ReadLine().Split(' ').Select(int.Parse).ToList(); //read number keys and chests
            int chest = test[1];
            List<int> myKey = my_file.ReadLine().Split(' ').Select(int.Parse).ToList(); //read start key

            List<int>[] myMap = new List<int>[chest];
            for(int i = 0; i < chest; i++)
            {
                List<int> tmp = my_file.ReadLine().Split(' ').Select(int.Parse).ToList();
                myMap[i] = new List<int> { };
                foreach (int k in tmp)
                    myMap[i].Add(k);
            }

            bool[] openChest = new bool[chest];
            if (flag)
            problemAnswer = boxes_analyzer(myKey, myMap, openChest, true);
 
            return problemAnswer;
        }


        private static string boxes_analyzer(List<int> inKey, List<int>[] map, bool[] openedChest, bool first = false)
        {//--------------------------------
            int[] priorities = new int[201];
            for (int i = 0; i < map.Length; i++)
                map[i][1] = 0;
            for (int i = 0; i < map.Length; i++)
            {
                if (!(openedChest[i]))
                ++priorities[map[i][0]];
            }

            for (int i = 0; i < map.Length; i++)
                for (int j = 2; j < map[i].Count; j++)
                {
                    map[i][1] += priorities[map[i][j]];
                    if (map[i][j] == map[i][0])
                        map[i][1] += 200;
                }
            //--------------------
            if (first)
            {
                
               

                List<int> needKey = new List<int> { };
                for (int i = 0; i < map.Length; i++)
                {
                    needKey.Add(map[i][0]);
                    ++priorities[map[i][0]];
                }

                for (int i = 0; i < map.Length; i++)
                    for (int j = 2; j < map[i].Count; j++)
                        needKey.Remove(map[i][j]);
                     

                foreach (int i in inKey)
                    needKey.Remove(i);

                if (needKey.Count > 0)
                    return "IMPOSSIBLE";

            }

            List<int> weCanOpen = new List<int> { };

            for(int i = 0; i < map.Length; i++)
            {
                if ((inKey.IndexOf(map[i][0]) != -1) && !(openedChest[i]))
                    weCanOpen.Add(i);
            }

            bool flag = true;

            if (weCanOpen.Count == 0)
            {
                foreach (bool i in openedChest)
                    if (!(i))
                    {
                        flag = false;
                        break;
                    }
                   
                
                if (flag)
                    return "";
                else
                    return "IMPOSSIBLE";
            }

            for (int d = weCanOpen.Count / 2; d >= 1; d /= 2)
                for (int i = d; i < weCanOpen.Count; i++)
                    for (int j = i; j >= d && map[weCanOpen[j]][1] > map[weCanOpen[j - d]][1]; j -= d)
                    {
                        int tmp = weCanOpen[j - d];
                        weCanOpen[j - d] = weCanOpen[j];
                        weCanOpen[j] = tmp;
                    }

          
            foreach (int i in weCanOpen)
            {
                if ((map[weCanOpen[0]][1] > 0) && (map[i][0] == 0))
                    return "IMPOSSIBLE";
                openedChest[i] = true;
                inKey.Remove(map[i][0]);
                List<int> myNewKey = new List<int> { };
                foreach (int k in inKey)
                    myNewKey.Add(k);
                for (int j = 2; j < map[i].Count; j++)
                    myNewKey.Add(map[i][j]);
                
                
                string trying = boxes_analyzer(myNewKey, map, openedChest);

                if (trying.IndexOf("IMPOSSIBLE") == -1)
                    return (i+1) + " " + trying;
                else
                {
                    openedChest[i] = false;
                    inKey.Add(map[i][0]);
                }
            }

            return "IMPOSSIBLE";
        }

    }
}
