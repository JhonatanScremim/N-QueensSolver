using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace N_QueensSolver
{
    class umnomeai
    {
        int[] array;
        int size;
        Random rand = new Random();
        float temperature;
        int counter = 0;

        public umnomeai()
        {
            size = 9;
            temperature = rand.Next(0, 20);

            createArray();
            validate();
            outputArray();
        }

        private void validate()
        {
            bool outputEveryStep = false;
            bool timed = false;
            int pauseTime = 1;
            int max = 100;

            do
            {
                shuffle();
                control(outputEveryStep, timed, pauseTime);
                temperature -= 0.1f;
            } while (counter < max || temperature >= 0);
            Console.WriteLine("");
        }

        private void control(bool outputEveryStep, bool timed, int pauseTime)
        {
            if (outputEveryStep)
                outputArray();
            counter++;
            //Console.Write("\r[{0}]", counter);
            if (timed)
                Thread.Sleep(pauseTime);
        }

        private void shuffle()
        {
            int a, b, c;
            getPositionsOfShuffle(out a, out b, out c);
            array[a] = array[b];
            array[b] = c;
        }

        private void getPositionsOfShuffle(out int a, out int b, out int c)
        {
            a = rand.Next(0, size);
            b = rand.Next(0, size);
            c = array[a];
        }

        private void outputArray()
        {
            for (int i = 0; i < size; i++)
            {
                Console.Write("[{0}]", array[i]);
            }
            Console.Write("\n");
        }

        private void createArray()
        {
            this.array = new int[size];
            for (int i = 1; i <= size; i++)
            {
                array[i - 1] = i;
            }
        }
    }
}
