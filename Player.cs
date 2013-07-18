using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace SpaceAdventure
{
    class Player
    {
        public int HP { get; set; }
        public bool isActive { get; private set; }
        public int score { get; set; }
        public int enemiesEliminated { get; set; }
        public String attackSoundEffect { get; set; }
        public String missSoundEffect { get; set; }
        public String hurtSoundEffect { get; private set; }
        public String lowHealthSoundEffect { get; set; }

        public Player() 
        {
            HP = 0;
            isActive = true;
            score = 0;
        }

        public Player(int hp)
        {
            this.HP = hp;
            isActive = true;
            score = 0;
        }

        //Returns true if attack is succesful, false otherwise
        public bool attack(Enemy monster, AttackType attack)
        {
            if (attack == monster.attackType)
            {
                monster.state = Enemy.State.destroyed;
                score += 535;
                enemiesEliminated += 1;
                return true;
            }

            else return false;
        }

        public void takeDamage(int damage) 
        {
            HP -= damage;
            if (HP <= 0)
                isActive = false;
        }

    }
}
