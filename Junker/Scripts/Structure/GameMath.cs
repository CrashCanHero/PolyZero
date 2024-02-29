using Godot;
using System;

public static partial class GameMath {
    public const double PI = 3.14159265358979323846264338327950288419716939937510582097494459d;
    public const double PI2 = PI * 2d;
    public const double Deg2Rad = PI2 / 360d;
    public const double Rad2Deg = 360d / PI2;

    public static int ConvertToInt(this bool check) => check ? 1 : 0;

    public static Vector2 ConvertTo2D(this Vector3 pos) => new Vector2(pos.X, pos.Z);

    public static Vector3 ConvertTo3D(this Vector2 pos, float zPos = 0f) => new Vector3(pos.X, pos.Y, zPos);

    public static float RoundToDirection(this float Input, int angleCount) => Input.RoundToAngle(360f / angleCount);

    public static float RoundToAngle(this float Input, float angle) {
        Input /= angle;
        Input = Mathf.Round(Input);
        Input *= angle;

        return Input;
    }

    public static float ConvertToDirection(this Vector2 ang) => ang.AngleTo(Vector2.Up) * (float)Rad2Deg;

    public static float Wrap(this float Input, float minWrap, float maxWrap) {
        while (Input < minWrap) {
            Input += maxWrap - minWrap;
        }
        while (Input > maxWrap) {
            Input -= maxWrap - minWrap;
        }
        return Input;
    }

    public static int Wrap(this int input, int minWrap, int maxWrap) {
        int delta = maxWrap - minWrap;
        while (input < minWrap) {
            input += delta;
        }
        while (input > maxWrap) {
            input -= delta;
        }
        return input;
    }

    public static Vector2 Wrap(this Vector2 input, Vector2 XMinmax, Vector2 YMinMax){
        input.X = input.X.Wrap(XMinmax.X, XMinmax.Y);
        input.Y = input.Y.Wrap(YMinMax.X, YMinMax.Y);
        return input;
    }
    

    public static Vector3 Wrap(this Vector3 input, Vector3 XMinmax, Vector3 YMinMax, Vector3 ZMinMax){
        input.X = input.X.Wrap(XMinmax.X, XMinmax.Y);
        input.Y = input.Y.Wrap(YMinMax.X, YMinMax.Y);
        input.Z = input.Z.Wrap(ZMinMax.X, ZMinMax.Y);
        return input;
    }

    public static float Dot01(Vector2 input, Vector2 check) {
        float dot = input.Dot(check);
        dot++;
        dot /= 2f;
        return dot;
    }

    public static float Dot01(Vector3 input, Vector3 check) {
        float dot = input.Dot(check);
        dot++;
        dot /= 2f;
        return dot;
    }

    public static Vector2 ConvertToVector(this float input) => new Vector2(
        (float)-TrigLookup.Cos((input / 360f) * (float)PI2), 
        (float)TrigLookup.Sin((input / 360f) * (float)PI2)
    );

    public static float ToRadians(this float input) => input * (float)Deg2Rad;

    public static float ToDegrees(this float input) => input * (float)Rad2Deg;

    public static Vector3 Round(this Vector3 input) => new Vector3(Mathf.Round(input.X), Mathf.Round(input.Y), Mathf.Round(input.Z));
    public static Vector2 Round(this Vector2 input) => new Vector2(Mathf.Round(input.X), Mathf.Round(input.Y));

    public static Vector3 Invert(this Vector3 input, bool x = false, bool y = false, bool z = false) => new Vector3(input.X * (x ? -1f : 1f), input.Y * (y ? -1f : 1f), input.Z * (z ? -1f : 1f));
    public static Vector2 Invert(this Vector2 input, bool x = false, bool y = false) => new Vector2(input.X * (x ? -1f : 1f), input.Y * (y ? -1f : 1f));
    public static Vector2 Flip(this Vector2 input) => new Vector2(input.Y, input.X);

    public static float Clamp(this float input, float Min, float Max) => Mathf.Clamp(input, Min, Max);
    public static int Clamp(this int input, int Min, int Max) => Mathf.Clamp(input, Min, Max);

    public static bool InRange(this int i, int min, int max, bool inclusive = true) => (inclusive && (i >= min && i <= max)) || (!inclusive && (i > min && i < max));
    public static bool InRange(this float i, float min, float max, bool inclusive = true) => (inclusive && (i >= min && i <= max)) || (!inclusive && (i > min && i < max));
}