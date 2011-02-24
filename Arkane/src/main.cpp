
// the entry point for any Windows program
int WINAPI WinMain(HINSTANCE hInstance, HINSTANCE hPrevInstance, LPSTR lpCmdLine, int nCmdShow)
{
	// create the system object
	System* system = new System();
	
	// Check if the system object was created.
	if(!system) return 0;

	// Initialize and run the system object.
	if(system->Initialize()) system->Run();

	// Shutdown and release the system object.
	system->Close();
	delete system;
	system = 0;

	return 0;
}