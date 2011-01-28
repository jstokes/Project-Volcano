#pragma unmanaged

#include <Windows.h>
#include <stdio.h>
#include <fmod.h>
#include <fmod_errors.h>
#include <conio.h>
#include <math.h>

//globals
const int NUM_CHANNELS = 7;
FMOD_SYSTEM      *system;
FMOD_CHANNEL     *channels[NUM_CHANNELS];
FMOD_DSP         *dsphighpass = 0;

FMOD_RESULT       result;
int               key = 0;
unsigned int      version;

void ERRCHECK(FMOD_RESULT result)
{
    if (result != FMOD_OK)
    {
        //printf("FMOD error! (%d) %s\n", result, FMOD_ErrorString(result));
        exit(-1);
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam){

	switch(message){

		case WM_CREATE:
			return 0;
			break;

		case WM_CLOSE:
			PostQuitMessage(0);
			return 0;
			break;

		case WM_PAINT:
			return 0;
			break;

		case WM_KEYDOWN:
			switch(wParam){
				case '1':

					return 0;
					break;
				default:
					break;
			}
			break;
		
		case WM_KEYUP:

		default:
			break;
	}
	return (DefWindowProc(hwnd, message, wParam, lParam));
}

void initializeChannels() {
	const float base_freq = 440.0f;
	float freq = base_freq;
	for (int i = 0; i < NUM_CHANNELS; i++) {
		channels[i] = 0;
		FMOD_Channel_SetFrequency(channels[i], freq);
		freq = base_freq * pow(2.0, (i/12)); // calculate next step up
	}
}

int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nShowCmd){

	WNDCLASSEX windowClass;
	HWND       hwnd;
	MSG        msg;

	windowClass.cbSize        = sizeof(WNDCLASSEX);
	windowClass.style         = CS_HREDRAW | CS_VREDRAW;
	windowClass.lpfnWndProc   = WndProc;
	windowClass.cbClsExtra    = 0;
	windowClass.cbWndExtra    = 0;
	windowClass.hInstance     = hInstance;
	windowClass.hIcon         = LoadIcon(NULL, IDI_APPLICATION);
	windowClass.hCursor       = LoadCursor(NULL, IDC_ARROW);
	windowClass.hbrBackground = (HBRUSH)GetStockObject(WHITE_BRUSH);
	windowClass.lpszMenuName  = NULL;
	windowClass.lpszClassName = L"MyClass";
	windowClass.hIconSm       = LoadIcon(NULL, IDI_WINLOGO);

	if(!RegisterClassEx(&windowClass))
		return 0;

	hwnd = CreateWindowEx(NULL,
						  L"MyClass",
						  L"VolcanoStudio",
						  WS_OVERLAPPEDWINDOW | WS_VISIBLE | WS_SYSMENU | WS_CAPTION,
						  100, 100,
						  400, 400,
						  NULL,
						  NULL,
						  hInstance,
						  NULL);
	if(!hwnd)
		return 0;

	result = FMOD_System_Create(&system); ERRCHECK(result);
	result = FMOD_System_Init(system, NUM_CHANNELS, FMOD_INIT_NORMAL, NULL); ERRCHECK(result);

	initializeChannels();
}