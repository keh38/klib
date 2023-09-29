using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KLib
{
    public class VolumeManager
    {
        //The Unit to use when getting and setting the volume
        public enum VolumeUnit
        {
            //Perform volume action in decibels</param>
            Decibel,
            //Perform volume action in scalar
            Scalar
        }

        public VolumeManager()
        {
            Initialize();
        }

        [DllImport("SystemVolumePlugin")]
        public static extern float GetVolume();

        [DllImport("SystemVolumePlugin")]
        public static extern float GetVolumeDB();

        [DllImport("SystemVolumePlugin")]
        public static extern int SetVolume(float volume);

        [DllImport("SystemVolumePlugin")]
        public static extern int SetVolumeDB(float volume);

        [DllImport("SystemVolumePlugin")]
        public static extern int InitializeVolume();

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        public delegate void LogDelegate(string str);

        [DllImport("SystemVolumePlugin")]
        public static extern void SetLoggingCallback(System.IntPtr func);

        [DllImport("SystemVolumePlugin")]
        [return: MarshalAs(UnmanagedType.BStr)]
        public static extern string GetFriendlyName();

        public float GetMasterVolume(VolumeUnit vUnit)
        {
            if (vUnit == VolumeUnit.Scalar)
            {
                return GetVolume();
            }
            return GetVolumeDB();
            //        return GetSystemVolume(vUnit);
        }
        public void SetMasterVolume(float volume, VolumeUnit vUnit)
        {
            if (vUnit == VolumeUnit.Scalar)
            {
                SetVolume(volume);
            }
            else
            {
                SetVolumeDB(volume);
            }

            //        SetSystemVolume(volume, vUnit);
        }
        public string FriendlyName { get { return GetFriendlyName(); } }

        private void Initialize()
        {
            InitializeVolume();
        }

    }
}
