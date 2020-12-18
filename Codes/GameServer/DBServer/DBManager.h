#pragma once

#include <string>
#include "../GameServer/BaseServer.h"
#include "../DBConnection/DBResultMap.h"
#include "../DBConnection/DBConnection.h"
#include "../Common/User.h"
#include "../Common/Record.h"
#include "boost/format.hpp"

using namespace std;

class DBManager
{
private:
    DBConnection *conn;

public:
    DBManager(string host, string id, string pwd, string db);
    ~DBManager();

    void Connect();
    void Close();

    UserUniquePtr SelectUserInfo(const char* id);

    UserUniquePtr SelectUserInfo(const char* id, const char* password);

    bool InsertUserInfo(short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, int& birthday);
    bool UpdateUserInfo(short uuid, float x, float y, int map_id);
    bool DeleteUserInfo(const char* id, const char*password);
    
    bool SelectMyRecord(int user_uid, double &dest_record_time, double &dest_check_time);
    bool UpdateMyRecord(int user_uid, double record_time, double check_time);
    //bool SelectRankUserRecord(char** dest_username, float*& dest_record_time, float*& dest_check_time, int& item_count);
    bool SelectRankUserRecord(char dest_username[10][12], double dest_record_time[10], double dest_check_time[10], int& item_count);

    bool SelectMyReplayRecords(short uuid, unsigned char records[5][120], int &record_count, double record_time[5]);
    bool UpdateMyReplayRecords(short uuid, unsigned char record[120]);
};