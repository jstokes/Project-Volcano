#pragma unmanaged

#include <Windows.h>
#include <stdio.h>
#include <fmod.hpp>
#include <fmod_errors.h>
#include <conio.h>
#include <math.h>
#include <process.h>

const int NUM_CHANNELS = 7;
FMOD::Channel *channel[NUM_CHANNELS];


struct thread_args {
	FMOD::System      *system;
	FMOD::DSP         *dsp;
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
	FMOD::System   *system  = t->system;
	FMOD::DSP      *dsp     = t->dsp;
	int threadNo            = t->threadNo;
	bool paused             = t->paused;
	FMOD_RESULT result;
	bool current_status;

	channel[threadNo]->getPaused(&current_status);

	// if no change needs to be made
	if (current_status == paused) _endthread();

	result = system->playDSP(FMOD_CHANNEL_REUSE, dsp, true, &channel[threadNo]);
	channel[threadNo]->setPaused(paused);
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
	FMOD::DSP      *dsp[NUM_CHANNELS];

    // Create system and initialize
	result = FMOD::System_Create(&system); ERRCHECK(result);
	result = system->init(NUM_CHANNELS, FMOD_INIT_NORMAL, NULL); ERRCHECK(result);

	// Set the frequencies of all the DSPs
	// Based on piano key frequencies
	const float base_freq = 440.0f;
	float freq = base_freq;
	for (int i = 0; i < NUM_CHANNELS; i++) {
		
		result = system->createDSPByType(FMOD_DSP_TYPE_OSCILLATOR, &dsp[i]);
		ERRCHECK(result);

		result = dsp[i]->setParameter(FMOD_DSP_OSCILLATOR_RATE, freq);
		ERRCHECK(result);
		freq = base_freq * pow(2.0f, ((i + 1)/12.0f)); // calculate next step up
	}

	thread_args t[NUM_CHANNELS];
	// Create thread arguments for each key
	for (int i = 0; i < NUM_CHANNELS; i++) {
		/* Args for threads! */
		t[i].threadNo = i; // thread number
		t[i].system   = system;
		t[i].dsp      = dsp[i];
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
		
		Sleep(10);
	
	} while(true);

	return 0;
}
