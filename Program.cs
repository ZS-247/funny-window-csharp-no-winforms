using System;
using System.Runtime.InteropServices;
namespace fun;

// Gui window with a button using winapi directly
class Program
{
    // WM_CLOSE and WM_DESTROY constants 
    const uint WM_CLOSE = 0x0010;
    const uint WM_DESTROY = 0x0002;
    const uint WM_CREATE = 0x0001;
    const uint WM_COMMAND = 0x0111; // button click
    const uint WM_PAINT = 0x000F;
    const uint WM_QUIT = 0x0012;
    const uint WM_SIZE = 0x0005;
    // button id's 
    const int ID_BUTTON1 = 100;
    const int ID_BUTTON2 = 101;
    const int ID_BUTTON3 = 102;
    // Winapi imports 
    [DllImport("user32.dll")]
    static extern IntPtr CreateWindowEx(
        uint dwExStyle,
        string lpClassName,
        string lpWindowName,
        uint dwStyle,
        int x,
        int y,
        int nWidth,
        int nHeight,
        IntPtr hWndParent,
        IntPtr hMenu,
        IntPtr hInstance,
        IntPtr lpParam
    );
    [DllImport("user32.dll")]
    static extern IntPtr DefWindowProc(
        IntPtr hWnd,
        uint uMsg,
        IntPtr wParam,
        IntPtr lParam
    );
    public struct Message
    {
        public IntPtr hwnd;
        public uint message;
        public IntPtr wParam;
        public IntPtr lParam;
        public uint time;
        public System.Drawing.Point p;
    }
    // WNDCLASS
    [StructLayout(LayoutKind.Sequential)]
    public struct WNDCLASS
    {
        public uint style;
        public IntPtr lpfnWndProc;
        public int cbClsExtra;
        public int cbWndExtra;
        public IntPtr hInstance;
        public IntPtr hIcon;
        public IntPtr hCursor;
        public IntPtr hbrBackground;
        public IntPtr lpszMenuName;
        public string lpszClassName;
    }
    // WindowProc 
    [UnmanagedFunctionPointer(CallingConvention.StdCall)]
    public delegate IntPtr WindowProc(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam);
    [DllImport("user32.dll")]
    static extern void PostQuitMessage(int nExitCode);
    [DllImport("user32.dll")]
    static extern bool DestroyWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool GetMessage(out Message lpMsg, IntPtr hWnd, uint wMsgFilterMin, uint wMsgFilterMax);
    [DllImport("user32.dll")]
    static extern bool TranslateMessage([In] ref Message lpMsg);
    [DllImport("user32.dll")]
    static extern IntPtr DispatchMessage([In] ref Message lpmsg);
    [DllImport("user32.dll")]
    static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
    [DllImport("user32.dll")]
    static extern bool UpdateWindow(IntPtr hWnd);
    [DllImport("user32.dll")]
    static extern bool SetWindowText(IntPtr hWnd, string lpString);
    [DllImport("user32.dll")]
    static extern int MessageBox(IntPtr hWnd, string text, string caption, uint type);
    [DllImport("user32.dll")]
    static extern bool RegisterClass([In] ref WNDCLASS lpWndClass);
    [DllImport("user32.dll")]
    static extern bool UnregisterClass(string lpClassName, IntPtr hInstance);

