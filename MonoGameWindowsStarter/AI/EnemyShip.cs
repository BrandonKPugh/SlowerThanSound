using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.AI
{
    public class EnemyShip
    {
        public int shipHealth;
        public int weaponHealth;
        public int materialStorageHealth;
        public int powerGeneratorHealth;

        public int materialStored;
        public int weaponDamage;
        public int weaponFireRate;
        public int weaponPowerNeeded;
        public int weaponCooldown;
        
        public int powerGeneration;
        public int currentPower;

        public EnemyShip(int playerLevel)
        {

        }

    }
}
