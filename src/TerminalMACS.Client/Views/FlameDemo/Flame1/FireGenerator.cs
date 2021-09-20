using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TerminalMACS.Client.Views.FlameDemo.Flame1
{
    public class FireGenerator
    {
        #region Private Members

        private Random r = new Random();

        private int _width;
        private int _height;
        private byte[] _fireData;

        #endregion

        #region Constructor

        public FireGenerator(int width, int height)
        {
            _width = width;
            _height = height;

            _fireData = new byte[_width * _height];
        }

        #endregion

        public byte[] FireData
        {
            get { return _fireData; }
        }

        public int Height
        {
            get { return _height; }
        }

        public int Width
        {
            get { return _width; }
        }

        #region Private Methods

        private void GenerateBaseline()
        {
            for (int x = 0; x < _width; x++)
            {
                int nBytePos = GetBytePos(x, _height - 1);
                _fireData[nBytePos] = GetRandomNumber();
            }
        }

        public void UpdateFire()
        {
            GenerateBaseline();

            for (int y = 0; y < _height - 1; y++)
            {
                for (int x = 0; x < _width; x++)
                {
                    int leftVal;

                    if (x == 0)
                        leftVal = _fireData[GetBytePos(_width - 1, y)];
                    else
                        leftVal = _fireData[GetBytePos(x - 1, y)];

                    int rightVal;
                    if (x == _width - 1)
                        rightVal = _fireData[GetBytePos(0, y)];
                    else
                        rightVal = _fireData[GetBytePos(x + 1, y)];

                    int belowVal = _fireData[GetBytePos(x, y + 1)];

                    int sum = leftVal + rightVal + (belowVal * 2);
                    int avg = sum / 4;

                    // auto reduce it so you get lest of the forced fade and more vibrant fire waves
                    if (avg > 0)
                        avg--;

                    if (avg < 0 || avg > 255)
                        throw new Exception("Average color calc is out of range 0-255");

                    _fireData[GetBytePos(x, y)] = (byte)avg;
                }
            }
        }

        private byte GetRandomNumber()
        {
            int randomValue = r.Next(2);
            if (randomValue == 0)
                return (byte)0;
            else if (randomValue == 1)
                return (byte)255;
            else
                throw new Exception("Random returned out of bounds");
        }

        private int GetBytePos(int x, int y)
        {
            return ((y * _width) + x);
        }

        #endregion
    }

}
