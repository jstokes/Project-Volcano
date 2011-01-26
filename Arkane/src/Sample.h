#ifndef SAMPLE_H
#define SAMPLE_H

#include "Channel.h"
#include <fmod.h>
#include <fmod_errors.h>

/*
Sample Class
----------------------------------------------------
Responsible for holding the information about the
sample.
Assumes Time Signature = 4/4

-When a sample is added to a channel, a copy of the sample
 object is given to the channel, rather than a reference to
 the sample.

-When a sample is added to the channel, it needs to load
 itself into memory using the information in the Sample
 class.

-Contains a variable that holds the channel that it is
 being loaded into

-Must include a function that calculates distance from
 beat, and starts playing at that distance. Meaning 
 that if the sample is started off the beat, it will start
 the sample on the previous beat. For example: if the sample
 is triggered at beat 2.6, it will behave as if it were
 triggered at beat 2

-Assuption the sample will start on the beat(1-2-3-4)

TODO
-Must define the maximum distance from the beat that is allowed
-Make a name  attribute for easier reading.  The name will be the 
 filepath unless the file contains a song title.

*/

class Sample{

	private:
		char* filePath;
		int channelLoadedInto; 
		int length;
		int key;
		float BPM;
		bool isLoop;
		FMOD_SOUND sampleName;

	public:
		Sample();
		~Sample();


};

#endif