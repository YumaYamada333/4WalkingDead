using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MathClass : MonoBehaviour {

    // ループ変数
    public class Looper
    {
        int value;
        int max;
        int min;
        int zeroLevel;

        public Looper(int max, int min, int zeroLevel, int value)
        {
            this.value = value;
            this.max = max;
            this.min = min;
            this.zeroLevel = zeroLevel;
            if (this.max < this.min) Debug.Log("値が無効");
            if (!(this.min <= this.zeroLevel && this.zeroLevel <= this.max)) Debug.Log("値が無効");
            if (!(this.min <= this.value && this.value <= this.max)) Debug.Log("値が無効");
        }

        public void Plus(int a)
        {
            a = a % ((max - min) + 1);
            //value = value + a > max ? min + (value + a - max) :
            //    value + a < min ? max + (value + a - min) : 
            //    value + a;

            if (value + a < min || value + a > max)
            {
                if (value + a > max)
                {
                    value = min - 1 + (value + a - max);
                }
                else
                {
                    value = max + 1 + (value + a - min);
                }
            }
            else
            {
                value = value + a;
            }
        }

        public void Reset()
        {
            value = zeroLevel;
        }

        public int Get()
        {
            return value;
        }
    }

    // Vector Lerp
    static public Vector3 Lerp(Vector3 sta, Vector3 end, float t)
    {
        return new Vector3(Mathf.Lerp(sta.x, end.x, t),
            Mathf.Lerp(sta.y, end.y, t),
            Mathf.Lerp(sta.z, end.z, t));
    }

}
