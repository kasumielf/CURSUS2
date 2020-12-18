#include "GameWorld.h"

GameWorld::GameWorld()
{
}

GameWorld::~GameWorld()
{
    auto iter_b = users.begin();
    auto iter_e = users.end();

    for (; iter_b != iter_e; ++iter_b)
    {
        delete (*iter_b).second;
    }

    users.clear();
}

void GameWorld::Init()
{
    Map* hangang_map = new Map();

    hangang_map->Init();

    // 하드코딩
    current_map = hangang_map;
}

bool GameWorld::isExistUser(const int user_uid)
{
    for each(std::pair<int, User*> user in users)
    {
        if (user.second->getUserUid() == user_uid)
            return true;
    }
    return false;
}

void GameWorld::AddUser(const int session_id, User *user)
{
    users[session_id] = user;
    current_map->AddUser(user);
}

void GameWorld::RemoveUser(const int session_id)
{
    if (users[session_id] != nullptr)
    {
        current_map->RemoveUser(users[session_id]);
        users[session_id] = nullptr;
        delete users[session_id];
        users.erase(session_id);
    }
}

void GameWorld::SetPosition(const int session_id, float x, float y, float z, float v, float r)
{
    if (users.count(session_id) >= 0)
    {
        User* user = users[session_id];
        user->setPosition(x, y, z);
        user->setSpeed(v);

        if (current_map->GetSectorByLocation(x, y)->GetSectorIndex() != user->getCurrentMap())
        {
            int old_index = user->getCurrentMap();
            int new_index = current_map->GetSectorByLocation(x, y)->GetSectorIndex();

            current_map->SwitchSector(user, old_index, new_index);
        }
    }
}

bool GameWorld::isNear(const int from, const int to)
{    
    return 
        (users[from]->getX() - users[to]->getX()) * (users[from]->getX() - users[to]->getX()) +
        (users[from]->getY() - users[to]->getY()) * (users[from]->getY() - users[to]->getY())
         <= MAX_SIGHT_RANGE * MAX_SIGHT_RANGE;
}    

