﻿using System.Drawing;
using System.Drawing.Imaging;
using OpenTK.Graphics.OpenGL;
using PixelFormat = System.Drawing.Imaging.PixelFormat;

namespace Roton.Interface.Video.Renderer
{
    /// <summary>
    /// Basic OpenGL 3.0 renderer. This renderer does not support shaders or
    /// anything as long as it supports frame buffer objects (FBOs).
    /// </summary>
    public class OpenGl3 : Renderer
    {
        private int _glLastTexture = -1;

        private void GenerateTexture(ref IFastBitmap gameBitmap) {
            if(gameBitmap == null) return;

            var glNewTexture = GL.GenTexture();

            var fbData = gameBitmap.LockBits(new Rectangle(0, 0, gameBitmap.Width, gameBitmap.Height), ImageLockMode.ReadOnly,
                PixelFormat.Format32bppArgb);
            GL.BindTexture(TextureTarget.Texture2D, glNewTexture);
            GL.TexImage2D(TextureTarget.Texture2D, 0, PixelInternalFormat.Rgba, gameBitmap.Width, gameBitmap.Height, 0,
                OpenTK.Graphics.OpenGL.PixelFormat.Bgra, PixelType.UnsignedByte, fbData.Scan0);
            gameBitmap.UnlockBits(fbData);

            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMinFilter,
                (int)TextureMinFilter.Nearest);
            GL.TexParameter(TextureTarget.Texture2D, TextureParameterName.TextureMagFilter,
                (int)TextureMagFilter.Nearest);

            if(_glLastTexture != -1)
                GL.DeleteTexture(_glLastTexture);
            _glLastTexture = glNewTexture;
        }

        /// <summary>
        /// Initializes the <see cref="OpenGl3" /> Rrnderer.
        /// </summary>
        protected override void InitImplementation()
        {
            GL.ClearColor(Color.Black);
            GL.Disable(EnableCap.Lighting); // unnecessary
            GL.Disable(EnableCap.DepthTest); // unnecessary
            GL.Enable(EnableCap.Texture2D); // required for FBOs to work
        }

        protected override void RenderImplementation(ref IFastBitmap gameBitmap)
        {
            GL.Clear(ClearBufferMask.ColorBufferBit);

            // Don't draw anything if the Bitmap is null.
            if(gameBitmap == null) return;

            GenerateTexture(ref gameBitmap);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadIdentity();

            GL.Begin(BeginMode.Quads);
            GL.TexCoord2(0.0f, 0.0f);
            GL.Vertex2(0.0f, 0.0f);
            GL.TexCoord2(1.0f, 0.0f);
            GL.Vertex2(TerminalWidth, 0.0f);
            GL.TexCoord2(1.0f, 1.0f);
            GL.Vertex2(TerminalWidth, TerminalHeight);
            GL.TexCoord2(0.0f, 1.0f);
            GL.Vertex2(0.0f, TerminalHeight);
            GL.End();
        }

        protected override void UpdateViewportImplementation()
        {
            GL.Viewport(0, 0, FormControl.Width, FormControl.Height);

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadIdentity();
            GL.Ortho(0.0, TerminalWidth, TerminalHeight, 0.0, -1.0, 1.0);
        }
    }
}