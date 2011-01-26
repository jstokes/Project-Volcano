
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

#include <fmod.h>
#include <fmod_errors.h>
#include "Sample.h"

class Channel{

	private:
		Sample * sample;
		float volume;
		float pan;
		//Effect effectsList[];
		bool isActive;
		float volumeBeforeMuting;

	public:
		//Constructors and Destructors
		Channel();
		~Channel();

		//SampleManipulation
		//UnLoadSample();
		//sample = newSample
		//char[] sampleName = &sample.GetSampleName();
		//FMOD_SOUND* sampleRef = &sample.GetSampleReference();
		//FMOD_System_CreateSound(system, sampleName, FMOD_SOFTWARE, 0, &sampleRef);
		FMOD_RESULT LoadSample(FMOD_SYSTEM * system , Sample * newSample);
		FMOD_RESULT PlaySample();
		FMOD_RESULT StopSample();
		void LoopSample();
		void StopLoopingSample();
		void UnLoadSample();                          //Checks to make sure there is a sample
		void Mute();
		void Unmute();
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
		
};