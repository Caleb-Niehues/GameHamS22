using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using TimeGame.Collisions;
using TimeGame.Views;

namespace TimeGame.Sprites
{

    /// <summary>
    /// The type of crate to create
    /// </summary>
    public enum CrateType
    {
        Slats = 0,
        Cross,
        DarkCross
    }

    /// <summary>
    /// A class representing a crate
    /// </summary>
    public class Crate : Sprite
    {
        // The game this crate belongs to
        Game game;

        // The VertexBuffer of crate vertices
        VertexBuffer vertexBuffer;

        // The IndexBuffer defining the Crate's triangles
        IndexBuffer indexBuffer;

        // The effect to render the crate with
        BasicEffect effect;

        // The texture to apply to the crate
        Texture2D texture;

        BoundingRectangle box;

        CirclingCamera Camera;

        /// <summary>
        /// Creates a new crate instance
        /// </summary>
        /// <param name="game">The game this crate belongs to</param>
        /// <param name="type">The type of crate to use</param>
        /// <param name="heightSpawn">spawning location of box (1-4)</param>
        /// <param name="windowWidth">width of window (1-4)</param>
        public Crate(Game game, CrateType type, int heightSpawn, int windowWidth)
        {
            this.game = game;
            this.texture = game.Content.Load<Texture2D>($"crate{(int)type}_diffuse");
            InitializeEffect();
            switch ((heightSpawn - 1) % 4)
            {
                case 1:
                    effect.World = Matrix.CreateTranslation(25, 4.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 10, 30), 1f);

                    box = new BoundingRectangle(windowWidth + 40, 125, 50, 75);
                    break;
                case 2:
                    effect.World = Matrix.CreateTranslation(25, -1.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 0, 30), 1f);

                    box = new BoundingRectangle(windowWidth + 40, 225, 50, 75);
                    break;
                case 3:
                    effect.World = Matrix.CreateTranslation(25, -6, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, -5, 30), 1f);

                    box = new BoundingRectangle(windowWidth + 40, 325, 50, 75);
                    break;

                default:
                    effect.World = Matrix.CreateTranslation(25, 10, 0);
                    // where 25 is the x coordinate of the box (not pixels)
                    //where 10 is the y coordinate of the box (not pixels)
                    //where 0 is vield of view

                    Camera = new CirclingCamera(game, new Vector3(0, 15, 30), 1f);
                    //where 0 is x (unused)
                    //where 15 is height of camera compared to box (0 is dead straight)
                    //where 30 is distance to box (larger means smaller box)
                    //1f also to size of box

                    box = new BoundingRectangle(windowWidth + 40, 25, 50, 75);
                    break;
            }
        }

        /// <summary>
        /// Initializes the vertex of the cube
        /// </summary>
        public void InitializeVertices()
        {
            var vertexData = new VertexPositionNormalTexture[] { 
                // Front Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Forward },

                // Back Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Backward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Forward },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, 1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Forward },

                // Top Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Up },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, 1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Up },

                // Bottom Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Down },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Down },

                // Left Face
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Left },
                new VertexPositionNormalTexture() { Position = new Vector3(-1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Left },

                // Right Face
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 1.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f, -1.0f), TextureCoordinate = new Vector2(0.0f, 0.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f,  1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 0.0f), Normal = Vector3.Right },
                new VertexPositionNormalTexture() { Position = new Vector3( 1.0f, -1.0f,  1.0f), TextureCoordinate = new Vector2(1.0f, 1.0f), Normal = Vector3.Right },
            };
            vertexBuffer = new VertexBuffer(game.GraphicsDevice, typeof(VertexPositionNormalTexture), vertexData.Length, BufferUsage.None);
            vertexBuffer.SetData<VertexPositionNormalTexture>(vertexData);
        }

        /// <summary>
        /// Initializes the Index Buffer
        /// </summary>
        public void InitializeIndices()
        {
            var indexData = new short[]
            {
                // Front face
                0, 2, 1,
                0, 3, 2,

                // Back face 
                4, 6, 5,
                4, 7, 6,

                // Top face
                8, 10, 9,
                8, 11, 10,

                // Bottom face 
                12, 14, 13,
                12, 15, 14,

                // Left face 
                16, 18, 17,
                16, 19, 18,

                // Right face 
                20, 22, 21,
                20, 23, 22
            };
            indexBuffer = new IndexBuffer(game.GraphicsDevice, IndexElementSize.SixteenBits, indexData.Length, BufferUsage.None);
            indexBuffer.SetData<short>(indexData);
        }

        /// <summary>
        /// Initializes the BasicEffect to render our crate
        /// </summary>
        void InitializeEffect()
        {
            effect = new BasicEffect(game.GraphicsDevice);
            effect.World = Matrix.CreateScale(2.0f);
            effect.View = Matrix.CreateLookAt(
                new Vector3(8, 9, 12), // The camera position
                new Vector3(0, 0, 0), // The camera target,
                Vector3.Up            // The camera up vector
            );
            effect.Projection = Matrix.CreatePerspectiveFieldOfView(
                MathHelper.PiOver4,                         // The field-of-view 
                game.GraphicsDevice.Viewport.AspectRatio,   // The aspect ratio
                0.1f, // The near plane distance 
                100.0f // The far plane distance
            );
            effect.TextureEnabled = true;
            effect.Texture = texture;
            // Turn on lighting

            /*effect.LightingEnabled = true;
            // Set up light 0
            effect.DirectionalLight0.Enabled = true;
            effect.DirectionalLight0.Direction = new Vector3(1f, 0, 1f);
            effect.DirectionalLight0.DiffuseColor = new Vector3(0.8f, 0, 0);
            effect.DirectionalLight0.SpecularColor = new Vector3(1f, 0.4f, 0.4f);
            effect.AmbientLightColor = new Vector3(0.3f, 0.3f, 0.3f);*/
        }

        public override void LoadContent(ContentManager content)
        {
            InitializeVertices();
            InitializeIndices();
            box.LoadContent(content);
        }

        /// <summary>
        /// Updates the Cube
        /// </summary>
        /// <param name="gameTime"></param>
        public override void Update(GameTime gameTime)
        {
            //Rotate object here? 

            float angle = 2 * (float)gameTime.TotalGameTime.TotalSeconds;
            effect.World = Matrix.CreateRotationY(angle) * Matrix.CreateTranslation(effect.World.Translation);

            box.X = box.X - (75 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            Camera.Update(gameTime);
        }

        /// <summary>
        /// Draws the crate
        /// </summary>
        /// <param name="gameTime"></param>
        /// <param name="spriteBatch"></param>
        public override void Draw(GameTime gameTime, SpriteBatch spriteBatch)
        {
            // set the view and projection matrices
            effect.View = Camera.View;
            effect.Projection = Camera.Projection;
            // apply the effect 
            effect.CurrentTechnique.Passes[0].Apply();
            // set the vertex buffer
            game.GraphicsDevice.SetVertexBuffer(vertexBuffer);
            // set the index buffer
            game.GraphicsDevice.Indices = indexBuffer;
            // Draw the triangles
            game.GraphicsDevice.DrawIndexedPrimitives(
                PrimitiveType.TriangleList, // Tye type to draw
                0,                          // The first vertex to use
                0,                          // The first index to use
                12                          // the number of triangles to draw
            );
            
            box.Draw(gameTime, spriteBatch);
        }
    }
}
