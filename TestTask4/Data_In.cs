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

            StreamReader in_file = new StreamReader(@"..\..\D-small-practice.in");
            num = Int32.Parse(in_file.ReadLine());


            
            for (int i = 0; i < num; i++)
            {
                
                Console.WriteLine("Case #" + (i+1) + ": " +  disassemble_quest(in_file));
                //function

            }
        }


        private static string disassemble_quest(StreamReader my_file) // read map
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

            
            problemAnswer = boxes_analyzer(myKey, myMap, openChest);

            return problemAnswer;
        }


        private static string boxes_analyzer(List<int> inKey, List<int>[] map, bool[] openedChest)
        {

            bool flag = true;

            foreach (bool i in openedChest)
                if (!(i))
                {
                    flag = false;
                    break;
                }

            if (flag)
                return "";

            string qwerty;
            List<int> chestOpened = new List<int> {};

            for (int i = 0; i < map.Length; i++)
            {
                if ((inKey.IndexOf(map[i][0]) >= 0) && !(openedChest[i]))
                    chestOpened.Add(i);
            }

            //sorting by the number of keys in the trunk
            for(int d = chestOpened.Count/2; d>=1; d/=2)
                for(int i = d; i < chestOpened.Count; i++)
                    for(int j = i; j>=d && map[chestOpened[j]][1] > map[chestOpened[j-d]][1]; j -=d) //warning!!!
                    {
                        int tmp = chestOpened[j - d];
                        chestOpened[j - d] = chestOpened[j];
                        chestOpened[j] = tmp;
                    }


            while(chestOpened.Count > 0)
            {
                int tmp = chestOpened[0];
                openedChest[tmp] = true;
                chestOpened.Remove(tmp);

                List<int> keyPlus = inKey;
                keyPlus.Remove(map[tmp][1]);
                for (int i = 2; i < map[tmp].Count; i++)
                    keyPlus.Add(map[tmp][i]);

                string result = boxes_analyzer(keyPlus, map, openedChest);

                if (result.IndexOf("IMPOSSIBLE") == -1)
                {
                    result = ++tmp + " " + result;
                    return result;
                }
                    

                openedChest[tmp] = false;
            }


            return "IMPOSSIBLE";
        }

    }
}
