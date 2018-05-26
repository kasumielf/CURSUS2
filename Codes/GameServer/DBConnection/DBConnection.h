#pragma once

#include <stdlib.h>
#include <iostream>
#include <string>
#include <vector>

#include <mysql_driver.h>
#include <mysql_connection.h>

#include <cppconn/driver.h>
#include <cppconn/exception.h>
#include <cppconn/resultset.h>
#include <cppconn/statement.h>
#include <cppconn/prepared_statement.h>

#include "DBResultMap.h"

typedef std::vector<ResultMap> ResultSets;

class DBConnection
{
private:
	std::string host;
	std::string username;
	std::string pwd;
	std::string db;

	sql::Driver *driver;
	sql::Connection *conn;
	sql::Statement *stmt;
	sql::ResultSet *rslt;

	void execute(const char* query, ...);

public:
	DBConnection(std::string _host, std::string _username, std::string _pwd, std::string _db);
	~DBConnection();

	void Connect();
	void Close();

	ResultSets selectQuery(const char* query, bool is_transactional, ...);
	int updateQuery(const char*query, bool is_transactional);
};

