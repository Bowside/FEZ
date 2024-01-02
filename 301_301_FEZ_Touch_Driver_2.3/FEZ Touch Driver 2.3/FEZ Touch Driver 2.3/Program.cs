using System;
using System.Threading;
using Microsoft.SPOT.Hardware;

using GHIElectronics.NETMF.FEZ;


namespace FEZTouchDriver_Example
{
	public class Program
	{
		#region Constants and Global Variables

		const int IDLE_TIME = 20000;

		// define global variables
		static FEZ_Components.FEZTouch fezTouch;
		static FEZ_Components.FEZTouch.Font bannerFont;

		static int bannerHeight;
		static int drawingHeight;
		static int buttonWidth;
		static int buttonHeight;

		static FEZ_Components.FEZTouch.Color[] buttonColors;
		static FEZ_Components.FEZTouch.Image fezImage;

		static int lineStartX = 0;
		static int lineStartY = 0;
		static FEZ_Components.FEZTouch.Color currentColor;

		static int idleTimer;
		static FEZ_Components.FEZTouch.DisplayMode displayMode;

		#endregion


		#region Methods

		// execute main
		public static void Main()
		{
			Microsoft.SPOT.Debug.Print(DateTime.Now.Ticks.ToString());

			// init graphics
			InitGraphics();

			// display graphics
			DisplayGraphics();

			Microsoft.SPOT.Debug.Print(DateTime.Now.Ticks.ToString());

			// attach event handlers
			fezTouch.TouchMoveEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchMoveEvent);
			fezTouch.TouchDownEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchDownEvent);
			fezTouch.TouchUpEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchUpEvent);

			// dim, and then turn off, the display if not interrupted within idleTime
			displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
			idleTimer = IDLE_TIME;
			while (true)
			{
				switch (displayMode)
				{
					case FEZ_Components.FEZTouch.DisplayMode.Normal:
						fezTouch.SetDisplayMode(displayMode);
						displayMode = FEZ_Components.FEZTouch.DisplayMode.Dim;
						break;
					case FEZ_Components.FEZTouch.DisplayMode.Dim:
						fezTouch.SetDisplayMode(displayMode);
						displayMode = FEZ_Components.FEZTouch.DisplayMode.Off;
						break;
					case FEZ_Components.FEZTouch.DisplayMode.Off:
						fezTouch.SetDisplayMode(displayMode);
						displayMode = FEZ_Components.FEZTouch.DisplayMode.Off;
						break;
				}
				Thread.Sleep(idleTimer);
			}
		}

		public static void InitGraphics()
		{
			// create lcd configuration for FEZ Panda II
			FEZ_Components.FEZTouch.LCDConfiguration lcdConfig = new FEZ_Components.FEZTouch.LCDConfiguration(
				FEZ_Pin.Digital.Di28,
				FEZ_Pin.Digital.Di20,
				FEZ_Pin.Digital.Di22,
				FEZ_Pin.Digital.Di23,
				new FEZ_Pin.Digital[8] { FEZ_Pin.Digital.Di51, FEZ_Pin.Digital.Di50, FEZ_Pin.Digital.Di49, FEZ_Pin.Digital.Di48, FEZ_Pin.Digital.Di47, FEZ_Pin.Digital.Di46, FEZ_Pin.Digital.Di45, FEZ_Pin.Digital.Di44 },
				FEZ_Pin.Digital.Di24,
				FEZ_Pin.Digital.Di26,
				FEZ_Components.FEZTouch.Orientation.PortraitInverse
				);

			// create touch configuration for FEZ Panda II
			FEZ_Components.FEZTouch.TouchConfiguration touchConfig = new FEZ_Components.FEZTouch.TouchConfiguration(SPI.SPI_module.SPI2, FEZ_Pin.Digital.Di25, FEZ_Pin.Digital.Di34);

			// create fez touch
			fezTouch = new FEZ_Components.FEZTouch(lcdConfig, touchConfig);

			// create font for text
			bannerFont = new FEZTouch.Fonts.FontCourierNew10();

			// load image from resources
			fezImage = new FEZ_Components.FEZTouch.Image(Resources.GetBytes(Resources.BinaryResources.img));

			// set colors for color buttons
			buttonColors = new FEZ_Components.FEZTouch.Color[7] { FEZ_Components.FEZTouch.Color.Black, FEZ_Components.FEZTouch.Color.Blue, FEZ_Components.FEZTouch.Color.Cyan, FEZ_Components.FEZTouch.Color.Green, FEZ_Components.FEZTouch.Color.Magneta, FEZ_Components.FEZTouch.Color.Yellow, FEZ_Components.FEZTouch.Color.Red };

			// set starting color for scribbling
			currentColor = FEZ_Components.FEZTouch.Color.Black;

			// set dimensions
			bannerHeight = bannerFont.Height * 2;
			buttonWidth = fezTouch.ScreenWidth / 8;
			buttonHeight = buttonWidth;
			drawingHeight = fezTouch.ScreenHeight - buttonHeight - bannerHeight;

			// clear the screen
			fezTouch.ClearScreen();
		}

		public static void DisplayGraphics()
		{
			int posX = 0;
			int posY = 0;

			// draw banner line 1
			string bannerLine1 = "GHI Electronics, LLC";
			posX = (fezTouch.ScreenWidth - bannerFont.GetTextWidth(bannerLine1)) / 2;
			posY = 0;
			fezTouch.DrawString(posX, posY, bannerLine1, FEZ_Components.FEZTouch.Color.Green, FEZ_Components.FEZTouch.Color.Black, bannerFont);

			// draw banner line 2
			string bannerLine2 = "FEZ Touch";
			posX = (fezTouch.ScreenWidth - bannerFont.GetTextWidth(bannerLine2)) / 2;
			posY = bannerFont.Height;
			fezTouch.DrawString(posX, posY, bannerLine2, FEZ_Components.FEZTouch.Color.Green, FEZ_Components.FEZTouch.Color.Black, bannerFont);

			// clear scribbling area
			posX = 0;
			posY = bannerHeight;
			fezTouch.FillRectangle(posX, posY, fezTouch.ScreenWidth, drawingHeight, FEZ_Components.FEZTouch.Color.White);
			
			// draw line
		//	fezTouch.DrawLine(0, bannerHeight, fezTouch.ScreenWidth-1, bannerHeight + drawingHeight - 1, FEZ_Components.FEZTouch.Color.Red);
		//	fezTouch.DrawLine(0, bannerHeight + drawingHeight, fezTouch.ScreenWidth - 1, bannerHeight, FEZ_Components.FEZTouch.Color.Black);

			// draw image
			posX = (fezTouch.ScreenWidth - fezImage.Width) / 2;
			posY = bannerHeight + ((fezTouch.ScreenHeight - buttonHeight - bannerHeight) - fezImage.Height) / 2;
			fezTouch.DrawImage(posX, posY, fezImage);

			// draw color buttons
			for (int i = 0; i < 7; i++)
			{
				posX = i * buttonWidth;
				posY = fezTouch.ScreenHeight - buttonHeight;
				fezTouch.FillRectangle(posX, posY, buttonWidth, buttonHeight, buttonColors[i]);
			}

			// draw clear button
			posX = 7 * buttonWidth;
			posY = fezTouch.ScreenHeight - buttonHeight;
			fezTouch.FillRectangle(posX, posY, buttonWidth, buttonHeight, FEZ_Components.FEZTouch.Color.Gray);
			posX = posX + (buttonWidth - bannerFont['C'].Width) / 2;
			posY = posY + (buttonHeight - bannerFont.Height) / 2;
			fezTouch.DrawString(posX, posY, "C", FEZ_Components.FEZTouch.Color.Green, FEZ_Components.FEZTouch.Color.Gray, bannerFont);
		}

		#endregion


		#region Event Handlers

		static void fezTouch_TouchDownEvent(int x, int y)
		{
			// wake up
			displayMode = FEZ_Components.FEZTouch.DisplayMode.Normal;
			fezTouch.SetDisplayMode(displayMode);

			int posX = 0;
			int posY = 0;

			// select a color or clear the scribble area
			if (y >= fezTouch.ScreenHeight - buttonHeight)
			{
				if (x >= fezTouch.ScreenWidth - buttonWidth)
				{
					// clear scribbling area
					posX = 0;
					posY = bannerHeight;
					fezTouch.FillRectangle(posX, posY, fezTouch.ScreenWidth, drawingHeight, FEZ_Components.FEZTouch.Color.White);
					
					// draw image
					posX = (fezTouch.ScreenWidth - fezImage.Width) / 2;
					posY = bannerHeight + ((fezTouch.ScreenHeight - buttonHeight - bannerHeight) - fezImage.Height) / 2;
					fezTouch.DrawImage(posX, posY, fezImage);
				}
				else
				{
					// select a color
					currentColor = buttonColors[x / buttonWidth];
				}
				return;
			}

			// save current position
			lineStartX = x;
			lineStartY = y;
		}

		static void fezTouch_TouchMoveEvent(int x, int y)
		{
			// draw?
			if (lineStartY >= bannerHeight && lineStartY <= fezTouch.ScreenHeight - buttonHeight && y >= bannerHeight && y <= fezTouch.ScreenHeight - buttonHeight)
			{
				fezTouch.DrawLine(lineStartX, lineStartY, x, y, currentColor);
			}

			lineStartX = x;
			lineStartY = y;
		}

		static void fezTouch_TouchUpEvent(int x, int y)
		{
		}

		#endregion
	}
}
