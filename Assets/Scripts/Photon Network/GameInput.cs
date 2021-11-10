using Fusion;

public struct GameInput : INetworkInput
{
    #region Information

    public uint buttons;

    public const uint READY = 1 << 3;
    public const uint FORWARD = 1 << 4;
    public const uint BACKWARD = 1 << 5;
    public const uint LEFT = 1 << 6;
    public const uint RIGHT = 1 << 7;
    public const uint JUMP = 1 << 8;
   
    #endregion

    public bool IsUp(uint button)
    {
        return IsDown(button) == false;
    }

    public bool IsDown(uint button)
    {
        return (buttons & button) == button;
    }
}