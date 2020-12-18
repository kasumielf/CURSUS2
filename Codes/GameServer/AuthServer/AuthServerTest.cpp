#include "AuthServerTest.h"

AuthServerTest::AuthServerTest() : m_testAuthServer(7000, 1000, io_service)
{
}


AuthServerTest::~AuthServerTest()
{
}

void AuthServerTest::SetUp()
{
    m_testUser.setId("kasumielf");
    m_testUser.setUsername("GyuHyon Ryu");
    m_testUser.setUserUid(10);
    m_testUser.setWeight(76);


    m_testAuthServer.Init();
    m_testAuthServer.Start();

    m_testAuthServer.Connect("DB", "127.0.0.1", 7001);
    m_testAuthServer.Connect("Front", "127.0.0.1", 7002);
}

void AuthServerTest::TearDown()
{
}

TEST_F(AuthServerTest, IsSuccessToCreateAccount)
{
    //EXPECT_EQ(true, m_testAuthServer.CreateAccount(99, m_testUser.getId(), m_testUser., "zktmal1#5"));
}

TEST_F(AuthServerTest, IsSuccessToLoginAccount)
{
    int result = m_testAuthServer.LoginAccount(9, m_testUser.getId(), "zktmal1#5");
    EXPECT_EQ(1, result);
}
