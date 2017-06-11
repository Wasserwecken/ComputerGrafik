using OpenTK;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lib.Tools
{
    public static class AlignmentTools
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="item"></param>
        public static Alignment EvaluateAlignment(Vector2 sourceCenter, Vector2 itemCenter)
        {
            if (Math.Abs(sourceCenter.X - itemCenter.X) > Math.Abs(sourceCenter.Y - itemCenter.Y))
            {
                if (itemCenter.X > sourceCenter.X)
                    return Alignment.Right;
                else
                    return Alignment.Left;
            }
            else
            {
                if (itemCenter.Y > sourceCenter.Y)
                    return Alignment.Top;
                else
                    return Alignment.Bottom;
            }
        }
    }
}
