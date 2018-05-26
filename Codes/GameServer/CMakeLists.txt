#
# CMakeLists for Main Solution
#

cmake_minimum_required(VERSION 2.6)
project(GameServer)

set(CMAKE_MACOSX_RPATH 1)

set (BOOST_ROOT /usr/local/Cellar/boost/1.63.0)
set (BOOST_ROOT /usr/local/Cellar/boost/1.63.0)

find_package(Boost 1.6.2 REQUIRED COMPONENTS system thread)

if( WIN32 )
	find_path( MYSQL_INCLUDE_DIR
		NAMES "mysql.h"
		PATHS "$ENV{PROGRAMFILES}/MySQL/*/include"
			  "$ENV{PROGRAMFILES(x86)}/MySQL/*/include"
			  "$ENV{SYSTEMDRIVE}/MySQL/*/include" )
	
	find_library( MYSQL_LIBRARY
		NAMES "mysqlclient" "mysqlclient_r"
		PATHS "$ENV{PROGRAMFILES}/MySQL/*/lib"
			  "$ENV{PROGRAMFILES(x86)}/MySQL/*/lib"
			  "$ENV{SYSTEMDRIVE}/MySQL/*/lib" )
else()
	find_path( MYSQL_INCLUDE_DIR
		NAMES "mysql.h"
		PATHS "/usr/include/mysql"
			  "/usr/local/include/mysql"
			  "/usr/mysql/include/mysql" )
	
	find_library( MYSQL_LIBRARY
		NAMES "mysqlclient" "mysqlclient_r"
		PATHS "/lib/mysql"
			  "/lib64/mysql"
			  "/usr/lib/mysql"
			  "/usr/lib64/mysql"
			  "/usr/local/lib/mysql"
			  "/usr/local/lib64/mysql"
			  "/usr/mysql/lib/mysql"
			  "/usr/mysql/lib64/mysql" )
endif()

set(EXTERNAL_LIB_PATH ${GameServer_SOURCE_DIR}/external_libs)
set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -std=c++11 -O2 -Wall -DNDEBUG")
set(gtest_INCLUDE_DIRS ${EXTERNAL_LIB_PATH}/gtest)

add_subdirectory(GameServer)
add_subdirectory(DBConnection)
add_subdirectory(FrontServer)
add_subdirectory(WorldServer)
add_subdirectory(AuthServer)
add_subdirectory(DBServer)
