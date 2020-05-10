﻿using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using MonoGameWindowsStarter.Components;
using MonoGameWindowsStarter.Rendering;
using MonoGameWindowsStarter.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.Spaceship
{
    public class Projectile
    {
        public enum Attack_Against
        {
            Player,
            EnemyWeapon,
            EnemyStorage,
            EnemyGenerator
        }

        private Point _target;
        private Vector2 _position;
        private int _speed;
        private int _damage;
        public Texture2D _texture;
        public Sprite _sprite;
        public bool AtTarget = false;
        private CombatState _combatState;
        private Attack_Against _against;

        

        public Projectile(Point target, Vector2 position, int damage, Attack_Against against,CombatState combatState)
        {
            _target = target;
            _position = position;
            _speed = 10;
            _damage = damage;
            _combatState = combatState;
            _against = against;
        }

        public void Update(GameTime gameTime)
        {
            var dx = _target.X - _position.X;
            var dy = _target.Y - _position.Y;
            var dist = Math.Sqrt(dx * dx + dy * dy);
            var step = _speed;

            if (dist <= step)
            {
                HitTarget(_target);
                AtTarget = true;
            }
            else
            {
                var nx = dx / dist;
                var ny = dy / dist;
                _position.X += (float)(nx * step);
                _position.Y += (float)(ny * step);
            }
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            var rect = new Rectangle((int)_position.X, (int)_position.Y, 10, 10);
            spriteBatch.Draw(_texture, rect, Color.Red);
        }

        public void HitTarget(Point target)
        {

            switch (_against)
            {
                case (Attack_Against.EnemyGenerator):
                    {
                        break;
                    }
                case (Attack_Against.EnemyStorage):
                    {
                        break;
                    }
                case (Attack_Against.EnemyWeapon):
                    {
                        break;
                    }
                default:
                    {
                        bool hitOnTile = _combatState.Ship.Grid.PixelToTile(target.X, target.Y, out int tileX, out int tileY);
                        Point tileHit = new Point(tileX, tileY);
                        foreach (Room room in _combatState.Ship.Rooms)
                        {
                            if (room.Contains(tileHit))
                            {
                                room.AlterHealth(_damage);
                            }
                        }
                        break;
                    }
            }
        }
    }
}
