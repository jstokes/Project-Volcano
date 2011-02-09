#include "InputClass.h"

InputClass::InputClass(void)
{
}

InputClass::~InputClass(void)
{
}

void InputClass::Initialize()
{
	// Initialize all the keys to being released and not pressed.
	for(int i=0; i<256; i++)
		_keyInput[i] = false;
	_mouseDown = false;
}

void InputClass::KeyDown(unsigned int input)
{
	// If a key is pressed then save that state in the key array.
	_keyInput[input] = true;
}
void InputClass::KeyUp(unsigned int input)
{
	// If a key is released then clear that state in the key array.
	_keyInput[input] = false;
}
void InputClass::MouseDown(int xCoord, int yCoord)
{
	// If a key is pressed then save that state in the key array.
	mouseXCoord = xCoord;
	mouseYCoord = yCoord;
	_mouseDown = true;
}
void InputClass::MouseUp()
{
	// If a key is released then clear that state in the key array.
	_mouseDown = false;
}
bool InputClass::IsKeyDown(unsigned int key)
{
	// Return what state the key is in (pressed/not pressed).
	return _keyInput[key];
}
bool InputClass::IsMouseDown()
{
	// Return what state the key is in (pressed/not pressed).
	return _mouseDown;
}
int InputClass::MouseXCoordinate()
{
	return mouseXCoord;
}
int InputClass::MouseYCoordinate()
{
	return mouseYCoord;
}


