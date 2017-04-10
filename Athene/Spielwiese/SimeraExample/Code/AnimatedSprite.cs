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
    class AnimatedSprite : ISprite
    {
        private List<Sprite> _textures;
        public float AnimationLength { get; set; }
        private Stopwatch _timeSource = new Stopwatch();

        public AnimatedSprite(List<Sprite> textures, float animationLength)
        {
            AnimationLength = animationLength;
            _textures = textures;
        }

        public  AnimatedSprite() { }

        public void AddFrame(Sprite textureFrame)
        {
            _textures.Add(textureFrame);
        }

        public void ResetTimer()
        {
            _timeSource.Reset();
        }

        private Sprite GetTextureFromTime(float time)
        {

            float normalizedDeltaTime = (time % AnimationLength) / AnimationLength;
            var index = Math.Round(normalizedDeltaTime * (_textures.Count - 1));
            return _textures[(int)index];
        }

		public void Draw(Vector2 position, Vector2 scale)
        {
            if (!_timeSource.IsRunning) _timeSource.Start();
            var sprite = GetTextureFromTime((float)_timeSource.Elapsed.TotalSeconds);
            sprite.Draw(position, scale);

          
        }
    }
}
