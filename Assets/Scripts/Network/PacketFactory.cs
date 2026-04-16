public static class PacketFactory
{
    public static Packet SendInt(PacketTypeSend header, int value)
    {
        Packet pck = new Packet();
        pck.WriteInt((int)header);
        pck.WriteInt(value);
        pck.WriteLength();
        return pck;
    }

    public static Packet SendPacketPos(PacketTypeSend header, int roomID, string player, int index)
    {
        Packet pck = new Packet();
        pck.WriteInt((int)header);
        pck.WriteInt(roomID);
        pck.WriteString(player);
        pck.WriteInt(index);
        pck.WriteLength();
        return pck;
    }

}