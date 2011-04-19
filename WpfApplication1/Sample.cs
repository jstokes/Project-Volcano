using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using System.Timers;

namespace WpfApplication1
{
    class Sample
    {
        FMOD.System fmodSystem;
        Sound sample = null;
        RESULT result;
        Channel channel;
        bool isSeeking = false;

        public Sample(FMOD.System sys)
        {
            this.fmodSystem = sys;
        }

        public void CreateSampleFromFile(String fileName)
        {
            result = fmodSystem.createSound(fileName, MODE.CREATESAMPLE, ref sample);
            ERRCHECK(result);
        }

        public void Play(FMOD.Channel channel)
        {
            result = fmodSystem.playSound(CHANNELINDEX.REUSE, sample, false, ref channel);
            ERRCHECK(result);
        }

        public void Play(FMOD.Channel channel, uint startTime)
        {
            result = fmodSystem.playSound(CHANNELINDEX.REUSE, sample, true, ref channel);
            ERRCHECK(result);
            result = channel.setPosition(startTime, TIMEUNIT.MS); ERRCHECK(result);
            result = channel.setPaused(false); ERRCHECK(result);
        }

        public void Play(FMOD.Channel fmodChan, Channel channel, uint startTime, uint duration)
        {
            this.channel = channel;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = duration;
            timer.AutoReset = false;
            timer.Enabled = true;
            if (!isSeeking)
            {
                result = fmodSystem.playSound(CHANNELINDEX.REUSE, sample, true, ref fmodChan);
                ERRCHECK(result);
                result = fmodChan.setPosition(startTime, TIMEUNIT.MS); ERRCHECK(result);
                result = fmodChan.setPaused(false); ERRCHECK(result);
            }
            isSeeking = true;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            if (isSeeking)
            {
                this.channel.Pause();
                isSeeking = false;
            }
        }

        public static void ERRCHECK(FMOD.RESULT result)
        {
            if (result != FMOD.RESULT.OK)
            {
                Console.WriteLine("FMOD error! " + result + " - " + FMOD.Error.String(result));
                //Environment.Exit(-1);
            }
        }
        public bool IsSeeking()
        {
            return isSeeking;
        }
        public void StopSeeking()
        {
            isSeeking = false;
        }
    }
}