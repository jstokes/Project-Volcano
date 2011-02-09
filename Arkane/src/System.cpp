#include "system.h"
#include "Sample.h"
#include <fmod.hpp>
#include <fmod_errors.h>
#include <CommCtrl.h>

#define IDB_BUTTON1      201
#define IDB_BUTTON2      202
#define IDB_BUTTON3      203
#define IDB_BUTTON4      204
#define IDB_BUTTON5      205
#define IDB_BUTTON6      206
#define IDB_BUTTON7      207
#define IDB_BUTTON8      208
#define CHANNEL_1_REVERB 209
#define CHANNEL_1_HPASS  210
#define CHANNEL_1_LPASS  211

HWND channel1Button,channel2Button,channel3Button,channel4Button,
	 channel5Button,channel6Button,channel7Button,channel8Button;
HWND channel1Reverb,channel1HighPass,channel1LowPass;

System::System(void){
	inputClass = 0;
}
System::~System(void){
}
bool System::Initialize()
{
	InitializeWindow();

	inputClass = new InputClass();
	if(!inputClass) return false;
	inputClass->Initialize();
	
	//initialize fmod system;
	channel = 0;
    fmodResult = FMOD::System_Create(&fmodSystem);
    fmodResult = fmodSystem->init(32, FMOD_INIT_NORMAL, 0);
    fmodResult = fmodSystem->createSound("media/drumloop.wav", FMOD_SOFTWARE, 0, &sample);

	//initialize the sample abstraction
	newSample = new Sample();
	newSample->CreateSampleFromFile("media/drumloop.wav", fmodSystem);

	//initialize the channel abstraction
	for(int i = 1; i < NUM_CHANNELS + INDEXZEROBURN; i++){
		channelList[i] = new Channel();
		channelList[i]->Initialize(fmodSystem, i);
	}
	channelList[1]->LoadSample("media/drumloop.wav");
	return true;
}

void System::Run()
{
	MSG msg;
	bool done, result;
	//initialize the message struct.
	ZeroMemory(&msg, sizeof(MSG));
	//main windows loop
	done = false;
	while(!done)
	{
		//handle windows messages
		if(PeekMessage(&msg, NULL, 0, 0, PM_REMOVE))
		{
			TranslateMessage(&msg);
			DispatchMessage(&msg);
		}
		//if windows sends WM_QUIT message, then exit loop
		if(msg.message == WM_QUIT)
		{
			done = true;
		}
		else
		{
			if(inputClass->IsMouseDown())
			{
				//do stuff here if the mouse was
				//clicked (not on button)
			}
		}
		if(inputClass->IsKeyDown(VK_ESCAPE))
		{
			done = true;
		}
	}
}

void System::Close()
{
	if(inputClass) delete inputClass;
	DestroyWindow(hWnd);
	hWnd = NULL;
	//remove the application instance.
	UnregisterClass(applicationName, hInstance);
	hInstance = NULL;
	//release the pointer to this class
	ApplicationHandle = NULL;
}

LRESULT CALLBACK WindowProc(HWND _hWnd, UINT message, WPARAM wParam, LPARAM lParam)
{
	switch(message)
	{
		//check if window is being destroyed
		case WM_DESTROY:
		{
			PostQuitMessage(0);
			return 0;
		} break;
		case WM_CLOSE:
		{
			PostQuitMessage(0);
			return 0;
		} break;
		default:
		{
			return ApplicationHandle->MessageHandler(_hWnd, message, wParam, lParam);
		} break;
	}
	return DefWindowProc(_hWnd, message, wParam, lParam);
}

