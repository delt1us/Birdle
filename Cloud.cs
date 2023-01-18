using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    internal class Cloud
    {
        private Texture2D m_Texture;
        private Vector2 vec_Location;
        private float f_Speed;
        // 1 means right, -1 means left
        private int i_Direction;
        private Rectangle rec_SpriteArea;

        public Cloud(Texture2D texture, Vector2 startLocation, int direction, float speed, Rectangle spriteArea)
        {
            m_Texture = texture;
            vec_Location = startLocation;
            f_Speed = speed;
            i_Direction = direction;
            rec_SpriteArea = spriteArea;
        }

        public void Update(float f_TimeElapsed)
        {
            float displacement = i_Direction * f_Speed * f_TimeElapsed;
            vec_Location.X += displacement;
        }

        public void Render(SpriteBatch m_SpriteBatch)
        {
            m_SpriteBatch.Draw(m_Texture, vec_Location, rec_SpriteArea, Color.White);
        }
    }
}
