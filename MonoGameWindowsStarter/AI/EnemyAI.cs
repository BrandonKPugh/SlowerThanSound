using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Spaceship;
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

        protected Ship _enemyShip;

        protected Ship _playerShip;

        #endregion

        public EnemyAI(Ship enemyShip, Ship playerShip)
        {

        }

        private void AttackPlayer()
        {
            var priorityDict = _playerShip.GetRoomPriorities();
            Rectangle rect; 
            priorityDict.TryGetValue(priorityDict.Keys.Max(), out rect);
        }

        private void FireWeapon(Rectangle target)
        {
            Random rand = new Random();
            var x = rand.Next(target.X, target.X + target.Width);
            var y = rand.Next(target.Y, target.Y + target.Height);
            //Spawn new projectile with random start position outside of the screen that flies towards (x,y)
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
