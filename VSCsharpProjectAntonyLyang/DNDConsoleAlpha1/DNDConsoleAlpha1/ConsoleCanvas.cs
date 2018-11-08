using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DNDConsole1
{
    class ConsoleCanvas
    {
        public int height;
        public int width;
        public string empty = "  ";
        public string[,] bufferbackup;
        public string[,] buffer;
        public ConsoleColor[,] colorbuffer;
        public void Clearbuffer()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    buffer[i, j] = empty;
                    colorbuffer[i, j] = ConsoleColor.Gray;
                }
            }
        }//清空buffer
        public ConsoleCanvas(int h, int w)
        {
            width = w;
            height = h;
            buffer = new string[width, height];
            bufferbackup = new string[width, height];
            colorbuffer = new ConsoleColor[width, height];
            Console.CursorVisible = false;
            Clearbuffer();
        }
        public string[,] GetBuffer()
        {
            return buffer;
        }
        public ConsoleColor[,] GetColors()
        {
            return colorbuffer;
        }
        public void CopyBuffer(string[,] dest, string[,] source)
        {
            for (int i = 0; i < source.GetLength(0); i++)
            {
                for (int j = 0; j < source.GetLength(1); j++)
                {
                    dest[i, j] = source[i, j];
                }
            }
        }
        public void RefreshBuffer()
        {
            for (int i = 0; i < height; i++)
            {
                for (int j = 0; j < width; j++)
                {
                    if (buffer[j, i] != bufferbackup[j, i])
                    {
                        Console.SetCursorPosition(j * 2, i);
                        ConsoleColor color = colorbuffer[j, i];
                        Console.ForegroundColor = color;
                        Console.Write(buffer[j, i]);
                    }
                }
            }
        }


    }
}
