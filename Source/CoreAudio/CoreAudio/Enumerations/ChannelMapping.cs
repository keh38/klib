namespace CoreAudio
{
    public enum ChannelMapping
    {
        SpeakerFrontLeft = 0x1,
        SpeakerFrontRight = 0x2,
        SpeakerFrontLeftRight = SpeakerFrontLeft | SpeakerFrontRight,
        SpeakerStereo = SpeakerFrontLeftRight,
        SpeakerFrontCenter = 0x4,
        SpeakerSub = 0x8,
        SpeakerCSUB = SpeakerFrontCenter | SpeakerSub,
        SpeakerBackLeft = 0x10,
        SpeakerBackRight = 0x20,
        SpeakerBackLeftRight = SpeakerBackLeft | SpeakerBackRight,
        SpeakerFrontLeftOfCenter = 0x40,
        SpeakerFrontRightOfCenter = 0x80,
        SpeakerFrontLeftRightOfCenter = SpeakerFrontLeftOfCenter | SpeakerFrontRightOfCenter,
        SpeakerBackCenter = 0x100,
        SpeakerSideLeft = 0x200,
        SpeakerSideRight = 0x400,
        SpeakerSideLeftRight = SpeakerSideLeft | SpeakerSideRight,
        SpeakerTopCenter = 0x800,
        SpeakerTopFrontLeft = 0x1000,
        SpeakerTopFrontCenter = 0x2000,
        SpeakerTopFrontRight = 0x4000,
        SpeakerTopBackLeft = 0x8000,
        SpeakerTopBackCenter = 0x10000,
        SpeakerTopBackRight = 0x20000,
        Surround7point1 = SpeakerFrontLeftRight | SpeakerCSUB | SpeakerBackLeftRight | SpeakerSideLeftRight
    }
}