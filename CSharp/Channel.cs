using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace CSharp
{
    class Channel
    {
        FMOD.Channel channel;
        int channelIndex;
        float volume = 1;
        float pan = 0;
        bool isActive = false;
        bool isPlaying = false;
        bool isMuted = false;
        float volumeBeforeMuting;
        Sample sample1;
        bool active;
        bool dsphighpass_active;
        bool dsplowpass_active;
        float highPassLevel, lowPassLevel = 0;
        FMOD.DSP dspLowPass, dspHighPass, dspFlange;
        FMOD.RESULT result;
        FMOD.System fmodSystem;
        DSPConnection con = new DSPConnection();

        Channel(FMOD.System sys)
        {
            this.fmodSystem = sys;
            this.sample1 = new Sample(fmodSystem);
        }

        public void Initialize(int index)
        {
            channelIndex = index;
            fmodSystem.getChannel(channelIndex, ref channel);
            // -------Low Pass
            fmodSystem.createDSPByType(DSP_TYPE.LOWPASS, ref dspLowPass);
            fmodSystem.addDSP(dspLowPass, ref con);
            dspLowPass.setParameter((int)DSP_LOWPASS.CUTOFF, 0);
            dspLowPass.getActive(ref dsplowpass_active);

            // --------High Pass
            fmodSystem.createDSPByType(DSP_TYPE.HIGHPASS, ref dspHighPass);
            fmodSystem.addDSP(dspHighPass, ref con);
            dspHighPass.setParameter((int)DSP_LOWPASS.CUTOFF, 0);
            dspHighPass.getActive(ref dsphighpass_active);

        }

        public void LoadSample(String fileName)
        {
            sample1.CreateSampleFromFile(fileName);
        }

        public void PlaySample()
        {
            sample1.Play(channel);
            SetVolume(volume);
        }

        public void SetVolume(float newVol)
        {
            if (!isMuted)
            {
                result = channel.setVolume(newVol);
                Sample.ERRCHECK(result);
            }
        }

        public float GetVolume()
        {
            float vol = 0;
            result = channel.getVolume(ref vol);
            return vol;
        }

        public void SetPlaying(bool playing)
        {
            isPlaying = playing;
        }

        public bool IsPlaying()
        {
            return isPlaying;
        }

        public void Mute()
        {
            volumeBeforeMuting = GetVolume();
            SetVolume(0.0f);
            isMuted = true;
        }

        public void SetReverb()
        {
            DSP dsphighpass = null;
            result = fmodSystem.createDSPByType(DSP_TYPE.HIGHPASS, ref dsphighpass);
            Sample.ERRCHECK(result);
            result = fmodSystem.addDSP(dsphighpass, ref con);
            Sample.ERRCHECK(result);
        }

        public void SetHighPass(float input)
        {
            // I don't like magic numbers
            highPassLevel = (float)(input * 220 + 7.8);

            if (highPassLevel < 10) highPassLevel = 10;
            else if (highPassLevel > 22000) highPassLevel = 22000;
            result = dspHighPass.setParameter((int)DSP_HIGHPASS.CUTOFF, highPassLevel);
            Sample.ERRCHECK(result);
        }

        public float GetHighPass()
        {
            return highPassLevel;
        }

        public void SetLowPass(float input)
        {
            input = 100 - input;
            lowPassLevel = (float)(input * 220 + 7.8);
            
            if (lowPassLevel < 10) lowPassLevel = 10;
            else if (lowPassLevel > 22000) lowPassLevel = 22000;

            result = dspLowPass.setParameter((int)DSP_LOWPASS.CUTOFF, lowPassLevel);
            Sample.ERRCHECK(result);
        }

        public float GetLowPass()
        {
            return lowPassLevel;
        }

        public void SetPanLevel(float newPanLevel)
        {
            pan = newPanLevel;
            result = channel.setPan(newPanLevel);
            Sample.ERRCHECK(result);
        }

        public float GetPanLevel()
        {
            result = channel.getPan(ref pan);
            Sample.ERRCHECK(result);
            return pan;
        }

        public float[] GetPCMLevel()
        {
            float[] waveArray = null;
            result = channel.getWaveData(waveArray, 500, 0); Sample.ERRCHECK(result);
            return waveArray;
        }
    }
}
