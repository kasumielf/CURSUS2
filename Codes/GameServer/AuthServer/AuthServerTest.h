#pragma once
#include <gtest/gtest.h>
#include "../Common/User.h"
#include "AuthServer.h"


class AuthServerTest : public testing::Test
{
protected:
	AuthServer m_testAuthServer;
	boost::asio::io_service io_service;

public:
	AuthServerTest();
	~AuthServerTest();

	User m_testUser;
protected:
	virtual void SetUp();
	virtual void TearDown();

};
