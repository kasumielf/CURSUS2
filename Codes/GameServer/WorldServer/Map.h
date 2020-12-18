#pragma once

#include "Sector.h"

#include <vector>

class Map
{
private:
    std::vector<Sector*> sectors;
public:
    Map();
    ~Map();

    void Init();
    void AddUser(User* user);
    void RemoveUser(User* user);
    Sector* GetSectorByLocation(float x, float y);
    void SwitchSector(User* user, int from, int to);

    std::list<User*>::iterator GetUsersIterator_Begin(int sector);
    std::list<User*>::iterator GetUsersIterator_End(int sector);

};