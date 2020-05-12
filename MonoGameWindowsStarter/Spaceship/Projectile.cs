using Microsoft.Xna.Framework;
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
        Random rand = new Random();
        public enum Attack_Against
        {
            Player,
            EnemyWeapon,
            EnemyStorage,
            EnemyGenerator,
            EnemyPowerStorage,
            EnemyHull
        }

        private Point _target;
        private Vector2 _position;
        private int _speed;
        private float _damage;
        public Texture2D _texture;
        public Sprite _sprite;
        public bool AtTarget = false;
        private CombatState _combatState;
        private Attack_Against _against;

        

        public Projectile(Point target, Vector2 position, float damage, Attack_Against against,CombatState combatState)
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
                        _combatState.enemyAI.powerGeneratorHealth -= _damage;
                        break;
                    }
                case (Attack_Against.EnemyStorage):
                    {
                        _combatState.enemyAI.materialStorageHealth -= _damage;
                        break;
                    }
                case (Attack_Against.EnemyWeapon):
                    {
                        _combatState.enemyAI.weaponHealth -= _damage;
                        break;
                    }
                case (Attack_Against.EnemyPowerStorage):
                    {
                        _combatState.enemyAI.powerStorageHealth -= _damage;
                        break;
                    }
                case (Attack_Against.EnemyHull):
                    {
                        int waysToSplit = 0;
                        if (_combatState.enemyAI.powerGeneratorHealth > 0)
                            waysToSplit++;
                        if (_combatState.enemyAI.materialStorageHealth > 0)
                            waysToSplit++;
                        if (_combatState.enemyAI.weaponHealth > 0)
                            waysToSplit++;
                        if (_combatState.enemyAI.powerStorageHealth > 0)
                            waysToSplit++;

                        _damage /= (float)waysToSplit;
                        _damage *= 1.5f;
                        if (_combatState.enemyAI.powerGeneratorHealth > 0)
                            _combatState.enemyAI.powerGeneratorHealth -= _damage;
                        if (_combatState.enemyAI.materialStorageHealth > 0)
                            _combatState.enemyAI.materialStorageHealth -= _damage;
                        if (_combatState.enemyAI.weaponHealth > 0)
                            _combatState.enemyAI.weaponHealth -= _damage;
                        if (_combatState.enemyAI.powerStorageHealth > 0)
                            _combatState.enemyAI.powerStorageHealth -= _damage;
                        break;

                    }
                default:
                    {
                        bool hitOnTile = _combatState.Ship.Grid.PixelToTile(target.X, target.Y, out int tileX, out int tileY);
                        Point tileHit = new Point(tileX, tileY);
                        _combatState.Ship.AlterHealth(_damage);
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
