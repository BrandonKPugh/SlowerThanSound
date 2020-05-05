using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Spaceship
{
    class Projectile
    {
        private Point _target;
        private Vector2 _position;
        private int _speed;
        public bool AtTarget = false;

        public Projectile(Game1 game, Point target, Point _position)
        {
            _speed = 10;
        }

        public void Update(GameTime gameTime)
        {
            var dx = _target.X - _position.X;
            var dy = _target.Y - _position.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);
            var step = _speed;

            if (dist <= step)
                AtTarget = true;
            else
            {
                var nx = dx / dist;
                var ny = dy / dist;
                _position.X += (float)(nx*step);
                _position.Y += (float)(ny * step);
            }
        }

    }
}
