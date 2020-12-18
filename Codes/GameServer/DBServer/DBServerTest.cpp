#include "DBServerTest.h"

DBServerTest::DBServerTest() : m_db("127.0.0.1", "root" ,"zktmal1#5", "cursus2")
{
    //m_db.Connect();

    //m_testUser.setUserUid(1);
    //m_testUser.setId("kasumielf");
    //m_testUser.setUsername("GyuHyon Ryu");
    //m_testUser.setUserUid(10);
    //m_testUser.setWeight(76);
    //m_testUser.setBirthday(0);

}

DBServerTest::~DBServerTest()
{
    //m_db.Close();
}

void DBServerTest::SetUp()
{
}

void DBServerTest::TearDown()
{
}


TEST_F(DBServerTest, IsSuccessInsertUserInfo)
{
    //m_db.InsertUserInfo(99, m_testUser.getId(), "zktmal1#5", m_testUser.getUsername(), m_testUser.getWeight(), m_testUser.getGender(), m_testUser.getBirthday());
    //EXPECT_EQ(true, );
}

TEST_F(DBServerTest, IsExistUserInfo)
{
    //UserUniquePtr user = m_db.SelectUserInfo("kasumielf", "zktmal1#5");
    
    //m_testUser.setUserUid(user->getUserUid());

    //EXPECT_EQ(*(user.get()), m_testUser);
}

TEST_F(DBServerTest, IsSuccessDeleteUserInfo)
{
    //EXPECT_EQ(true, m_db.DeleteUserInfo("kasumielf", "zktmal1#5"));
}

