#include <iostream>
#include "DBServer.h"
#include "../GameServer/BaseServer.h"
#include "../DBConnection/DBConnection.h"
#include "DBServerTest.h"
#include <boost/thread.hpp>

int main(int argc, char** argv)
{
	//::testing::InitGoogleTest(&argc, argv);
	//RUN_ALL_TESTS();

	std::string db_ip(argv[1]);
	std::string db_user(argv[2]);
	std::string db_pwd(argv[3]);
	std::string db_scheme(argv[4]);
	
	boost::asio::io_service io_service;

	//DBServer server(7001, 1000, io_service, "192.168.99.100", "root" ,"zktmal1#5", "cursus2");
	DBServer server(7001, 1000, io_service, db_ip, db_user, db_pwd, db_scheme);

	server.Init();
	server.Start();

	boost::thread t(boost::bind(&boost::asio::io_service::run, &io_service));

	std::cout << "Connection end" << std::endl;
	
	int c = 0;
	
	int d = 1;

	while (c != 27)
	{
		c = std::cin.get();
	}

	return 0;
}