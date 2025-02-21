using System;
using System.Runtime.InteropServices;

using CoreAudio.Interfaces;

namespace CoreAudio
{
    public enum EPcxConnectionType
    {
        eConnTypeUnknown,
        eConnType3Point5mm,
        eConnTypeQuarter,
        eConnTypeAtapiInternal,
        eConnTypeRCA,
        eConnTypeOptical,
        eConnTypeOtherDigital,
        eConnTypeOtherAnalog,
        eConnTypeMultichannelAnalogDIN,
        eConnTypeXlrProfessional,
        eConnTypeRJ11Modem,
        eConnTypeCombination
    }

    public enum EPcxGeoLocation
    {
        eGeoLocRear,
        eGeoLocFront,
        eGeoLocLeft,
        eGeoLocRight,
        eGeoLocTop,
        eGeoLocBottom,
        eGeoLocRearPanel,
        eGeoLocRiser,
        eGeoLocInsideMobileLid,
        eGeoLocDrivebay,
        eGeoLocHDMI,
        eGeoLocOutsideMobileLid,
        eGeoLocATAPI,
        eGeoLocNotApplicable,
        eGeoLocReserved6,
        EPcxGeoLocation_enum_count
    }

    public enum EPcxGenLocation
    {
        eGenLocPrimaryBox,
        eGenLocInternal,
        eGenLocSeperate,
        eGenLocOther
    }
    public enum EPxcPortConnection
    {
        ePortConnJack,
        ePortConnIntegratedDevice,
        ePortConnBothIntegratedAndJack,
        ePortConnUnknown
    }
        
    public struct KSJACK_DESCRIPTION
    {
        public ChannelMapping ChannelMapping;
        public uint Color;
        public EPcxConnectionType ConnectionType;
        public EPcxGeoLocation GeoLocation;
        public EPcxGenLocation GenLocation;
        public EPxcPortConnection PortConnection;
        public bool IsConnected;
    }

    public class KsJackDescription
    {
        private readonly IKsJackDescription ksJackDescriptionInterface;

        internal KsJackDescription(IKsJackDescription ksJackDescription)
        {
            ksJackDescriptionInterface = ksJackDescription;
        }

        public uint Count
        {
            get
            {
                ksJackDescriptionInterface.GetJackCount(out var result);
                return result;
            }
        }

        public KSJACK_DESCRIPTION this[uint index]
        {
            get
            {
                byte[] result = new byte[100];
                ksJackDescriptionInterface.GetJackDescription(index, result);

                KSJACK_DESCRIPTION desc = new KSJACK_DESCRIPTION();
                IntPtr ptr = IntPtr.Zero;
                try
                {
                    ptr = Marshal.AllocHGlobal(result.Length);
                    Marshal.Copy(result, 0, ptr, result.Length);
                    desc = (KSJACK_DESCRIPTION)Marshal.PtrToStructure(ptr, typeof(KSJACK_DESCRIPTION));
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }

                return desc;
            }
        }
    }
}
