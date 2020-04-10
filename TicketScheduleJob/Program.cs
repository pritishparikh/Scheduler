using Microsoft.Extensions.Configuration;
using System;
using System.IO;
using System.Timers;

namespace TicketScheduleJob
{
    class Program
    {
        private static string _Connectionstring;

        static void Main(string[] args)
        {

            var builder = new ConfigurationBuilder()
               .SetBasePath(Directory.GetCurrentDirectory())
               .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
               .AddUserSecrets<Program>()
               .AddEnvironmentVariables();

            IConfigurationRoot configuration = builder.Build();
            var mySettingsConfig = new MySettingsConfig();
            configuration.GetSection("MySettings").Bind(mySettingsConfig);

            _Connectionstring = configuration.GetConnectionString("DefaultConnection");
            string interval = mySettingsConfig.IntervalInMinutes;


            Program obj = new Program();
             //obj.GetScheduleDetails();

            double intervalInMinutes = Convert.ToDouble(interval);// 60 * 5000; // milliseconds to one min


            Timer checkForTime = new Timer(intervalInMinutes);
            checkForTime.Elapsed += new ElapsedEventHandler(obj.GetScheduleDetails);
            checkForTime.Enabled = true;


            Console.ReadLine();
        }

        public void GetScheduleDetails(object sender, ElapsedEventArgs e)
        //public void GetScheduleDetails()
        {
            Exceptions exceptions = new Exceptions();
            try
            {
                Console.WriteLine("New Process is going on... please wait...");

                BAL bALobj = new BAL(_Connectionstring);


                bALobj.GetScheduleDetails();

                Console.WriteLine("New Process Complete...");
            }
            catch (Exception ex)
            {
                exceptions.SendErrorToText(ex);
            }

        }
    }
}
