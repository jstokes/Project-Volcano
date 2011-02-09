#include "Sample.h"

Sample::Sample()
{
}
Sample::~Sample()
{
}
bool Sample::CreateSampleFromFile(char* fileName, FMOD::System * fmodSystem)
{
	 FMOD_RESULT fmodResult = fmodSystem->createSound(fileName, FMOD_SOFTWARE, 0, &sample);
	 return true;
}
bool Sample::Play(FMOD::System * fmodSystem, FMOD::Channel* channel)
{
	FMOD_RESULT fmodResult = fmodSystem->playSound(FMOD_CHANNEL_REUSE, sample, false, &channel);
	return true;
}