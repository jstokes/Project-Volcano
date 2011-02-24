#include "Channel.h"
#include "Sample.h"

Channel::Channel(void)
{
	sample1 = new Sample();
	highPassLevel=lowPassLevel=0;
	dspLowPass = 0;
    dspHighPass = 0;
	dspflange=0;
	pan=0;
	volume=1;
	isMuted = false;
	
}
Channel::~Channel(void)
{

}
bool Channel::Initialize(FMOD::System* fmodSystemPtr, int index)
{
	channelIndex = index;
	fmodSystem = fmodSystemPtr;
	fmodSystem->getChannel(channelIndex, &fmodChannel);
	//-----LowPass
	fmodSystem->createDSPByType(FMOD_DSP_TYPE_LOWPASS, &dspLowPass);
	fmodSystem->addDSP(dspLowPass, 0);
	dspLowPass->setParameter(FMOD_DSP_LOWPASS_CUTOFF,22000);
	dspLowPass->getActive(&dsplowpass_active);
	//------HighPass
	fmodSystem->createDSPByType(FMOD_DSP_TYPE_HIGHPASS, &dspHighPass);
	fmodSystem->addDSP(dspHighPass, 0);
	dspHighPass->setParameter(FMOD_DSP_HIGHPASS_CUTOFF,10);
	dspHighPass->getActive(&dsphighpass_active);
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
	SetVolume(volume);
	return FMOD_OK;
}
void Channel::SetVolume(float newVol)
{
	if(!isMuted){
		FMOD_RESULT result = fmodChannel->setVolume(newVol);
	}

}
float Channel::GetVolume()
{
	float vol;
	FMOD_RESULT result = fmodChannel->getVolume(&vol);
	return vol;
}
void Channel::SetPlaying(bool playing)
{
	isPlaying = playing;
}
bool Channel::IsMuted(){
	if(isMuted)return true;
	else return false;
}
bool Channel::IsPlaying(){
	if(isPlaying)return true;
	else return false;
}
void Channel::Mute(){
	volumeBeforeMuting=GetVolume();
	SetVolume(0.0f);
	isMuted=true;
}
void Channel::UnMute()
{
	isMuted=false;
}
void Channel::SetReverb(){
	FMOD::DSP *dsphighpass   = 0;
	FMOD_RESULT result = fmodSystem->createDSPByType(FMOD_DSP_TYPE_HIGHPASS, &dsphighpass);
	result = fmodSystem->addDSP(dsphighpass, 0);
}
void Channel::SetHighPass(float input)
{
	//highPassLevel=input;
	highPassLevel=input*220+7.8;
	
	if(highPassLevel<10)highPassLevel=10;
	else if(highPassLevel>22000)highPassLevel=22000;
	dspHighPass->setParameter(FMOD_DSP_HIGHPASS_CUTOFF,highPassLevel);	
}
float Channel::GetHighPass()
{
	return highPassLevel;	
}

void Channel::SetLowPass(float input)
{
	input = 100 - input;
	lowPassLevel=input*220+7.8;
	
	if(lowPassLevel<10)lowPassLevel=10;
	else if(lowPassLevel>22000)lowPassLevel=22000;

	dspLowPass->setParameter(FMOD_DSP_LOWPASS_CUTOFF,lowPassLevel);
	
}
float Channel::GetLowPass()
{
	return lowPassLevel;
}
void Channel::SetPanLevel(float newPanLevel)
{
	pan=newPanLevel;
	fmodChannel->setPan(newPanLevel);
}
float Channel::GetPanLevel()
{
	fmodChannel->getPan(&pan);
	return pan;
}
