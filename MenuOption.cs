using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace SpaceAdventure
{
    class MenuOption
    {
        public Texture2D unselected { get; private set; }
        public Texture2D selected { get; private set; }
        public bool isSelected { get; set; }

        public MenuOption(Texture2D unselected, Texture2D selected)
        {
            this.selected = selected;
            this.unselected = unselected;
            isSelected = false;
        }

        public void Draw(SpriteBatch spriteBatch, Vector2 location)
        {
            if (isSelected)
                spriteBatch.Draw(selected, location,Color.White);
            else
                spriteBatch.Draw(unselected, location, Color.White);
        }
    }
}
