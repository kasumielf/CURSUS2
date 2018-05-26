class Record {
private:
	short game_type;
	short record;
	char record_distance;
	int recorded_time;
	char consumed_cal;

public:
	void setGameType(short type) { game_type = type; }
	void setRecord(short _record) { record = _record; }
	void setRecordDistance(char _distance) { record_distance = _distance; }
	void setRecordedTime(int _time) { recorded_time = _time; }
	void setConsumedCal(char _cal) { consumed_cal = _cal; }

	short getGameType() { return game_type; }
	short getRecord() { return record; }
	char getRecordDistance() { return record_distance; }
	int getRecordedTime() { return recorded_time; }
	char getConsumedCal() { return consumed_cal; }
};