namespace navdi3
{
    using UnityEngine;
    public class Util
    {
        public static void shufl<T>(ref T[] a)
        {
            for (int i = 0; i < a.Length - 1; i++)
            {
                int j = Random.Range(i, a.Length);
                if (i == j) continue;
                T temp = a[i];
                a[i] = a[j];
                a[j] = temp;
            }
        }
        public static int tow(int a, int b, int rate)
        {
            if (a + rate < b) return a + rate; else if (a - rate > b) return a - rate; else return b;
        }
        public static float tow(float a, float b, float rate)
        {
            if (a + rate < b) return a + rate; else if (a - rate > b) return a - rate; else return b;
        }
        public static float remap(float a1, float b1, float a2, float b2, float originalValue)
        {
            return (originalValue - a1) / (b1 - a1) * (b2 - a2) + a2;
        }
    }
}