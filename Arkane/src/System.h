#ifndef SYSTEM_H
#define SYSTEM_H
#define WIN32_LEAN_AND_MEAN

#include <fmod.hpp>
#include <fmod_errors.h>
#include <Windows.h>

#include "Channel.h"
#include "InputClass.h"
#include "../bmp/resource.h"

#define NUM_CHANNELS  10
#define DECK1          1
#define DECK2          2
#define INDEXZEROBURN  1
#define SCREEN_WIDTH  1280
#define SCREEN_HEIGHT 720

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
		FMOD::System *fmodSystem;
		//FMOD::Channel *channelList;
		FMOD::Channel *channel;
		FMOD::Sound *sample;
		Channel* channelList[NUM_CHANNELS + INDEXZEROBURN];
		Sample* newSample;
		FMOD_RESULT fmodResult;
		int currentTime;
		float masterBPM;
		int masterKey;
		HWND hWnd;
		HINSTANCE hInstance;
		LPCWSTR applicationName;
		//GraphicsClass* graphicsClass;
		InputClass* inputClass;
		//private procedures
		void InitializeWindow();
		void CloseWindows();

	public:
		System(void);
		~System(void);
		bool Initialize();
		Channel* SelectChannel(int);
		void Run();
		void Close();
		LRESULT CALLBACK MessageHandler(HWND, UINT, WPARAM, LPARAM);
};
static LRESULT CALLBACK WindowProc(HWND, UINT, WPARAM, LPARAM);
static System* ApplicationHandle = 0;

#endif