    // message callback 
    static IntPtr WindowCallback(IntPtr hWnd, uint msg, IntPtr wParam, IntPtr lParam)
    {
        // 
        if (msg == WM_CREATE)
        {
            // // create button 
            // IntPtr button = CreateWindowEx(
            //     0,
            //     "BUTTON",
            //     "Click To see something silly",
            //     0x50000000 | 0x00000001 | 0x00000002,
            //     10,
            //     10,
            //     100,
            //     30,
            //     hWnd,
            //     IntPtr.Zero,
            //     Marshal.GetHINSTANCE(typeof(Program).Module),
            //     IntPtr.Zero
            // );

            // create three buttons 
                        return DefWindowProc(hWnd, msg, wParam, lParam);
        }
        if (msg == WM_COMMAND)
        {
            // check if button was clicked 
            if (wParam.ToInt32() == ID_BUTTON1)
            {
                MessageBox(hWnd, "You clicked the first button!", "Button", 0);
            }
            if (wParam.ToInt32() == ID_BUTTON2)
            {
                MessageBox(hWnd, "You clicked the second button!", "Button", 0);
            }
            if (wParam.ToInt32() == ID_BUTTON3)
            {
                MessageBox(hWnd, "You clicked the third button!", "Button", 0);
            }
            return DefWindowProc(hWnd, msg, wParam, lParam);
        }
        if (msg == WM_CLOSE)
        {
            Console.WriteLine("closing ..");
            DestroyWindow(hWnd);
            return IntPtr.Zero;
        }
        if (msg == WM_DESTROY)
        {
            Console.WriteLine("destroying ..");
            PostQuitMessage(0);
            return IntPtr.Zero;
        }
        return DefWindowProc(hWnd, msg, wParam, lParam);
    }

    static void Main(string[] args)
    {
        Console.WriteLine("starting up ..");
        Message msg;
        WNDCLASS wc = new WNDCLASS();
        wc.style = 0;
        wc.lpfnWndProc = Marshal.GetFunctionPointerForDelegate((WindowProc)WindowCallback);
        wc.cbClsExtra = 0;
        wc.cbWndExtra = 0;
        wc.hInstance = Marshal.GetHINSTANCE(typeof(Program).Module);
        wc.hIcon = IntPtr.Zero;
        wc.hCursor = IntPtr.Zero;
        wc.hbrBackground = IntPtr.Zero;
        wc.lpszMenuName = IntPtr.Zero;
        wc.lpszClassName = "MyWindow";
        // Register window class 
        RegisterClass(ref wc);
        // Create window 
        IntPtr hWnd = CreateWindowEx(
            0,
            "MyWindow",
            "Hello, I am a window!",
            0x10CF0000,
            100,
            100,
            500,
            500,
            IntPtr.Zero,
            IntPtr.Zero,
            Marshal.GetHINSTANCE(typeof(Program).Module),
            IntPtr.Zero
        );
        // Create buttons
        IntPtr button1 = CreateWindowEx(
            0,
            "BUTTON",
            "Click To see something silly",
            0x50000000 | 0x00000001 | 0x00000002,
            10,
            10,
            100,
            30,
            hWnd,
            (IntPtr)ID_BUTTON1,
            Marshal.GetHINSTANCE(typeof(Program).Module),
            IntPtr.Zero
        );
        IntPtr button2 = CreateWindowEx(
            0,
            "BUTTON",
            "Click To see something silly",
            0x50000000 | 0x00000001 | 0x00000002,
            10,
            50,
            100,
            30,
            hWnd,
            (IntPtr)ID_BUTTON2,
            Marshal.GetHINSTANCE(typeof(Program).Module),
            IntPtr.Zero
        );
        IntPtr button3 = CreateWindowEx(
            0,
            "BUTTON",
            "Click To see something silly",
            0x50000000 | 0x00000001 | 0x00000002,
            10,
            90,
            100,
            30,
            hWnd,
            (IntPtr)ID_BUTTON3,
            Marshal.GetHINSTANCE(typeof(Program).Module),
            IntPtr.Zero
        );
        

        // Show window 
        ShowWindow(hWnd, 1);
        UpdateWindow(hWnd);
        // Message loop 
        while (GetMessage(out msg, IntPtr.Zero, 0, 0))
        {
            TranslateMessage(ref msg);
            DispatchMessage(ref msg);
            // Check if the callback returned IntPtr.Zero 

        }

        UnregisterClass("MyWindow", Marshal.GetHINSTANCE(typeof(Program).Module));
        MessageBox(IntPtr.Zero, "See you later!", "Goodbye!", 0);
    }
}
