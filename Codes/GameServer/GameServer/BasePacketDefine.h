#pragma once
#pragma pack(push, 1)
#include <string>

const int MaxBufferSize = 100000;

struct PACKET_HEADER
{
    short Id;
    short Size;
};

const short DUMMY_PACKET = 1;

const short REQ_TEST = 90;
const short RES_TEST = 91;
const short REQ_S2S = 40;
const short RES_S2S = 41;

struct PACKET_DUMMY : public PACKET_HEADER
{
    void Init()
    {
        Id = DUMMY_PACKET;
        Size = sizeof(PACKET_DUMMY);
        data = -1;
    }
    int data;
};


struct PACKET_REQ_TEST : public PACKET_HEADER
{
    void Init()
    {
        Id = REQ_TEST;
        Size = sizeof(PACKET_REQ_TEST);
    }

    std::string message;
};

struct PACKET_RES_TEST : public PACKET_HEADER
{
    void Init()
    {
        Id = RES_TEST;
        Size = sizeof(PACKET_RES_TEST);
        result = 100;
    }

    int result;
};

struct PACKET_REQ_S2S : public PACKET_HEADER
{
    void Init()
    {
        Id = REQ_S2S;
        Size = sizeof(PACKET_REQ_S2S);
    }

    std::string message;

};

struct PACKET_RES_S2S : public PACKET_HEADER
{
    void Init()
    {
        Id = RES_S2S;
        Size = sizeof(PACKET_RES_S2S);
        message = "DONE!";
    }

    std::string message;
};

#pragma pack(pop)