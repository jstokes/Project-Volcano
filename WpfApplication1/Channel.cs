using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace WpfApplication1
{
    public class Channel
    {
        FMOD.Channel channel;
        int channelIndex;
        float pan = 0;
        float vol = 1.0f;
        bool isActive = false;
        bool isPlaying = false;
        bool isMuted = false;
        bool isPaused = false;
        uint currentTime;
        float volumeBeforeMuting;
        Sample sample1;
        bool active;
        bool dsphighpass_active;
        bool dsplowpass_active;
        float highPassLevel, lowPassLevel = 0;
        FMOD.DSP dspLowPass, dspHighPass, dspFlange = new FMOD.DSP();
        public FMOD.RESULT result;
        FMOD.System fmodSystem;
        DSPConnection con = new DSPConnection();
        public FMOD.System system;

        public Channel(FMOD.System sys)
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
            SetLowPass(0);

            // --------High Pass
            fmodSystem.createDSPByType(DSP_TYPE.HIGHPASS, ref dspHighPass);
            fmodSystem.addDSP(dspHighPass, ref con);
            SetHighPass(0);

        }

        public void LoadSample(String fileName)
        {
            sample1.CreateSampleFromFile(fileName);
        }

        public void PlaySample()
        {
            if (isPaused)
            {
                sample1.Play(channel, currentTime);
                SetVolume(vol);
                isPlaying = true;
            }
            else
            {
                sample1.Play(channel);
                SetVolume(vol);
                isPlaying = true;
            }
        }

        public void SetVolume(float newVol)
        {
            if (!isMuted)
            {
                vol = newVol;
                result = channel.setVolume(newVol);
                Sample.ERRCHECK(result);
            }
        }

        public float GetVolume()
        {
            float vol = -1;
            result = channel.getVolume(ref vol); Sample.ERRCHECK(result);
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
        public float GetHighPass()
        {
            return highPassLevel;
        }

        public void SetLowPass(float input)
        {
            input = 100 - input;
            input = input / 10;
            if (input > 10) input = 10;
            else if (input < 0) input = 0;

            lowPassLevel = 100 + (float)Math.Pow(Math.E, (1.5 * input));
            Console.Write(lowPassLevel);
            if (lowPassLevel < 400) lowPassLevel = 400;
            else if (lowPassLevel > 22000) lowPassLevel = 22000;

            result = dspLowPass.setParameter((int)DSP_LOWPASS.CUTOFF, lowPassLevel);
            Sample.ERRCHECK(result);
        }
        public void SetHighPass(float input)
        {

            input = input / 10;
            if (input > 9.5) input = (float)9.5;
            else if (input < 0) input = 0;

            highPassLevel = (float)Math.Pow(Math.E, input);

            if (highPassLevel > 15000) highPassLevel = 15000;
            else if (highPassLevel < 0) highPassLevel = 0;
            result = dspHighPass.setParameter((int)DSP_HIGHPASS.CUTOFF, highPassLevel);
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

        public void PlaySoundAt(float start, float duration)
        {
            // TODO
        }

        public void SetLoop(bool isLoop)
        {
            if (isLoop) result = channel.setMode(MODE.LOOP_NORMAL);
            else result = channel.setMode(MODE.LOOP_OFF);
            Sample.ERRCHECK(result);
        }
        public void StopSample()
        {
            result = channel.stop(); Sample.ERRCHECK(result);
            isPlaying = false;
            isPaused = false;
        }
        public uint GetPosition()
        {
            uint time = 0;
            channel.getPosition(ref time, TIMEUNIT.MS);
            return time;
        }

        public void Pause()
        {
            result = channel.getPosition(ref this.currentTime, TIMEUNIT.MS);
            this.StopSample();
            this.isPaused = true;
            Sample.ERRCHECK(result);
        }
        public void PlaySoundAt(uint start, uint duration)
        {
            currentTime = start;
            sample1.Play(channel, this, start, duration);
            SetVolume(vol);
        }
        public bool IsPaused()
        {
            return isPaused;
        }
        public uint GetCurrentTime()
        {
            return currentTime;
        }
        public bool IsSeeking()
        {
            return sample1.IsSeeking();
        }
        public void StopSeeking()
        {
            sample1.StopSeeking();
        }
    }
}