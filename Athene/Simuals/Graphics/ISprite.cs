using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;

namespace Simuals.Graphics
{
	/// <summary>
	/// Defines a sprite
	/// </summary>
    public interface ISprite
    {
		/// <summary>
		/// Draws the sprite on screen
		/// </summary>
		/// <param name="position"></param>
		/// <param name="scale"></param>
        void Draw(Vector2 position, Vector2 scale);
    }
}
