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

        public float shipHealth = 100;
        public float maxShipHealth;
        public float weaponHealth = 5;
        public float weaponMaxHealth;
        public float materialStorageHealth = 5;
        public float materialMaxHealth;
        public float powerGeneratorHealth = 5;
        public float generatorMaxHealth;
        public float powerStorageHealth = 5;
        public float powerStorageMaxHealth;

        public float materialStored = 100;
        public float weaponDamage = 10;
        public float weaponFireRate = 10f;
        public float weaponPowerNeeded = 5;
        public float weaponCooldown = 5;

        public float powerGeneration = 5;
        public float currentPower = 50;

        public EnemyAI(Ship playerShip, CombatState combatState)
        {
            _playerShip = playerShip;
            _combatState = combatState;
            //var scale = _playerShip.MaxHealth * 0.01;
            var scale = Math.Pow(_playerShip.MaxHealth, 0.1f);

            //shipHealth = (int)(shipHealth * scale);
            weaponHealth = (float)(weaponHealth * scale);
            materialStorageHealth = (float)(materialStorageHealth * scale);
            powerGeneratorHealth = (float)(powerGeneratorHealth * scale);
            powerStorageHealth = (float)(powerStorageHealth * scale);
            materialStored = (float)(materialStored * scale);
            weaponDamage = (float)(weaponDamage * scale);
            weaponFireRate =((float)weaponFireRate / (float)scale);
            weaponPowerNeeded = (float)(weaponPowerNeeded * scale);
            powerGeneration = (float)(powerGeneration * scale);

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
            return (int)(weaponHealth + materialStorageHealth + powerGeneratorHealth + powerStorageHealth);
        }
    }
}
