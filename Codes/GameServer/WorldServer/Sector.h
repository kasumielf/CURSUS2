#pragma once

#include "../Common/User.h"
#include "Vector3D.h"
#include <list>

class Sector
{
private:
	int index;
    Vector2D start;
    Vector2D end;
    std::list<User*> users;

public:
    Sector(Vector2D _start, Vector2D _end, int _index);
    ~Sector();

	int GetSectorIndex() { return index; };
    void AddUser(User* user);
    void RemoveUser(const int user_uid);

    bool IsCurrectSector(float x, float y);

    auto GetUsersIteratorBegin(){ return users.begin(); };
    auto GetUsersIteratorEnd(){ return users.end(); }

};
