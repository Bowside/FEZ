
// Copyright 2011 GHI Electronics LLC
// Licensed under the Apache License, Version 2.0 (the "License"); you may not use this file except in compliance with the License. 
// You may obtain a copy of the License at http://www.apache.org/licenses/LICENSE-2.0
// Unless required by applicable law or agreed to in writing, software distributed under the License is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, 
// either express or implied. See the License for the specific language governing permissions and limitations under the License. 
// 
// This driver has been modified / extended by community members jasdev and Jeff_Birt

using System;
using System.Threading;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.Hardware;

namespace GHIElectronics.NETMF.FEZ
{
    public static partial class FEZ_Components
    {
        public class FEZTouch : IDisposable
        {

            #region Constants Globals

            // we are using the first four bytes for forecolor and backcolor
            private const int CHARS_PER_BUFFER = BUFFER_SIZE / (FONT_WIDTH * FONT_HEIGHT * 2 - 4);

            // constants
            public const int FONT_WIDTH = 8;
            public const int FONT_HEIGHT = 12;
            private const int SCREEN_WIDTH = 240;
            private const int SCREEN_HEIGHT = 320;
            private const int BUFFER_SIZE = 2048;
            private const byte REGISTER_WRITE_GRAM = 0x22;
            private const int TOUCH_SAMPLING_TIME = 10;

            private bool Disposed = false;
            private byte[] DisplayBuffer = new byte[BUFFER_SIZE];
            private byte[] RegisterBuffer = new byte[2];

            // lcd configuration
            private ParallelPort ParPort;
            private OutputPort LCDReset;
            private OutputPort LCDChipSelect;
            private OutputPort LCDRegSelect;
            private OutputCompare LCDBackLight;
            private Orientation LCDOrientation;

            // touch configuration
            private SPI SPIBus;
            private InputPort TouchIRQ;
            private bool TerminateTouchThread;
            private Thread TouchThread;
            private static ManualResetEvent TouchManualResetEvent;

            // display mode
            private DisplayMode DisplyMode;
            private int BacklightLevel;
            private int Width = SCREEN_WIDTH;
            private int Height = SCREEN_HEIGHT;

            Point CurrentPoint = new Point(0, 0);
            Point LastPoint = new Point(0, 0);
            Point NewPoint = new Point(0, 0);

            bool TouchStatus;
            bool LastTouchStatus = false; // true means there are touches

            byte[] TouchWriteBuffer = new byte[] { 0, 0, 0, 0 };
            byte[] TouchReadBuffer = new byte[2];

            #region Structures
            /// <summary>
            /// Respresents a singel point (pixel) on the screen
            /// </summary>
            public struct Point
            {
                public Point(int x, int y)
                {
                    this.X = x;
                    this.Y = y;
                }

                public int X;
                public int Y;

            }

            /// <summary>
            /// Line Geometry
            /// </summary>
            public struct Line
            {
                public Line(int xStart, int yStart, int xEnd, int yEnd)
                {
                    this.XStart = xStart;
                    this.YStart = yStart;
                    this.XEnd = xEnd;
                    this.YEnd = yEnd;
                }

                public Line(Point start, Point end)
                {
                    this.XStart = start.X;
                    this.YStart = start.Y;
                    this.XEnd = end.X;
                    this.YEnd = end.Y;
                }

                public int XStart;
                public int YStart;
                public int XEnd;
                public int YEnd;
            }

            /// <summary>
            /// Rectangle Geometry
            /// </summary>
            public struct Rectangle
            {
                public Rectangle(int x, int y, int width, int height)
                {
                    this.X = x;
                    this.Y = y;
                    this.Width = width;
                    this.Height = height;
                }

                public int X;
                public int Y;
                public int Width;
                public int Height;
            }

            #endregion //Structures

            #endregion //Constants Globals

            #region Enumerated Values

            /// <summary>
            /// Describes the orintation of the display
            /// </summary>
            public enum Orientation
            {
                Portrait = 0,
                PortraitInverse = 1,
                Landscape = 2,
                LandscapeInverse = 3
            }

