#pragma once

const int MAX_ROOM_PLAYER_COUNT = 8;

class TrackMapData
{
private:
    int map_id;
    int track_count;
    float start_point[MAX_ROOM_PLAYER_COUNT][4];

public:
    int GetFinishTrackCount() { return track_count; }
    float* GetStartPoint(int position) { return start_point[position]; }

public:
    TrackMapData(int _map_id, int _track_count, float _start_point[MAX_ROOM_PLAYER_COUNT][4]) : map_id(_map_id), track_count(_track_count)
    {
        for (int i = 0; i < MAX_ROOM_PLAYER_COUNT; ++i)
        {
            for (int j = 0; j < 4; ++j)
            {
                start_point[i][j] = _start_point[i][j];
            }
        }
    }
};
