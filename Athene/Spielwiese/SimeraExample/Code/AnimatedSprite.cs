using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OpenTK;
using OpenTK.Graphics.OpenGL;

namespace SimeraExample.Code
{
    class AnimatedSprite
    {
        private List<Texture2D> _textures;
        public float AnimationLength { get; set; }
        private Stopwatch _timeSource = new Stopwatch();

        public AnimatedSprite(List<Texture2D> textures, float animationLength)
        {
            AnimationLength = animationLength;
            _textures = textures;
        }

        public  AnimatedSprite() { }

        public void AddFrame(Texture2D textureFrame)
        {
            _textures.Add(textureFrame);
        }


        private Texture2D GetTextureFromTime(float time)
        {

            float normalizedDeltaTime = (time % AnimationLength) / AnimationLength;
            var index = Math.Round(normalizedDeltaTime * (_textures.Count - 1));
            return _textures[(int)index];
        }

		public void Draw(Vector2 position, Vector2 scale)
        {
            if (!_timeSource.IsRunning) _timeSource.Start();
            Texture2D texture = GetTextureFromTime((float)_timeSource.Elapsed.TotalSeconds);


            //return;
            texture.Enable();
            GL.Begin(PrimitiveType.Quads);

            var vertices = new Vector2[4]
            {
                new Vector2(0, 0),
                new Vector2(0, 1),
                new Vector2(1, 1),
                new Vector2(1, 0),
            };

            for (int index = 0; index < 4; index++)
            {
                GL.TexCoord2(vertices[index]);

                vertices[index].X *= texture.Width;
                vertices[index].Y *= texture.Height;
                vertices[index] *= scale;
                vertices[index] += position;

                GL.Vertex2(vertices[index]);
            }

            GL.End();
            texture.Disable();
        }
    }
}
