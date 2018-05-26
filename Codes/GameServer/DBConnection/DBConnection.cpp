#include "DBConnection.h"
#include <cstdarg>

DBConnection::DBConnection(std::string _host, std::string _username, std::string _pwd, std::string _db)
	: host(_host.c_str()), username(_username.c_str()), pwd(_pwd.c_str()), db(_db.c_str())
{
}

DBConnection::~DBConnection()
{
	Close();
}

void DBConnection::Connect()
{
	try
	{
		driver = get_driver_instance();
		conn = driver->connect(host.c_str(), username.c_str(), pwd.c_str());
		conn->setSchema(db.c_str());
	}
	catch (sql::SQLException &e)
	{
		throw std::runtime_error(e.what());
	}
}

void DBConnection::Close()
{
	try
	{
		conn->close();
	}
	catch(sql::SQLException e)
	{
		std::cout << e.what() << std::endl;
	}
}

void DBConnection::execute(const char* query, ...)
{
	stmt->executeQuery(query);
}

ResultSets DBConnection::selectQuery(const char* query, bool is_transactional, ...)
{
	stmt = conn->createStatement();
	
	//std::shared_ptr<sql::ResultSet> rs(stmt->executeQuery(query));

	sql::ResultSet *rs = stmt->executeQuery(query);

	ResultSets rset;

	while (rs->next()) {
		sql::ResultSetMetaData *rsmt = rs->getMetaData();

		int column_len = rsmt->getColumnCount();

		ResultMap map;

		for (int i = 1; i <= column_len; i++)
		{	
			std::string message(rs->getString(i).c_str());
			map.insert(rsmt->getColumnName(i).c_str(), message);
		}

		rset.push_back((map));
	}

	delete rs;

	return rset;
}

int DBConnection::updateQuery(const char * query, bool is_transactional)
{
	int rs = 0;
	stmt = conn->createStatement();

	try
	{
		rs = stmt->executeUpdate(query);
	}
	catch (sql::SQLException &e)
	{
		std::cout << e.what() << std::endl;
		throw std::runtime_error(e.what());
	}

	return rs;
}
