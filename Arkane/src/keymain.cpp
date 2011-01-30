#pragma unmanaged

#include <Windows.h>
#include <stdio.h>
#include <fmod.hpp>
#include <fmod_errors.h>
#include <conio.h>
#include <math.h>
#include <process.h>

struct thread_args {
	FMOD::System      *system;
	FMOD::Sound       *sound;
	int               threadNo;
	bool              paused;
};

const int NUM_CHANNELS = 12;
FMOD::Channel *channel[NUM_CHANNELS];
void createSounds(FMOD::Sound** sound, FMOD::System* system);
void printPlayingChannels(thread_args* t);

void ERRCHECK(FMOD_RESULT result)
{
    if (result != FMOD_OK)
    {
        printf("FMOD error! (%d) %s\n", result, FMOD_ErrorString(result));
        exit(-1);
    }
}

void startKey(void* args) {

	thread_args* t = (thread_args*)args;

	int key = 0;
	FMOD::System   *system   = t->system;
	FMOD::Sound    *sound    = t->sound;
	int threadNo             = t->threadNo;
	bool paused              = t->paused;
	FMOD_RESULT result;
	bool current_status;

	channel[threadNo]->getPaused(&current_status);

	// if no change needs to be made
	if (current_status == paused) _endthread();

	channel[threadNo]->setPaused(paused);
	result = system->playSound(FMOD_CHANNEL_REUSE, sound, paused, &channel[threadNo]);
	ERRCHECK(result);
	_endthread();
}

int main(int argc, char* argv[]) {
	
	for (int i = 0; i < 80; i++) printf("-");
	printf("\n");
	for (int i = 0; i < 4; i++) {
		for (int i = 0; i < 30; i++) printf("-");
		for (int i = 0; i < 20; i++) printf(" ");
		for (int i = 0; i < 30; i++) printf("-");
		printf("\n");
	}

	printf("\t\tArkane Studios Presents:\n");
	printf("\t\t\tA Team Jeff Production\n");
	printf("\t\t\t\tUltimate keyboard 3000\n");

	for (int i = 0; i < 4; i++) {
		for (int i = 0; i < 30; i++) printf("-");
		for (int i = 0; i < 20; i++) printf(" ");
		for (int i = 0; i < 30; i++) printf("-");
		printf("\n");
	}
	for (int i = 0; i < 80; i++) printf("-");

	//printf("To use press letters a-'.  They each represent a different key on \nthe piano\n\n");
	printf("Use the number keys 1-9. Only 8 sounds work, I'll have to hardcode in the others");

	/*
		Initialize variables for program
	*/
	FMOD::System   *system;
	FMOD_RESULT     result;
	int             key = 0;
	FMOD::Sound     *sound[NUM_CHANNELS];

    // Create system and initialize
	result = FMOD::System_Create(&system); ERRCHECK(result);
	result = system->init(NUM_CHANNELS, FMOD_INIT_NORMAL, NULL); ERRCHECK(result);

	createSounds(sound, system);

	thread_args t[NUM_CHANNELS];
	// Create thread arguments for each key
	for (int i = 0; i < NUM_CHANNELS; i++) {
		/* Args for threads! */
		t[i].threadNo = i; // thread number
		t[i].system   = system;
		t[i].sound    = sound[i];
	}

	int begin = 49; // Beginning key is the '1' key, for now
	do {
		for (int i = 0; i < NUM_CHANNELS; i++) { // check keys from 0-num_channels
			if (GetKeyState(begin + i) & 0x80) { // key is pressed
				t[i].paused = false;
			} else t[i].paused = true;           // key is not pressed

			_beginthread(startKey, 0, (void*)&t[i]);
		}
		result = system->update();

		printPlayingChannels(t);
	
	} while(true);

	return 0;
}


void printPlayingChannels(thread_args* t) {
		
	char* channels_playing[NUM_CHANNELS];

	for (int i = 0; i < NUM_CHANNELS; i++) {
		if (!t[i].paused) {
			channels_playing[i] = (char*)2;
		} else channels_playing[i] = " ";
	}
	
	printf("Channels playing: ");
	for (int i = 0; i < NUM_CHANNELS; i++) {
		printf("%c", channels_playing[i]);
		if (i != NUM_CHANNELS - 1) printf(" ");
	}
	printf("\r");
}


void createSounds(FMOD::Sound** sound, FMOD::System* system) {

	FMOD_RESULT result;
	char* dir =  "./media/piano/";
	char* notes[NUM_CHANNELS];
	char full_dir[25];

	notes[0]   =  "G#.wav";
	notes[1]   =  "A.wav";
	notes[2]   =  "A#.wav";
	notes[3]   =  "B.wav";
	notes[4]   =  "C.wav";
	notes[5]   =  "C#.wav";
	notes[6]   =  "D.wav";
	notes[7]   =  "Eb.wav";
	notes[8]   =  "E.wav";
	notes[9]   =  "F.wav";
	notes[10]  =  "F#.wav";
	notes[11]  =  "G.wav";

	for (int i = 0; i < NUM_CHANNELS; i++) {
		full_dir[0] = '\0';
		strcat(full_dir, dir);
		strcat(full_dir, notes[i]);
		result = system->createSound(full_dir, FMOD_SOFTWARE, 0, &sound[i]); 
		ERRCHECK(result);
	}
}
