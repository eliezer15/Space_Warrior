using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceAdventure
{
    class Enemy

    {
        public AttackType attackType {get; private set;}
        public int damageCaused {get; private set;}
        public String attackSoundEffect { get; private set; }
        public  AnimatedSprite stanceImage {get; private set;}
        public AnimatedSprite attackImage { get; private set; }
        public Vector2 location;
        public float originalLocationX;
        public float originalLocationY;
        public Color color;
       

        public enum State { active, inactive, attacking, fadingOut, fadingIn, destroyed };
        public State state;
        

        public Enemy() //used to create the first active monster
        {
            this.state = State.inactive;
        }

        public Enemy(AttackType attackType, AnimatedSprite stanceImage, AnimatedSprite attackImage,
            int damageCaused, String attackSoundEffect, Vector2 location, Color tint)
        {
            this.attackType = attackType;
            this.damageCaused = damageCaused;
            this.stanceImage = stanceImage;
            this.attackImage = attackImage;
            this.attackSoundEffect = attackSoundEffect;
            this.location = location;
            this.originalLocationX = location.X;
            this.originalLocationY = location.Y;
            this.color = tint;
            this.state = State.inactive;
        }

        public void activate()
        {
            state = State.active;
        }

        public void deactivate()
        {
            state = State.inactive;
        }

        public void attack(Player player)
        {
            player.takeDamage(damageCaused);
        }

        public void drawAttackImage(SpriteBatch spriteBatch)
        {
            this.attackImage.Draw(spriteBatch, location, color);
        }

        public void drawStanceImage(SpriteBatch spriteBatch)
        {
            this.stanceImage.Draw(spriteBatch, location, color);
        }
        public void UpdateAttackImage()
        {
            attackImage.Update();
        }

        public void UpdateStanceImage()
        {
            stanceImage.Update();
        }

        public void moveBackToOrigin()
        {
            location = new Vector2(originalLocationX, originalLocationY);
        }

    }
}
