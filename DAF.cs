using System;
using NAudio.Wave;
using System.Timers;
namespace DAFCommandLine
{

    public class DAF
    {
        private Timer _timer;
        private WaveInEvent _waveSrc;
        private BufferedWaveProvider bufferedWaveProvider;
        private WaveOutEvent player;
        static void Main(string[] args)
        {
            DAF p = new DAF();
            p.run(args);
        }

        public void run(string[] args)
        {
            int MILLIS = 150;
            Console.WriteLine("Anything less than 50 ms is not recommended.");
            if(args.Length < 1)
            { 
                Console.WriteLine("Please enter the amount of delay you wish in milliseconds.");
                MILLIS = int.Parse(Console.ReadLine());
            } else
            {
                try
                {
                    MILLIS = int.Parse(Console.ReadLine());
                } catch (FormatException e) {
                    
                }
                
            }
            if(MILLIS <= 0)
            {
                MILLIS = 50;
                Console.WriteLine("Invalid ms value, setting to default 50 ms.");
            }
            recordAudio(MILLIS);
        }
        public void recordAudio(int delay)
        {
            _waveSrc = new WaveInEvent

            {
                WaveFormat = new WaveFormat(44100, 1),
                BufferMilliseconds = 10
            };
            
            _waveSrc.DataAvailable += RecorderOnDataAvailable;
            player = new WaveOutEvent();
            player.DesiredLatency = delay;
            bufferedWaveProvider = new BufferedWaveProvider(_waveSrc.WaveFormat);
            player.Init(bufferedWaveProvider);

            _timer = new Timer(1);
            _timer.AutoReset = false;
            _timer.Elapsed += play;
            _waveSrc.StartRecording();
            _timer.Start();
           
            Console.WriteLine("{0} ms delay.", delay);
            Console.WriteLine("Press Enter to exit.");
            
            Console.ReadLine();
            

        }
        private void RecorderOnDataAvailable(object sender, WaveInEventArgs e)
        {
            bufferedWaveProvider.AddSamples(e.Buffer, 0, e.BytesRecorded);
        }
       
        private void play(object sender, ElapsedEventArgs e)
        {
            player.Play();

        }
     
    }
}
