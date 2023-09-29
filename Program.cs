namespace Movie_review
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //create database connect
            MovieReview_connect MovieReview_connect = new MovieReview_connect();

            //try to create database
            bool t1 = false;
            try
            {
                t1 = await MovieReview_connect.CreateDatabase();
            }
            catch (Exception e){ Console.WriteLine(e); }

            // check data status needed to seeding
            if (t1) helper.dataseeding(MovieReview_connect);

            //menu function
            int app_service = 0;
            do
            {
                Console.WriteLine("~~~~~~~~Movie Review application~~~~~~~~~");
                Console.WriteLine("1.to Check movie information, press 1");
                Console.WriteLine("2.to Add Movie, press 2");
                Console.WriteLine("3.to Movie sorted , press 3");
                Console.WriteLine("4.quit, press 4");

                app_service = helper.getint();
                Console.Clear();
                if (app_service < 1 || app_service > 4)
                    Console.WriteLine("Error, please type again");
                else
                    helper.actions[app_service-1](MovieReview_connect);
            } while (app_service != 4);
        }
    }
}