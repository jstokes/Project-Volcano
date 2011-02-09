#pragma once

class InputClass
{
public:
	InputClass();
	~InputClass();
	void Initialize();
	void KeyDown(unsigned int);
	void KeyUp(unsigned int);
	void MouseDown(int xCoord, int yCoord);
	void MouseUp();
	bool IsMouseDown();
	bool IsKeyDown(unsigned int);
	int MouseXCoordinate();
	int MouseYCoordinate();
private:
	int mouseXCoord;
	int mouseYCoord;
	bool _mouseDown;
	bool _keyInput[256];
};

