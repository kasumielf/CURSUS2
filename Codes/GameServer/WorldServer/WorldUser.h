#pragma once

#include "../Common/User.h"

class WorldUser : public User
{
private:
    std::list<int> view_list;
	Sector* my_sector;

public:
	void setMySector(Sector* _sector){ my_sector = sector; }

	Secotr* const getMySector(){ return my_sector; }

	void AddViewList(const int id){ view_list.push_back(id); }
	void RemoveViewList(const int id){ view_list.erase(); }
	bool isExistInViewList(const int id){ return view_list.count(id) > 0 ? true : false; }
}