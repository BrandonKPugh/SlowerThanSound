using Microsoft.Xna.Framework;
using MonoGameWindowsStarter.Spaceship;
using MonoGameWindowsStarter.States;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MonoGameWindowsStarter.AI
{
    public class EnemyAI
    {
        #region Fields

        protected Ship _playerShip;

        protected CombatState _combatState;

        private TimeSpan timer;
        #endregion

        public int shipHealth = 100;
        public int maxShipHealth;
        public int weaponHealth = 10;
        public int weaponMaxHealth;
        public int materialStorageHealth = 10;
        public int materialMaxHealth;
        public int powerGeneratorHealth = 10;
        public int generatorMaxHealth;
        public int powerStorageHealth = 10;
        public int powerStorageMaxHealth;

        public int materialStored = 100;
        public int weaponDamage = 10;
        public int weaponFireRate = 10;
        public int weaponPowerNeeded = 5;
        public int weaponCooldown = 5;

        public int powerGeneration = 5;
        public int currentPower = 50;

        public EnemyAI(Ship playerShip, CombatState combatState)
        {
            _playerShip = playerShip;
            _combatState = combatState;
            var scale = _playerShip.MaxHealth * 0.01;

            //shipHealth = (int)(shipHealth * scale);
            weaponHealth = (int)(weaponHealth * scale);
            materialStorageHealth = (int)(materialStorageHealth * scale);
            powerGeneratorHealth = (int)(powerGeneratorHealth * scale);
            powerStorageHealth = (int)(powerStorageHealth * scale);
            materialStored = (int)(materialStored * scale);
            weaponDamage = (int)(weaponDamage * scale);
            weaponFireRate = (int)(weaponFireRate / scale);
            weaponPowerNeeded = (int)(weaponPowerNeeded * scale);
            powerGeneration = (int)(powerGeneration * scale);

            weaponMaxHealth = weaponHealth;
            materialMaxHealth = materialStorageHealth;
            generatorMaxHealth = powerGeneratorHealth;
            powerStorageMaxHealth = powerStorageHealth;

            shipHealth = CalculateShipHealth();
            maxShipHealth = shipHealth;

            timer = new TimeSpan();
        }

        private void AttackPlayer()
        {

            var priorityList = _playerShip.GetRoomPriorities();
            Rectangle rect = new Rectangle();
            int priority = 0;
            foreach (Tuple<int, Rectangle> pair in priorityList)
            {
                if (pair.Item1 > priority)
                {
                    priority = pair.Item1;
                    rect = pair.Item2;
                }
            }
            if (priority > 0)
                FireWeapon(rect);
        }

        private void FireWeapon(Rectangle target)
        {
            Random rand = new Random();
            var x = rand.Next(target.X, target.X + target.Width);
            var y = rand.Next(target.Y, target.Y + target.Height);
            _combatState.AddProjectile(new Projectile(new Point(x, y), new Vector2(1000, 1000), weaponDamage, Projectile.Attack_Against.Player,_combatState));
        }

        public void Update(GameTime gameTime)
        {
            timer += gameTime.ElapsedGameTime;
            if (timer.TotalSeconds > weaponFireRate)
            {
                if (weaponHealth > 0)
                {
                    AttackPlayer();
                }
                timer = new TimeSpan();
            }

            if(shipHealth < 0)
            {

            }

            if(weaponHealth < 0)
                weaponHealth = 0;
            if (materialStorageHealth < 0)
                materialStorageHealth = 0;
            if (powerGeneratorHealth < 0)
                powerGeneratorHealth = 0;
            if (powerStorageHealth < 0)
                powerStorageHealth = 0;
            shipHealth = CalculateShipHealth();
        }

        public int CalculateShipHealth()
        {
            return (weaponHealth + materialStorageHealth + powerGeneratorHealth + powerStorageHealth);
        }
    }
}
