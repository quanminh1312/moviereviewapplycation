using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;
using System.Data.Common;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Data.SqlClient;

namespace Movie_review
{
    internal static class helper
    {
        //create list service
        static public List<Action<MovieReview_connect>> actions = new List<Action<MovieReview_connect>>() { app_service1, app_service2, app_service3, app_service4 };
        static public int getint()
        {
            while (true)
            {
                string tam = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(tam))
                {
                    int tamm = 0;
                    try
                    {
                        tamm = int.Parse(tam);
                        return tamm;
                    }
                    catch { }
                }
                Console.WriteLine("Error, please type again");
            }
        }
        static public string getstring()
        {
            while (true)
            {
                string tam = Console.ReadLine()!;
                if (!string.IsNullOrEmpty(tam))
                {
                    return tam;
                }
                Console.WriteLine("Error, please type again");
            }
        }
        //print movie data
        static public void printmovieinfor(Movie Movie)
        {
            Console.WriteLine("id Movie: " + Movie.IDMovie + " Name: " + Movie.Name + " Year: " + Movie.Year + " Director: " + Movie.Director + " Country: " + Movie.Country);
        }
        //set movie infor
        static public void SetMovie(Movie Movie)
        {
            Console.Write("Movie Name: "); Movie.Name = getstring();
            Console.Write("Movie Year: "); Movie.Year = getint().ToString();
            Console.Write("Movie Director: "); Movie.Director = getstring();
            Console.Write("Movie country: "); Movie.Country = getstring();
        }
        //seeding data
        static public async void dataseeding(MovieReview_connect MovieReview_connect)
        {
            try
            {
                List<Movie> Movies = new List<Movie>();
                using (StreamReader sr = new StreamReader("tb_Movie.json"))
                {
                    string json = sr.ReadToEnd();
                    Movies = JsonSerializer.Deserialize<List<Movie>>(json)!;
                    foreach (Movie item in Movies!)
                    {
                        await MovieReview_connect.InsertMovie(item);
                    }
                }
                List<Review>? Reviews = new List<Review>();
                using (StreamReader sr = new StreamReader("tb_Review.json"))
                {
                    string json = sr.ReadToEnd();
                    Reviews = JsonSerializer.Deserialize<List<Review>>(json);
                    foreach (Review item in Reviews!)
                    {
                        await MovieReview_connect.InsertReview(item);
                    }
                }
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        //print movie page
        static void printpage(List<Movie> movies, int page)
        {
            Console.WriteLine("page: " + page);
            var tam = (from movie in movies
                       select movie).Skip((page-1) * 10).Take(10);
            foreach (Movie item in tam) printmovieinfor(item);
        }
        //movie page
        static public void app_service1(MovieReview_connect MovieReview_connect)
        {
            int page = 1;
            char app_service = ' ';
            do
            {
                var t1 = MovieReview_connect.getMovies();
                t1.Wait();
                List<Movie> Movie = t1.Result;

                int ket = (int)Math.Ceiling((float)Movie.Count / 10);
                printpage(Movie, page);
                Console.WriteLine("~~~~~~~~Page Movie Information~~~~~~~~~");
                Console.WriteLine("1.Next page, press N");
                Console.WriteLine("2.Previous page, press P");
                Console.WriteLine("3.Search Movie, press C");
                Console.WriteLine("4.Go Back, press B");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                try
                {
                    app_service = char.Parse(getstring().ToUpper());
                }
                catch { Console.WriteLine("Error, please type again"); }
                switch (app_service)
                {
                    case 'N':
                        {
                            Console.Clear();
                            if (page != ket) page++;
                            break;
                        }
                    case 'P':
                        {
                            Console.Clear();
                            if (page != 1) page--;
                            break;
                        }
                    case 'B':
                        {
                            Console.Clear();
                            return;
                        }
                    case 'C':
                        {
                            Console.Clear();
                            app_service4(MovieReview_connect);
                            ket = (int)Math.Ceiling((float)Movie.Count / 10);
                            break;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Error, please type again");
                            break;
                        }
                }
            } while (app_service != 'B');
        }
        // new movie
        static public void app_service2(MovieReview_connect MovieReview_connect)
        {
            try
            {
                Console.WriteLine("Please set Movie info: ");
                Movie Movie = new Movie();
                SetMovie(Movie);
                var t1 = MovieReview_connect.InsertMovie(Movie);
            }
            catch (Exception e) { Console.WriteLine(e); }
        }
        //sorting movie
        static public void app_service3(MovieReview_connect MovieReview_connect)
        {
            int app_service = 0;
            string name = "";
            string year = "";
            string director = "";
            string country = "";
            do
            {
                Console.WriteLine("~~~~~~~~Movie Sorting~~~~~~~~~");
                Console.WriteLine("1.set name, press 1");
                Console.WriteLine("2.set year, press 2");
                Console.WriteLine("3.set director, press 3");
                Console.WriteLine("4.set country, press 4");
                Console.WriteLine("5.sorting, press 5");
                Console.WriteLine("6.go back, press 6");
                Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
                Console.WriteLine();
                app_service = getint();
                switch (app_service)
                {
                    case 1:
                        {
                            Console.Clear();
                            Console.Write("Set name: ");
                            name = new string(getstring());
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            Console.Write("Set year: ");
                            year = new string(getint().ToString());
                            break;
                        }
                    case 3:
                        {
                            Console.Clear();
                            Console.Write("Set director: ");
                            director = new string(getstring());
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            Console.Write("Set country: ");
                            country = new string(getstring());
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            try
                            {
                                var t1 = MovieReview_connect.getMoviesSorted(name, year, director, country);
                                t1.Wait();
                                List<Movie> list = t1.Result;
                                if (list == null) Console.WriteLine("Not Found");
                                else foreach (Movie item in list) printmovieinfor(item);
                            }
                            catch { Console.WriteLine("error"); }
                            break;
                        }
                    case 6:
                        {
                            Console.Clear();
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Error, please type again");
                            break;
                        }
                }
            } while (true) ;
        }
        //movie searching
        static public void app_service4(MovieReview_connect MovieReview_connect)
        {
            int app_service = 0;
            Console.Write("Please type ID Movie: ");
            int idMovie = getint();
            Movie Movie = new Movie();
            try
            {
                var t1 = MovieReview_connect.ReadMoviesortid(idMovie);
                t1.Wait();
                Movie = t1.Result;
                if (Movie == null)
                {
                    Console.WriteLine("Not Found");
                    return;
                }
            }
            catch { Console.WriteLine("Error, please type again"); }
            do
            {
                printmovieinfor(Movie);
                Console.WriteLine("~~~~~~~~Movie information~~~~~~~~~");
                Console.WriteLine("1.update infor, press 1");
                Console.WriteLine("2.Delete movie, press 2");
                Console.WriteLine("3.set review, press 3");
                Console.WriteLine("4.see review, press 4");
                Console.WriteLine("5.go back, press 5");
                app_service = getint();
                switch (app_service)
                {
                    case 1:
                        {
                            Console.Clear();
                            var t1 = MovieReview_connect.updateMovie(idMovie);
                            t1.Wait();
                            var t = MovieReview_connect.ReadMoviesortid(idMovie);
                            t1.Wait();
                            Movie = t.Result;
                            break;
                        }
                    case 2:
                        {
                            Console.Clear();
                            var t1 = MovieReview_connect.DeleteMovie(idMovie);
                            t1.Wait();
                            return;
                        }
                    case 3:
                        {
                            try
                            {
                                Console.Clear();
                                Review Review = new Review();
                                Console.Write("Please type your review: ");
                                Review.Moviereview = getstring();
                                Review.IDMovie = idMovie;
                                var t1 = MovieReview_connect.GetReviews();
                                t1.Wait();
                                List<Review> list = t1.Result;
                                Review.IDReview = (list.Count + 1);
                                var t = MovieReview_connect.InsertReview(Review);
                                t.Wait();
                            }
                            catch { Console.WriteLine("Error, please type again"); }
                            break;
                        }
                    case 4:
                        {
                            Console.Clear();
                            var t1 = MovieReview_connect.GetReviewssortid(idMovie);
                            t1.Wait();
                            List<Review> list = t1.Result;
                            if (list == null) Console.WriteLine("Not found");
                            else
                                foreach (var item in list)
                                {
                                    Console.WriteLine("ID: " + item.IDReview + " Review: " + item.Moviereview);
                                }
                            break;
                        }
                    case 5:
                        {
                            Console.Clear();
                            return;
                        }
                    default:
                        {
                            Console.Clear();
                            Console.WriteLine("Error, please type again");
                            break;
                        }
                }
            } while (app_service != '5');
        }
    }
}
