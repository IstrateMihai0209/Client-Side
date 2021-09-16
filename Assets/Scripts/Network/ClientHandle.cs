using System.Net;
using UnityEngine;

public class ClientHandle : MonoBehaviour
{
    /// <summary>
    /// Reads data from the server
    /// </summary>
    /// <param name="_packet">The Packet that is sent</param>
    public static void Welcome(Packet _packet)
    {
        string msg = _packet.ReadString();
        int _myId = _packet.ReadInt();

        Debug.Log($"Message from server: {msg}");
        Client.instance.myId = _myId;
        ClientSend.WelcomeReceived();

        Client.instance.udp.Connect(((IPEndPoint)Client.instance.tcp.socket.Client.LocalEndPoint).Port);
    }

    public static void SpawnPlayer(Packet _packet)
    {
        int _id = _packet.ReadInt();
        string _username = _packet.ReadString();
        Vector3 _position = _packet.ReadVector3();
        Quaternion _rotation = _packet.ReadQuaternion();

        GameManager.instance.SpawnPlayer(_id, _username, _position, _rotation);
    }

    public static void PlayerDisconnected(Packet _packet)
    {
        int id = _packet.ReadInt();

        Destroy(GameManager.players[id].gameObject);
        GameManager.players.Remove(id);
    }

    public static void PlayerPositionToAll(Packet _packet)
    {
        int id = _packet.ReadInt();
        Vector3 position = _packet.ReadVector3();
        Quaternion rotation = _packet.ReadQuaternion();

        GameManager.players[id].transform.position = position;
        GameManager.players[id].transform.rotation = rotation;
    }
}