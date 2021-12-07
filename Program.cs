using System;
using System.Collections.Generic;
using System.Linq;
using MovieLensDB.Context;
using MovieLensDB.DataModels;

namespace MovieLensDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {


                System.Console.WriteLine("1. Search movies");
                System.Console.WriteLine("2. Add movie");
                System.Console.WriteLine("3. Update movie");
                System.Console.WriteLine("4. Delete movie");
                System.Console.WriteLine("5. Exit");
                var choice = Console.ReadLine();
                // search movies
                if (choice == "1")
                {
                    System.Console.WriteLine("Enter title to search");
                    var searchTerm = Console.ReadLine();
                    using (var db = new MovieContext())
                    {
                        var movies = db.Movies.Where(c => c.Title == searchTerm).Take(1).ToList();
                        if (movies.Count() > 0)
                        {
                            System.Console.WriteLine("{0, 6}{1, 30}{2, 15}{3}", "Movie ID", "Movie Title", "Release Date", "Genres");
                            foreach (var movie in movies)
                            {
                                var movieGenres = db.MovieGenres.Where(c => c.Movie == movie).ToList();
                                var genres = new List<Genre>();
                                foreach (var genre in movieGenres)
                                {
                                    genres.Add(genre.Genre);
                                }
                                var genreNames = new List<string>();
                                foreach (var item in genres)
                                {
                                    genreNames.Add(item.Name);
                                }
                                System.Console.WriteLine("{0, 6}{1, 30}{2, 15}", movie.Id, movie.Title, movie.ReleaseDate, String.Join(", ", genreNames));
                            }
                        }
                    }
                }
                // add movie
                else if (choice == "2")
                {
                    using (var db = new MovieContext())
                    {

                        var newM = new Movie();
                        newM.Title = Console.ReadLine();


                        var movieTitles = db.Movies.Select(m => m.Title);
                        var exists = movieTitles.ToString().ToLower().Contains(newM.Title.ToLower());
                        if (exists == true)
                        {
                            System.Console.WriteLine("This movie is already in the database.");
                        }
                        else
                        {
                            System.Console.WriteLine("");
                        }
                        newM.ReleaseDate = DateTime.Now;
                        if (exists == false)
                        {
                            db.Movies.Add(newM);
                            db.SaveChanges();
                        }
                    }

                }
                // update movie
                else if (choice == "3")
                {
                    using (var db = new MovieContext())
                    {
                        System.Console.WriteLine("Enter movie ID to update:");
                        var movieID = Convert.ToInt16(Console.ReadLine());
                        var movie = db.Movies.Single(c => c.Id == movieID);
                        System.Console.WriteLine("Enter new title: ");
                        movie.Title = Console.ReadLine();
                        db.SaveChanges();
                    }

                }
                // delete movie
                else if (choice == "4")
                {
                    using (var db = new MovieContext())
                    {
                        System.Console.WriteLine("Enter movie ID to delete:");
                        var movieID = Convert.ToInt16(Console.ReadLine());
                        var movie = db.Movies.Single(c => c.Id == movieID);
                        System.Console.WriteLine("Are you sure you want to delete this movie? (Y/N)");
                        var confirm = Console.ReadLine();

                        var confirmBool = false;
                        while (!confirmBool)
                        {
                            if (confirm.ToLower() == "y")
                            {
                                db.Remove(movie);
                                db.SaveChanges();
                                confirmBool = true;
                            }
                            else if (confirm.ToLower() == "n")
                            {
                                break;
                            }
                            else
                            {
                                System.Console.WriteLine("Try again");
                            }
                        }
                    }
                }
                // leave
                else if (choice == "5")
                {
                    exit = true;
                }
            }
        }
    }
}