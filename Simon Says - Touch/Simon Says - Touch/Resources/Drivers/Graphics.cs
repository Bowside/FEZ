using System;
using System.Threading;
using Microsoft.SPOT;
using Microsoft.SPOT.Hardware;
using GHIElectronics.NETMF.Hardware;
using GHIElectronics.NETMF.FEZ;

namespace Simon_Says
{
    class Graphics
    {
        const int IDLE_TIME = 20000;

        // define global variables
        public static FEZ_Components.FEZTouch fezTouch;
        public static FEZ_Components.FEZTouch.Font FezTouchFont;
      
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
                FEZ_Components.FEZTouch.Orientation.Landscape
                );

            // create touch configuration for FEZ Panda II
            FEZ_Components.FEZTouch.TouchConfiguration touchConfig = new FEZ_Components.FEZTouch.TouchConfiguration(SPI.SPI_module.SPI2, FEZ_Pin.Digital.Di25, FEZ_Pin.Digital.Di34);

            // create fez touch
            fezTouch = new FEZ_Components.FEZTouch(lcdConfig, touchConfig);
            
            // create font for text
            FezTouchFont = new FEZTouch.Fonts.FontCourierNew10();

            // clear the screen
            fezTouch.ClearScreen();
        }
        public abstract class Control
        {
            protected int X_Pos;
            protected int Y_Pos;
            protected int Width;
            protected int Height;
            public FEZ_Components.FEZTouch.Color BGColor;

            protected virtual void Draw()
            {
                throw new Exception("Draw() not implemented");
            }

            public virtual void Refresh()
            {
                Draw();
            }

            protected virtual Boolean IsPointInControl(int x, int y)
            {
                if ((x >= X_Pos) && (x <= X_Pos + Width) &&
                    (y >= Y_Pos) && (y <= Y_Pos + Height))
                {
                    return true;
                }
                return false;
            }
        }

        public class Label : Control
        {
            public FEZ_Components.FEZTouch.Color TextColor { get; set; }
            private String myText;
            public String Text
            {
                get { return myText; }
                set
                {
                    myText = value;
                    Draw();
                }
            }

            public Label(int x, int y, FEZ_Components.FEZTouch ft, String txt)
            {
                Init(x, y, ft, 50, 10, txt);
            }

            public Label(int x, int y, FEZ_Components.FEZTouch ft, int w, int h, String txt)
            {
                Init(x, y, ft, w, h, txt);
            }

            void Init(int x, int y, FEZ_Components.FEZTouch ft, int w, int h, String txt)
            {
                X_Pos = x;
                Y_Pos = y;
                Width = w;
                Height = h;
                fezTouch = ft;
                Text = txt;
                BGColor = FEZ_Components.FEZTouch.Color.Black;
                TextColor = FEZ_Components.FEZTouch.Color.White;

                Draw();
            }

            protected override void Draw()
            {
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);
                fezTouch.DrawString(X_Pos, Y_Pos, Text, TextColor, BGColor, FezTouchFont);
            }
        }

        public class Slider : Control
        {
            public enum eOrientation
            {
                Horizontal,
                Vertical
            }

            const int HandleSize = 20;
            int line_size;
            int value_range;

            int Size;

            public int MinValue { get; private set; }
            public int MaxValue { get; private set; }
            public eOrientation Orientation { get; private set; }

            int current_val;
            public int Value
            {
                get { return current_val; }
                set
                {
                    if (value < MinValue || value > MaxValue)
                        throw new Exception("Slider value out of range");

                    current_val = value;
                    if (Orientation == eOrientation.Horizontal)
                    {
                        current_pos = X_Pos + (int)(((float)(current_val - MinValue) / (float)value_range) * line_size) + (HandleSize / 2);
                    }
                    else
                    {
                        current_pos = (Y_Pos + (HandleSize / 2) + line_size) - (int)(((float)(current_val - MinValue) / (float)value_range) * line_size);
                    }
                    Draw();
                    DoValueChanged(value);
                }
            }

            public FEZ_Components.FEZTouch.Color LineColor;
            public FEZ_Components.FEZTouch.Color HandleColor;

            int prev_pos;
            int current_pos;

            public Slider(int x, int y, FEZ_Components.FEZTouch ft)
            {
                Init(x, y, ft, fezTouch.ScreenWidth, 0, 99, eOrientation.Horizontal);
            }

            public Slider(int x, int y, FEZ_Components.FEZTouch ft, int min, int max)
            {
                Init(x, y, ft, fezTouch.ScreenWidth, min, max, eOrientation.Horizontal);
            }

            public Slider(int x, int y, FEZ_Components.FEZTouch ft, int min, int max, eOrientation orient)
            {
                Init(x, y, ft, 150, min, max, orient);
            }

            public Slider(int x, int y, FEZ_Components.FEZTouch ft, int sz, int min, int max, eOrientation orient)
            {
                Init(x, y, ft, sz, min, max, orient);
            }

            public delegate void ValueChangedDelegate(int val);
            public event ValueChangedDelegate ValueChanged;
            internal void DoValueChanged(int val)
            {
                if (ValueChanged != null) ValueChanged(val);
            }

            void Init(int x, int y, FEZ_Components.FEZTouch ft, int sz, int min, int max, eOrientation orient)
            {
                X_Pos = x;
                Y_Pos = y;
                Size = sz;
                line_size = Size - HandleSize;
                MinValue = min;
                MaxValue = max;
                value_range = MaxValue - MinValue;
                fezTouch = ft;
                Orientation = orient;
                Width = orient == eOrientation.Horizontal ? sz : HandleSize;
                Height = orient == eOrientation.Horizontal ? HandleSize : sz;
                //Value = MinValue;
                LineColor = FEZ_Components.FEZTouch.Color.White;
                HandleColor = FEZ_Components.FEZTouch.Color.Red;
                BGColor = FEZ_Components.FEZTouch.Color.Black;

                // Set slider to minimum position
                current_val = MinValue;
                if (Orientation == eOrientation.Horizontal)
                {
                    current_pos = X_Pos + (int)(((float)(current_val - MinValue) / (float)value_range) * line_size) + (HandleSize / 2);
                    prev_pos = current_pos;
                }
                else
                {
                    current_pos = (Y_Pos + (HandleSize / 2) + line_size) - (int)(((float)(current_val - MinValue) / (float)value_range) * line_size);
                    prev_pos = current_pos;
                }

                Draw();

                fezTouch.TouchMoveEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchMoveEvent);
            }

            protected override void Draw()
            {
                if (Orientation == eOrientation.Horizontal)
                {
                    fezTouch.FillRectangle(prev_pos - (HandleSize / 2), Y_Pos, HandleSize, HandleSize, BGColor); // Clear old handle position
                    fezTouch.FillRectangle(X_Pos + (HandleSize / 2), Y_Pos + (HandleSize / 2), Size - HandleSize, 1, LineColor); // Draw line
                    fezTouch.FillRectangle(current_pos - (HandleSize / 2), Y_Pos, HandleSize, HandleSize, HandleColor);  // Draw new handle position
                    prev_pos = current_pos;         // Save for clearing, next time we draw
                }
                else
                {
                    fezTouch.FillRectangle(X_Pos, prev_pos - (HandleSize / 2), HandleSize, HandleSize, BGColor);
                    fezTouch.FillRectangle(X_Pos + (HandleSize / 2), Y_Pos + (HandleSize / 2), 1, Size - HandleSize, LineColor);
                    fezTouch.FillRectangle(X_Pos, current_pos - (HandleSize / 2), HandleSize, HandleSize, HandleColor);
                    prev_pos = current_pos;         // Save for clearing, next time we draw
                }
            }

            void fezTouch_TouchMoveEvent(int x, int y)
            {
                int pos_delta;      // The number of pixels we are from the pixel position that represents MinValue

                if (IsPointInControl(x, y))
                {
                    if (Orientation == eOrientation.Horizontal)
                    {
                        // Handle edge cases
                        if (x < (X_Pos + (HandleSize / 2)))
                            current_pos = X_Pos + (HandleSize / 2);
                        else if (x > ((X_Pos + Size) - (HandleSize / 2)))
                            current_pos = (X_Pos + Size) - (HandleSize / 2);
                        else
                            current_pos = x;

                        pos_delta = current_pos - X_Pos - (HandleSize / 2);

                        Value = MinValue + (int)((float)value_range * ((float)pos_delta / (float)line_size));
                    }
                    else
                    {
                        // Handle edge cases
                        if (y < (Y_Pos + (HandleSize / 2)))
                            current_pos = Y_Pos + (HandleSize / 2);
                        else if (y > ((Y_Pos + Size) - (HandleSize / 2)))
                            current_pos = Y_Pos + Size - (HandleSize / 2);
                        else
                            current_pos = y;

                        pos_delta = line_size - ((current_pos - Y_Pos) - (HandleSize / 2));

                        Value = MinValue + (int)((float)value_range * ((float)pos_delta / (float)line_size));
                    }
                }
            }

            //protected override Boolean IsPointInControl(int x, int y)
            //{
            //    int extra_space = 20;
            //    int y1_w_extra;
            //    int y2_w_extra;
            //    int x1_w_extra;
            //    int x2_w_extra;

            //    if (Orientation == eOrientation.Horizontal)
            //    {
            //        x1_w_extra = X_Pos - extra_space;
            //        if (x1_w_extra < 0) x1_w_extra = 0;
            //        x2_w_extra = X_Pos + Size + extra_space;
            //        if (x2_w_extra > FEZ_Components.FEZTouch.ScreenWidth) x2_w_extra = FEZ_Components.FEZTouch.ScreenWidth;

            //        if ((x >= x1_w_extra) &&
            //            (x <= x2_w_extra) &&
            //            (y >= Y_Pos) &&
            //            (y <= (Y_Pos + HandleSize)))
            //        {
            //            return true;
            //        }
            //        return false;
            //    }
            //    else
            //    {
            //        y1_w_extra = Y_Pos - extra_space;
            //        if (y1_w_extra < 0) y1_w_extra = 0;
            //        y2_w_extra = Y_Pos + Size + extra_space;
            //        if (y2_w_extra > FEZ_Components.FEZTouch.ScreenHeight) y2_w_extra = FEZ_Components.FEZTouch.ScreenHeight;

            //        if ((x >= X_Pos) &&
            //            (x <= (X_Pos + HandleSize)) &&
            //            (y >= y1_w_extra) &&
            //            (y <= y2_w_extra))
            //        {
            //            return true;
            //        }
            //        return false;
            //    }
            //}
        }

        public class LED : Control
        {
            protected int Radius;

            public FEZ_Components.FEZTouch.Color OnColor;

            public FEZ_Components.FEZTouch.Color OffColor;

            Boolean is_on;
            public Boolean IsOn
            {
                get { return is_on; }
                set { is_on = value; Draw(); }
            }

            public LED(int x, int y, FEZ_Components.FEZTouch ft)
            {
                Init(x, y, 10, ft, ft.ColorFromRGB(255, 0, 0), ft.ColorFromRGB(120, 0, 0), FEZ_Components.FEZTouch.Color.Black);
            }

            public LED(int x, int y, FEZ_Components.FEZTouch ft, FEZ_Components.FEZTouch.Color on, FEZ_Components.FEZTouch.Color off, FEZ_Components.FEZTouch.Color bg)
            {
                Init(x, y, 10, ft, on, off, bg);
            }

            public LED(int x, int y, int r, FEZ_Components.FEZTouch ft, FEZ_Components.FEZTouch.Color on, FEZ_Components.FEZTouch.Color off, FEZ_Components.FEZTouch.Color bg)
            {
                Init(x, y, r, ft, on, off, bg);
            }


            void Init(int x, int y, int r, FEZ_Components.FEZTouch ts, FEZ_Components.FEZTouch.Color on, FEZ_Components.FEZTouch.Color off, FEZ_Components.FEZTouch.Color bg)
            {
                X_Pos = x;
                Y_Pos = y;
                Radius = r;
                fezTouch = ts;
                OnColor = on;
                OffColor = off;
                BGColor = bg;
                is_on = false;

                Draw();
            }

            protected override void Draw()
            {
                // If a FillCircle() method is available, use it to create a circular LED.
                //if (IsOn)
                //    fezTouch.FillCircle(X_Pos, Y_Pos, Radius, OnColor, BGColor);
                //else
                //    fezTouch.FillCircle(X_Pos, Y_Pos, Radius, OffColor, BGColor);

                if (is_on)
                    fezTouch.FillRectangle(X_Pos, Y_Pos, (Radius * 2), (Radius * 2), OnColor);
                else
                    fezTouch.FillRectangle(X_Pos, Y_Pos, (Radius * 2), (Radius * 2), OffColor);
            }

            public void Toggle()
            {
                is_on = !is_on;
                Draw();
            }
        }

        public class TextArea : Control
        {
            public FEZ_Components.FEZTouch.Color TextColor;

            int cursor_x_pos;
            int cursor_y_pos;
            int num_chars_per_line;
            int num_lines_in_window;
            String[] text_lines;
            int top_text_line_idx;
            int curr_text_insert_idx;
            int num_lines_written;

            public TextArea(int x, int y, int width, int height, FEZ_Components.FEZTouch ft)
            {
                Init(x, y, width, height, ft, FEZ_Components.FEZTouch.Color.Gray, FEZ_Components.FEZTouch.Color.White);
            }

            public TextArea(int x, int y, int w, int h, FEZ_Components.FEZTouch ft, FEZ_Components.FEZTouch.Color bg, FEZ_Components.FEZTouch.Color fg)
            {
                Init(x, y, w, h, ft, bg, fg);
            }

            void Init(int x, int y, int w, int h, FEZ_Components.FEZTouch ft, FEZ_Components.FEZTouch.Color bg, FEZ_Components.FEZTouch.Color fg)
            {
                fezTouch = ft;
                X_Pos = x;
                Y_Pos = y;
                if ((w <= FezTouchFont.AverageWidth) || (h <= FezTouchFont.Height))
                    throw new ArgumentException("TextArea window size too small");
                Width = w;
                Height = h;
                BGColor = bg;
                TextColor = fg;
                cursor_x_pos = X_Pos + 2;
                cursor_y_pos = Y_Pos + 2;
                num_chars_per_line = (Width / FezTouchFont.AverageWidth) - 1;  // Subtract 1 to allow for "margins"
                num_lines_in_window = (Height / FezTouchFont.Height) - 1;
                text_lines = new String[num_lines_in_window];
                top_text_line_idx = 0;      // The 1st element in the text_lines array is at the top of the window
                curr_text_insert_idx = 0;
                num_lines_written = 0;

                Clear();
            }

            public void Clear()
            {
                int i;
                top_text_line_idx = 0;
                curr_text_insert_idx = 0;
                num_lines_written = 0;
                for (i = 0; i < text_lines.Length; i++)
                {
                    text_lines[i] = null;
                }
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);
            }

            public void WriteLine(String txt)
            {
                int num_lines;
                int i;
                int cur_idx = 0;
                int num_chars_to_write;
                String s;

                num_lines = txt.Length / num_chars_per_line;
                if (txt.Length % num_chars_per_line != 0) num_lines++;

                for (i = 0; i < num_lines; i++)
                {
                    if ((txt.Length - cur_idx) <= num_chars_per_line)
                        num_chars_to_write = (txt.Length - cur_idx);
                    else
                        num_chars_to_write = num_chars_per_line;

                    s = txt.Substring(cur_idx, num_chars_to_write);

                    text_lines[curr_text_insert_idx] = s;

                    if (curr_text_insert_idx == (text_lines.Length - 1))
                        curr_text_insert_idx = 0;
                    else
                        curr_text_insert_idx++;

                    cur_idx += num_chars_to_write;
                }
                num_lines_written += num_lines;

                Draw();

                if (num_lines_written >= num_lines_in_window)
                    top_text_line_idx = curr_text_insert_idx + 1;
            }

            protected override void Draw()
            {
                int i;
                int curr_line_num = 0;
                Object lock_obj = new Object();

                lock (lock_obj)
                {
                    if (num_lines_written > num_lines_in_window)
                    {
                        fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);
                    }

                    if (text_lines != null)
                    {
                        for (i = top_text_line_idx; i < text_lines.Length; i++)
                        {
                            if ((text_lines[i] != null) && (text_lines[i] != String.Empty))
                            {
                                fezTouch.DrawString(X_Pos, Y_Pos + ((i - top_text_line_idx) * FezTouchFont.Height), text_lines[i], TextColor, BGColor, FezTouchFont);
                                curr_line_num++;
                            }
                        }
                    }
                    for (i = 0; i < top_text_line_idx; i++)
                    {
                        if ((text_lines[i] != null) && (text_lines[i] != String.Empty))
                        {
                            fezTouch.DrawString(X_Pos, Y_Pos + ((i + curr_line_num) * FezTouchFont.Height), text_lines[i], TextColor, BGColor, FezTouchFont);
                        }
                    }
                }
            }
        }

        public class StateButton : Control
        {
            public enum eCurrState
            {
                state1,
                state2
            };

            public FEZ_Components.FEZTouch.Color State1Color;

            public FEZ_Components.FEZTouch.Color State2Color;

            public FEZ_Components.FEZTouch.Color Text1Color;

            public FEZ_Components.FEZTouch.Color Text2Color;

            eCurrState curr_state = eCurrState.state1;

            String state1_text;

            public String State1Text
            {
                get { return state1_text; }
                set
                {
                    int text_width = value.Length * FezTouchFont.AverageWidth;

                    if (text_width > Width)
                        throw new ArgumentException();

                    state1_text = value;
                    fezTouch.DrawString(X_Pos + (Width - text_width) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, value, Text1Color, State1Color, FezTouchFont);
                }
            }

            String state2_text;

            public String State2Text
            {
                get { return state2_text; }
                set
                {
                    int text_width = value.Length * FezTouchFont.AverageWidth;

                    if (text_width > Width)
                        throw new ArgumentException();

                    state2_text = value;
                    fezTouch.DrawString(X_Pos + (Width - text_width) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, value, Text2Color, State2Color, FezTouchFont);
                }
            }

            public delegate void State1ActiveDelegate(eCurrState new_state);

            public event State1ActiveDelegate StateChanged;

            internal void DoStateChanged(eCurrState new_state)
            {
                if (StateChanged != null) StateChanged(new_state);
            }

            public StateButton(int x, int y, FEZ_Components.FEZTouch ft, int width, int height, String t1, String t2)
            {
                Init(x, y, ft, width, height, FEZ_Components.FEZTouch.Color.Gray, FEZ_Components.FEZTouch.Color.Red, FEZ_Components.FEZTouch.Color.White, FEZ_Components.FEZTouch.Color.White, t1, t2);
            }

            public StateButton(int x, int y, FEZ_Components.FEZTouch ft, String t1, String t2)
            {
                Init(x, y, ft, 50, 50, FEZ_Components.FEZTouch.Color.Gray, FEZ_Components.FEZTouch.Color.Red, FEZ_Components.FEZTouch.Color.White, FEZ_Components.FEZTouch.Color.White, t1, t2);
            }

            void Init(int x, int y, FEZ_Components.FEZTouch ft, int w, int h,
                      FEZ_Components.FEZTouch.Color st1_c, FEZ_Components.FEZTouch.Color st2_c, FEZ_Components.FEZTouch.Color t1_c, FEZ_Components.FEZTouch.Color t2_c,
                      String text1, String text2)
            {
                fezTouch = ft;
                X_Pos = x;
                Y_Pos = y;
                Width = w;
                Height = h;
                State1Color = st1_c;
                State2Color = st2_c;
                Text1Color = t1_c;
                Text2Color = t2_c;
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, State1Color);
                state1_text = text1;
                state2_text = text2;
                fezTouch.DrawString(X_Pos + (Width - (state1_text.Length * FezTouchFont.AverageWidth)) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, state1_text, Text1Color, State1Color, FezTouchFont);

                fezTouch.TouchDownEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchDownEvent);
            }

            void fezTouch_TouchDownEvent(int x, int y)
            {
                FEZ_Components.FEZTouch.Color new_btn_color;
                FEZ_Components.FEZTouch.Color new_txt_color;
                String new_text;

                if (IsPointInControl(x, y) == true)
                {
                    if (curr_state == eCurrState.state1)
                    {
                        new_btn_color = State2Color;
                        new_txt_color = Text2Color;
                        new_text = state2_text;
                        curr_state = eCurrState.state2;
                    }
                    else
                    {
                        new_btn_color = State1Color;
                        new_txt_color = Text1Color;
                        new_text = state1_text;
                        curr_state = eCurrState.state1;
                    }
                    fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, new_btn_color);
                    fezTouch.DrawString(X_Pos + (Width - (new_text.Length * FezTouchFont.AverageWidth)) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, new_text, new_txt_color, new_btn_color, FezTouchFont);

                    DoStateChanged(curr_state);
                }
            }
        }

        public class MomentaryButton : Control
        {
            public FEZ_Components.FEZTouch.Color NotPressedColor;

            public FEZ_Components.FEZTouch.Color PressedColor;

            public FEZ_Components.FEZTouch.Color TextColor;

            String button_text;

            public String Text
            {
                get { return button_text; }
                set
                {
                    int text_width = value.Length * FezTouchFont.AverageWidth;

                    if (text_width > Width)
                        throw new ArgumentException("Label width too long");

                    button_text = value;
                    fezTouch.DrawString(X_Pos + (Width - text_width) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, value, TextColor, NotPressedColor, FezTouchFont);
                }
            }

            public delegate void ButtonPressedDelegate();

            public event ButtonPressedDelegate Pressed;

            internal void DoButtonPressed()
            {
                if (Pressed != null) Pressed();
            }

            public delegate void ButtonReleasedDelegate();

            public event ButtonReleasedDelegate Released;

            internal void DoButtonReleased()
            {
                if (Released != null) Released();
            }

            public MomentaryButton(int x, int y, FEZ_Components.FEZTouch ft, int width, int height, String txt)
            {
                Init(x, y, ft, width, height, FEZ_Components.FEZTouch.Color.Gray, FEZ_Components.FEZTouch.Color.Red, txt);
            }

            public MomentaryButton(int x, int y, FEZ_Components.FEZTouch ft, int width, int height, FEZ_Components.FEZTouch.Color pc, FEZ_Components.FEZTouch.Color rc, String txt)
            {
                Init(x, y, ft, width, height, pc, rc, txt);
            }

            public MomentaryButton(int x, int y, FEZ_Components.FEZTouch ft, String txt)
            {
                Init(x, y, ft, 50, 50, FEZ_Components.FEZTouch.Color.Gray, FEZ_Components.FEZTouch.Color.Red, txt);
            }

            void Init(int x, int y, FEZ_Components.FEZTouch ft, int w, int h, FEZ_Components.FEZTouch.Color pc, FEZ_Components.FEZTouch.Color rc, String txt)
            {
                fezTouch = ft;
                X_Pos = x;
                Y_Pos = y;
                Width = w;
                Height = h;
                PressedColor = pc;
                NotPressedColor = rc;
                TextColor = FEZ_Components.FEZTouch.Color.White;
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, NotPressedColor);
                Text = txt;
                fezTouch.DrawString(X_Pos + (Width - (Text.Length * FezTouchFont.AverageWidth)) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, Text, TextColor, NotPressedColor, FezTouchFont);

                fezTouch.TouchDownEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchDownEvent);
                fezTouch.TouchUpEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchUpEvent);
            }

            void fezTouch_TouchUpEvent(int x, int y)
            {
                if (IsPointInControl(x, y) == true)
                {
                    fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, NotPressedColor);
                    fezTouch.DrawString(X_Pos + (Width - (Text.Length * FezTouchFont.AverageWidth)) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, Text, TextColor, NotPressedColor, FezTouchFont);
                    DoButtonReleased();
                }
            }

            void fezTouch_TouchDownEvent(int x, int y)
            {

                if (IsPointInControl(x, y) == true)
                {
                    fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, PressedColor);
                    fezTouch.DrawString(X_Pos + (Width - (Text.Length * FezTouchFont.AverageWidth)) / 2, Y_Pos + (Height - FezTouchFont.Height) / 2, Text, TextColor, PressedColor, FezTouchFont);
                    DoButtonPressed();
                }
            }
        }

        public class ScrollingGraph : Control
        {
            public delegate int DataGrabberDelegate();

            public FEZ_Components.FEZTouch.Color BarColor;

            public int MinValue;

            public int MaxValue;

            Timer refresh_timer;
            int next_idx;
            int num_bars = 50;
            int bar_width;
            int[] bar_vals;

            public DataGrabberDelegate DataGrabberFunction;

            public ScrollingGraph(int x, int y, int w, int h, FEZ_Components.FEZTouch ft)
            {
                Init(x, y, w, h, ft);
            }

            void Init(int x, int y, int w, int h, FEZ_Components.FEZTouch ft)
            {
                X_Pos = x;
                Y_Pos = y;
                Width = w;
                Height = h;
                fezTouch = ft;
                MinValue = 0;
                MaxValue = 99;
                BGColor = FEZ_Components.FEZTouch.Color.Black;
                BarColor = FEZ_Components.FEZTouch.Color.White;
                refresh_timer = new Timer(new TimerCallback(Refresh), null, 1000, 1000);
                bar_width = Width / num_bars;
                bar_vals = new int[num_bars];
                next_idx = 0;
                Clear();
            }

            void Clear()
            {
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);
            }

            void Refresh(Object o)
            {
                int val;

                val = DataGrabberFunction();
                bar_vals[next_idx] = val;
                //Debug.Print("Add " + val.ToString() + " at idx " + next_idx.ToString());
                next_idx = (next_idx + 1) % num_bars;
                Draw();
            }

            protected override void Draw()
            {
                int i;
                int val;
                int cur_idx;

                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);

                for (i = 0; i < num_bars; i++)
                {
                    cur_idx = (next_idx + i) % num_bars;
                    if (bar_vals[cur_idx] == 0)
                        val = 0;
                    else
                        val = (int)((float)Height * ((float)(bar_vals[cur_idx]) / (float)MaxValue));
                    //Debug.Print(i.ToString() + ":" + val.ToString());
                    fezTouch.FillRectangle(X_Pos + (i * bar_width), (Y_Pos + (Height - val)), bar_width, val, BarColor);
                }
            }
        }

        public class CheckBox : Control
        {
            public delegate void CheckStateChangedDelegate(Boolean check);

            public event CheckStateChangedDelegate CheckStateChanged;

            internal void DoCheckStateChanged(Boolean check)
            {
                if (CheckStateChanged != null) CheckStateChanged(check);
            }

            const int box_size_px = 20;

            Boolean is_checked;

            public Boolean IsChecked
            {
                get { return is_checked; }
                set
                {
                    is_checked = value;
                    Draw();
                    DoCheckStateChanged(IsChecked);
                }
            }

            public FEZ_Components.FEZTouch.Color TextColor;

            public FEZ_Components.FEZTouch.Color CheckColor;

            public String Text;

            public CheckBox(int x, int y, FEZ_Components.FEZTouch ft, String txt)
            {
                Init(x, y, ft, txt, FEZ_Components.FEZTouch.Color.White, FEZ_Components.FEZTouch.Color.Black, FEZ_Components.FEZTouch.Color.Blue);
            }

            public CheckBox(int x, int y, FEZ_Components.FEZTouch ft, String txt, FEZ_Components.FEZTouch.Color tc, FEZ_Components.FEZTouch.Color bgc, FEZ_Components.FEZTouch.Color cc)
            {
                Init(x, y, ft, txt, tc, bgc, cc);
            }

            void Init(int x, int y, FEZ_Components.FEZTouch ft, String txt, FEZ_Components.FEZTouch.Color tc, FEZ_Components.FEZTouch.Color bgc, FEZ_Components.FEZTouch.Color cc)
            {
                X_Pos = x;
                Y_Pos = y;
                fezTouch = ft;
                Text = txt;
                Width = box_size_px + 3 + (txt.Length * FezTouchFont.AverageWidth);
                Height = box_size_px;
                BGColor = bgc;
                TextColor = tc;
                CheckColor = cc;

                // Draw background
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);

                // Draw text
                fezTouch.DrawString(X_Pos + box_size_px + 3, Y_Pos + (Height / 2) - (FezTouchFont.Height / 2), Text, TextColor, BGColor, FezTouchFont);

                // Draw frame
                fezTouch.FillRectangle(X_Pos, Y_Pos, box_size_px, box_size_px, TextColor);
                fezTouch.FillRectangle(X_Pos + 1, Y_Pos + 1, box_size_px - 2, box_size_px - 2, BGColor);

                Draw();

                fezTouch.TouchDownEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchDownEvent);
            }

            void fezTouch_TouchDownEvent(int x, int y)
            {
                if (IsPointInControl(x, y))
                {
                    is_checked = !is_checked;
                    Draw();
                    DoCheckStateChanged(is_checked);
                }
            }

            protected override void Draw()
            {
                fezTouch.FillRectangle(X_Pos + 2, Y_Pos + 2, box_size_px - 4, box_size_px - 4, IsChecked ? CheckColor : BGColor);
            }
        }

        public class RadioButton : Control
        {
            public delegate void SelectedIndexChangedDelegate(int idx);

            public event SelectedIndexChangedDelegate SelectedIndexChanged;

            internal void DoIndexChanged(int idx, String lbl)
            {
                if (SelectedIndexChanged != null) SelectedIndexChanged(idx);
                if (SelectedButtonChanged != null) SelectedButtonChanged(lbl);
            }

            public delegate void SelectedButtonChangedDelegate(String lbl);

            public event SelectedButtonChangedDelegate SelectedButtonChanged;

            const int box_width_px = 20;

            public FEZ_Components.FEZTouch.Color TextColor;

            public FEZ_Components.FEZTouch.Color ButtonColor;

            String[] button_labels;
            int selected_idx;

            public RadioButton(int x, int y, FEZ_Components.FEZTouch ft, String[] lbls)
            {
                int max_len = 0;

                foreach (String s in lbls)
                {
                    if (s.Length > max_len)
                        max_len = s.Length;
                }
                max_len += (box_width_px + 3);
                Init(x, y, max_len, (lbls.Length * (box_width_px + 1)), ft, lbls, FEZ_Components.FEZTouch.Color.White, FEZ_Components.FEZTouch.Color.Black, FEZ_Components.FEZTouch.Color.Green);
            }

            public RadioButton(int x, int y, FEZ_Components.FEZTouch ft, String[] lbls, FEZ_Components.FEZTouch.Color tc, FEZ_Components.FEZTouch.Color bgc, FEZ_Components.FEZTouch.Color bc)
            {
                int max_len = 0;

                foreach (String s in lbls)
                {
                    if ((s.Length * FezTouchFont.AverageWidth) > max_len)
                        max_len = (s.Length * FezTouchFont.AverageWidth);
                }
                max_len += (box_width_px + 3);
                Init(x, y, max_len, (lbls.Length * (box_width_px + 1)), ft, lbls, tc, bgc, bc);
            }

            void Init(int x, int y, int w, int h, FEZ_Components.FEZTouch ft, String[] lbls, FEZ_Components.FEZTouch.Color tc, FEZ_Components.FEZTouch.Color bgc, FEZ_Components.FEZTouch.Color bc)
            {
                X_Pos = x;
                Y_Pos = y;

                Width = w;
                Height = h;
                fezTouch = ft;
                button_labels = lbls;
                selected_idx = 0;
                BGColor = bgc;
                TextColor = tc;
                ButtonColor = bc;

                fezTouch.TouchDownEvent += new FEZ_Components.FEZTouch.TouchEventHandler(fezTouch_TouchDownEvent);
                //Draw Background
                fezTouch.FillRectangle(X_Pos, Y_Pos, Width, Height, BGColor);

                //Draw Text
                int i;
                int y_offset;

                for (i = 0; i < button_labels.Length; i++)
                {
                    y_offset = (i * box_width_px) + (box_width_px / 2) - (FezTouchFont.Height / 2) + i;
                    fezTouch.DrawString(X_Pos + box_width_px + 3, Y_Pos + y_offset, button_labels[i], TextColor, BGColor, FezTouchFont);
                }

                // Draw Frames
                for (i = 0; i < button_labels.Length; i++)
                {
                    fezTouch.FillRectangle(X_Pos, Y_Pos + (box_width_px * i) + 1 + i, box_width_px, box_width_px, TextColor);
                    fezTouch.FillRectangle(X_Pos + 1, (Y_Pos + (box_width_px * i)) + 2 + i, box_width_px - 2, box_width_px - 2, BGColor);
                }

                Draw();
            }

            void fezTouch_TouchDownEvent(int x, int y)
            {
                int idx;
                if (IsPointInControl(x, y))
                {
                    idx = (y - Y_Pos) / (box_width_px + 2);
                    if (idx != selected_idx)
                    {
                        selected_idx = idx;
                        Draw();
                        DoIndexChanged(selected_idx, button_labels[selected_idx]);
                    }
                }
            }

            protected override void Draw()
            {
                int i;

                for (i = 0; i < button_labels.Length; i++)
                {
                    if (i == selected_idx)
                    {
                        fezTouch.FillRectangle(X_Pos + 2, (Y_Pos + (box_width_px * i)) + 3 + i, box_width_px - 4, box_width_px - 4, ButtonColor);
                    }
                    else
                    {
                        fezTouch.FillRectangle(X_Pos + 2, (Y_Pos + (box_width_px * i)) + 3 + i, box_width_px - 4, box_width_px - 4, BGColor);
                    }
                }
            }
        }

        public class ListBox
        {
            CheckBox[] items;

            public ListBox(CheckBox[] cb_items)
            {
                items = cb_items;
            }

            public int[] SelectedIndices
            {
                get
                {
                    int i;
                    int[] tmp;
                    int cnt = 0;

                    foreach (CheckBox cb in items)
                    {
                        if (cb.IsChecked)
                            cnt++;
                    }

                    tmp = new int[cnt];
                    cnt = 0;

                    for (i = 0; i < items.Length; i++)
                    {
                        if (items[i].IsChecked)
                            tmp[cnt++] = i;
                    }

                    return tmp;
                }
            }

            public String[] SelectedValues
            {
                get
                {
                    int i;
                    String[] tmp;
                    int cnt = 0;

                    foreach (CheckBox cb in items)
                    {
                        if (cb.IsChecked)
                            cnt++;
                    }

                    tmp = new String[cnt];
                    cnt = 0;

                    for (i = 0; i < items.Length; i++)
                    {
                        if (items[i].IsChecked)
                            tmp[cnt++] = items[i].Text;
                    }

                    return tmp;
                }
            }

            public void ClearAll()
            {
                foreach (CheckBox cb in items)
                    cb.IsChecked = false;
            }

            public void SelectAll()
            {
                foreach (CheckBox cb in items)
                    cb.IsChecked = true;
            }
        }
    }
}
