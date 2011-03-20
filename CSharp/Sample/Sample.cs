using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;

namespace Sample
{
    class Sample
    {
        FMOD.System fmodSystem;
        Sound sample;
        RESULT result;
        String filePath;
        int channelLoadedInto;
        int key;
        bool isLoop;
        
        public Sample(FMOD.System sys)
        {
            this.fmodSystem = sys;
        }

        public void CreateSampleFromFile(String fileName)
        {
            result = fmodSystem.createSound(fileName, MODE.SOFTWARE, ref sample);
        }

        public void Play(FMOD.Channel channel)
        {
            result = fmodSystem.playSound(CHANNELINDEX.REUSE, sample, false, ref channel);
            ERRCHECK(result);
        }

        private void ERRCHECK(FMOD.RESULT result) 
        {
            Console.WriteLine("FMOD error! " + result + " - " + FMOD.Error.String(result);
            Environment.Exit(-1);
        }         
    }
}
