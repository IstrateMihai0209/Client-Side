using UnityEngine;

public class ClientSend : MonoBehaviour
{
    /// <summary>
    /// Sends packets to the server through TCP
    /// </summary>
    private static void SendTCPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.tcp.SendData(_packet);
    }

    /// <summary>
    /// Sends packets to the server through UDP
    /// </summary>
    private static void SendUDPData(Packet _packet)
    {
        _packet.WriteLength();
        Client.instance.udp.SendData(_packet);
    }

    #region Packets
    /// <summary>
    /// Sends the packets back to the server once the client receives the welcome method
    /// </summary>
    public static void WelcomeReceived()
    {
        //creating a new packet in the using statement and passing it to ClientPackets.welcomeReceived id
        using (Packet _packet = new Packet((int)ClientPackets.welcomeReceived))
        {
            //writing the client's id to the packet so the server can confirm that the client claimed the correct id and username
            _packet.Write(Client.instance.myId);
            _packet.Write(UIManager.instance.usernameField.text);

            SendTCPData(_packet);
        }
    }

    public static void PlayerPosition(Vector3 _position, Quaternion _rotation)
    {
        using(Packet _packet = new Packet((int)ClientPackets.playerPosition))
        {
            _packet.Write(_position);
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }

    public static void CurrentWeapon(int _weaponId)
    {
        using(Packet _packet = new Packet((int)ClientPackets.currentWeapon))
        {
            _packet.Write(_weaponId);

            SendTCPData(_packet);
        }
    }

    public static void CameraRotation(Quaternion _rotation)
    {
        using(Packet _packet = new Packet((int)ClientPackets.cameraRotation))
        {
            _packet.Write(_rotation);

            SendUDPData(_packet);
        }
    }

    #endregion
}
