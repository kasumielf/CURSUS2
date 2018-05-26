#include "Map.h"
#include <iostream>

Map::Map()
{
}

void Map::Init()
{
	float x = -50;
	float y = 1000;

	for (int i = 0; i < 6; ++i)
	{
		Vector2D start;
		Vector2D end;

		start.x = x * i;
		start.y = y * i;

		end.x = x * (i + 1);
		end.y = y * (i + 1);

		Sector* sector = new Sector(start, end, i);
		sectors.push_back(sector);
	}
}

void Map::AddUser(User* user)
{
	sectors[user->getCurrentMap()]->AddUser(user);
}


void Map::RemoveUser(User* user)
{
	sectors[user->getCurrentMap()]->RemoveUser(user->getUserUid());
}

Sector* Map::GetSectorByLocation(float x, float y)
{
	for (Sector* sec : sectors)
	{
		if (sec->IsCurrectSector(x, y))
			return sec;
	}

	// null 리턴될린 없어야겠지만...
	return sectors[0];
}

void Map::SwitchSector(User* user, int from, int to)
{
	sectors[from]->RemoveUser(user->getUserUid());
	sectors[to]->AddUser(user);
}

std::list<User*>::iterator Map::GetUsersIterator_Begin(int sector)
{
	return sectors[sector]->GetUsersIteratorBegin();
}

std::list<User*>::iterator Map::GetUsersIterator_End(int sector)
{
	return sectors[sector]->GetUsersIteratorEnd();
}
