using System.Collections.Generic;
using UnityEngine;

namespace Toolbox
{
    /// <summary>
    /// These locations often correspond to position surrounding a player or NPC. Game objects 
    /// are typically used as markers for these points in space in the player or NPC's heirarchy.
    /// </summary>
    public enum CharacterLocation
    {
        Above,
        Behind,
        Center,
        Dynamic,
        Feet,
        Front,
        Head,
        WaterLine
    }

    public enum AxisDirection
    {
        up,
        down,
        left,
        right,
        forward,
        back
    }

    /// < summary >
    /// MathKit is a static class containing utility functions for various mathematical operations.
    /// </summary>
    public static class MathKit
    {
        public static readonly Vector3[] VectorDirection = new Vector3[]
        {
        Vector3.up,
        Vector3.down,
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back,
        };

        /// <summary>
        /// Rotates a vector around the up axis by a given number of degrees.
        /// </summary>
        public static Vector3 RotateAroundUp(Vector3 _characterForward, float _degrees)
        {
            /*  [ cos(a) 0 sin(a) ]
                [   0    1   0   ]
                [-sin(a) 0 cos(a) ]  */

            float _radians = _degrees * Mathf.Deg2Rad;

            float cos = Mathf.Cos(_radians);
            float sin = Mathf.Sin(_radians);

            Vector3 _newForward = new(
                (cos * _characterForward.x) + (sin * _characterForward.z),
                _characterForward.y,
                (cos * _characterForward.z) - (sin * _characterForward.x)
            );

            return _newForward;
        }

        /// <summary>
        /// Returns the vector direction for the given AxisDirection.
        /// </summary>
        public static Vector3 GetAxisDirection(AxisDirection _axis)
        {
            return VectorDirection[(int)_axis];
        }

        /// <summary>
        /// Return True only when one of the three values is true.
        /// </summary>
        public static bool XOR3(bool a, bool b, bool c) => (a && !b && !c) || (!a && b && !c) || (!a && !b && c);

        /// <summary>
        /// Returns -1 when to the left, 1 to the right, and 0 for forward/backward
        /// </summary>
        public static float AngleDir(Vector3 fwd, Vector3 targetDir, Vector3 up)
        {
            Vector3 perp = Vector3.Cross(fwd, targetDir);
            float dir = Vector3.Dot(perp, up);

            if (dir > 0.0f)
            {
                return 1.0f;
            }
            else if (dir < 0.0f)
            {
                return -1.0f;
            }
            else
            {
                return 0.0f;
            }
        }

        /// <summary>
        /// Chooses a string from the given choices, based on provided weights.
        /// </summary>
        public static string Choice(IEnumerable<string> choices, IEnumerable<int> weights)
        {
            // https://stackoverflow.com/questions/66298705/does-c-sharp-have-something-equivalent-to-pythons-random-choices

            var cumulativeWeight = new List<int>();
            int last = 0;
            foreach (var cur in weights)
            {
                last += cur;
                cumulativeWeight.Add(last);
            }
            int choice = Random.Range(0, last);
            int i = 0;
            foreach (var cur in choices)
            {
                if (choice < cumulativeWeight[i])
                {
                    return cur;
                }
                i++;
            }
            return null;
        }

        /// <summary>
        /// Remaps a value x in interval [A,B], to the proportional value in interval [C,D]
        /// </summary>
        public static float Remap(float x, float A, float B, float C, float D)
        {
            float remappedValue = C + (x - A) / (B - A) * (D - C);
            return remappedValue;
        }

        /// <summary>
        /// Shuffles the elements of the provided list.
        /// </summary>
        public static void Shuffle<T>(this IList<T> list)
        {
            for (int i = 0; i < list.Count; i++)
            {
                int j = Random.Range(i, list.Count);
                (list[j], list[i]) = (list[i], list[j]);
            }
        }
    }
}

