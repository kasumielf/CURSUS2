#include <algorithm>
#include <functional>
#include "Sector.h"

Sector::Sector(Vector2D _start, Vector2D _end, int _index) : start(_start), end(_end), index(_index)
 {
 }

 Sector::~Sector()
 {
 }

void Sector::AddUser(User* user)
{
    user->setCurrentMap(this->GetSectorIndex());
    users.push_back(user);
}

void Sector::RemoveUser(const int user_uid)
{
    users.erase(std::remove_if(users.begin(), users.end(), [user_uid](User* u) { return user_uid == u->getUserUid(); }), users.end());
}

bool Sector::IsCurrectSector(float x, float y)
{
    return start.y <= y & end.y > y;
}