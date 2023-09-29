using Microsoft.EntityFrameworkCore;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Movie_review
{
    internal class MovieReview_connect : DbContext
    {
        //create database receive
        public DbSet<Movie> movies { set; get; }
        public DbSet<Review> reviews { set; get; }

        //create connectionstring to database
        private const string connectionString = @"Data Source= MSI;Initial Catalog=DANHGIAPHIM;Integrated Security=True;Encrypt=False; Trusted_Connection=True
                                                 ;TrustServerCertificate=True; MultipleActiveResultSets=True";
        //connect to database
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            optionsBuilder.UseSqlServer(connectionString);
        }

        // fluent API
        override protected void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Movie>(entity =>
            {

                // primary key
                entity.ToTable("TB_Movie").HasKey(e => e.IDMovie);
                //set basic sql datatype
                entity.Property(p => p.IDMovie).HasColumnName("IDMovie").HasColumnType("int");
                entity.Property(p => p.Name).HasColumnName("Name").HasColumnType("nvarchar(200)");
                entity.Property(p => p.Country).HasColumnName("Country").HasDefaultValue("VN").HasColumnType("varchar(50)");
                entity.Property(p => p.Director).HasColumnName("Director").HasColumnType("nvarchar(50)");
                entity.Property(p => p.Year).HasColumnName("Year").HasColumnType("varchar(4)").HasDefaultValue(DateTime.Now.Year);

            });
            modelBuilder.Entity<Review>(entity =>
            {

                // primary key
                entity.ToTable("TB_Review").HasKey(e => e.IDReview);
                entity.HasOne(p => p.Movie).WithMany(r => r.reviews).HasForeignKey(p => p.IDMovie).OnDelete(DeleteBehavior.SetNull);
                // set basic sql datatype
                entity.Property(p => p.IDReview).HasColumnType("int").HasColumnName("IDReview");
                entity.Property(p => p.IDMovie).HasColumnType("int").HasColumnName("IDMovie");
                entity.Property(p => p.Moviereview).HasDefaultValue("None").HasColumnType("nvarchar(200)").HasColumnName("Review");



            });
        }
        // create database
        public async Task<bool> CreateDatabase()
        {
            using (var dbcontext = new MovieReview_connect())
            {
                String databasename = dbcontext.Database.GetDbConnection().Database;
                bool result = await dbcontext.Database.EnsureCreatedAsync();
                return result;
            }
        }
        //delete database
        public async Task DeleteDatabase()
        {

            using (var context = new MovieReview_connect())
            {
                String databasename = context.Database.GetDbConnection().Database;
                bool deleted = await context.Database.EnsureDeletedAsync();
                string deletionInfo = deleted ? "deleted" : "ERROR";
                Console.WriteLine($"{databasename} {deletionInfo}");
            }

        }

        // insert movie
        public static async Task InsertMovie(Movie movie)
        {
            using (var context = new MovieReview_connect())
            {
                await context.movies.AddAsync(movie);
                var rows = await context.SaveChangesAsync();
                Console.WriteLine("movie being stored");
            }
        }
        //insert review
        public static async Task InsertReview(Review review) 
        {
            using (var context = new MovieReview_connect())
            {
                await context.reviews.AddAsync(review);
                var rows = await context.SaveChangesAsync();
                Console.WriteLine("review being stored");

            }
        }
        // get movie data
        public async Task<List<Movie>> getMovies()
        {
            using (var context = new MovieReview_connect())
            {
                var movies = await context.movies.ToListAsync();
                return movies!;
            }
        }
        // get review list
        public async Task<List<Review>> GetReviews()
        {
            using (var context = new MovieReview_connect())
            {
                var reviews = await (from p in context.reviews  select p).ToListAsync();
                return reviews!;
            }
        }
        // get review data
        public async Task<List<Review>> GetReviewssortid(int IDMovie)
        {
            using (var context = new MovieReview_connect())
            {
                var reviews = await (from p in context.reviews where (p.IDMovie == IDMovie) select p).ToListAsync();
                return reviews!;
            }
        }
        // get movie sorted
        public async Task<List<Movie>> getMoviesSorted(string name, string year, string director, string country)
        {
            using (var context = new MovieReview_connect())
            {
                var Movies = await (from p in context.movies
                                    where (p.Country.Contains(country.ToLower()))
                                    where (p.Name.Contains(name.ToLower()))
                                    where (p.Year.Contains(year.ToLower()))
                                    where (p.Director.Contains(director.ToLower()))
                                    select p
                                 ).ToListAsync();
                return Movies!;
            }
        }
        //set specific movie
        public async Task<Movie> ReadMoviesortid(int id)
        {
            using (var context = new MovieReview_connect())
            {
                var Movies = await (from p in context.movies where (p.IDMovie == id) select p).FirstOrDefaultAsync();
                return Movies!;
            }
        }
        //update movie
        public async Task updateMovie(int id)
        {
            using (var context = new MovieReview_connect())
            {
                // context.SetLogging();
                var Movie = await (from e in context.movies where (e.IDMovie == id) select e).FirstOrDefaultAsync();

                if (Movie != null)
                {
                    try
                    {
                        helper.printmovieinfor(Movie);
                        helper.SetMovie(Movie);
                        await context.SaveChangesAsync();
                        Console.WriteLine("Movie being updated");
                    }
                    catch { Console.WriteLine("Error"); }
                }
            }
        }
        //delete movie
        public async Task DeleteMovie(int id)
        {
            using (var context = new MovieReview_connect())
            {
                var Movie = await (from p in context.movies where (p.IDMovie == id) select p).FirstOrDefaultAsync();
                Console.Write($"are you sure (y/n) ? ");
                string input = helper.getstring();
                if (Movie != null)
                {
                    if (input.ToLower() == "y")
                    {
                        context.Remove(Movie);
                        Console.WriteLine($"delete {Movie.IDMovie}");
                        await context.SaveChangesAsync();
                    }
                }
            }
        }

    }
}
