#pragma once

#include <boost/asio.hpp>
#include <boost/bind.hpp>
#include <array>
#include <vector>
#include <deque>
#include <iostream>
#include <unordered_map>

#include "Session.h"
#include "InternalSession.h"

class BaseServer
{
protected:
	typedef struct InternalSendData
	{
		std::string name;
		int size;
		char* data;
	}InternalSendData;

	std::unordered_map<int, Session*> m_sessionList;
	std::unordered_map<int, InternalSession*> m_internalSessionList;
	std::unordered_map<std::string, short> m_internalSessionId;
	std::deque<InternalSendData> m_internalSessionSendQueue;

	std::deque<int> m_sessionIdQueue;
	std::vector<std::string> m_addrs;

	boost::asio::ip::tcp::acceptor m_acceptor;
	int m_maxSessionIdCount;
	int m_maxSessionCount;
	bool m_isAccepting;
	
	boost::mutex lock;
	

public:
	BaseServer(const unsigned int port, const unsigned int maxSessionCount, boost::asio::io_service& io_service);
	~BaseServer();

	void Init();
	void Start();

	void CloseSession(const int sessionId);
	void RemoveSession(const int sessionId);
	void SendData(const int sessionId, int size, char* data);
	void SendDataToInternalServer(const std::string name, const int size, char* data);
	void SendDataToInternalServer(const short internal_id, const int size, char* data);

	void Connect(const std::string name, const std::string ip, const unsigned short port);

	virtual void ProcessPacket(const int sessionId, char* packet) = 0;
	void Logging(const char* message, ...);
	void Logging(const boost::system::error_code &ec);

	void SendBoradCast(char* data, const int size);
	bool PostAccept();

	std::string GetMyAddr(int index) { return m_addrs[index]; }

protected:
	int GetServerSessionId(std::string name) { return m_internalSessionId[name]; }

private:
	void handle_accept(Session* session, const boost::system::error_code& error);
	void InternalSendQueueWorkerThread();
};

