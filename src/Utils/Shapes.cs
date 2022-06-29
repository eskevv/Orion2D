using System;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Orion2D;

public class Shapes 
{
    // -- Fields

    private static GraphicsDevice _device;
    private static BasicEffect _effect;

    public void Initialize(CoreGame game) {
        _device = game.GraphicsDevice;
        _effect = new BasicEffect(game.GraphicsDevice);

        _effect.VertexColorEnabled = true;
        _effect.Projection = Matrix.CreateOrthographicOffCenter(0f, _device.Viewport.Width, _device.Viewport.Height, 0f, -10f, 10f);
        _effect.World = Matrix.CreateTranslation(0.5f, 0.5f, 0f);
        _effect.View = Matrix.Identity;
    }

    // -- Configurations

    private static void DrawPrimitives(PrimitiveType type, VertexPositionColor[] vertices, short[] indeces, int primitiveCount) {

        foreach (var effectPass in _effect.CurrentTechnique.Passes) {
            effectPass.Apply();
            _device.DrawUserIndexedPrimitives(type, vertices, 0, vertices.Length, indeces, 0, primitiveCount);
        }
    }

    private static short[] GetContinuousIndeces(int vertexCount) {
        var indeces = new short[vertexCount];
        for (short i = 0; i < indeces.Length; i++) {
            indeces[i] = i;
        }

        return indeces;
    }

    private static short[] GetClosingIndeces(int vertexCount) {
        var indeces = new short[vertexCount + 1];
        for (short i = 0; i < indeces.Length; i++) {
            if (i == indeces.Length - 1 && i != 1) {
                indeces[i] = 0;
                break;
            }
            indeces[i] = i;
        }

        return indeces;
    }

    private static short[] GetPiTriangleIndeces(int faces) {
        short[] indeces = new short[faces * 3];
        short last_index = 1;
        for (int i = 0; i < faces; i++) {
            indeces[i * 3 + 0] = 0;
            indeces[i * 3 + 1] = last_index;
            indeces[i * 3 + 2] = (short)(last_index + 1);
            last_index++;
        }

        return indeces;
    }

    private static VertexPositionColor[] GetPiVertices(Vector2 position, int sides, float radius, float rotation, Color color) {
        float pi_rota = MathHelper.TwoPi / sides;
        float cos = MathF.Cos(pi_rota);
        float sin = MathF.Sin(pi_rota);

        /* Rotation Formula =>
            x2 = cos_x1 - sin_y1
            y2 = sin_x1 + cos_y1 
        */

        float cos_i = MathF.Cos(rotation);
        float sin_i = MathF.Sin(rotation);
        float ax = cos_i * (radius * CoreGame.ScreenWidth) - sin_i * 0;
        float ay = sin_i * (radius * CoreGame.ScreenWidth) + cos_i * 0;

        var vertices = new VertexPositionColor[sides];
        for (int i = 0; i < vertices.Length; i++) {
            float x = (cos * ax - sin * ay);
            float y = (sin * ax + cos * ay);

            ax = x;
            ay = y;

            vertices[i] = new VertexPositionColor(new Vector3(x + position.X, y + position.Y, 0), color);
        }

        return vertices;
    }

    //-- Draw Methods

    public static void DrawLine(Vector2 start, Vector2 end, Color color) {
        var vertices = new VertexPositionColor[2] {
            new VertexPositionColor(new Vector3(start.X, start.Y , 0), color),
            new VertexPositionColor(new Vector3(end.X * CoreGame.ScreenWidth, end.Y * CoreGame.ScreenHeight, 0), color),
        };

        short[] indeces = GetContinuousIndeces(2);

        DrawPrimitives(PrimitiveType.LineStrip, vertices, indeces, 1);
    }

    public static void DrawRect(Vector2 position, int w, int h, Color color) {
        float left = position.X;
        float top = position.Y;
        float right = position.X + (w - 1) * CoreGame.ScreenWidth;
        float bottom = position.Y + (h - 1) * CoreGame.ScreenHeight;

        var vertices = new VertexPositionColor[4] {
            new VertexPositionColor(new Vector3(left, top, 0), color),
            new VertexPositionColor(new Vector3(right, top, 0), color),
            new VertexPositionColor(new Vector3(right, bottom, 0), color),
            new VertexPositionColor(new Vector3(left, bottom, 0), color),
        };

        short[] indeces = GetClosingIndeces(4);

        DrawPrimitives(PrimitiveType.LineStrip, vertices, indeces, 4);
    }

    public static void DrawFillRect(Vector2 position, int w, int h, Color color) {
        float left = position.X;
        float top = position.Y;
        float right = position.X + (w * CoreGame.ScreenWidth) - 1;
        float bottom = position.Y + (h * CoreGame.ScreenHeight) - 1;

        var vertices = new VertexPositionColor[6] { // essentially two triangles
            new VertexPositionColor(new Vector3(left, top, 0), color),
            new VertexPositionColor(new Vector3(right, top, 0), color),
            new VertexPositionColor(new Vector3(right, bottom, 0), color),

            new VertexPositionColor(new Vector3(right, bottom, 0), color),
            new VertexPositionColor(new Vector3(left, bottom, 0), color),
            new VertexPositionColor(new Vector3(left, top, 0), color),
        };

        short[] indeces = GetContinuousIndeces(vertices.Length);

        DrawPrimitives(PrimitiveType.TriangleList, vertices, indeces, 2);
    }

    public static void DrawPolygon(Vector2 position, int sides, float radius, float rotation, Color color) {
        VertexPositionColor[] vertices = GetPiVertices(position, sides, radius, rotation, color);
        short[] indeces = GetClosingIndeces(vertices.Length);

        DrawPrimitives(PrimitiveType.LineStrip, vertices, indeces, sides);
    }

    public static void DrawFillPolygon(Vector2 position, int sides, float radius, float rotation, Color color) {
        int faces = sides - 2;

        VertexPositionColor[] vertices = GetPiVertices(position, sides, radius, rotation, color);
        short[] indeces = GetPiTriangleIndeces(faces);

        DrawPrimitives(PrimitiveType.TriangleList, vertices, indeces, faces);
    }

    public static void DrawCircle(Vector2 position, float radius, Color color) {
        DrawPolygon(position, 200, radius, 0, color);
    }

    public static void DrawFillCircle(Vector2 position, float radius, Color color) {
        DrawFillPolygon(position, 200, radius, 0, color);
    }
}
