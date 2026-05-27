using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;
using System.Windows.Interop;
using StarStrap.UI.Elements;

namespace StarStrap.Integrations
{
    public class AIAgentWatcher : IDisposable
    {
        [DllImport("user32.dll")]
        private static extern bool RegisterHotKey(IntPtr hWnd, int id, uint fsModifiers, uint vk);

        [DllImport("user32.dll")]
        private static extern bool UnregisterHotKey(IntPtr hWnd, int id);

        private const int HOTKEY_ID = 9000;
        private bool _isDisposed = false;
        private readonly ActivityWatcher _activityWatcher;
        private HwndSource? _hwndSource;

        // Modifiers
        private const uint MOD_ALT = 0x0001;
        private const uint MOD_CONTROL = 0x0002;
        private const uint MOD_SHIFT = 0x0004;
        private const uint MOD_WIN = 0x0008;

        public AIAgentWatcher(ActivityWatcher activityWatcher)
        {
            _activityWatcher = activityWatcher;
            _activityWatcher.OnGameJoin += OnGameJoin;
            _activityWatcher.OnGameLeave += OnGameLeave;
        }

        private void OnGameJoin(object? sender, EventArgs e)
        {
            if (App.Settings.Prop.AIAgentEnabled)
            {
                RegisterGlobalHotkey();

                if (App.Settings.Prop.AIAgentLaunchWithRoblox)
                {
                    Task.Delay(5000).ContinueWith(_ => 
                    {
                        if (!_isDisposed)
                            OnHotkeyPressed();
                    });
                }
            }
        }

        private void OnGameLeave(object? sender, EventArgs e)
        {
            UnregisterGlobalHotkey();
        }

        private void RegisterGlobalHotkey()
        {
            // By default, let's use Alt+A if not configured correctly.
            uint modifiers = MOD_ALT;
            uint key = (uint)KeyInterop.VirtualKeyFromKey(Key.A);

            string configuredHotkey = App.Settings.Prop.AIAgentHotkey.ToLower();
            if (configuredHotkey.Contains("ctrl")) modifiers = MOD_CONTROL;
            if (configuredHotkey.Contains("shift")) modifiers = MOD_SHIFT;

            if (configuredHotkey.EndsWith("a")) key = (uint)KeyInterop.VirtualKeyFromKey(Key.A);
            else if (configuredHotkey.EndsWith("x")) key = (uint)KeyInterop.VirtualKeyFromKey(Key.X);
            else if (configuredHotkey.EndsWith("z")) key = (uint)KeyInterop.VirtualKeyFromKey(Key.Z);
            else if (configuredHotkey.EndsWith("c")) key = (uint)KeyInterop.VirtualKeyFromKey(Key.C);

            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_hwndSource == null)
                {
                    var parameters = new HwndSourceParameters("AIAgentHotkeyWindow")
                    {
                        Width = 1, Height = 1, WindowStyle = 0
                    };
                    _hwndSource = new HwndSource(parameters);
                    _hwndSource.AddHook(HwndHook);
                }
                RegisterHotKey(_hwndSource.Handle, HOTKEY_ID, modifiers, key);
            });
            App.Logger.WriteLine("AIAgent", "Registered AI Agent global hotkey.");
        }

        private void UnregisterGlobalHotkey()
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                if (_hwndSource != null)
                {
                    UnregisterHotKey(_hwndSource.Handle, HOTKEY_ID);
                    _hwndSource.RemoveHook(HwndHook);
                    _hwndSource.Dispose();
                    _hwndSource = null;
                }
            });
            App.Logger.WriteLine("AIAgent", "Unregistered AI Agent global hotkey.");
        }

        private IntPtr HwndHook(IntPtr hwnd, int msg, IntPtr wParam, IntPtr lParam, ref bool handled)
        {
            const int WM_HOTKEY = 0x0312;
            if (msg == WM_HOTKEY && wParam.ToInt32() == HOTKEY_ID)
            {
                OnHotkeyPressed();
                handled = true;
            }
            return IntPtr.Zero;
        }

        private async void OnHotkeyPressed()
        {
            App.Logger.WriteLine("AIAgent", "AI Agent Hotkey pressed. Taking screenshot...");
            string base64Image = CaptureScreenToBase64();
            
            App.Logger.WriteLine("AIAgent", "Getting advice from API...");
            string advice = await AIApiClient.GetAdviceAsync(
                App.Settings.Prop.AIAgentProvider,
                App.Settings.Prop.AIAgentApiKey,
                App.Settings.Prop.AIAgentSystemPrompt,
                base64Image);

            App.Logger.WriteLine("AIAgent", "Displaying advice window.");
            Application.Current.Dispatcher.Invoke(() =>
            {
                var window = new AIAgentAdviceWindow(advice);
                window.Show();
            });
        }

        private string CaptureScreenToBase64()
        {
            try
            {
                int screenWidth = (int)SystemParameters.PrimaryScreenWidth;
                int screenHeight = (int)SystemParameters.PrimaryScreenHeight;

                using (Bitmap bitmap = new Bitmap(screenWidth, screenHeight))
                {
                    using (Graphics g = Graphics.FromImage(bitmap))
                    {
                        g.CopyFromScreen(0, 0, 0, 0, bitmap.Size);
                    }

                    // Resize to avoid giant payloads
                    int newWidth = 1280;
                    int newHeight = (int)((float)screenHeight / screenWidth * newWidth);
                    using (Bitmap resized = new Bitmap(bitmap, newWidth, newHeight))
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            resized.Save(ms, ImageFormat.Jpeg);
                            byte[] byteImage = ms.ToArray();
                            return Convert.ToBase64String(byteImage);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                App.Logger.WriteLine("AIAgent", $"Screenshot error: {ex.Message}");
                return "";
            }
        }

        public void Dispose()
        {
            if (!_isDisposed)
            {
                UnregisterGlobalHotkey();
                if (_activityWatcher != null)
                {
                    _activityWatcher.OnGameJoin -= OnGameJoin;
                    _activityWatcher.OnGameLeave -= OnGameLeave;
                }
                _isDisposed = true;
            }
            GC.SuppressFinalize(this);
        }
    }
}
