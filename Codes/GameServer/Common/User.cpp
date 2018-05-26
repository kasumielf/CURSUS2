#include "User.h"

User::User(const User& user)
{
	DeepCopy(user);
}

User::User& operator=(const User& user)
{
	if (this != &user)
	{
		DeepCopy(user);
	}

	return *this;
}


void User::DeepCopy(const User& _src)
{
	userUid = _src.getUserUid();
	memcpy(id, _src.getId(), sizeof(id));
	memcpy(username, _src.getUsername(), sizeof(username));
    weight = _src.getWeight();
	gender = _src.getGender();
	birthday = _src.getBirthday();
	x = _src.getX();
	y = _src.getY();
	velocity = _src.getVelocity();
	rad = _src.getRadian();
}
