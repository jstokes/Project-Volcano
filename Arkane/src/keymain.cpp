#pragma unmanaged

#include <Windows.h>
#include <stdio.h>
#include <fmod.hpp>
#include <fmod_errors.h>
#include <conio.h>
#include <math.h>
#include <process.h>

void printPlayingChannels(thread_args* t);
void createSounds(FMOD::Sound** sound);

const int NUM_CHANNELS = 12;

struct thread_args {
	FMOD::System      *system;
	FMOD::Sound       *sound;
	FMOD::Channel     *channel;
	int               threadNo;
	bool              paused;
};

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
	FMOD::Channel  *channel = t->channel;
	int threadNo             = t->threadNo;
	bool paused              = t->paused;
	FMOD_RESULT result;
	bool current_status;

	channel->getPaused(&current_status);

	// if no change needs to be made
	if (current_status == paused) _endthread();

	channel->setPaused(paused);
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

	printf("To use press key numbers 1-7.  They each represent a different key on \nthe electronic keyboard\n\n");

	/*
		Initialize variables for program
	*/
	FMOD::System   *system;
	FMOD_RESULT     result;
	int             key = 0;
	FMOD::Sound     *sound[NUM_CHANNELS];
	FMOD::Channel   *channel[NUM_CHANNELS];

    // Create system and initialize
	result = FMOD::System_Create(&system); ERRCHECK(result);
	result = system->init(NUM_CHANNELS, FMOD_INIT_NORMAL, NULL); ERRCHECK(result);

	createSounds(sound);

	thread_args t[NUM_CHANNELS];
	// Create thread arguments for each key
	for (int i = 0; i < NUM_CHANNELS; i++) {
		/* Args for threads! */
		t[i].threadNo = i; // thread number
		t[i].system   = system;
		t[i].sound    = sound[i];
		t[i].channel  = channel[i];
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


void createSounds(FMOD::Sound** sound) {
	char* piano[NUM_CHANNELS];

}
