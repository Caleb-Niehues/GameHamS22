using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace TimeGame.Views
{
    /// <summary>
    /// A camera that circles the origin 
    /// </summary>
    public class CirclingCamera : ICamera
    {
        // The camera's angle 
        float angle;

        // The camera's position
        Vector3 position;

        // The camera's target
        Vector3 target;

        // The camera's speed 
        float speed;

        // The game this camera belongs to 
        Game game;

        // The view matrix 
        Matrix view;

        // The projection matrix 
        Matrix projection;

        /// <summary>
        /// The camera's view matrix 
        /// </summary>
        public Matrix View => view;

        /// <summary>
        /// The camera's projection matrix 
        /// </summary>
        public Matrix Projection => projection;

        /// <summary>
        /// Constructs a new camera that circles the origin
        /// </summary>
        /// <param name="game">The game this camera belongs to</param>
        /// <param name="position">The initial position of the camera</param>
        /// <param name="speed">The speed of the camera</param>
        public CirclingCamera(Game game, Vector3 position, float speed)
        {
            this.game = game;
            this.position = position;
            this.speed = speed;
            this.projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,
                game.GraphicsDevice.Viewport.AspectRatio,
                1,
                1000
            );
            this.view = Matrix.CreateLookAt(
                position,
                Vector3.Zero,
                Vector3.Up
            );

            this.target = new Vector3(0, 0, 0);
        }

        /// <summary>
        /// Updates the camera's positon
        /// </summary>
        /// <param name="gameTime">The GameTime object</param>
        public void Update(GameTime gameTime)
        {
            // update the angle based on the elapsed time and speed
            angle += speed * (float)gameTime.ElapsedGameTime.TotalSeconds;

            // Calculate a new view matrix
            target += new Vector3(4 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0);
            position += new Vector3(4 * (float)gameTime.ElapsedGameTime.TotalSeconds, 0, 0);

            // Calculate a new view matrix
            this.view =
                //Matrix.CreateRotationY(angle) *
                Matrix.CreateLookAt(position, target, Vector3.Up);
        }
    }
}
