using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Birdle
{
    internal class Tile
    {
        public bool b_IsVisible;
        private Rectangle m_Rectangle;
        private Rectangle m_SourceRectangle;

        public Tile(Rectangle rectangle, Rectangle sourceRectangle)
        {
            m_Rectangle = rectangle;
            m_SourceRectangle = sourceRectangle;
            b_IsVisible = true;
        }
        
        // Used to change location gradually like an animation
        public void Update()
        {

        }

        public void Render(SpriteBatch m_SpriteBatch, Texture2D texture)
        {
            if (b_IsVisible)
            {
                m_SpriteBatch.Draw(texture, m_Rectangle, m_SourceRectangle, Color.White);
            }
        }
    }
}