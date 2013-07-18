using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceAdventure
{
    class Level
    {

        public Texture2D bkgdImage { get; private set; }
        private int difficulty;
        public int Difficulty 
        {
            get
            {
                return difficulty;
            }
            set
            {
                difficulty = value;
                enemyAttackDelay = 5500 - (difficulty * 1000);
                enemyEntranceDelay = 1000;
            }
        } //from 1 to 10
        public String bkgdSong { get; private set; }
        public int enemyAttackDelay { get; private set; }
        public int enemyEntranceDelay { get; private set; }


        /* In order to keep variety, a lot of games reuse their game sprites for different levels, and only change the color scheme of each monster
         * This is whay I plan to do, each level will have a scheme, and the color tint of each monster will be kept in this array.
         */
        public Level(Texture2D bkgdImage, String bkgdSong) 
        {
            this.bkgdImage = bkgdImage;
            this.bkgdSong = bkgdSong;
        }

    }
}
