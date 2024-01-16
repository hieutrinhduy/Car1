using System;
using UnityEngine;

namespace Others
{
    public static class Range
    {
        #region Bounds

        public static Vector3 Random(this Bounds bounds)
        {
            Vector3 position = new Vector3
            {
                x = UnityEngine.Random.Range(bounds.min.x, bounds.max.x),
                y = UnityEngine.Random.Range(bounds.min.y, bounds.max.y),
                z = UnityEngine.Random.Range(bounds.min.z, bounds.max.z)
            };
            return position;
        }

        #endregion

        #region Float

        public static float Lerp(FloatRange range, float time)
        {
            return Mathf.Lerp(range.min, range.max, time);
        }

        public static float InverseLerp(FloatRange range, float value)
        {
            return Mathf.InverseLerp(range.min, range.max, value);
        }

        public static float Clamp(FloatRange range, float value)
        {
            return Mathf.Clamp(value, range.min, range.max);
        }

        #endregion 
        
        #region Float01

        public static float Lerp(FloatRange01 range, float time)
        {
            return Mathf.Lerp(range.min, range.max, time);
        }

        public static float InverseLerp(FloatRange01 range, float value)
        {
            return Mathf.InverseLerp(range.min, range.max, value);
        }

        public static float Clamp(FloatRange01 range, float value)
        {
            return Mathf.Clamp(value, range.min, range.max);
        }

        #endregion

        #region Int

        public static float Lerp(IntRange range, float time)
        {
            return Mathf.Lerp(range.min, range.max, time);
        }

        public static float InverseLerp(IntRange range, float value)
        {
            return Mathf.InverseLerp(range.min, range.max, value);
        }

        #endregion

        #region Vector2

        public static Vector2 Lerp(Vector2Range range, float time)
        {
            return Vector2.Lerp(range.min, range.max, time);
        }


        public static float InverseLerp(Vector2Range range, Vector2 value)
        {
            Vector2 AB = range.max - range.min;
            Vector2 AV = value - range.min;

            return Vector2.Dot(AV, AB) / Vector2.Dot(AB, AB);
        }

        #endregion

        #region Vector3

        public static Vector3 Lerp(Vector3Range range, float time)
        {
            return Vector3.Lerp(range.min, range.max, time);
        }

        public static float InverseLerp(Vector3Range range, Vector3 value)
        {
            Vector3 AB = range.max - range.min;
            Vector3 AV = value - range.min;

            return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
        }

        #endregion

        #region Quaternion

        public static Quaternion Lerp(QuaternionRange range, float time)
        {
            return Quaternion.Lerp(range.min, range.max, time);
        }

        #endregion
    }

    #region Bounds structures

    [Serializable]
    public class FloatRange
    {
        public float min;
        public float max;

        public float Random => UnityEngine.Random.Range(min, max);


        public FloatRange()
        {
        }

        public FloatRange(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class FloatRange01
    {
        [Range(0, 1)]
        public float min;

        [Range(0, 1)]
        public float max;

        public float Random => UnityEngine.Random.Range(min, max);


        public FloatRange01()
        {
        }

        public FloatRange01(float min, float max)
        {
            this.min = min;
            this.max = max;
        }
    }


    [Serializable]
    public class IntRange
    {
        public int min;
        public int max;

        public int Random => UnityEngine.Random.Range(min, max);


        public IntRange()
        {
        }

        public IntRange(int min, int max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class Vector2Range
    {
        public Vector2 min;
        public Vector2 max;

        public Vector2 Random
        {
            get
            {
                Vector2 result = new Vector2
                {
                    x = UnityEngine.Random.Range(min.x, max.x),
                    y = UnityEngine.Random.Range(min.y, max.y),
                };

                return result;
            }
        }


        public Vector2Range()
        {
        }

        public Vector2Range(Vector2 min, Vector2 max)
        {
            this.min = min;
            this.max = max;
        }
    }

    [Serializable]
    public class Vector3Range
    {
        public Vector3 min;
        public Vector3 max;

        public Vector3 Random
        {
            get
            {
                Vector3 result = new Vector3
                {
                    x = UnityEngine.Random.Range(min.x, max.x),
                    y = UnityEngine.Random.Range(min.y, max.y),
                    z = UnityEngine.Random.Range(min.z, max.z)
                };

                return result;
            }
        }


        public Vector3Range()
        {
        }

        public Vector3Range(Vector3 min, Vector3 max)
        {
            this.min = min;
            this.max = max;
        }
    }


    [Serializable]
    public class QuaternionRange
    {
        public Quaternion min;
        public Quaternion max;


        public QuaternionRange()
        {
        }

        public QuaternionRange(Quaternion min, Quaternion max)
        {
            this.min = min;
            this.max = max;
        }
    }

    #endregion
}