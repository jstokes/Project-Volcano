#include "Channel.h"
#include "Sample.h"

Channel::Channel(void)
{
	sample1 = new Sample();
}
Channel::~Channel(void)
{

}
bool Channel::Initialize(FMOD::System* fmodSystemPtr, int index)
{
	channelIndex = index;
	fmodSystem = fmodSystemPtr;
	fmodSystem->getChannel(channelIndex, &fmodChannel);
	return true;
}
FMOD_RESULT Channel::LoadSample(char* fileName)
{
	FMOD_RESULT result = FMOD_OK;
	sample1->CreateSampleFromFile(fileName, fmodSystem);
	return result;
}
FMOD_RESULT Channel::PlaySample(){
	sample1->Play(fmodSystem, fmodChannel);
	return FMOD_OK;
}
void Channel::SetVolume(float newVol)
{
	FMOD_RESULT result = fmodChannel->setVolume(newVol);
}
float Channel::GetVolume()
{
	float vol;
	FMOD_RESULT result = fmodChannel->getVolume(&vol);
	return vol;
}
void Channel::SetReverb(){
	FMOD::DSP        *dsphighpass   = 0;
	FMOD_RESULT result = fmodSystem->createDSPByType(FMOD_DSP_TYPE_HIGHPASS, &dsphighpass);
	result = fmodSystem->addDSP(dsphighpass, 0);
}