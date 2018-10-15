using System;
using System.IO;
using System.Runtime.CompilerServices;
using OJE.GL;

namespace CellEngine.Shaders
{
    public class ShaderProgram
    {
        public readonly string VertexShader;
        public readonly string FragmentShader;
        public readonly string GeometryShader;
        private readonly int Object;

        public ShaderProgram(string vertexShader, string fragmentShader, string geometryShader = null)
        {
            VertexShader = vertexShader;
            FragmentShader = fragmentShader;
            GeometryShader = geometryShader;

            Object = GL.CreateProgram();

            int shader;
            string message;

            if (!string.IsNullOrWhiteSpace(vertexShader))
            {
                shader = GL.CreateShader(GL.VERTEX_SHADER);
                GL.ShaderSource(shader, 1, new[] { File.ReadAllText(Path.Combine("Shaders\\", vertexShader)) }, null);
                GL.CompileShader(shader);
                GL.AttachShader(Object, shader);
                GL.DeleteShader(shader);

                message = GL.GetShaderInfoLog(shader, 512);
                Console.WriteLine(message);
            }

            if (!string.IsNullOrWhiteSpace(fragmentShader))
            {
                shader = GL.CreateShader(GL.FRAGMENT_SHADER);
                GL.ShaderSource(shader, 1, new[] { File.ReadAllText(Path.Combine("Shaders\\", fragmentShader)) }, null);
                GL.CompileShader(shader);
                GL.AttachShader(Object, shader);
                GL.DeleteShader(shader);

                message = GL.GetShaderInfoLog(shader, 512);
                Console.WriteLine(message);
            }

            if (!string.IsNullOrWhiteSpace(geometryShader))
            {
                shader = GL.CreateShader(GL.GEOMETRY_SHADER);
                GL.ShaderSource(shader, 1, new[] { File.ReadAllText(Path.Combine("Shaders\\", geometryShader)) }, null);
                GL.CompileShader(shader);
                GL.AttachShader(Object, shader);
                GL.DeleteShader(shader);

                message = GL.GetShaderInfoLog(shader, 512);
                Console.WriteLine(message);
            }
            
            GL.LinkProgram(Object);

            Console.WriteLine(GL.GetProgramInfoLog(Object, 512));
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public void Use()
        {
            GL.UseProgram(Object);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public static void Unbind()
        {
            GL.UseProgram(0);
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public int GetUniformLocation(string location)
        {
            return GL.GetUniformLocation(Object, location);
        }
    }
}