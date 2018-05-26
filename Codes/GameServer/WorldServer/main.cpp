#include <iostream>
#include "WorldServer.h"
#include "../GameServer/BaseServer.h"
#include <boost/thread.hpp>

int main(int argc, char** argv)
{
	boost::asio::io_service io_service;

	WorldServer server(7003, 1000, io_service);

	server.Init();
	server.InitMapData();
	server.Start();

	server.Connect("DB", "127.0.0.1", 7001);

	boost::thread t(boost::bind(&boost::asio::io_service::run, &io_service));

	int c = 0;

	while (c != 27)
	{
		c = std::cin.get();

		switch (c)
		{
		case 'a':
			server.SetForecastToRain();
			std::cout << "Set forecast Rainy manually" << std::endl;
			break;
		case 's':
			server.SetForecastToSnow();
			std::cout << "Set forecast Snow manually" << std::endl;
			break;
		case 'd':
			server.SetForecastToSunny();
			std::cout << "Set forecast Sunny manually" << std::endl;
			break;
		case 'f':
			server.SetForecastToUpdating();
			std::cout << "Set forecast Normal manually" << std::endl;
			break;
		}
	}

	std::cout << "Connection end" << std::endl;

	return 0;
}