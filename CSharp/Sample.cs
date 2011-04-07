using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace arkane
{
    class Sample
    {
        FMOD.System fmodSystem;
        Sound sample = null;
        RESULT result;
        
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
