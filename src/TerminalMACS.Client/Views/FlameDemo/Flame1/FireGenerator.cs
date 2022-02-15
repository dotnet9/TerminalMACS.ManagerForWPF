using System;

namespace TerminalMACS.Client.Views.FlameDemo.Flame1;

public class FireGenerator
{
    #region Constructor

    public FireGenerator(int width, int height)
    {
        Width = width;
        Height = height;

        FireData = new byte[Width * Height];
    }

    #endregion

    public byte[] FireData { get; }

    public int Height { get; }

    public int Width { get; }

    #region Private Members

    private readonly Random r = new();

    #endregion

    #region Private Methods

    private void GenerateBaseline()
    {
        for (var x = 0; x < Width; x++)
        {
            var nBytePos = GetBytePos(x, Height - 1);
            FireData[nBytePos] = GetRandomNumber();
        }
    }

    public void UpdateFire()
    {
        GenerateBaseline();

        for (var y = 0; y < Height - 1; y++)
        for (var x = 0; x < Width; x++)
        {
            int leftVal;

            if (x == 0)
                leftVal = FireData[GetBytePos(Width - 1, y)];
            else
                leftVal = FireData[GetBytePos(x - 1, y)];

            int rightVal;
            if (x == Width - 1)
                rightVal = FireData[GetBytePos(0, y)];
            else
                rightVal = FireData[GetBytePos(x + 1, y)];

            int belowVal = FireData[GetBytePos(x, y + 1)];

            var sum = leftVal + rightVal + belowVal * 2;
            var avg = sum / 4;

            // auto reduce it so you get lest of the forced fade and more vibrant fire waves
            if (avg > 0)
                avg--;

            if (avg < 0 || avg > 255)
                throw new Exception("Average color calc is out of range 0-255");

            FireData[GetBytePos(x, y)] = (byte)avg;
        }
    }

    private byte GetRandomNumber()
    {
        var randomValue = r.Next(2);
        if (randomValue == 0)
            return 0;
        if (randomValue == 1)
            return 255;
        throw new Exception("Random returned out of bounds");
    }

    private int GetBytePos(int x, int y)
    {
        return y * Width + x;
    }

    #endregion
}