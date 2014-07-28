using UnityEngine;
using System.Collections;
using MathS = System.Math;

namespace SGC.Math {
    [System.Serializable]
    public class Math {
        //-----------------------------------------------------------------------------------------------------------------------------//	
        public Vector2 minimumDistance;
        public Vector2 maximumDistance;
        public float X;
        public float Y;
        public float XAbs;
        public float YAbs;

        public enum MathStatements {
            Normal,
            Floor,
            Ceiling,
            Round
        }

        //-----------------------------------------------------------------------------------------------------------------------------//	
        public static float Distance(float a, float b, bool abs = false) {
            var d = a - b;
            return (abs) ? (Mathf.Abs(d)) : d;
        }
        //-----------------------------------------------------------------------------------------------------------------------------//	

        public static float GetValueByPercentage(float v, float p, MathStatements ms = MathStatements.Normal) {
            double x = (p * v) / 100;
            switch (ms) {
                case MathStatements.Floor:
                    x = MathS.Floor(x);
                    break;
                case MathStatements.Ceiling:
                    x = MathS.Ceiling(x);
                    break;
                case MathStatements.Round:
                    x = MathS.Round(x);
                    break;
            }
            return (float)x;
        }
        //-----------------------------------------------------------------------------------------------------------------------------//	
    }
}