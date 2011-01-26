#define WIN32_LEAN_AND_MEAN

#pragma unmanaged
#include <windows.h>
#include <stdio.h>
#include <conio.h>

#include <fmod.h>
#include <fmod_errors.h>

//globals
FMOD_SYSTEM      *system;
FMOD_SOUND       *sound1, *sound2, *sound3;
FMOD_CHANNEL     *channel = 0;
FMOD_CHANNEL     *channel2 = 0;
FMOD_CHANNEL     *channel3 = 0;
FMOD_DSP         *dsphighpass = 0;

FMOD_RESULT       result;
int               key = 0;
unsigned int      version;
bool keyDown      = false;
char message1[] = "Channel 1 Playing";

void ERRCHECK(FMOD_RESULT result)
{
    if (result != FMOD_OK)
    {
        //printf("FMOD error! (%d) %s\n", result, FMOD_ErrorString(result));
        //exit(-1);
    }
}

LRESULT CALLBACK WndProc(HWND hwnd, UINT message, WPARAM wParam, LPARAM lParam){
	PAINTSTRUCT ps;
	HDC hDC;
	char string[] = "Hello World!";

	switch(message){

		case WM_CREATE:
			return 0;
			break;

		case WM_CLOSE:
			PostQuitMessage(0);
			return 0;
			break;

		case WM_PAINT:
			/*hDC = BeginPaint(hwnd, &ps);
			SetTextColor(hDC, COLORREF(0x00FF0000));
			TextOut(hDC, 150, 150, string, sizeof(string)-1);
			EndPaint(hwnd, &ps);*/
			return 0;
			break;

		case WM_KEYDOWN:
			if(keyDown)
				break;
			switch(wParam){
				case 0x31:
					keyDown = true;
					result = FMOD_System_PlaySound(system, (FMOD_CHANNELINDEX)1, sound1, 0, &channel);
					return 0;
					break;
				case 0x32:
					keyDown = true;
					result = FMOD_System_PlaySound(system, (FMOD_CHANNELINDEX)2, sound2, 0, &channel2);
					return 0;
					break;
				case 0x33:
					keyDown = true;
					result = FMOD_System_PlaySound(system, (FMOD_CHANNELINDEX)3, sound3, 0, &channel3);
					return 0;
					break;
				case 0x34:
					int active;
                    result = FMOD_DSP_GetActive(dsphighpass, &active);
                    ERRCHECK(result);
                    if (active){
                        result = FMOD_DSP_Remove(dsphighpass);
                        ERRCHECK(result);
                    }
                    else{
                        result = FMOD_System_AddDSP(system, dsphighpass, 0);
                        ERRCHECK(result);
                    }
					return 0;
					break;
				default:
					break;
			}
			break;
		
		case WM_KEYUP:
			keyDown = false;

		default:
			break;
	}
	return (DefWindowProc(hwnd, message, wParam, lParam));
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

    result = FMOD_System_Create(&system);
    result = FMOD_System_Init(system, 32, FMOD_INIT_NORMAL, NULL);
    result = FMOD_System_CreateSound(system, "drumloop.wav", FMOD_HARDWARE, 0, &sound1);
    result = FMOD_Sound_SetMode(sound1, FMOD_LOOP_NORMAL);
    result = FMOD_System_CreateSound(system, "bass_off_beat.wav", FMOD_SOFTWARE, 0, &sound2);
	result = FMOD_Sound_SetMode(sound2, FMOD_LOOP_NORMAL);
    result = FMOD_System_CreateSound(system, "trance_lead.wav", FMOD_HARDWARE, 0, &sound3);
	result = FMOD_System_CreateDSPByType(system, FMOD_DSP_TYPE_HIGHPASS, &dsphighpass);

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



	while(true){
		FMOD_System_Update(system);

        {
            unsigned int ms = 0;
            unsigned int lenms = 0;
            int          playing = 0;
            int          paused = 0;
            int          channelsplaying = 0;

            if (channel)
            {
                FMOD_SOUND *currentsound = 0;
                result = FMOD_Channel_IsPlaying(channel, &playing);
                if ((result != FMOD_OK) && (result != FMOD_ERR_INVALID_HANDLE) && (result != FMOD_ERR_CHANNEL_STOLEN)){
                    ERRCHECK(result);
                }
                result = FMOD_Channel_GetPaused(channel, &paused);
                if ((result != FMOD_OK) && (result != FMOD_ERR_INVALID_HANDLE) && (result != FMOD_ERR_CHANNEL_STOLEN)){
                    ERRCHECK(result);
                }

                result = FMOD_Channel_GetPosition(channel, &ms, FMOD_TIMEUNIT_MS);
                if ((result != FMOD_OK) && (result != FMOD_ERR_INVALID_HANDLE) && (result != FMOD_ERR_CHANNEL_STOLEN)){
                    ERRCHECK(result);
                }
                FMOD_Channel_GetCurrentSound(channel, &currentsound);
                if (currentsound){
                    result = FMOD_Sound_GetLength(currentsound, &lenms, FMOD_TIMEUNIT_MS);
                    if ((result != FMOD_OK) && (result != FMOD_ERR_INVALID_HANDLE) && (result != FMOD_ERR_CHANNEL_STOLEN)){
                        ERRCHECK(result);
                    }
                }
            }
            result = FMOD_Sound_GetLength(sound1, &lenms, FMOD_TIMEUNIT_MS);
            if ((result != FMOD_OK) && (result != FMOD_ERR_INVALID_HANDLE) && (result != FMOD_ERR_CHANNEL_STOLEN)){
                ERRCHECK(result);
            }
            FMOD_System_GetChannelsPlaying(system, &channelsplaying);
            //printf("Time %02d:%02d:%02d/%02d:%02d:%02d : %s : Channels Playing %2d\r", ms / 1000 / 60, ms / 1000 % 60, ms / 10 % 100, lenms / 1000 / 60, lenms / 1000 % 60, lenms / 10 % 100, paused ? "Paused " : playing ? "Playing" : "Stopped", channelsplaying);
        }
        Sleep(10);


		PeekMessage(&msg, hwnd, NULL, NULL, PM_REMOVE);
		if(msg.message == WM_QUIT){
			break;
		}
		else{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
	}

	return msg.wParam;
    
    result = FMOD_Sound_Release(sound1);
    ERRCHECK(result);
    result = FMOD_Sound_Release(sound2);
    ERRCHECK(result);
    result = FMOD_Sound_Release(sound3);
    ERRCHECK(result);
    result = FMOD_System_Close(system);
    ERRCHECK(result);
    result = FMOD_System_Release(system);
    ERRCHECK(result);

}


