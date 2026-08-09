using Basalt.Framework.Networking;

namespace BlasII.Multiplayer.Core.Packets;

public class AnimationPacket : BasePacket, INamedPacket
{
    public string Name { get; set; }

    public int State { get; }

    public float Time { get; }

    public float Length { get; }

    public bool Request { get; }

    public AnimationPacket(string name, int state, float time, float length, bool request)
    {
        Name = name;
        State = state;
        Time = time;
        Length = length;
        Request = request;
    }
}