LRESULT CALLBACK System::MessageHandler(HWND hwnd, UINT umsg, WPARAM wparam, LPARAM lparam)
{
	switch(umsg)
	{
		// Check if a key has been pressed on the keyboard.
		case WM_KEYDOWN:
		{
			// If a key is pressed send it to the input object so it can record that state.
			inputClass->KeyDown((unsigned int)wparam);
			return 0;
		}

		// Check if a key has been released on the keyboard.
		case WM_KEYUP:
		{
			// If a key is released then send it to the input object so it can unset the state for that key.
			inputClass->KeyUp((unsigned int)wparam);
			return 0;
		}

		//THIS IS WHERE ALL OF THE BUTTON STUFF GOES
		case WM_COMMAND:
		{
			if (LOWORD(wparam) == IDB_BUTTON1 &&
			HIWORD(wparam) == BN_CLICKED &&
			(HWND) lparam == channel1Button)
			{
				//play the sound through the Channel Class
				channelList[1]->PlaySample();
			}
			if (LOWORD(wparam) == IDB_BUTTON2 &&
			HIWORD(wparam) == BN_CLICKED &&
			(HWND) lparam == channel2Button)
			{
				if(channelList[1]->GetVolume() < 1.0f)
				{
					channelList[1]->SetVolume(1.0f);
				} else {
					channelList[1]->SetVolume(0.0f);
				}
			}
			if (LOWORD(wparam) == IDB_BUTTON3 &&
			HIWORD(wparam) == BN_CLICKED &&
			(HWND) lparam == channel3Button)
			{
				//Play sample thru Channel class
				channelList[1]->SetReverb();
			}
			return 0;
		}
		

		case WM_LBUTTONDOWN:
		{
			inputClass->MouseDown(LOWORD(lparam),HIWORD(lparam));
			break;
		}
		case WM_LBUTTONUP:
		{
			inputClass->MouseUp();
			break;
		}

		// Any other messages send to the default message handler as our application won't make use of them.
		default:
		{
			return DefWindowProc(hwnd, umsg, wparam, lparam);
		}
	}
}
void System::InitializeWindow()
{
	WNDCLASSEX wc;

	// Get an external pointer to this object.
	ApplicationHandle = this;

	ZeroMemory(&wc, sizeof(WNDCLASSEX));	// clear out the window class for use
	hInstance = GetModuleHandle(NULL);		// get handle to our application
	applicationName = L"MyApplication";	// name of application
    
    wc.cbSize = sizeof(WNDCLASSEX);
	wc.style = CS_HREDRAW | CS_VREDRAW;
	wc.lpfnWndProc = WindowProc;
	wc.hInstance = hInstance;
	wc.hIcon = LoadIcon(hInstance, MAKEINTRESOURCE(IDI_ICON1));
	wc.lpszMenuName = NULL;
	wc.hCursor = LoadCursor(NULL, IDC_ARROW);
	wc.lpszClassName = applicationName;
	wc.hIconSm = LoadIcon(wc.hInstance, MAKEINTRESOURCE(IDI_ICON1));

	RegisterClassEx(&wc);

	RECT wr = {0, 0, SCREEN_WIDTH, SCREEN_HEIGHT};
	AdjustWindowRect(&wr, WS_OVERLAPPEDWINDOW, FALSE);

	int winX = (GetSystemMetrics(SM_CXSCREEN) - (wr.right - wr.left)) / 2;
	int winY = (GetSystemMetrics(SM_CYSCREEN) - (wr.bottom - wr.top)) / 2;

	hWnd = CreateWindowEx(NULL,
		applicationName,
		L"Our First DirectX Program",
		WS_OVERLAPPEDWINDOW,
		winX,
		winY,
		wr.right - wr.left,
		wr.bottom - wr.top,
		NULL,
		NULL,
		hInstance,
		NULL);
	channel1Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		5, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON1,hInstance,NULL);
	channel1Reverb = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		5, 44, 150, 40, hWnd, (HMENU)CHANNEL_1_REVERB,hInstance,NULL);
	channel1HighPass = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		5, 83, 150, 40, hWnd, (HMENU)CHANNEL_1_HPASS,hInstance,NULL);
	channel2Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		154, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON2,hInstance,NULL);
	channel3Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		303, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON3,hInstance,NULL);
	channel4Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		452, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON4,hInstance,NULL);
	channel5Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		601, 5, 150, 40,hWnd, (HMENU)IDB_BUTTON5,hInstance,NULL);
	channel6Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		750, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON6,hInstance,NULL);
	channel7Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		899, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON7,hInstance,NULL);
	channel8Button = CreateWindow(L"BUTTON", L"Class Options",  
		BS_FLAT | BS_BITMAP | WS_VISIBLE | WS_CHILD | BS_PUSHBUTTON, 
		1048, 5, 150, 40, hWnd, (HMENU)IDB_BUTTON8,hInstance,NULL);
	SendMessage(channel1Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP1)));
	SendMessage(channel1Reverb,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP11)));
	SendMessage(channel1HighPass,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP9)));
	SendMessage(channel2Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP2)));
	SendMessage(channel3Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP3)));
	SendMessage(channel4Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP4)));
	SendMessage(channel5Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP5)));
	SendMessage(channel6Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP6)));
	SendMessage(channel7Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP7)));
	SendMessage(channel8Button,BM_SETIMAGE,IMAGE_BITMAP,
		(LPARAM)LoadBitmap(hInstance, MAKEINTRESOURCE(IDB_BITMAP8)));
	ShowWindow(hWnd, SW_SHOW);
	SetForegroundWindow(hWnd);
	SetFocus(hWnd);
}