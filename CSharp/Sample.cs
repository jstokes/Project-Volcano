using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using System.Timers;

namespace arkane
{
    class Sample
    {
        FMOD.System fmodSystem;
        Sound sample = null;
        RESULT result;
        ArkaneChannel channel;
        
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
            result = fmodSystem.playSound(CHANNELINDEX.FREE, sample, true, ref channel);
            ERRCHECK(result);
            result = channel.setPosition(startTime, TIMEUNIT.BUFFERED); ERRCHECK(result);
            result = channel.setPaused(false); ERRCHECK(result);
        }

        public void Play(ArkaneChannel channel, uint startTime, uint duration)
        {
            this.channel = channel;
            System.Timers.Timer timer = new System.Timers.Timer();
            timer.Elapsed += new ElapsedEventHandler(OnTimedEvent);
            timer.Interval = duration;
            timer.AutoReset = false;
            timer.Enabled = true;
        }

        public void OnTimedEvent(object source, ElapsedEventArgs e)
        {
            this.channel.Pause();
        }
        
        public static void ERRCHECK(FMOD.RESULT result) 
        {
            if (result != FMOD.RESULT.OK)
            {
                Console.WriteLine("FMOD error! " + result + " - " + FMOD.Error.String(result));
                //Environment.Exit(-1);
            }
        }
    }
}
