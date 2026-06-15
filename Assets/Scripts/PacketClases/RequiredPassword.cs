public class RequiredPassword : IPacketHandler
{
    public int Header => (int)PacketTypeReceive.ReceivedPassword;
    private UIManager _uIManager;

    public RequiredPassword(UIManager uim) => _uIManager = uim;
    public void Handle(Packet payload, ISession session = null)
    {
        string dto = payload.ReadString();
        _uIManager.OnSendPassword?.Invoke(dto);
    }
}