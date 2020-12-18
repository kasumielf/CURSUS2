#pragma once
#include "../GameServer/BaseServer.h"
#include "../Common/commonpacket.h"
#include <boost/asio.hpp>
#include <boost/thread.hpp>
#include <unordered_map>

struct Channel
{
    char DisplyaName[20];
    char ServerName[20];
    char Ip[15];
    short Port;
    short UserCount = 0;
    char Status;
    bool online = false;
};

class FrontServer : public BaseServer
{
private:
    std::unordered_map<short, Channel> m_channels;
    const short MAX_CHANNEL_USER_COUNT = 500;
    CommonPacket::PACKET_RES_CHANNEL_LIST channelListPacket;
    boost::thread channelUpdateThread;

public:
    FrontServer(const int port, const int maxCount, boost::asio::io_service& io_service);
    virtual void ProcessPacket(const int sessionId, char* data);

public:
    void WorldServerScanWork();
    void AddChannel(const char* display_name, const char* servername, const char* ip, short port);
    void UpdateChannelListPacket();
    void Start();
};