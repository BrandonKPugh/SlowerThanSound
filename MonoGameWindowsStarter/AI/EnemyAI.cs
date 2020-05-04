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

        private void FireWeapon()
        {
            var priorityDict = _playerShip.GetRoomPriorities();
            priorityDict.Max<uint>
        }

        public void Update(GameTime gameTime)
        {
            
        }
    }
}
