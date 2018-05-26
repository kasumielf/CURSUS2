#include <string>
#include <boost/lexical_cast.hpp>
#include <boost/algorithm/string.hpp>
#include <chrono>
#include "DBManager.h"

using namespace std;

DBManager::DBManager(string host, string id, string pwd, string db)
{
	conn = new DBConnection(host, id, pwd, db);
}

DBManager::~DBManager()
{
	if (conn != nullptr)
		conn->Close();
	conn = nullptr;

	delete conn;
}

void DBManager::Connect()
{
	try
	{
		conn->Connect();
	}
	catch (runtime_error e)
	{
		throw e;
	}
}

void DBManager::Close()
{
	conn->Close();
}

UserUniquePtr DBManager::SelectUserInfo(const char * id)
{
	std::string query = str(boost::format("SELECT * FROM tbl_user_data WHERE id = '%s'") % id);

	ResultSets result = conn->selectQuery(query.c_str(), false);

	if (result.empty())
	{
		return nullptr;
	}
	else
	{
		ResultMap data = result.front();

		UserUniquePtr user(new User());
		//User *user = new User();

		user->setUserUid(boost::lexical_cast<short>(data["uuid"]));
		user->setId(data["id"].c_str());
		user->setUsername(data["username"].c_str());
		//user->setBirthday(0);
		user->setWeight(std::stoi(data["weight"]));
		user->setPosition(std::stof(data["x"]), std::stof(data["y"]), 0.0f);
		user->setCurrentMap(std::stoi(data["current_map"]));

		return user;
	}

	return nullptr;
}

UserUniquePtr DBManager::SelectUserInfo(const char* id, const char* password)
{
	std::string query = str(boost::format("SELECT * FROM tbl_user_data WHERE id = '%s' AND password = '%s'") % id % password);

	ResultSets result = conn->selectQuery(query.c_str(), false);

	if (result.empty())
	{
		return nullptr;
	}
	else
	{
		ResultMap data = result.front();

		UserUniquePtr user(new User());

		user->setUserUid(boost::lexical_cast<short>(data["uuid"]));
		user->setId(data["id"].c_str());
		user->setUsername(data["username"].c_str());
		user->setBirthday(std::stoi(data["birthday"]));
		user->setWeight(std::stoi(data["weight"]));
		user->setGender(std::stoi(data["gender"]));
		user->setGender(0);
		user->setPosition(std::stof(data["x"]), std::stof(data["y"]), 0.0f);
		user->setCurrentMap(std::stoi(data["current_map"]));

		return user;
	}

	return nullptr;
}

bool DBManager::InsertUserInfo(short sessionId, const char* id, const char* password, const char* username, const char& weight, const bool& gender, int& birthday)
{
	std::string query = str(boost::format("INSERT INTO tbl_user_data(id, password, username, birthday, weight, gender) VALUE('%s', '%s', '%s', from_unixtime(%d), %d, %d)") % id % password % username % birthday % (int)weight % gender);

	int result = conn->updateQuery(query.c_str(), false);

	return result == 1;
}

bool DBManager::UpdateUserInfo(short uuid, float x, float y, int map_id)
{
	std::string query = str(boost::format("UPDATE tbl_user_data SET x = %f, y = %f, current_map = %d WHERE uuid = %d") % x % y % map_id % uuid);

	int result = conn->updateQuery(query.c_str(), false);

	return result == 1;
}

bool DBManager::DeleteUserInfo(const char *id, const char *password)
{
	std::string query = str(boost::format("DELETE FROM tbl_user_data WHERE id = '%s' AND password = '%s'") % id % password);

	int result = conn->updateQuery(query.c_str(), false);

	return result == 1;
}

bool DBManager::SelectMyRecord(int user_uid, double & dest_record_time, double & dest_check_time)
{

	std::string query = str(boost::format("SELECT * FROM tbl_records WHERE user_uid = %d ORDER BY recorded_time ASC LIMIT 0, 1") % user_uid);

	ResultSets result = conn->selectQuery(query.c_str(), false);

	if (result.empty())
	{
		return false;
	}
	else
	{
		ResultMap data = result.front();

		dest_record_time = std::stod(data["recorded_time"]);
		dest_check_time = std::stod(data["checked_time"]);

		return true;
	}

	return false;
}

bool DBManager::UpdateMyRecord(int user_uid, double record_time, double check_time)
{
	std::string query = str(boost::format("INSERT INTO tbl_records(user_uid, recorded_time, checked_time) VALUE (%d, %d, %d);") % user_uid % record_time % check_time);
	
	int result = conn->updateQuery(query.c_str(), false);

	return result == 1;
}

bool DBManager::SelectRankUserRecord(char dest_username[10][12], double dest_record_time[10], double dest_check_time[10], int& item_count)
{
	std::string query = str(boost::format("SELECT a.username username, b.recorded_time recorded_time, b.checked_time checked_time FROM tbl_user_data AS a INNER JOIN tbl_records AS b WHERE a.uuid = b.user_uid ORDER BY b.recorded_time ASC LIMIT 0, 5"));

	ResultSets result = conn->selectQuery(query.c_str(), false);

	if (result.empty())
	{
		return false;
	}
	else
	{
		auto iter_b = result.begin();
		auto iter_e = result.end();

		int index = 0;

		for (; iter_b != iter_e; ++iter_b)
		{
			memcpy(dest_username[index], (*iter_b)["username"].c_str(), sizeof((*iter_b)["username"].c_str()));
			dest_record_time[index] = std::stod((*iter_b)["recorded_time"]);
			dest_check_time[index] = std::stod((*iter_b)["checked_time"]);

			index++;
		}

		item_count = index;
		return true;
	}

	return false;
}

bool DBManager::SelectMyReplayRecords(short uuid, unsigned char records[5][120], int &record_count, double record_time[5])
{
	std::string query = str(boost::format("SELECT record, recorded_time FROM tbl_replay_records WHERE user_uid = %d ORDER BY recorded_time DESC LIMIT 0, 5") % uuid);
	
	ResultSets result = conn->selectQuery(query.c_str(), false);

	if (result.empty())
	{
		return false;
	}
	else
	{
		auto iter_b = result.begin();
		auto iter_e = result.end();

		int index = 0;

		for (; iter_b != iter_e; ++iter_b)
		{
			std::string val = (*iter_b)["record"];
			
			std::vector<std::string> results;
			boost::split(results, val, [](char c){return c == ',';});

			for (int i = 0; i < 120; i++)
			{
				records[index][i] = std::stoi(results[i]);
			}
			record_time[index] = std::stod((*iter_b)["recorded_time"]);

			index++;
		}

		record_count = index;
		return true;
	}

	return false;
}

bool DBManager::UpdateMyReplayRecords(short uuid, unsigned char record[120])
{
	std::string v;

	for (int i = 0; i < 120; i++)
	{
		v.append(std::to_string(record[i]));
		v.append(",");
	}

	auto duration = std::chrono::system_clock::now().time_since_epoch();
	double millis = std::chrono::duration<double>(duration).count();
	
	std::string query = str(boost::format("INSERT INTO tbl_replay_records(user_uid, record, recorded_time) VALUE(%d, '%s', %d)") % uuid % v % millis);

	int result = conn->updateQuery(query.c_str(), false);

	return result == 1;
}
