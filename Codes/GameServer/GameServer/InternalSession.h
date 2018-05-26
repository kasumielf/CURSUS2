#pragma once

#include <boost/asio.hpp>
#include <boost/bind.hpp>

#include <array>
#include <deque>

#include "Session.h"

class InternalSession : public Session
{
private:
	bool m_isLogin;

public:
	InternalSession(int sessionId, boost::asio::io_service& io_service, BaseServer* server);
	~InternalSession();

	bool isConnecting() { return getSocket().is_open(); }
	bool isLogin() { return m_isLogin; }
	void LoginOk()
	{
		m_isLogin = true;
	}

	void PostConnect(std::string ip, const unsigned short port);

private:

	void handle_connect(const boost::system::error_code& err);
};

