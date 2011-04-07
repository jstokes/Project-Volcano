using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FMOD;
using arkane;


// Note: You must change the properties to be Console Application! (just for testing)
namespace Sample
{
    class Test
    {
        FMOD.RESULT result;
        public Test()
        {
            //Create System to Test
            FMOD.System system = new FMOD.System();
            result = Factory.System_Create(ref system); arkane.Sample.ERRCHECK(result);
            system.init(8, INITFLAGS.NORMAL, (IntPtr)null);
            
            // Create Channel to Test on
            ArkaneChannel theChannel = new ArkaneChannel(system);
            theChannel.Initialize(0);
            // Load and Play a sample
            theChannel.LoadSample("../../media/wave.mp3");
            theChannel.PlaySample();

            // So console doesn't exit
            Console.Read();
        }

        static void Main()
        {
            Test tester = new Test();
        }
    }
}