            /// <summary>
            /// Describes color triplets
            /// </summary>
            public enum Color : ushort
            {
                AliceBlue = (0xF0 >> 3) | ((0xF8 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                AntiqueWhite = (0xFA >> 3) | ((0xEB & 0xFC) << 3) | ((0xD7 & 0xF8) << 8),
                Aqua = (0x00 >> 3) | ((0xFF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Aquamarine = (0x7F >> 3) | ((0xFF & 0xFC) << 3) | ((0xD4 & 0xF8) << 8),
                Azure = (0xF0 >> 3) | ((0xFF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Beige = (0xF5 >> 3) | ((0xF5 & 0xFC) << 3) | ((0xDC & 0xF8) << 8),
                Bisque = (0xFF >> 3) | ((0xE4 & 0xFC) << 3) | ((0xC4 & 0xF8) << 8),
                Black = (0x00 >> 3) | ((0x00 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                BlanchedAlmond = (0xFF >> 3) | ((0xEB & 0xFC) << 3) | ((0xCD & 0xF8) << 8),
                Blue = (0x00 >> 3) | ((0x00 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                BlueViolet = (0x8A >> 3) | ((0x2B & 0xFC) << 3) | ((0xE2 & 0xF8) << 8),
                Brown = (0xA5 >> 3) | ((0x2A & 0xFC) << 3) | ((0x2A & 0xF8) << 8),
                BurlyWood = (0xDE >> 3) | ((0xB8 & 0xFC) << 3) | ((0x87 & 0xF8) << 8),
                CadetBlue = (0x5F >> 3) | ((0x9E & 0xFC) << 3) | ((0xA0 & 0xF8) << 8),
                Chartreuse = (0x7F >> 3) | ((0xFF & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                Chocolate = (0xD2 >> 3) | ((0x69 & 0xFC) << 3) | ((0x1E & 0xF8) << 8),
                Coral = (0xFF >> 3) | ((0x7F & 0xFC) << 3) | ((0x50 & 0xF8) << 8),
                CornflowerBlue = (0x64 >> 3) | ((0x95 & 0xFC) << 3) | ((0xED & 0xF8) << 8),
                Cornsilk = (0xFF >> 3) | ((0xF8 & 0xFC) << 3) | ((0xDC & 0xF8) << 8),
                Crimson = (0xDC >> 3) | ((0x14 & 0xFC) << 3) | ((0x3C & 0xF8) << 8),
                Cyan = (0x00 >> 3) | ((0xFF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                DarkBlue = (0x00 >> 3) | ((0x00 & 0xFC) << 3) | ((0x8B & 0xF8) << 8),
                DarkCyan = (0x00 >> 3) | ((0x8B & 0xFC) << 3) | ((0x8B & 0xF8) << 8),
                DarkGoldenrod = (0xB8 >> 3) | ((0x86 & 0xFC) << 3) | ((0x0B & 0xF8) << 8),
                DarkGray = (0xA9 >> 3) | ((0xA9 & 0xFC) << 3) | ((0xA9 & 0xF8) << 8),
                DarkGreen = (0x00 >> 3) | ((0x64 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                DarkKhaki = (0xBD >> 3) | ((0xB7 & 0xFC) << 3) | ((0x6B & 0xF8) << 8),
                DarkMagenta = (0x8B >> 3) | ((0x00 & 0xFC) << 3) | ((0x8B & 0xF8) << 8),
                DarkOliveGreen = (0x55 >> 3) | ((0x6B & 0xFC) << 3) | ((0x2F & 0xF8) << 8),
                DarkOrange = (0xFF >> 3) | ((0x8C & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                DarkOrchid = (0x99 >> 3) | ((0x32 & 0xFC) << 3) | ((0xCC & 0xF8) << 8),
                DarkRed = (0x8B >> 3) | ((0x00 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                DarkSalmon = (0xE9 >> 3) | ((0x96 & 0xFC) << 3) | ((0x7A & 0xF8) << 8),
                DarkSeaGreen = (0x8F >> 3) | ((0xBC & 0xFC) << 3) | ((0x8B & 0xF8) << 8),
                DarkSlateBlue = (0x48 >> 3) | ((0x3D & 0xFC) << 3) | ((0x8B & 0xF8) << 8),
                DarkSlateGray = (0x2F >> 3) | ((0x4F & 0xFC) << 3) | ((0x4F & 0xF8) << 8),
                DarkTurquoise = (0x00 >> 3) | ((0xCE & 0xFC) << 3) | ((0xD1 & 0xF8) << 8),
                DarkViolet = (0x94 >> 3) | ((0x00 & 0xFC) << 3) | ((0xD3 & 0xF8) << 8),
                DeepPink = (0xFF >> 3) | ((0x14 & 0xFC) << 3) | ((0x93 & 0xF8) << 8),
                DeepSkyBlue = (0x00 >> 3) | ((0xBF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                DimGray = (0x69 >> 3) | ((0x69 & 0xFC) << 3) | ((0x69 & 0xF8) << 8),
                DodgerBlue = (0x1E >> 3) | ((0x90 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Firebrick = (0xB2 >> 3) | ((0x22 & 0xFC) << 3) | ((0x22 & 0xF8) << 8),
                FloralWhite = (0xFF >> 3) | ((0xFA & 0xFC) << 3) | ((0xF0 & 0xF8) << 8),
                ForestGreen = (0x22 >> 3) | ((0x8B & 0xFC) << 3) | ((0x22 & 0xF8) << 8),
                Fuchsia = (0xFF >> 3) | ((0x00 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Gainsboro = (0xDC >> 3) | ((0xDC & 0xFC) << 3) | ((0xDC & 0xF8) << 8),
                GhostWhite = (0xF8 >> 3) | ((0xF8 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Gold = (0xFF >> 3) | ((0xD7 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                Goldenrod = (0xDA >> 3) | ((0xA5 & 0xFC) << 3) | ((0x20 & 0xF8) << 8),
                Gray = (0x80 >> 3) | ((0x80 & 0xFC) << 3) | ((0x80 & 0xF8) << 8),
                Green = (0x00 >> 3) | ((0x80 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                GreenYellow = (0xAD >> 3) | ((0xFF & 0xFC) << 3) | ((0x2F & 0xF8) << 8),
                Honeydew = (0xF0 >> 3) | ((0xFF & 0xFC) << 3) | ((0xF0 & 0xF8) << 8),
                HotPink = (0xFF >> 3) | ((0x69 & 0xFC) << 3) | ((0xB4 & 0xF8) << 8),
                IndianRed = (0xCD >> 3) | ((0x5C & 0xFC) << 3) | ((0x5C & 0xF8) << 8),
                Indigo = (0x4B >> 3) | ((0x00 & 0xFC) << 3) | ((0x82 & 0xF8) << 8),
                Ivory = (0xFF >> 3) | ((0xFF & 0xFC) << 3) | ((0xF0 & 0xF8) << 8),
                Khaki = (0xF0 >> 3) | ((0xE6 & 0xFC) << 3) | ((0x8C & 0xF8) << 8),
                Lavender = (0xE6 >> 3) | ((0xE6 & 0xFC) << 3) | ((0xFA & 0xF8) << 8),
                LavenderBlush = (0xFF >> 3) | ((0xF0 & 0xFC) << 3) | ((0xF5 & 0xF8) << 8),
                LawnGreen = (0x7C >> 3) | ((0xFC & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                LemonChiffon = (0xFF >> 3) | ((0xFA & 0xFC) << 3) | ((0xCD & 0xF8) << 8),
                LightBlue = (0xAD >> 3) | ((0xD8 & 0xFC) << 3) | ((0xE6 & 0xF8) << 8),
                LightCoral = (0xF0 >> 3) | ((0x80 & 0xFC) << 3) | ((0x80 & 0xF8) << 8),
                LightCyan = (0xE0 >> 3) | ((0xFF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                LightGoldenrodYellow = (0xFA >> 3) | ((0xFA & 0xFC) << 3) | ((0xD2 & 0xF8) << 8),
                LightGreen = (0x90 >> 3) | ((0xEE & 0xFC) << 3) | ((0x90 & 0xF8) << 8),
                LightGray = (0xD3 >> 3) | ((0xD3 & 0xFC) << 3) | ((0xD3 & 0xF8) << 8),
                LightPink = (0xFF >> 3) | ((0xB6 & 0xFC) << 3) | ((0xC1 & 0xF8) << 8),
                LightSalmon = (0xFF >> 3) | ((0xA0 & 0xFC) << 3) | ((0x7A & 0xF8) << 8),
                LightSeaGreen = (0x20 >> 3) | ((0xB2 & 0xFC) << 3) | ((0xAA & 0xF8) << 8),
                LightSkyBlue = (0x87 >> 3) | ((0xCE & 0xFC) << 3) | ((0xFA & 0xF8) << 8),
                LightSlateGray = (0x77 >> 3) | ((0x88 & 0xFC) << 3) | ((0x99 & 0xF8) << 8),
                LightSteelBlue = (0xB0 >> 3) | ((0xC4 & 0xFC) << 3) | ((0xDE & 0xF8) << 8),
                LightYellow = (0xFF >> 3) | ((0xFF & 0xFC) << 3) | ((0xE0 & 0xF8) << 8),
                Lime = (0x00 >> 3) | ((0xFF & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                LimeGreen = (0x32 >> 3) | ((0xCD & 0xFC) << 3) | ((0x32 & 0xF8) << 8),
                Linen = (0xFA >> 3) | ((0xF0 & 0xFC) << 3) | ((0xE6 & 0xF8) << 8),
                Magenta = (0xFF >> 3) | ((0x00 & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                Maroon = (0x80 >> 3) | ((0x00 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                MediumAquamarine = (0x66 >> 3) | ((0xCD & 0xFC) << 3) | ((0xAA & 0xF8) << 8),
                MediumBlue = (0x00 >> 3) | ((0x00 & 0xFC) << 3) | ((0xCD & 0xF8) << 8),
                MediumOrchid = (0xBA >> 3) | ((0x55 & 0xFC) << 3) | ((0xD3 & 0xF8) << 8),
                MediumPurple = (0x93 >> 3) | ((0x70 & 0xFC) << 3) | ((0xDB & 0xF8) << 8),
                MediumSeaGreen = (0x3C >> 3) | ((0xB3 & 0xFC) << 3) | ((0x71 & 0xF8) << 8),
                MediumSlateBlue = (0x7B >> 3) | ((0x68 & 0xFC) << 3) | ((0xEE & 0xF8) << 8),
                MediumSpringGreen = (0x00 >> 3) | ((0xFA & 0xFC) << 3) | ((0x9A & 0xF8) << 8),
                MediumTurquoise = (0x48 >> 3) | ((0xD1 & 0xFC) << 3) | ((0xCC & 0xF8) << 8),
                MediumVioletRed = (0xC7 >> 3) | ((0x15 & 0xFC) << 3) | ((0x85 & 0xF8) << 8),
                MidnightBlue = (0x19 >> 3) | ((0x19 & 0xFC) << 3) | ((0x70 & 0xF8) << 8),
                MintCream = (0xF5 >> 3) | ((0xFF & 0xFC) << 3) | ((0xFA & 0xF8) << 8),
                MistyRose = (0xFF >> 3) | ((0xE4 & 0xFC) << 3) | ((0xE1 & 0xF8) << 8),
                Moccasin = (0xFF >> 3) | ((0xE4 & 0xFC) << 3) | ((0xB5 & 0xF8) << 8),
                NavajoWhite = (0xFF >> 3) | ((0xDE & 0xFC) << 3) | ((0xAD & 0xF8) << 8),
                Navy = (0x00 >> 3) | ((0x00 & 0xFC) << 3) | ((0x80 & 0xF8) << 8),
                OldLace = (0xFD >> 3) | ((0xF5 & 0xFC) << 3) | ((0xE6 & 0xF8) << 8),
                Olive = (0x80 >> 3) | ((0x80 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                OliveDrab = (0x6B >> 3) | ((0x8E & 0xFC) << 3) | ((0x23 & 0xF8) << 8),
                Orange = (0xFF >> 3) | ((0xA5 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                OrangeRed = (0xFF >> 3) | ((0x45 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                Orchid = (0xDA >> 3) | ((0x70 & 0xFC) << 3) | ((0xD6 & 0xF8) << 8),
                PaleGoldenrod = (0xEE >> 3) | ((0xE8 & 0xFC) << 3) | ((0xAA & 0xF8) << 8),
                PaleGreen = (0x98 >> 3) | ((0xFB & 0xFC) << 3) | ((0x98 & 0xF8) << 8),
                PaleTurquoise = (0xAF >> 3) | ((0xEE & 0xFC) << 3) | ((0xEE & 0xF8) << 8),
                PaleVioletRed = (0xDB >> 3) | ((0x70 & 0xFC) << 3) | ((0x93 & 0xF8) << 8),
                PapayaWhip = (0xFF >> 3) | ((0xEF & 0xFC) << 3) | ((0xD5 & 0xF8) << 8),
                PeachPuff = (0xFF >> 3) | ((0xDA & 0xFC) << 3) | ((0xB9 & 0xF8) << 8),
                Peru = (0xCD >> 3) | ((0x85 & 0xFC) << 3) | ((0x3F & 0xF8) << 8),
                Pink = (0xFF >> 3) | ((0xC0 & 0xFC) << 3) | ((0xCB & 0xF8) << 8),
                Plum = (0xDD >> 3) | ((0xA0 & 0xFC) << 3) | ((0xDD & 0xF8) << 8),
                PowderBlue = (0xB0 >> 3) | ((0xE0 & 0xFC) << 3) | ((0xE6 & 0xF8) << 8),
                Purple = (0x80 >> 3) | ((0x00 & 0xFC) << 3) | ((0x80 & 0xF8) << 8),
                Red = (0xFF >> 3) | ((0x00 & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                RosyBrown = (0xBC >> 3) | ((0x8F & 0xFC) << 3) | ((0x8F & 0xF8) << 8),
                RoyalBlue = (0x41 >> 3) | ((0x69 & 0xFC) << 3) | ((0xE1 & 0xF8) << 8),
                SaddleBrown = (0x8B >> 3) | ((0x45 & 0xFC) << 3) | ((0x13 & 0xF8) << 8),
                Salmon = (0xFA >> 3) | ((0x80 & 0xFC) << 3) | ((0x72 & 0xF8) << 8),
                SandyBrown = (0xF4 >> 3) | ((0xA4 & 0xFC) << 3) | ((0x60 & 0xF8) << 8),
                SeaGreen = (0x2E >> 3) | ((0x8B & 0xFC) << 3) | ((0x57 & 0xF8) << 8),
                SeaShell = (0xFF >> 3) | ((0xF5 & 0xFC) << 3) | ((0xEE & 0xF8) << 8),
                Sienna = (0xA0 >> 3) | ((0x52 & 0xFC) << 3) | ((0x2D & 0xF8) << 8),
                Silver = (0xC0 >> 3) | ((0xC0 & 0xFC) << 3) | ((0xC0 & 0xF8) << 8),
                SkyBlue = (0x87 >> 3) | ((0xCE & 0xFC) << 3) | ((0xEB & 0xF8) << 8),
                SlateBlue = (0x6A >> 3) | ((0x5A & 0xFC) << 3) | ((0xCD & 0xF8) << 8),
                SlateGray = (0x70 >> 3) | ((0x80 & 0xFC) << 3) | ((0x90 & 0xF8) << 8),
                Snow = (0xFF >> 3) | ((0xFA & 0xFC) << 3) | ((0xFA & 0xF8) << 8),
                SpringGreen = (0x00 >> 3) | ((0xFF & 0xFC) << 3) | ((0x7F & 0xF8) << 8),
                SteelBlue = (0x46 >> 3) | ((0x82 & 0xFC) << 3) | ((0xB4 & 0xF8) << 8),
                Tan = (0xD2 >> 3) | ((0xB4 & 0xFC) << 3) | ((0x8C & 0xF8) << 8),
                Teal = (0x00 >> 3) | ((0x80 & 0xFC) << 3) | ((0x80 & 0xF8) << 8),
                Thistle = (0xD8 >> 3) | ((0xBF & 0xFC) << 3) | ((0xD8 & 0xF8) << 8),
                Tomato = (0xFF >> 3) | ((0x63 & 0xFC) << 3) | ((0x47 & 0xF8) << 8),
                Turquoise = (0x40 >> 3) | ((0xE0 & 0xFC) << 3) | ((0xD0 & 0xF8) << 8),
                Violet = (0xEE >> 3) | ((0x82 & 0xFC) << 3) | ((0xEE & 0xF8) << 8),
                Wheat = (0xF5 >> 3) | ((0xDE & 0xFC) << 3) | ((0xB3 & 0xF8) << 8),
                White = (0xFF >> 3) | ((0xFF & 0xFC) << 3) | ((0xFF & 0xF8) << 8),
                WhiteSmoke = (0xF5 >> 3) | ((0xF5 & 0xFC) << 3) | ((0xF5 & 0xF8) << 8),
                Yellow = (0xFF >> 3) | ((0xFF & 0xFC) << 3) | ((0x00 & 0xF8) << 8),
                YellowGreen = (0x9A >> 3) | ((0xCD & 0xFC) << 3) | ((0x32 & 0xF8) << 8),
            }

            /// <summary>
            /// Describes display power modes
            /// </summary>
            public enum PowerMode
            {
                Normal = 0,
                Sleep = 1,
                StandBy = 2,
                DeepStandBy = 3
            }

            /// <summary>
            /// Describes display brighness levels
            /// </summary>
            public enum DisplayMode
            {
                Normal = 0,
                Dim = 1,
                Off = 2
            }

            /// <summary>
            /// Touch controller registers
            /// </summary>
            private enum TouchRegister
            {
                X = 0xD0,
                Y = 0x90,
                Z1 = 0xB0,
                Z2 = 0xC0,
                Temp = 0x84
            }

            #endregion // Enumerated Values

            #region Events

            public delegate void TouchEventHandler(ref Point location);

            public event TouchEventHandler TouchDownEvent = delegate { };
            public event TouchEventHandler TouchMoveEvent = delegate { };
            public event TouchEventHandler TouchUpEvent = delegate { };

            #endregion // Events

            #region Constructors / Initiazation

            /// <summary>
            /// Sets up default Panda II settings
            /// </summary>
            protected FEZTouch()
            {
                // not allowed to call this constructor
                // todo: make this default to a PandaII configuration
            }

            /// <summary>
            /// Configure display only, touch screen will not be functional
            /// </summary>
            /// <param name="lcdConfig">LCD configuration settings</param>
            public FEZTouch(LCDConfiguration lcdConfig)
            {
                InitLCD(lcdConfig);
            }

            /// <summary>
            /// Configure both the display and the touch screen
            /// </summary>
            /// <param name="lcdConfig">LCD configuration settings</param>
            /// <param name="touchConfig">Touch screen configuration settings</param>
            public FEZTouch(LCDConfiguration lcdConfig, TouchConfiguration touchConfig)
            {
                InitLCD(lcdConfig);
                InitTouch(touchConfig);
            }

            #region Initialization Methods

            /// <summary>
            /// Initialize the LCD
            /// </summary>
            /// <param name="lcdConfig">LCD configuration settings</param>
            private void InitLCD(LCDConfiguration lcdConfig)
            {
                // save config values
                ParPort = new ParallelPort(lcdConfig.DataPins, lcdConfig.WritePin, lcdConfig.ReadPin);
                LCDReset = new OutputPort(lcdConfig.Reset, true);
                LCDChipSelect = new OutputPort(lcdConfig.ChipSelect, true);
                LCDRegSelect = new OutputPort(lcdConfig.RS, true);
                LCDBackLight = new OutputCompare(lcdConfig.BackLight, true, 2);
                LCDOrientation = lcdConfig.LCDOrientation;

                if (LCDOrientation == Orientation.Landscape || LCDOrientation == Orientation.LandscapeInverse)
                {
                    Width = SCREEN_HEIGHT;
                    Height = SCREEN_WIDTH;
                }

                // set initial power and display modes
                DisplyMode = DisplayMode.Normal;
                BacklightLevel = 100;

                // toggle reset pin
                LCDReset.Write(true);
                Thread.Sleep(5);
                LCDReset.Write(false);
                Thread.Sleep(5);
                LCDReset.Write(true);
                Thread.Sleep(5);

                LCDChipSelect.Write(false);

                //************* Start Initial Sequence **********//
                WriteRegister(0x01, 0x0100); // set SS and SM bit
                WriteRegister(0x02, 0x0200); // set 1 line inversion
                switch (LCDOrientation)
                {
                    case Orientation.Portrait:
                        // AM		=	0
                        // ID1-ID0	=	11
                        // ORG		=	0
                        // HWM		=	1
                        WriteRegister(0x03, 0x0230);
                        break;
                    case Orientation.PortraitInverse:
                        // AM		=	0
                        // ID1-ID0	=	00
                        // ORG		=	0
                        // HWM		=	1
                        WriteRegister(0x03, 0x0200);
                        break;
                    case Orientation.Landscape:
                        // AM		=	1
                        // ID1-ID0	=	10
                        // ORG		=	0
                        // HWM		=	1
                        WriteRegister(0x03, 0x0228);
                        break;
                    case Orientation.LandscapeInverse:
                        // AM		=	1
                        // ID1-ID0	=	01
                        // ORG		=	0
                        // HWM		=	1 
                        WriteRegister(0x03, 0x0218);
                        break;
                }
                WriteRegister(0x04, 0x0000); // Resize register
                WriteRegister(0x08, 0x0207); // set the back porch and front porch
                WriteRegister(0x09, 0x0000); // set non-display area refresh cycle ISC[3:0]
                WriteRegister(0x0A, 0x0000); // FMARK function
                WriteRegister(0x0C, 0x0000); // RGB interface setting
                WriteRegister(0x0D, 0x0000); // Frame marker Position
                WriteRegister(0x0F, 0x0000); // RGB interface polarity

                //*************Power On sequence ****************//
                WriteRegister(0x10, 0x0000); // SAP, BT[3:0], AP, DSTB, SLP, STB
                WriteRegister(0x11, 0x0007); // DC1[2:0], DC0[2:0], VC[2:0]
                WriteRegister(0x12, 0x0000); // VREG1OUT voltage
                WriteRegister(0x13, 0x0000); // VDV[4:0] for VCOM amplitude
                WriteRegister(0x07, 0x0001);
                Thread.Sleep(200);           // Dis-charge capacitor power voltage
                WriteRegister(0x10, 0x1690); // SAP, BT[3:0], AP, DSTB, SLP, STB
                WriteRegister(0x11, 0x0227); // Set DC1[2:0], DC0[2:0], VC[2:0]
                Thread.Sleep(50);            // Delay 50ms
                WriteRegister(0x12, 0x000D); // 0012
                Thread.Sleep(50);            // Delay 50ms
                WriteRegister(0x13, 0x1200); // VDV[4:0] for VCOM amplitude
                WriteRegister(0x29, 0x000A); // 04 VCM[5:0] for VCOMH
                WriteRegister(0x2B, 0x000D); // Set Frame Rate
                Thread.Sleep(50);            // delay 50ms
                WriteRegister(0x20, 0x0000); // GRAM horizontal Address
                WriteRegister(0x21, 0x0000); // GRAM Vertical Address

                // ----------- Adjust the Gamma Curve ----------//
                WriteRegister(0x30, 0x0000);
                WriteRegister(0x31, 0x0404);
                WriteRegister(0x32, 0x0003);
                WriteRegister(0x35, 0x0405);
                WriteRegister(0x36, 0x0808);
                WriteRegister(0x37, 0x0407);
                WriteRegister(0x38, 0x0303);
                WriteRegister(0x39, 0x0707);
                WriteRegister(0x3C, 0x0504);
                WriteRegister(0x3D, 0x0808);
                //------------------ Set GRAM area ---------------//
                WriteRegister(0x50, 0x0000); // Horizontal GRAM Start Address
                WriteRegister(0x51, 0x00EF); // Horizontal GRAM End Address
                WriteRegister(0x52, 0x0000); // Vertical GRAM Start Address
                WriteRegister(0x53, 0x013F); // Vertical GRAM Start Address
                WriteRegister(0x60, 0xA700); // Gate Scan Line
                WriteRegister(0x61, 0x0001); // NDL, VLE, REV
                WriteRegister(0x6A, 0x0000); // set scrolling line

                //-------------- Partial Display Control ---------//
                WriteRegister(0x80, 0x0000);
                WriteRegister(0x81, 0x0000);
                WriteRegister(0x82, 0x0000);
                WriteRegister(0x83, 0x0000);
                WriteRegister(0x84, 0x0000);
                WriteRegister(0x85, 0x0000);

                //-------------- Panel Control -------------------//
                WriteRegister(0x90, 0x0010);
                WriteRegister(0x92, 0x0000);
                WriteRegister(0x07, 0x0133); // 262K color and display ON

                LCDChipSelect.Write(true);
            }

            /// <summary>
            /// Initialize the touch screen
            /// </summary>
            /// <param name="touchConfig">Touch screen configuration settings</param>
            private void InitTouch(TouchConfiguration touchConfig)
            {
                this.SPIBus = new SPI(new SPI.Configuration(touchConfig.ChipSelect, false, 1, 1, false, true, 2000, touchConfig.Channel));

                // set up the touch controller first!
                TouchWriteBuffer[0] = 0x80;
                SPIBus.WriteRead(TouchWriteBuffer, TouchReadBuffer, 1);

                // Usde for polling IRQ
                TouchManualResetEvent = new ManualResetEvent(false);
                TerminateTouchThread = false;
                TouchThread = new Thread(TouchThreadMethod);
                TouchThread.Priority = ThreadPriority.Highest;
                TouchThread.Start();
                
                // This interrupt starts polling of touch screen
                TouchIRQ = new InterruptPort(touchConfig.TouchIRQ, false, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
                TouchIRQ.OnInterrupt += new NativeEventHandler(TouchInt);
            }



            #endregion // Initialization Methods

            #endregion // Constructors

            #region Destructors

            /// <summary>
            /// Destructor
            /// </summary>
            ~FEZTouch()
            {
                Dispose();
            }

            /// <summary>
            /// Takes care of cleaning up the mess
            /// todo: I'm not sure we really need to do this manually
            /// if we switch the touch polling over to a timer
            /// </summary>
            public void Dispose()
            {
                if (!Disposed)
                {
                    Disposed = true;

                    if (SPIBus != null)
                    {
                        TerminateTouchThread = true;
                        //TouchThread.Join();
                        TouchThread = null;
                        SPIBus.Dispose();
                        TouchIRQ.Dispose();
                    }

                    TouchDownEvent = null;
                    TouchUpEvent = null;
                    TouchMoveEvent = null;

                    ParPort.Dispose();
                    LCDReset.Dispose();
                    LCDChipSelect.Dispose();
                    LCDRegSelect.Dispose();
                    LCDBackLight.Dispose();
                }
            }

            #endregion // Destructors

            #region Properties

            /// <summary>
            /// Width of screen w.r.t. display orintation
            /// </summary>
            public int ScreenWidth
            {
                get
                {
                    return Width;
                }
            }

            /// <summary>
            /// Height of screen w.r.t. display orintation
            /// </summary>
            public int ScreenHeight
            {
                get
                {
                    return Height;
                }
            }

            #endregion // Properties

            #region Public Interface

            #region Public Drawing Methods

            /// <summary>
            /// Return Color object derived from RGB values
            /// </summary>
            /// <param name="red">Red component</param>
            /// <param name="green">Green component</param>
            /// <param name="blue">Blue component</param>
            /// <returns></returns>
            public Color ColorFromRGB(byte red, byte green, byte blue)
            {
                return (Color)((red >> 3) | ((green & 0xFC) << 3) | ((blue & 0xF8) << 8));
            }

            /// <summary>
            /// Clear screen, fill with black (default)
            /// </summary>
            public void ClearScreen()
            {
                ClearScreen(FEZ_Components.FEZTouch.Color.Black);
            }

            /// <summary>
            /// Clear screen, fill with desired color
            /// </summary>
            /// <param name="color">Color of blank screen</param>
            public void ClearScreen(Color color)
            {
                FillRectangle(new Rectangle(0, 0, Width, Height), color);
            }

            /// <summary>
            /// Set pixel to given color
            /// </summary>
            /// <param name="point">Location of pixel to set</param>
            /// <param name="color">Color of pixel</param>
            public void SetPixel(Point point, Color color)
            {
                if (point.X < 0 || point.Y < 0 || point.X >= Width || point.Y >= Height)
                {
                    throw new ArgumentException("SetPixel: Invalid parameter.");
                }

                SetPixelInternal(point, color);
            }

            /// <summary>
            /// Draw a line on the screen
            /// </summary>
            /// <param name="line">Line struct to draw</param>
            /// <param name="color">Color to draw line</param>
            public void DrawLine(Line line, Color color)
            {
                if (line.XStart < 0 || line.YStart < 0 || line.XStart >= Width || line.YStart >= Height
                    || line.XEnd < 0 || line.YEnd < 0 || line.XEnd >= Width || line.YEnd >= Height)
                {
                    throw new ArgumentException("DrawLine: Invalid line parameter");
                }

                DrawLineInternal(line, color);
            }

            /// <summary>
            /// Draw a filled rectangle
            /// </summary>
            /// <param name="rect">Rectangle to draw</param>
            /// <param name="color">Color of fill</param>
            public void FillRectangle(Rectangle rect, Color color)
            {
                // validate parameters
                if (rect.X < 0 || rect.Y < 0 || (rect.X + rect.Width) > Width || (rect.Y + rect.Height) > Height)
                {
                    throw new ArgumentException("FillRectangle: Invalid argument.");
                }

                FillRectangleInternal(rect, color);
            }

            /// <summary>
            /// Draws image, FEZ Image format, to the screen
            /// </summary>
            /// <param name="x">Top left corner of image x-coord</param>
            /// <param name="y">Top left corner of image y-coord</param>
            /// <param name="image">FEZ Touch image</param>
            public void DrawImage(int x, int y, Image image)
            {
                // validate parameters
                if (x < 0 || y < 0 || (x + image.Width) > Width || (y + image.Height) > Height)
                {
                    throw new ArgumentException("DrawImage: Invalid parameter values.");
                }

                DrawImageInternal(x, y, image);
            }

            /// <summary>
            /// Draw string to screen
            /// </summary>
            /// <param name="str">String to display</param>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            /// <param name="foreColor">Foreground (text) color</param>
            /// <param name="backColor">Background color</param>
            public void DrawString(int x, int y, string str, Color foreColor, Color backColor)
            {
                // validate parameters
                if (x < 0 || y < 0 || (x + str.Length * FONT_WIDTH) > Width || (y + FONT_HEIGHT) > Height)
                {
                    throw new ArgumentException("DrawString: Invalid argument.");
                }

                DrawStringInternal(x, y, str, foreColor, backColor);
            }

            #endregion // Public Drawing Methods

            #region Display Control

            /// <summary>
            /// Control the brightness of the screen
            /// </summary>
            /// <param name="displayMode">Brightness of screen</param>
            public void SetDisplayMode(DisplayMode displayMode)
            {
                if (this.DisplyMode != displayMode)
                {
                    // save display mode
                    this.DisplyMode = displayMode;

                    // set new display mode
                    this.LCDChipSelect.Write(false);

                    switch (this.DisplyMode)
                    {
                        case DisplayMode.Normal:
                            this.SetBackLightLevel(100);
                            this.WriteRegister(0x07, 0x0133);
                            break;
                        case DisplayMode.Dim:
                            this.SetBackLightLevel(5);
                            this.WriteRegister(0x07, 0x0133);
                            break;
                        case DisplayMode.Off:
                            this.SetBackLightLevel(0);
                            this.WriteRegister(0x07, 0x0131);
                            break;
                    }

                    this.LCDChipSelect.Write(true);
                }
            }

            /// <summary>
            /// Sets backlight value of screen
            /// </summary>
            /// <param name="backlightLevel">Backlight level</param>
            public void SetBackLightLevel(int backlightLevel)
            {
                // check for valid parameters
                if (backlightLevel < 0 || backlightLevel > 100)
                {
                    throw new ArgumentException();
                }

                if (this.BacklightLevel != backlightLevel)
                {
                    // save back light level
                    this.BacklightLevel = backlightLevel;

                    // set new back light level
                    if (this.BacklightLevel == 0)
                    {
                        this.LCDBackLight.Set(false);
                    }
                    else if (this.BacklightLevel == 100)
                    {
                        this.LCDBackLight.Set(true);
                    }
                    else
                    {
                        const int PERIOD = 10000;
                        int highTime = PERIOD * this.BacklightLevel / 100;
                        this.LCDBackLight.Set(true, new uint[2] { (uint)highTime, (uint)(PERIOD - highTime) }, 0, 2, true);
                    }
                }
            }


            //public void SetPowerMode(PowerMode powerMode)
            //{
            //    if (this.powerMode != powerMode)
            //    {
            //         save power mode
            //        this.powerMode = powerMode;

            //         set new power mode
            //        this.lcdChipSelect.Write(false);

            //        switch (this.powerMode)
            //        {
            //            case PowerMode.Normal:
            //                this.WriteRegister(0x10, 0x1690);
            //                break;
            //            case PowerMode.Sleep:
            //                this.WriteRegister(0x10, 0x1691);
            //                break;
            //            case PowerMode.StandBy:
            //                this.WriteRegister(0x10, 0x1692);
            //                break;
            //            case PowerMode.DeepStandBy:
            //                this.WriteRegister(0x10, 0x1694);
            //                break;
            //        }

            //        this.lcdChipSelect.Write(true);
            //    }
            //}


            #endregion // Display Control

            #endregion // Public Interface

            #region Private Methods

            #region Private Drawing Methods
            /// <summary>
            /// Internal method to set a single pixel
            /// </summary>
            /// <param name="point">Location of pixel</param>
            /// <param name="color">Color of pixel</param>
            private void SetPixelInternal(Point point, Color color)
            {
                LCDChipSelect.Write(false);

                DisplayBuffer[0] = (byte)((int)color >> 8);
                DisplayBuffer[1] = (byte)(color);

                Rectangle rect = TranslateRectangle(new Rectangle(point.X, point.Y, 1, 1));
                SetDrawingWindow(rect);

                SetRegister(REGISTER_WRITE_GRAM);
                ParPort.Write(DisplayBuffer, 0, 2);

                LCDChipSelect.Write(true);
            }

            /// <summary>
            /// Draw a line on the screen
            /// </summary>
            /// <param name="line">Line to draw</param>
            /// <param name="color">Color to draw line</param>
            /// We are using a Bezier line function here to plot the line
            private void DrawLineInternal(Line line, Color color)
            {
                line = TranslateLine(line);

                LCDChipSelect.Write(false);

                DisplayBuffer[0] = (byte)((int)color >> 8);
                DisplayBuffer[1] = (byte)(color);

                SetDrawingWindow(new Rectangle(0, 0, SCREEN_WIDTH - 1, SCREEN_HEIGHT - 1)); // may be able to use line boundries too

                int steps = global::System.Math.Max(global::System.Math.Abs(line.YStart - line.YEnd),
                            global::System.Math.Abs(line.XStart - line.XEnd));

                float c = (1 / (float)steps);
                float a = 1.0F; //< Bezier interpolation variable
                //float iterator;

                // calculate a point on the line, adjust iterator variable at end of loop
                while (steps > 0)
                {
                    //iterator = (1.0F - a);
                    SetPixelAddress((ushort)(line.XStart * a + line.XEnd * (1.0F - a) + 0.5),
                                    (ushort)(line.YStart * a + line.YEnd * (1.0F - a) + 0.5));

                    SetRegister(REGISTER_WRITE_GRAM);
                    ParPort.Write(DisplayBuffer, 0, 2);

                    steps--;
                    a -= c; // bezier var
                } // iteration loop

                LCDChipSelect.Write(true);
            }

            /// <summary>
            /// Draw a filled rectangle
            /// </summary>
            /// <param name="rect">Rectangle to draw</param>
            /// <param name="color">Color of fill</param>
            private void FillRectangleInternal(Rectangle rect, Color color)
            {
                rect = TranslateRectangle(rect);

                LCDChipSelect.Write(false);

                int BufLength = DisplayBuffer.Length;
                int pixelCount = rect.Width * rect.Height;
                int bufferPixels = BufLength / 2; // every pixel is 2 bytes

                byte[] colorArray = new byte[] { (byte)((int)color >> 8), (byte)color, (byte)((int)color >> 8), (byte)color,
                                            (byte)((int)color >> 8), (byte)color, (byte)((int)color >> 8), (byte)color};

                // fill buffer using ArrayCopy, this method is faster and more compact
                for (int i = 0; i < BufLength; i = i + 32)
                {
                    Array.Copy(colorArray, 0, DisplayBuffer, i, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 4, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 8, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 12, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 16, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 20, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 24, 4);
                    Array.Copy(colorArray, 0, DisplayBuffer, i + 28, 4);
                }

                SetDrawingWindow(rect);
                SetRegister(REGISTER_WRITE_GRAM);

                int loops = pixelCount / bufferPixels;

                // why do we write the whole buffer out for each loop?
                for (int i = 0; i < loops; i++)
                {
                    ParPort.Write(DisplayBuffer, 0, BufLength);
                }

                int pixelsLeft = pixelCount % bufferPixels;
                if (pixelsLeft > 0)
                {
                    // every pixel is 2 bytes
                    ParPort.Write(DisplayBuffer, 0, pixelsLeft * 2);
                }

                LCDChipSelect.Write(true);
            }

            /// <summary>
            /// Draws image, FEZ Image format, to the screen
            /// </summary>
            /// <param name="x">X-coord of top left Pixel</param>
            /// <param name="y">Y-coord of top left Pixel</param>
            /// <param name="image">FEZ Touch image</param>
            private void DrawImageInternal(int x, int y, Image image)
            {
                // We are using a Rectangle to hold the bounds of the image bounds
                // translated to the current screen orintation
                Rectangle rect = TranslateRectangle(new Rectangle(x, y, image.Width, image.Height));

                this.LCDChipSelect.Write(false);
                this.SetDrawingWindow(rect);
                this.SetRegister(REGISTER_WRITE_GRAM);
                this.ParPort.Write(image.ImageBytes, Image.IMG_PIXELS_INDEX, image.ImageBytes.Length - Image.IMG_PIXELS_INDEX);
                this.LCDChipSelect.Write(true);
            }

            /// <summary>
            /// Draw string to screen
            /// </summary>
            /// <param name="str">String to display</param>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            /// <param name="foreColor">Foreground (text) color</param>
            /// <param name="backColor">Background color</param>
            private void DrawStringInternal(int x, int y, string str, Color foreColor, Color backColor)
            {
                byte[] ColorBuffer = new byte[4] {(byte)((int)backColor >> 8), (byte)(backColor),
                                                  (byte)((int)foreColor >> 8), (byte)(foreColor)};
                int SegmentIndex = 0;
                int SegmentLength;
                Rectangle SegmentPosition;

                byte Charecter;
                int CurrentChar;
                int BufferIndex;

                while (SegmentIndex < str.Length)
                {
                    LCDChipSelect.Write(false);

                    SegmentLength = Math.Min((str.Length - SegmentIndex), CHARS_PER_BUFFER);
                    SegmentPosition = TranslateRectangle(new Rectangle(x, y, FONT_WIDTH * SegmentLength, FONT_HEIGHT));
                    SetDrawingWindow(SegmentPosition);

                    BufferIndex = 0;
                    CurrentChar = 0;
                    for (int j = 0; j < 12; j++)
                    {
                        for (CurrentChar = 0; CurrentChar < SegmentLength; CurrentChar++)
                        {
                            Charecter = font[((str[CurrentChar + SegmentIndex] - 32) * 12) + j];

                            // If bit is set color is foreground, else background
                            // (Charecter & 0x80) >> 6) == 2 if bit set, else == 0, selects ColorBuffer location
                            Array.Copy(ColorBuffer, ((Charecter & 0x80) >> 6), DisplayBuffer, BufferIndex, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x40) >> 5), DisplayBuffer, BufferIndex + 2, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x20) >> 4), DisplayBuffer, BufferIndex + 4, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x10) >> 3), DisplayBuffer, BufferIndex + 6, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x08) >> 2), DisplayBuffer, BufferIndex + 8, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x04) >> 1), DisplayBuffer, BufferIndex + 10, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x02) >> 0), DisplayBuffer, BufferIndex + 12, 2);
                            Array.Copy(ColorBuffer, ((Charecter & 0x01) << 1), DisplayBuffer, BufferIndex + 14, 2);

                            BufferIndex += 16;
                        }
                    }

                    SetRegister(REGISTER_WRITE_GRAM);
                    ParPort.Write(DisplayBuffer, 0, BufferIndex);

                    LCDChipSelect.Write(true);

                    SegmentIndex += SegmentLength;
                    x += SegmentLength * FONT_WIDTH;
                }
            }

            #endregion // Private Drawing Methods

            #region Translations

            /// <summary>
            /// Translate a point based on screen orintation
            /// </summary>
            /// <param name="original">Original Point</param>
            /// <returns>Translated Point</returns>
            private Point TranslatePoint(Point original)
            {
                Point Result;
                switch (this.LCDOrientation)
                {
                    case Orientation.Portrait:
                        Result.X = original.X;
                        Result.Y = original.Y;
                        break;
                    case Orientation.PortraitInverse:
                        Result.X = SCREEN_WIDTH - original.X - 1;
                        Result.Y = SCREEN_HEIGHT - original.Y - 1;
                        break;
                    case Orientation.Landscape:
                        Result.X = original.Y;
                        Result.Y = SCREEN_WIDTH - original.X - 1;
                        break;
                    case Orientation.LandscapeInverse:
                    default:
                        Result.X = SCREEN_HEIGHT - original.Y - 1;
                        Result.Y = original.X;
                        break;
                }

                return Result;
            }

            /// <summary>
            /// Translate line coordinates w.r.t. display orientation
            /// </summary>
            /// <param name="original">Start of line X-coord</param>
            /// <returns>Line translated to current screen orintation</returns>
            private Line TranslateLine(Line original)
            {
                Line result;

                switch (this.LCDOrientation)
                {
                    case Orientation.Portrait:
                        result.XStart = original.XStart;
                        result.YStart = original.YStart;
                        result.XEnd = original.XEnd;
                        result.YEnd = original.YEnd;
                        break;
                    case Orientation.PortraitInverse:
                        result.XStart = (SCREEN_WIDTH - 1) - original.XStart;
                        result.YStart = (SCREEN_HEIGHT - 1) - original.YStart;
                        result.XEnd = (SCREEN_WIDTH - 1) - original.XEnd;
                        result.YEnd = (SCREEN_HEIGHT - 1) - original.YEnd;
                        break;
                    case Orientation.Landscape:
                        result.XStart = (SCREEN_WIDTH - 1) - original.YStart;
                        result.YStart = original.XStart;
                        result.XEnd = (SCREEN_WIDTH - 1) - original.YEnd;
                        result.YEnd = original.XEnd;
                        break;
                    case Orientation.LandscapeInverse:
                    default:
                        result.XStart = original.YStart;
                        result.YStart = (SCREEN_HEIGHT - 1) - original.XStart;
                        result.XEnd = original.YEnd;
                        result.YEnd = (SCREEN_HEIGHT - 1) - original.XEnd;
                        break;
                }

                return result;
            }

            /// <summary>
            /// Tranlate a rectangle coordinates w.r.t. display orintation
            /// </summary>
            /// <param name="original">Original rectangle to translate</param>
            /// <returns>Rectangle translated to current screen orintation</returns>
            private Rectangle TranslateRectangle(Rectangle original)
            {
                Rectangle result;

                switch (this.LCDOrientation)
                {
                    case Orientation.Portrait:
                        result.X = original.X;
                        result.Y = original.Y;
                        result.Width = original.Width;
                        result.Height = original.Height;
                        break;
                    case Orientation.PortraitInverse:
                        result.X = SCREEN_WIDTH - original.X - original.Width;
                        result.Y = SCREEN_HEIGHT - original.Y - original.Height;
                        result.Width = original.Width;
                        result.Height = original.Height;
                        break;
                    case Orientation.Landscape:
                        result.X = SCREEN_WIDTH - original.Y - original.Height;
                        result.Y = original.X;
                        result.Width = original.Height;
                        result.Height = original.Width;
                        break;
                    case Orientation.LandscapeInverse:
                    default:
                        result.X = original.Y;
                        result.Y = SCREEN_HEIGHT - original.X - original.Width;
                        result.Width = original.Height;
                        result.Height = original.Width;
                        break;
                }

                return result;
            }

            #endregion //Translations

            #region Interface Methods

            /// <summary>
            /// Move GRAM pointer to given location
            /// </summary>
            /// <param name="x">X coordinate</param>
            /// <param name="y">Y coordinate</param>
            private void SetPixelAddress(int x, int y)
            {
                WriteRegister(0x20, (ushort)x);
                WriteRegister(0x21, (ushort)y);
            }

            /// <summary>
            /// Set fast update windows
            /// </summary>
            /// <param name="rect">Rectangular drawing area</param>
            private void SetDrawingWindow(Rectangle rect)
            {
                // pixel address, top/left of rectangle
                SetPixelAddress(rect.X, rect.Y);

                // window
                WriteRegister(0x50, (ushort)rect.X);
                WriteRegister(0x52, (ushort)rect.Y);
                WriteRegister(0x51, (ushort)(rect.X + rect.Width - 1));
                WriteRegister(0x53, (ushort)(rect.Y + rect.Height - 1));

                // for some orientations we need to moved the 'cursor'
                // to the other end of the drawing area so we start drawing at correct place
                // else we will be one row off
                if (LCDOrientation == Orientation.Landscape)
                {
                    SetPixelAddress((rect.X + rect.Width - 1), (rect.Y));
                }
                else if (LCDOrientation == Orientation.LandscapeInverse)
                {
                    SetPixelAddress((rect.X), (rect.Y + rect.Height - 1));
                }
                else if (LCDOrientation == Orientation.PortraitInverse)
                {
                    SetPixelAddress((rect.X + rect.Width - 1), (rect.Y + rect.Height - 1));
                }

            }

            /// <summary>
            /// Write value to register on display controller
            /// </summary>
            /// <param name="register">Register to update</param>
            /// <param name="value">New value of register</param>
            private void WriteRegister(byte register, ushort value)
            {
                SetRegister(register);

                RegisterBuffer[0] = (byte)(value >> 8);
                RegisterBuffer[1] = (byte)(value);

                ParPort.Write(RegisterBuffer, 0, 2);
            }

            /// <summary>
            /// Set register which will be updated on next right
            /// </summary>
            /// <param name="register">Register to set</param>
            private void SetRegister(byte register)
            {
                LCDRegSelect.Write(false);

                RegisterBuffer[0] = 0;
                RegisterBuffer[1] = register;
                ParPort.Write(RegisterBuffer, 0, 2);

                LCDRegSelect.Write(true);
            }

            #endregion // Interface Methods

            #endregion // Private Methods

            #region FONT

            /// <summary>
            /// Byte array that holds pixel representation of each charecter
            /// </summary>
            static private byte[] font = new byte[]{
          
		    0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00, /* Space */
		    0x00,0x18,0x3C,0x3C,0x3C,0x18,0x18,0x00,0x18,0x18,0x00,0x00,
            0x00,0x66,0x66,0x66,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x36,0x36,0x7F,0x36,0x36,0x36,0x7F,0x36,0x36,0x00,0x00,
            0x08,0x3E,0x6B,0x68,0x68,0x3E,0x0B,0x0B,0x6B,0x3E,0x08,0x08,
            0x70,0xD8,0xDA,0x76,0x0C,0x18,0x30,0x6E,0x5B,0x1B,0x0E,0x00,
            0x00,0x38,0x6C,0x6C,0x38,0x6C,0xC7,0xC6,0x6E,0x3B,0x00,0x00,
            0x00,0x18,0x18,0x18,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x0C,0x18,0x18,0x30,0x30,0x30,0x30,0x30,0x18,0x18,0x0C,
            0x00,0x30,0x18,0x18,0x0C,0x0C,0x0C,0x0C,0x0C,0x18,0x18,0x30,
            0x00,0x00,0x00,0x36,0x1C,0x7F,0x1C,0x36,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x18,0x18,0x7E,0x18,0x18,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x18,0x18,0x30,0x00,
            0x00,0x00,0x00,0x00,0x00,0x7E,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x18,0x18,0x00,0x00,
            0x00,0x06,0x06,0x0C,0x0C,0x18,0x18,0x30,0x30,0x60,0x60,0x00,
            0x00,0x1C,0x36,0x63,0x6B,0x6B,0x6B,0x63,0x36,0x1C,0x00,0x00,
            0x00,0x0C,0x1C,0x3C,0x0C,0x0C,0x0C,0x0C,0x0C,0x3F,0x00,0x00,
            0x00,0x3C,0x66,0x06,0x06,0x0C,0x18,0x30,0x60,0x7E,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x06,0x1C,0x06,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x06,0x0E,0x1E,0x36,0x66,0x66,0x7F,0x06,0x06,0x00,0x00,
            0x00,0x7E,0x60,0x60,0x60,0x7C,0x06,0x06,0x66,0x3C,0x00,0x00,
            0x00,0x1C,0x30,0x60,0x60,0x7C,0x66,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x7E,0x06,0x0C,0x0C,0x18,0x18,0x18,0x18,0x18,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x66,0x3C,0x66,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x66,0x3E,0x06,0x06,0x0C,0x38,0x00,0x00,
            0x00,0x00,0x00,0x18,0x18,0x00,0x00,0x00,0x18,0x18,0x00,0x00,
            0x00,0x00,0x00,0x18,0x18,0x00,0x00,0x00,0x18,0x18,0x30,0x00,
            0x00,0x06,0x0C,0x18,0x30,0x60,0x30,0x18,0x0C,0x06,0x00,0x00,
            0x00,0x00,0x00,0x00,0x7E,0x00,0x7E,0x00,0x00,0x00,0x00,0x00,
            0x00,0x60,0x30,0x18,0x0C,0x06,0x0C,0x18,0x30,0x60,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x0C,0x18,0x18,0x00,0x18,0x18,0x00,0x00,
            0x00,0x7E,0xC3,0xC3,0xCF,0xDB,0xDB,0xCF,0xC0,0x7F,0x00,0x00,
            0x00,0x18,0x3C,0x66,0x66,0x66,0x7E,0x66,0x66,0x66,0x00,0x00,
            0x00,0x7C,0x66,0x66,0x66,0x7C,0x66,0x66,0x66,0x7C,0x00,0x00,
            0x00,0x3C,0x66,0x60,0x60,0x60,0x60,0x60,0x66,0x3C,0x00,0x00,
            0x00,0x78,0x6C,0x66,0x66,0x66,0x66,0x66,0x6C,0x78,0x00,0x00,
            0x00,0x7E,0x60,0x60,0x60,0x7C,0x60,0x60,0x60,0x7E,0x00,0x00,
            0x00,0x7E,0x60,0x60,0x60,0x7C,0x60,0x60,0x60,0x60,0x00,0x00,
            0x00,0x3C,0x66,0x60,0x60,0x60,0x6E,0x66,0x66,0x3E,0x00,0x00,
            0x00,0x66,0x66,0x66,0x66,0x7E,0x66,0x66,0x66,0x66,0x00,0x00,
            0x00,0x3C,0x18,0x18,0x18,0x18,0x18,0x18,0x18,0x3C,0x00,0x00,
            0x00,0x06,0x06,0x06,0x06,0x06,0x06,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x66,0x66,0x6C,0x6C,0x78,0x6C,0x6C,0x66,0x66,0x00,0x00,
            0x00,0x60,0x60,0x60,0x60,0x60,0x60,0x60,0x60,0x7E,0x00,0x00,
            0x00,0x63,0x77,0x7F,0x6B,0x6B,0x63,0x63,0x63,0x63,0x00,0x00,
            0x00,0x63,0x63,0x73,0x7B,0x6F,0x67,0x63,0x63,0x63,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x66,0x66,0x66,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x7C,0x66,0x66,0x66,0x7C,0x60,0x60,0x60,0x60,0x00,0x00,
            0x00,0x3C,0x66,0x66,0x66,0x66,0x66,0x66,0x66,0x3C,0x0C,0x06,
            0x00,0x7C,0x66,0x66,0x66,0x7C,0x6C,0x66,0x66,0x66,0x00,0x00,
            0x00,0x3C,0x66,0x60,0x30,0x18,0x0C,0x06,0x66,0x3C,0x00,0x00,
            0x00,0x7E,0x18,0x18,0x18,0x18,0x18,0x18,0x18,0x18,0x00,0x00,
            0x00,0x66,0x66,0x66,0x66,0x66,0x66,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x66,0x66,0x66,0x66,0x66,0x66,0x66,0x3C,0x18,0x00,0x00,
            0x00,0x63,0x63,0x63,0x63,0x6B,0x6B,0x6B,0x3E,0x36,0x00,0x00,
            0x00,0x66,0x66,0x66,0x3C,0x18,0x3C,0x66,0x66,0x66,0x00,0x00,
            0x00,0x66,0x66,0x66,0x66,0x3C,0x18,0x18,0x18,0x18,0x00,0x00,
            0x00,0x7E,0x06,0x06,0x0C,0x18,0x30,0x60,0x60,0x7E,0x00,0x00,
            0x3C,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x30,0x3C,
            0x00,0x60,0x60,0x30,0x30,0x18,0x18,0x0C,0x0C,0x06,0x06,0x00,
            0x3C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x3C,
            0x18,0x3C,0x66,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0xFF,
            0x30,0x30,0x18,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            0x00,0x00,0x00,0x3C,0x06,0x06,0x3E,0x66,0x66,0x3E,0x00,0x00,
            0x00,0x60,0x60,0x7C,0x66,0x66,0x66,0x66,0x66,0x7C,0x00,0x00,
            0x00,0x00,0x00,0x3C,0x66,0x60,0x60,0x60,0x66,0x3C,0x00,0x00,
            0x00,0x06,0x06,0x3E,0x66,0x66,0x66,0x66,0x66,0x3E,0x00,0x00,
            0x00,0x00,0x00,0x3C,0x66,0x66,0x7E,0x60,0x60,0x3C,0x00,0x00,
            0x00,0x1C,0x30,0x30,0x30,0x78,0x30,0x30,0x30,0x30,0x00,0x00,
            0x00,0x00,0x00,0x3E,0x66,0x66,0x66,0x66,0x66,0x3E,0x06,0x7C,
            0x00,0x60,0x60,0x7C,0x66,0x66,0x66,0x66,0x66,0x66,0x00,0x00,
            0x18,0x18,0x00,0x78,0x18,0x18,0x18,0x18,0x18,0x7E,0x00,0x00,
            0x0C,0x0C,0x00,0x3C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x0C,0x78,
            0x00,0x60,0x60,0x66,0x66,0x6C,0x78,0x6C,0x66,0x66,0x00,0x00,
            0x00,0x78,0x18,0x18,0x18,0x18,0x18,0x18,0x18,0x7E,0x00,0x00,
            0x00,0x00,0x00,0x7E,0x6B,0x6B,0x6B,0x6B,0x6B,0x63,0x00,0x00,
            0x00,0x00,0x00,0x7C,0x66,0x66,0x66,0x66,0x66,0x66,0x00,0x00,
            0x00,0x00,0x00,0x3C,0x66,0x66,0x66,0x66,0x66,0x3C,0x00,0x00,
            0x00,0x00,0x00,0x7C,0x66,0x66,0x66,0x66,0x66,0x7C,0x60,0x60,
            0x00,0x00,0x00,0x3E,0x66,0x66,0x66,0x66,0x66,0x3E,0x06,0x06,
            0x00,0x00,0x00,0x7C,0x66,0x60,0x60,0x60,0x60,0x60,0x00,0x00,
            0x00,0x00,0x00,0x3E,0x60,0x60,0x3C,0x06,0x06,0x7C,0x00,0x00,
            0x00,0x30,0x30,0x78,0x30,0x30,0x30,0x30,0x30,0x1C,0x00,0x00,
            0x00,0x00,0x00,0x66,0x66,0x66,0x66,0x66,0x66,0x3E,0x00,0x00,
            0x00,0x00,0x00,0x66,0x66,0x66,0x66,0x66,0x3C,0x18,0x00,0x00,
            0x00,0x00,0x00,0x63,0x63,0x63,0x6B,0x6B,0x3E,0x36,0x00,0x00,
            0x00,0x00,0x00,0x66,0x66,0x3C,0x18,0x3C,0x66,0x66,0x00,0x00,
            0x00,0x00,0x00,0x66,0x66,0x66,0x66,0x66,0x66,0x3E,0x06,0x7C,
            0x00,0x00,0x00,0x7E,0x06,0x0C,0x18,0x30,0x60,0x7E,0x00,0x00,
            0x0C,0x18,0x18,0x18,0x18,0x30,0x18,0x18,0x18,0x18,0x0C,0x00,
            0x18,0x18,0x18,0x18,0x18,0x00,0x18,0x18,0x18,0x18,0x18,0x00,
            0x30,0x18,0x18,0x18,0x18,0x0C,0x18,0x18,0x18,0x18,0x30,0x00,
            0x00,0x76,0xDC,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,0x00,
            };

            #endregion // font

            #region LCD Configuration Class

            /// <summary>
            /// Holds LCD configuration settings
            /// </summary>
            public struct LCDConfiguration
            {
                /// <summary>
                /// Constructor
                /// </summary>
                /// <param name="reset">Reset line</param>
                /// <param name="chipSelect">Chip Select line</param>
                /// <param name="RS">RS line</param>
                /// <param name="lcdBackLight">LCD Backlight control line</param>
                /// <param name="dataPins">Data lines (8)</param>
                /// <param name="writePin">Write select line</param>
                /// <param name="readPin">Read select line</param>
                /// <param name="lcdOrientation">Orintation (rotation) of display</param>
                public LCDConfiguration(FEZ_Pin.Digital reset, FEZ_Pin.Digital chipSelect,
                                         FEZ_Pin.Digital RS, FEZ_Pin.Digital lcdBackLight,
                                         FEZ_Pin.Digital[] dataPins, FEZ_Pin.Digital writePin,
                                         FEZ_Pin.Digital readPin, Orientation lcdOrientation)
                {
                    DataPins = new Cpu.Pin[8];

                    for (int i = 0; i < 8; i++)
                    {
                        DataPins[i] = (Cpu.Pin)dataPins[i];
                    }

                    WritePin = (Cpu.Pin)writePin;
                    ReadPin = (Cpu.Pin)readPin;
                    ChipSelect = (Cpu.Pin)chipSelect;
                    Reset = (Cpu.Pin)reset;
                    this.RS = (Cpu.Pin)RS;
                    BackLight = (Cpu.Pin)lcdBackLight;
                    LCDOrientation = lcdOrientation;
                }

                // Properties
                public Cpu.Pin[] DataPins;
                public Cpu.Pin WritePin;
                public Cpu.Pin ReadPin;
                public Cpu.Pin ChipSelect;
                public Cpu.Pin Reset;
                public Cpu.Pin RS;
                public Cpu.Pin BackLight;
                public Orientation LCDOrientation;
            }

            #endregion

            #region Touch Configuration Class

            public class TouchConfiguration
            {
                public SPI.SPI_module Channel;
                public Cpu.Pin ChipSelect;
                public Cpu.Pin TouchIRQ;

                public TouchConfiguration(SPI.SPI_module channel, FEZ_Pin.Digital chipSelect, FEZ_Pin.Digital touchIRQ)
                {
                    this.Channel = channel;
                    this.ChipSelect = (Cpu.Pin)chipSelect;
                    this.TouchIRQ = (Cpu.Pin)touchIRQ;
                }
            }

            #endregion

            #region Image Class

            public class Image
            {
                // CONSTRUCTORS
                public Image(byte[] imgBytes)
                {
                    if (Utility.ExtractValueFromArray(imgBytes, 0, 4) != SIGNATURE)
                    {
                        throw new ArgumentException("Image Class: Signature bytes not found.");
                    }

                    int width = (int)Utility.ExtractValueFromArray(imgBytes, 4, 2);
                    int height = (int)Utility.ExtractValueFromArray(imgBytes, 6, 2);

                    if (width * height * 2 + 8 != imgBytes.Length)
                    {
                        throw new ArgumentException("Image class: Width and height do not match size of byte array.");
                    }

                    this.ImageBytes = imgBytes;
                    this.Width = width;
                    this.Height = height;
                }

                // PROPERTIES
                public readonly int Width;
                public readonly int Height;
                public const uint SIGNATURE = 0x354A82B8;
                public const int IMG_PIXELS_INDEX = 8;
                public byte[] ImageBytes;
            }

            #endregion

            #region Touch Thread & Methods

            /// <summary>
            /// Called when screen is touched, starts polling thread
            /// </summary>
            /// <param name="port">??</param>
            /// <param name="state">State pin is in</param>
            /// <param name="time">Time interrupt occured</param>
            private void TouchInt(uint port, uint state, DateTime time)
            {
                if (TerminateTouchThread == false)
                {
                    if (!TouchIRQ.Read())
                    {
                        TouchManualResetEvent.Set();
                    }
                    else
                    {
                        TouchManualResetEvent.Reset();
                    }
                }
            }

            /// <summary>
            /// Thread to poll for touch events
            /// </summary>
            private void TouchThreadMethod()
            {
                // Make sure thread is not going to be killed off
                while (TerminateTouchThread == false)
                {
                    Thread.Sleep(TOUCH_SAMPLING_TIME);

                    // If IRQ is low read in registers
                    if (!TouchIRQ.Read())
                    {
                        NewPoint.Y = ReadTouchRegister(TouchRegister.Y);
                        NewPoint.X = ReadTouchRegister(TouchRegister.X);
                    }

                    // If IRQ is still low then we have good readings, if it went
                    // high then the pen/finger was pulled up while reading registers
                    TouchStatus = !TouchIRQ.Read();
                    if (TouchStatus == true)
                    {
                        CurrentPoint = NewPoint;
                        CalibratePoint(ref CurrentPoint);

                        if (LastTouchStatus == false) // First touch
                        {
                            FireTouchDownEvent(CurrentPoint);
                            LastTouchStatus = true;
                            LastPoint = CurrentPoint;
                        }
                        else // drag across screen
                        {
                            // filter small changes
                            if (global::System.Math.Abs(CurrentPoint.X - LastPoint.X) +
                                global::System.Math.Abs(CurrentPoint.Y - LastPoint.Y) > 5)
                            {
                                FireTouchMoveEvent(CurrentPoint);
                                LastPoint = CurrentPoint;
                            }
                        }
                    }
                    else if (LastTouchStatus == true) // finger pulled up
                    {
                        FireTouchUpEvent(LastPoint);
                        LastTouchStatus = false;
                        TouchManualResetEvent.WaitOne(); // we can pause thread here
                    }
                }
            }

            /// <summary>
            /// Read in register several times and return average value
            /// </summary>
            /// <param name="reg">Register to read</param>
            /// <returns>Average of readings</returns>
            private int ReadTouchRegister(TouchRegister reg)
            {
                const int Passes = 16;
                int Current = 0;
                int Total = 0;

                TouchWriteBuffer[0] = (byte)reg;

                for (int i = 0; i < Passes; i++)
                {
                    SPIBus.WriteRead(TouchWriteBuffer, TouchReadBuffer, 1);
                    Current = TouchReadBuffer[0]; Current <<= 8;
                    Current |= TouchReadBuffer[1]; Current >>= 3;
                    Total += Current;
                }

                return Total / Passes;
            }

            /// <summary>
            /// Calibrate touch point received to screen
            /// </summary>
            /// <param name="point">Touch point</param>
            private void CalibratePoint(ref Point point)
            {
                //Microsoft.SPOT.Debug.Print("(" + point.X.ToString() + ", " + point.Y.ToString() + ")");
                if (point.X > 3750)
                {
                    point.X = 3750;
                }
                else if (point.X < 280)
                {
                    point.X = 280;
                }

                if (point.Y > 3850)
                {
                    point.Y = 3850;
                }
                else if (point.Y < 450)
                {
                    point.Y = 450;
                }

                point.X = (3750 - point.X) * (SCREEN_WIDTH - 1) / (3750 - 280);
                point.Y = (3850 - point.Y) * (SCREEN_HEIGHT - 1) / (3850 - 450);
            }

            /// <summary>
            /// Raises TouchDownEvent
            /// </summary>
            /// <param name="point">Point on screen, translated w.r.t. display orientation</param>
            private void FireTouchDownEvent(Point point)
            {
                if (TouchDownEvent != null)
                {
                    point = TranslatePoint(point);

                    // fire the event
                    TouchDownEvent(ref point);
                }
            }

            /// <summary>
            /// Fires TouchMoveEvent
            /// </summary>
            /// <param name="point">Point on screen, translated w.r.t. display orientation</param>
            private void FireTouchMoveEvent(Point point)
            {
                if (TouchMoveEvent != null)
                {
                    point = TranslatePoint(point);

                    // fire the event
                    TouchMoveEvent(ref point);
                }
            }

            /// <summary>
            /// Fires TouchUpEvent
            /// </summary>
            /// <param name="point">Point on screen, translated w.r.t. display orientation</param>
            private void FireTouchUpEvent(Point point)
            {
                if (TouchMoveEvent != null)
                {
                    point = TranslatePoint(point);

                    // fire the event
                    TouchUpEvent(ref point);
                }
            }

            #endregion

        }
    }
}
