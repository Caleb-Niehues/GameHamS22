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

        public BoundingRectangle Bounds;

        CirclingCamera Camera;

        int _width;
        int _heightSpawn;

        /// <summary>
        /// Creates a new crate instance
        /// </summary>
        /// <param name="game">The game this crate belongs to</param>
        /// <param name="type">The type of crate to use</param>
        /// <param name="heightSpawn">spawning location of box (1-4)</param>
        /// <param name="windowWidth">width of window (1-4)</param>
        public Crate(Game game, CrateType type, int heightSpawn, int windowWidth)
        {
            this.IsActive = true;
            this.game = game;
            this.texture = game.Content.Load<Texture2D>($"crate{(int)type}_diffuse");
            InitializeEffect();
            _heightSpawn = heightSpawn;
            _width = windowWidth;
            switch ((heightSpawn - 1) % 4)
            {
                case 1:
                    effect.World = Matrix.CreateTranslation(25, 4.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 10, 30), 1f);

                    Bounds = new BoundingRectangle(_width + 40, 135, 50, 65);
                    break;
                case 2:
                    effect.World = Matrix.CreateTranslation(25, -1.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 0, 30), 1f);

                    Bounds = new BoundingRectangle(_width + 40, 225, 50, 65);
                    break;
                case 3:
                    effect.World = Matrix.CreateTranslation(25, -6, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, -5, 30), 1f);

                    Bounds = new BoundingRectangle(_width + 40, 325, 50, 65);
                    break;

                default:
                    effect.World = Matrix.CreateTranslation(25, 10, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 15, 30), 1f);

                    Bounds = new BoundingRectangle(_width + 40, 25, 50, 65);
                    break;
            }
        }

        /// <summary>
        /// reloads crate on right side of map
        /// </summary>
        /// <param name="heightSpawn">spawning location of box (1-4): default is previous</param>
        public void ResetCrate(int heightSpawn = 0)
        {
            if (heightSpawn != 0)
            {
                _heightSpawn = heightSpawn;
            }
            InitializeEffect();
            switch ((_heightSpawn - 1) % 4)
            {
                case 1:
                    effect.World = Matrix.CreateTranslation(25, 4.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 10, 30), 1f);

                    Bounds.X = _width + 40;
                    Bounds.Y = 135;
                    break;
                case 2:
                    effect.World = Matrix.CreateTranslation(25, -1.5f, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 0, 30), 1f);

                    Bounds.X = _width + 40;
                    Bounds.Y = 235;
                    break;
                case 3:
                    effect.World = Matrix.CreateTranslation(25, -6, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, -5, 30), 1f);

                    Bounds.X = _width + 40;
                    Bounds.Y = 335;
                    break;

                default:
                    effect.World = Matrix.CreateTranslation(25, 10, 0);
                    Camera = new CirclingCamera(game, new Vector3(0, 15, 30), 1f);

                    Bounds.X = _width + 40;
                    Bounds.Y = 35;
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
            Bounds.LoadContent(content);
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

            Bounds.X = Bounds.X - (75 * (float)gameTime.ElapsedGameTime.TotalSeconds);

            if (Bounds.X + Bounds.Width + 10 <= 0)
            {
                IsActive = false;
                //ResetCrate(_heightSpawn);
            }
            
            Camera.Update(gameTime);
        }

        public void Debug(GameTime gameTime, SpriteBatch spriteBatch)
        {
            Texture2D rect = new Texture2D(spriteBatch.GraphicsDevice, 1, 1);
            rect.SetData(new[] { Color.Red });
            spriteBatch.Draw(rect, new Rectangle((int)Bounds.X + 20, (int)Bounds.Y, 64, 64), Color.DarkRed * (float).8);
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

            Debug(gameTime, spriteBatch);
        }
    }
}
