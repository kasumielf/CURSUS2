#pragma once

#include <gtest/gtest.h>
#include "DBManager.h"

class DBServerTest : public testing::Test
{
protected:
    DBManager m_db;
    User m_testUser;
public:
    DBServerTest();
    ~DBServerTest();

protected:
    virtual void SetUp();
    virtual void TearDown();
    
};

