using System.Diagnostics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Graphics;

namespace Birdle
{
    // Button object that can be pressed
    internal class Button
    {
        private Rectangle rec_ButtonHitbox;
        private Texture2D m_Texture;

        private SpriteFont m_Font;
        private SpriteFont m_SmallFont;
        private SpriteFont m_LargeFont;
        public string str_Text;
        private Vector2 vec_TextLocation;

        public bool b_Pressed;
        // Used to disable buttons
        // Used in level select screen
        public bool b_Active;
        public bool b_TextVisible;
        public Color m_TextColor;
        private Vector2 vec_Location;
        private Vector2 vec_TextSize;

        public Button(Vector2 location, Texture2D texture, int width, int height, string text, SpriteFont font, SpriteFont largeFont, bool textVisible = true)
        {
            rec_ButtonHitbox = new Rectangle((int)location.X, (int)location.Y, width, height);
            m_Texture = texture;

            m_Font = font;
            m_SmallFont = font;
            m_LargeFont = largeFont;

            str_Text = text;

            b_Pressed = false;
            b_Active = true;    
            b_TextVisible = textVisible;
            m_TextColor = Color.Black;
        }
        // Called every frame
        public void Update()
        {
            var m_MouseState = Mouse.GetState();

            // If clicking, point of click is in the box and the button is not already pressed
            if (m_MouseState.LeftButton == ButtonState.Pressed && rec_ButtonHitbox.Contains(new Point(m_MouseState.X, m_MouseState.Y)) && b_Pressed == false)
            {
                b_Pressed = true;
            }

            if (rec_ButtonHitbox.Contains(m_MouseState.Position))
            {
                m_Font = m_LargeFont;
            }

            else
            {
                m_Font = m_SmallFont;
            }
        }

        // Renders button on screen
        public void Render(SpriteBatch m_SpriteBatch)
        {
            vec_TextSize = m_Font.MeasureString(str_Text);
            // Position relative to
            float f_XPosition = (int)((rec_ButtonHitbox.Width - vec_TextSize.X) / 2f);
            float f_YPosition = (int)((rec_ButtonHitbox.Height - vec_TextSize.Y) / 2f);

            vec_TextLocation = new Vector2(f_XPosition + (float)rec_ButtonHitbox.X, f_YPosition + (float)rec_ButtonHitbox.Y);

            if (b_Active)
            {
                m_SpriteBatch.Draw(m_Texture, rec_ButtonHitbox, null, Color.White);
            }
            else
            {
                m_SpriteBatch.Draw(m_Texture, rec_ButtonHitbox, null, Color.Black);
            }
            if (b_TextVisible)
            {
                m_SpriteBatch.DrawString(m_Font, str_Text, vec_TextLocation, m_TextColor);
            }
        }

        // Checks if given Point is in the button
        public void CheckCollisionWithMouse(Point mousePos)
        {
            if (rec_ButtonHitbox.Contains(mousePos))
            {
                b_Pressed = true;
            }
        }
    }
}
