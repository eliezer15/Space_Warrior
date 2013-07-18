using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
namespace SpaceAdventure

{
    public class AnimatedSprite 
    {
        public Texture2D texture { get; set; } //this is the texture atlas
        public int Rows { get; set; }
        public int Columns { get; set; }
        private int currentFrame;
        private int totalFrames;
        private int frameDelay { get; set; }
        private int frameCounter { get; set; }
        public int numberOfLoops { get; private set; }
      
        public AnimatedSprite(Texture2D texture, int rows, int columns, int totalFrames, int frameDelay)
        {
            this.texture = texture;
            this.Rows = rows;
            this.Columns = columns;
            this.totalFrames = totalFrames;
            this.frameDelay = frameDelay;
            this.frameCounter = 0;
            this.numberOfLoops = 0;
        }

        public void Update()
        {
            frameCounter++;
            if (frameCounter >= frameDelay)
            {
                currentFrame++;
                frameCounter = 0;
            }

            if (currentFrame == totalFrames)
            {
                currentFrame = 0;
                numberOfLoops++;
            }
            
        }

        public void resetNumberOfLoops()
        {
            numberOfLoops = 0;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location, Color tint)
        {
            int width = texture.Width / Columns;
            int height = texture.Height / Rows;
            int row = (int)((float)currentFrame/(float)Columns);
            int column = currentFrame % Columns;

            Rectangle sourceRectangle = new Rectangle(width * column, height * row, width, height);
            Rectangle destinationRectangle = new Rectangle((int)location.X, (int)location.Y, width, height);
            //If Game1.cs is already starting and ending the spriteBatch, no need to start them here
            //spriteBatch.Begin();
            spriteBatch.Draw(texture, destinationRectangle, sourceRectangle, tint);
            //spriteBatch.End();
        }


    }
}
