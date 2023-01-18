using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    internal class Tile
    {
        private static float f_TILE_SPEED = 2000f;
        public bool b_IsVisible;
        public Point m_GridCoordinates;
        public Rectangle m_Rectangle;
        private Rectangle m_SourceRectangle;

        public Point m_OriginalLocation;
        public Point m_TargetLocation;
        public float f_Opacity;

        public Tile(Rectangle rectangle, Rectangle sourceRectangle, Point gridCoordinates)
        {
            b_IsVisible = true;
            m_GridCoordinates = gridCoordinates;
            m_OriginalLocation = m_GridCoordinates;
            m_Rectangle = rectangle;
            m_SourceRectangle = sourceRectangle;
            f_Opacity = 1f;
        }

        // Used to change location gradually like an animation
        public void Update(float f_TimeElapsed)
        {
            if (m_Rectangle.Location != m_TargetLocation)
            {
                UpdateLocation(f_TimeElapsed);
            }
        }

        public void UpdateLocation(float f_TimeElapsed, bool b_Instant = false)
        {
            // Instant only when shuffled
            if (b_Instant)
            {
                m_Rectangle.Location = m_TargetLocation;
                return;
            }

            // Move X
            if (m_Rectangle.X > m_TargetLocation.X)
            {
                m_Rectangle.X -= (int)(f_TILE_SPEED * f_TimeElapsed);

                // If new location is past target location
                if (m_Rectangle.X < m_TargetLocation.X)
                {
                    m_Rectangle.X = m_TargetLocation.X;
                }
            }

            else if (m_Rectangle.X < m_TargetLocation.X)
            {
                m_Rectangle.X += (int)(f_TILE_SPEED * f_TimeElapsed);

                // If new location is past target location
                if (m_Rectangle.X > m_TargetLocation.X)
                {
                    m_Rectangle.X = m_TargetLocation.X;
                }
            }

            // Move Y
            if (m_Rectangle.Y > m_TargetLocation.Y)
            {
                m_Rectangle.Y -= (int)(f_TILE_SPEED * f_TimeElapsed);

                // If new location is past target location
                if (m_Rectangle.Y < m_TargetLocation.Y)
                {
                    m_Rectangle.Y = m_TargetLocation.Y;
                }
            }

            else if (m_Rectangle.Y < m_TargetLocation.Y)
            {
                m_Rectangle.Y += (int)(f_TILE_SPEED * f_TimeElapsed);

                // If new location is past target location
                if (m_Rectangle.Y > m_TargetLocation.Y)
                {
                    m_Rectangle.Y = m_TargetLocation.Y;
                }
            }
        }

        public void Render(SpriteBatch m_SpriteBatch, Texture2D texture)
        {
            m_SpriteBatch.Draw(texture, m_Rectangle, m_SourceRectangle, Color.White * f_Opacity);
        }
    }
}