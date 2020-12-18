#pragma once

#include <unordered_map>
#include <string>

class ResultMap
{
private:
    std::unordered_map<std::string, std::string> data;

public:
    void insert(std::string key, std::string value)
    {
        data.insert(std::make_pair(key, value));
    }

    std::string get(std::string key)
    {
        return data[key];
    }

    const std::string operator[](const std::string& param)
    {
        return data[param];
    }
};