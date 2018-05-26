#include <iostream>

#include <boost/asio.hpp>
#include <boost/thread.hpp>

#include "AuthServer.h"
#include "AuthServerTest.h"
#include "../GameServer/BaseServer.h"

int main(int argc, char** argv)
{
	//::testing::InitGoogleTest(&argc, argv);
	//RUN_ALL_TESTS();

	boost::asio::io_service io_service;

	AuthServer server(7000, 2000, io_service);

	server.Init();
	server.Start();

	server.Connect("DB", "127.0.0.1", 7001);
	server.Connect("Front", "127.0.0.1", 7002);

	boost::thread t(boost::bind(&boost::asio::io_service::run, &io_service));

	int c = 0;

	while (c != 27)
	{
		c = std::cin.get();
	}

	std::cout << "Connection end" << std::endl;

	return 0;
}