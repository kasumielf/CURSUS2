#include "FrontServer.h"

FrontServer::FrontServer(const int port, const int maxCount, boost::asio::io_service& io_service) : BaseServer(port, maxCount, io_service)
{
	channelListPacket.Init();
}

void FrontServer::ProcessPacket(const int sessionId, char* data)
{
	PACKET_HEADER* pheader = (PACKET_HEADER*)data;

	switch (pheader->Id)
	{
		case RES_CHANNEL_INFO_S2S:
		{
			CommonPacket::PACKET_RES_CHANNEL_INFO_S2S *pPacket = (CommonPacket::PACKET_RES_CHANNEL_INFO_S2S*)data;

			short capacity = pPacket->capacity;
			char status = pPacket->status;
			
			m_channels[pPacket->channel_index].UserCount = capacity;
			m_channels[pPacket->channel_index].Status = status;

			break;
		}

		case REQ_CHANNEL_LIST:
		{
			SendData(sessionId, channelListPacket.Size, (char*)&channelListPacket);

			break;
		}

		case REQ_ENTER_CHANNEL:
		{
			CommonPacket::PACKET_REQ_ENTER_CHANNEL *pPacket = (CommonPacket::PACKET_REQ_ENTER_CHANNEL*)data;

			short id = pPacket->selected_channel_id;

			CommonPacket::PACKET_RES_ENTER_CHANNEL sendPacket;

			sendPacket.Init();

			if (m_channels[id].UserCount >= MAX_CHANNEL_USER_COUNT)
			{
				sendPacket.result = false;
			}
			else
			{
				m_channels[id].UserCount++;

				channelListPacket.channel_user_count[id] = m_channels[id].UserCount;
				memcpy(sendPacket.channel_ip, m_channels[id].Ip, strlen(m_channels[id].Ip)+1);
				sendPacket.port = m_channels[id].Port;
				sendPacket.result = true;
			}

			SendData(sessionId, sendPacket.Size, (char*)&sendPacket);
		
			break;
		}
	}
}

void FrontServer::WorldServerScanWork()
{
	while(true)
	{
		auto iter_b = m_channels.begin();
		auto iter_e = m_channels.end();

		CommonPacket::PACEKT_REQ_CHANNEL_INFO_S2S packet;

		packet.Init();
		
		int i = 0;

		for (; iter_b != iter_e; ++iter_b)
		{
			packet.channel_index = (*iter_b).first;
			SendDataToInternalServer(packet.channel_index, packet.Size, (char*)&packet);

			memcpy(channelListPacket.channel_name[i], m_channels[i].DisplyaName, strlen(m_channels[i].DisplyaName) + 1);
			channelListPacket.channel_user_count[i] = m_channels[i].UserCount;

			i++;
		}

		boost::this_thread::sleep(boost::posix_time::milliseconds(5000));
	}
}

void FrontServer::AddChannel(const char * display_name, const char * servername, const char * ip, short port)
{
	short id = GetServerSessionId(servername);

	if (id >= 0 && m_channels.size() < 2)
	{
		Channel channel;

		memcpy(channel.DisplyaName, display_name, strlen(display_name) + 1);
		memcpy(channel.ServerName, servername, strlen(servername) + 1);
		memcpy(channel.Ip, ip, strlen(ip) + 1);
		channel.Port = port;
		channel.Status = 1;
		channel.UserCount = 0;

		m_channels[id] = channel;
	}
}

void FrontServer::Start()
{
	BaseServer::Start();

	channelUpdateThread = boost::thread(boost::bind(&FrontServer::WorldServerScanWork, this));
}
