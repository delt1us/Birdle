using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    internal class Tile
    {
        public bool b_IsVisible;
        public Point m_GridCoordinates;
        public Rectangle m_Rectangle;
        private Rectangle m_SourceRectangle;

        public Point m_OriginalLocation;
        public float f_Opacity;

        public Tile(Rectangle rectangle, Rectangle sourceRectangle, Point gridCoordinates, Point originalLocation)
        {
            b_IsVisible = true;
            m_GridCoordinates = gridCoordinates;
            m_Rectangle = rectangle;
            m_SourceRectangle = sourceRectangle;
            m_OriginalLocation = originalLocation;
            f_Opacity = 1f;
        }

        // Used to change location gradually like an animation
        public void Update()
        {

        }

        public void Render(SpriteBatch m_SpriteBatch, Texture2D texture)
        {
            m_SpriteBatch.Draw(texture, m_Rectangle, m_SourceRectangle, Color.White * f_Opacity);
        }
    }
}