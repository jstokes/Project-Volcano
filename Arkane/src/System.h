#ifndef SYSTEM_H
#define SYSTEM_H

#include "Sample.h"
#include "Channel.h"

#define NUM_CHANNELS  10
#define DECK1          1
#define DECK2          2
#define INDEXZEROBURN  1

#include "Sample.h"

/*
System class
--------------------------------------------------------------
Responsible for managing all of the resources used by the main
program.  It is mostly responsible for interacting with the
GUI interface and managing the channels.  Therefore it should
contain code which can manipulate the channel class.
*/

class System {

	private:
		Channel channelList[NUM_CHANNELS + INDEXZEROBURN];
		Sample sampleList[];
		int currentTime;
		float masterBPM;
		int masterKey;

	public:
		int Initialize();
		Channel* SelectChannel(int channelNumber);

};

#endif