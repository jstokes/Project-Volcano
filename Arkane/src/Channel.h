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
		float volumeBeforeMuting;
		bool isMuted;
		bool isPlaying;
		Sample* sample1;

		bool active;
		bool dsphighpass_active;
		bool dsplowpass_active;
		float highPassLevel,lowPassLevel;
		FMOD::DSP *dspLowPass;
		FMOD::DSP *dspHighPass;
		FMOD::DSP *dspflange;;
		FMOD_RESULT effectResult;

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
		bool IsPlaying();
		void SetPlaying(bool playing);
		bool IsMuted();
		void SetPan(float newPan);
		float GetVolume();
		float GetPan();
		
		float GetHighPass();
		float GetLowPass();
		//Will accept values 1 to 100, the value will be converted to the equivalent hz level between
		//  10 and 22000, with those being the max values possible even if the input passed in exceeds
		void SetHighPass(float);
		//TODO: Rework to exponential
		void SetLowPass(float);

		//Pass in new pan level,-1.0 = Full left, 0.0 = center, 1.0 = full right.
		void SetPanLevel(float);
		float GetPanLevel();
		
};
#endif