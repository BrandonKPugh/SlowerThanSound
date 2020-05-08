using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Spaceship;
using MonoGameWindowsStarter.States;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.AI
{
    public class EnemyAI
    {
        #region Fields

        //protected Ship _enemyShip;

        protected Ship _playerShip;

        protected CombatState _combatState;

        private TimeSpan timer;

        #endregion

        public EnemyAI(Ship playerShip, CombatState combatState)
        {
            _playerShip = playerShip;
            _combatState = combatState;
            timer = new TimeSpan();
        }

        private void AttackPlayer()
        {
            var priorityList = _playerShip.GetRoomPriorities();
            Rectangle rect = new Rectangle();
            int priority = 0;
            foreach (Tuple<int, Rectangle> pair in priorityList)
            {
                if(pair.Item1 > priority)
                {
                    priority = pair.Item1;
                    rect = pair.Item2;
                }
            }
            FireWeapon(rect);
        }

        private void FireWeapon(Rectangle target)
        {
            Random rand = new Random();
            var x = rand.Next(target.X, target.X + target.Width);
            var y = rand.Next(target.Y, target.Y + target.Height);
            _combatState.AddProjectile(new Projectile(new Point(x, y), new Vector2(1000, 1000)));
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if (timer.TotalSeconds > 1)
            {
                AttackPlayer();
                timer = new TimeSpan();
            }
        }
    }
}
