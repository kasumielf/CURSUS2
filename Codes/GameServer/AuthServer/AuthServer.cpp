#include "AuthServer.h"
#include "../Common/CommonPacket.h"
#include "../Common/User.h"

#include <boost/asio.hpp>

AuthServer::AuthServer(const int port, const int maxCount, boost::asio::io_service& io_service) : BaseServer(port, maxCount, io_service) 
{
}

void AuthServer::ProcessPacket(const int sessionId, char* data)
{
		PACKET_HEADER* pheader = (PACKET_HEADER*)data;

		switch(pheader->Id)
		{
			case REQ_CREATE_ACCOUNT:
			{
				CommonPacket::PACEKT_REQ_CREATE_ACCOUNT *pPacket = reinterpret_cast<CommonPacket::PACEKT_REQ_CREATE_ACCOUNT*>(data);
				CreateAccount(sessionId, pPacket->login_id, pPacket->password, pPacket->username, pPacket->weight, pPacket->gender, pPacket->birthday);
				break;
			}

			case RES_CREATE_ACCOUNT_S2S:
			{
				CommonPacket::PACKET_RES_CREATE_ACOUNT_S2S *pPacket = (CommonPacket::PACKET_RES_CREATE_ACOUNT_S2S*)data;
				CommonPacket::PACKET_RES_CREATE_ACCOUNT sendPacket;

				sendPacket.Init();
				sendPacket.result = pPacket->result;

				Logging("Create Account Response(%d)", pPacket->result);
				
				SendData(pPacket->session_id, sendPacket.Size, (char*)&sendPacket);
				break;
			}
			case REQ_LOGIN:
			{
				CommonPacket::PACKET_REQ_LOGIN *pPacket = (CommonPacket::PACKET_REQ_LOGIN*)data;
				LoginAccount(sessionId, pPacket->login_id, pPacket->password);

				Logging("Login Request(id : %s, pwd : %s)", pPacket->login_id, pPacket->password);

				break;
			}
			case RES_LOGIN_S2S:
			{
				CommonPacket::PACKET_RES_LOGIN_S2S *pPacket = (CommonPacket::PACKET_RES_LOGIN_S2S*)data;
				CommonPacket::PACKET_RES_LOGIN sendPacket;

				sendPacket.Init();
				sendPacket.result = pPacket->result;

				memcpy(sendPacket.lobby_ip, "127.0.0.1", 15);
				//strcpy_s(sendPacket.lobby_ip, "127.0.0.1");
				sendPacket.port = 7002;

				Logging("Login Response(session : %d, result : %d", pPacket->session_id, pPacket->result);
				SendData(pPacket->session_id, sendPacket.Size, (char*)&sendPacket);
				break;
			}
			default:
				Logging("Invalid AuthServer Request!");
				this->CloseSession(sessionId);
				break;                
		}
}


int AuthServer::CreateAccount(short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, const int& birthday)
{
	CommonPacket::PACKET_REQ_CREATE_ACCOUNT_S2S packet;

	packet.Init();
	packet.session_id = sessionId;

	memcpy(packet.login_id, id, strlen(id)+1);
	memcpy(packet.password, password, strlen(password)+1);
	memcpy(packet.username, username, strlen(username)+1);

	packet.weight = weight;
	packet.gender = gender;
	packet.birthday = birthday;

	SendDataToInternalServer("DB", packet.Size, (char*)&packet);

	return 0;
}

int AuthServer::LoginAccount(short sessionId, const char* id, const char* password)
{
	CommonPacket::PACKET_REQ_LOGIN_S2S packet;
		
	packet.Init();
	packet.session_id = sessionId;

	memcpy(packet.login_id, id, strlen(id)+1);
	memcpy(packet.password, password, strlen(password)+1);

	SendDataToInternalServer("DB", packet.Size, (char*)&packet);

	return 0;
}
