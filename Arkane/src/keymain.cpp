#pragma unmanaged

#include <Windows.h>
#include <stdio.h>
#include <fmod.hpp>
#include <fmod_errors.h>
#include <conio.h>
#include <math.h>
#include <process.h>

/*
	thread_args contains all the information that threads need
	such as the system, the sound to play, the number of the thread
	and whether or not the thread is paused.
*/
struct thread_args {
	FMOD::System      *system;
	FMOD::Sound       *sound;
	int               threadNo;
	bool              paused;
	char              key;
};

/*
	This is the number of channels (and therefore sounds to be played)
*/
const int NUM_CHANNELS = 12;

FMOD::Channel *channel[NUM_CHANNELS];
void createSounds(FMOD::Sound** sound, FMOD::System* system);
void printPlayingChannels(thread_args* t);
void createThreadArgs(FMOD::Sound** sound, FMOD::System* system, thread_args *t);
void startKey(void* args);
void playSounds(thread_args *t, FMOD::System* system);
void printIntro();

/*
	Makes sure nothing went wrong!
*/
void ERRCHECK(FMOD_RESULT result) {
    if (result != FMOD_OK) {
        printf("FMOD error! (%d) %s\n", result, FMOD_ErrorString(result));
        Sleep(10000);
		exit(-1);
    }
}

int main(int argc, char* argv[]) {
	
	/*
		Initialize variables for program
	*/
	FMOD::System   *system;
	FMOD_RESULT     result;
	int             key = 0;
	FMOD::Sound    *sound[NUM_CHANNELS];
	thread_args     t[NUM_CHANNELS]; 
	
	// Create system and initialize
	result = FMOD::System_Create(&system); ERRCHECK(result);
	result = system->init(NUM_CHANNELS, FMOD_INIT_NORMAL, NULL); ERRCHECK(result);

	printIntro(); // print main screen
	createSounds(sound, system); // intialize all sounds
	createThreadArgs(sound, system, t); // initialize arguments for threads
	playSounds(t, system); // capture input and play sounds!
	
	return 0;
}


/*
	Starts playing sound when key is pressed and stops it when key is released
*/
void startKey(void* args) {

	thread_args* t = (thread_args*)args;

	FMOD::System   *system   = t->system;
	FMOD::Sound    *sound    = t->sound;
	int threadNo             = t->threadNo;
	bool paused              = t->paused;
	char key                 = t->key;
	FMOD_RESULT result;
	bool current_status;

	while(1) {
		if (GetKeyState(key) & 0x80) paused = false;
		else paused = true;               // key is not pressed
	
		// if no change needs to be made
		channel[threadNo]->getPaused(&current_status);
		if (current_status == paused) continue;

		channel[threadNo]->setPaused(paused);
		result = system->playSound(FMOD_CHANNEL_REUSE, sound, paused, &channel[threadNo]);
		ERRCHECK(result);
	}
}

/*
	Prints the main (console) screen
*/
void printIntro() {
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

	printf("To use press letters a-'.  They each represent a different key on \nthe piano\n\n");
}

/*
	Checks to see if keys are being pressed, creates thread for each key
*/
void playSounds(thread_args *t, FMOD::System *system) {
	
	FMOD_RESULT result;

	for (int i = 0; i < NUM_CHANNELS; i++) {
		_beginthread(startKey, 0, (void*)&t[i]);
	}
	
	do {
		
		result = system->update(); ERRCHECK(result);

		printPlayingChannels(t);
	
	} while(true);
}

/*
	Initializes all thread arguments
*/
void createThreadArgs(FMOD::Sound** sound, FMOD::System* system, thread_args *t) {

	char keys[NUM_CHANNELS];
	keys[0] = 'A';
	keys[1] = 'W';
	keys[2] = 'S';
	keys[3] = 'E';
	keys[4] = 'D';
	keys[5] = 'F';
	keys[6] = 'T';
	keys[7] = 'G';
	keys[8] = 'Y';
	keys[9] = 'H';
	keys[10] = 'U';
	keys[11] = 'J';
	// Create thread arguments for each key
	for (int i = 0; i < NUM_CHANNELS; i++) {
		/* Args for threads! */
		t[i].threadNo = i; // thread number
		t[i].system   = system;
		t[i].sound    = sound[i];
		t[i].paused   = true; // all channels start out paused
		t[i].key      = keys[i];
	}
}

/*
	Prints channels that are currently being played
*/
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


/*
	Initializes all sounds
*/
void createSounds(FMOD::Sound** sound, FMOD::System* system) {

	FMOD_RESULT result;
	char* dir =  "./media/piano/";
	char* notes[NUM_CHANNELS];
	char full_dir[30];

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
