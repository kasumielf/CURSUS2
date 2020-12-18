#pragma once

#include <string>
#include <memory>
#include <list>

using namespace std;

class User
{
private:
    int userUid;
    char id[12];
    char username[12];
    char weight;
    char gender;
    unsigned int birthday;
    float x;
    float y;
    float z;
    float v;
    int current_map;
    short session_id;
    bool during_on_game;

public:
    User() {}

    User(const User& user)
    {
        during_on_game = false;

        userUid = user.getUserUid();
        strcpy_s(id, user.getId());
        strcpy_s(username, user.getUsername());
        weight = user.getWeight();
        gender = user.getGender();
        birthday = user.getBirthday();
        x = user.getX();
        y = user.getY();
        z = user.getZ();
        v = user.getSpeed();
        current_map = user.getCurrentMap();
    }

    User& operator=(const User& user)
    {
        if (this != &user)
        {
            userUid = user.getUserUid();
            strcpy_s(id, user.getId());
            strcpy_s(username, user.getUsername());
            weight = user.getWeight();
            gender = user.getGender();
            birthday = user.getBirthday();
            x = user.getX();
            y = user.getY();
            z = user.getZ();
            v = user.getSpeed();
            current_map = user.getCurrentMap();
        }

        return *this;
    }

    bool operator==(const User& rhs) const;

public:

public:
    void setUserUid(const int uid) { userUid = uid; }
    void setId(const char _id[12]) { memcpy(id, _id, strlen(_id) + 1); }
    void setUsername(const char _username[12]) { memcpy(username, _username, strlen(_username) + 1); }
    void setWeight(const char _weight) { weight = _weight; }
    void setGender(const char _gender) { gender = _gender; }
    void setBirthday(const int _birthday) { birthday = _birthday; }
    void setPosition(const float _x, const float _y, const float _z) { x = _x, y = _y; z = _z; }
    void setCurrentMap(const int map_id){ current_map = map_id; }
    void setSpeed(float _v) { v = _v; }
    //void setMySector(Sector* _sector){ my_sector = sector; }

    int getUserUid() const { return userUid; }
    const char* getId() const { return id; }
    const char* getUsername() const { return username; }
    char getWeight() const { return weight; }
    char getGender() const { return gender; }
    int getBirthday() const { return birthday; }

    float getX() const { return x; }
    float getY() const { return y; }
    float getZ() const { return z; }
    float getSpeed() const { return v; }

    int getCurrentMap() const { return current_map; }

    void setDuringOnGame(bool v) { during_on_game = v; }
    bool isDuringOnGame() { return during_on_game; }
    int getSessionId() { return session_id; }
    void setSessionId(int id) { session_id = id; }

};

using UserUniquePtr = std::unique_ptr<User> ;
using UserSharedPtr = std::shared_ptr<User> ;