using System;

namespace KLib
{
    public static class Brightness
    {
        public static void Set(IntPtr windowHandle, int brightness)
        {
            try
            {
                WindowsSettingsBrightnessController.SetBrightness(brightness);
            }
            catch (Exception ex)
            {
                using (var bc = new BrightnessController(windowHandle))
                {
                    bc.SetBrightness(brightness);
                }

            }

        }
    }
}