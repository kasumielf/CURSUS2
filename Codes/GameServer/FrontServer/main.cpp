#include <iostream>
#include "FrontServer.h"
#include <boost/thread.hpp>


int main(int argc, char** argv)
{
    boost::asio::io_service io_service;

    FrontServer server(7002, 1000, io_service);
    
    std::vector<Channel> worlds;

    server.Init();
    server.Start();

    server.Connect("World", "127.0.0.1", 7003);
    //server.AddChannel("World No.1", "World", server.GetMyAddr(0).c_str(), 7003);
    server.AddChannel("World No.1", "World", "127.0.0.1", 7003);

    boost::thread t(boost::bind(&boost::asio::io_service::run, &io_service));

    int c = 0;

    while (c != 27)
    {
        c = std::cin.get();

        if (c == 97)
        {

        }
    }

    std::cout << "Connection end" << std::endl;

    return 0;
}