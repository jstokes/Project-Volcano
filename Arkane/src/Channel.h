#ifndef CHANNEL_H
#define CHANNEL_H

/*
Channel Class
------------------------------------------------------------
-Must contain an int indicating which channel it is: n
 FMOD_System_PlaySound(system, (FMOD_CHANNELINDEX)n, sound1, 0, &channel);
 This is the call to play the sound

-Channel::PlaySample() will need to get the sample name from 
 Sample::GetSampleName()

-Channel::LoadSample(FMOD_SYSTEM system* ) must be passed the system reference
 so that FMOD_System_CreateSound can pass the system properly.  Do not create 
 a FMOD_SYSTEM variable, because the whole program shares a single system.
*/

#include <fmod.hpp>
#include <fmod_errors.h>
#include "Sample.h"

class Channel {
	
	private:
		FMOD::System*  fmodSystem;
		FMOD::Channel* fmodChannel;
		int channelIndex;
		float volume;
		float pan;
		//Effect effectsList[];
		bool isActive;
		bool isPlaying;
		bool isMuted;
		float volumeBeforeMuting;
		Sample* sample1;

	public:
		//Constructors and Destructors
		Channel();
		~Channel();
		bool Initialize(FMOD::System* fmodSystem, int index);
		//SampleManipulation
		//UnLoadSample();
		//sample = newSample
		//char[] sampleName = &sample.GetSampleName();
		//FMOD_SOUND* sampleRef = &sample.GetSampleReference();
		//FMOD_System_CreateSound(system, sampleName, FMOD_SOFTWARE, 0, &sampleRef);
		FMOD_RESULT LoadSample(char*);
		FMOD_RESULT PlaySample();
		FMOD_RESULT StopSample();
		void LoopSample();
		void StopLoopingSample();
		void UnLoadSample();                          //Checks to make sure there is a sample
		void Mute();
		void UnMute();
		void SetReverb();
		//bool DoesAnEmptyEffectSlotExistIfNotReturnTrueIfSoReturnFalse()
		//AddEffect(Effect * newEffect)
		//RemoveEffect(effectCount)
		//MuteEffect(Effect??????)
		bool IsActive();
		void Deactivate();
		void Activate();
		//Getters and Setters
		void SetVolume(float newVol);
		void SetPan(float newPan);
		float GetVolume();
		float GetPan();
		bool IsPlaying();
		bool IsMuted();
		void SetPlaying(bool);
		
};
#endif