using System;
using UnityEngine;

namespace Project_Portfolio.Global
{
    public struct UShortVector2
    {
        public ushort X;
        public ushort Y;
        
        public UShortVector2(ushort p_x, ushort p_y)
        {
            X = p_x;
            Y = p_y;
        }

        public static explicit operator Vector2(UShortVector2 p_vector)
        {
            return new Vector2(p_vector.X, p_vector.X);
        }
        
        public static UShortVector2 operator/(UShortVector2 p_vector, int p_divider)
        {
            return new UShortVector2
            {
                X = (ushort)(p_vector.X / p_divider),
                Y = (ushort)(p_vector.Y / p_divider),
            };
        }
        
        public static UShortVector2 operator/(UShortVector2 p_vector, uint p_divider)
        {
            return new UShortVector2
            {
                X = (ushort)(p_vector.X / p_divider),
                Y = (ushort)(p_vector.Y / p_divider),
            };
        }
        
        public static UShortVector2 operator%(UShortVector2 p_vector, int p_divider)
        {
            return new UShortVector2
            {
                X = (ushort)(p_vector.X % p_divider),
                Y = (ushort)(p_vector.Y % p_divider),
            };
        }
        
        public static UShortVector2 operator%(UShortVector2 p_vector, uint p_divider)
        {
            return new UShortVector2
            {
                X = (ushort)(p_vector.X % p_divider),
                Y = (ushort)(p_vector.Y % p_divider),
            };
        }
    }
}