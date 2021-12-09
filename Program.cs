using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using MovieLensDB.Context;
using MovieLensDB.DataModels;
using NLog;

namespace MovieLensDB
{
    class Program
    {
        static void Main(string[] args)
        {
            var exit = false;
            while (!exit)
            {
                string path = Directory.GetCurrentDirectory() + "\\nlog.config";
                var logger = NLog.Web.NLogBuilder.ConfigureNLog(path).GetCurrentClassLogger();


                System.Console.WriteLine("1. Search movies");
                System.Console.WriteLine("2. View all movies");
                System.Console.WriteLine("3. Add movie");
                System.Console.WriteLine("4. Update movie");
                System.Console.WriteLine("5. Delete movie");
                System.Console.WriteLine("6. Add user");
                System.Console.WriteLine("0. Exit");
                var choice = Console.ReadLine();
                // search movies
                if (choice == "1")
                {
                    Console.Clear();
                    logger.Info("Search");
                    System.Console.WriteLine("Enter title to search");
                    var searchTerm = Console.ReadLine();
                    using (var db = new MovieContext())
                    {
                        var movies = db.Movies.Where(c => c.Title.ToUpper().Contains(searchTerm.ToUpper())).ToList();
                        if (movies.Count() > 0)
                        {
                            logger.Trace("Search: {0}", searchTerm);
                            System.Console.WriteLine("{0, -10}{1, -75}{2, 15}", "ID", "Movie Title", "Release Date");
                            foreach (var movie in movies)
                            {
                                // var movieGenres = movie.MovieGenres.ToList();

                                // var genreNames = new List<string>();
                                // foreach (var item in movies)
                                // {
                                //     var f = item.MovieGenres.ToList();
                                //     foreach (var q in f)
                                //     {
                                //         genreNames.Add(q.Genre.Name);
                                //     }
                                // }
                                
                                System.Console.WriteLine("{0, -10}{1, -75}{2, 15}", movie.Id, movie.Title, movie.ReleaseDate);
                            }
                            System.Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                        }
                        else
                        {
                            System.Console.WriteLine("No matches found.");
                            System.Console.WriteLine("Press any key to continue");
                            Console.ReadKey();
                            logger.Trace("No matches found.");
                        }
                    }
                }
                // view all movies
                else if (choice == "2")
                {
                    using (var db = new MovieContext())
                    {
                        Console.Clear();
                        logger.Info("View all");
                        var movies = db.Movies.ToList();
                        var view = 0;
                        var moreBool = false;
                        var failBool = false;
                        while (!moreBool)
                        {


                            System.Console.WriteLine("{0, -10}{1, -30}{2, 15}", "ID", "Movie Title", "Release Date");
                            if (!failBool)
                            {
                                for (int i = 0; i < 10; i++)
                                {
                                    System.Console.WriteLine("{0, -10}{1, -30}{2, 15}", movies[view].Id, movies[view].Title, movies[view].ReleaseDate);
                                    view++;
                                }
                            }
                            System.Console.WriteLine("View more? (Y/N)");
                            var more = Console.ReadLine().ToUpper();
                            if (more == "N")
                            {
                                moreBool = true;
                            }
                            else if (more == "Y")
                            {
                                failBool = false;
                            }
                            else
                            {
                                System.Console.WriteLine("Try again.");
                                failBool = true;
                            }
                        }
                    }
                }

                // add movie
                else if (choice == "3")
                {
                    Console.Clear();
                    logger.Info("Add movie");
                    using (var db = new MovieContext())
                    {
                        System.Console.WriteLine("Enter movie title");
                        var newM = new Movie();
                        newM.Title = Console.ReadLine();


                        var movieTitles = db.Movies.Select(m => m.Title).ToList();
                        var exists = movieTitles.ToString().Contains(newM.Title);
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
                else if (choice == "4")
                {
                    try
                    {
                        Console.Clear();
                        logger.Info("Update");
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
                    catch (InvalidOperationException)
                    {
                        logger.Error("Invalid movie ID");
                        System.Console.WriteLine("This movie does not exist in the database.");
                        System.Console.WriteLine("Press any key to continue");
                        Console.ReadKey();
                    }
                }
                // delete movie
                else if (choice == "5")
                {
                    Console.Clear();
                    logger.Info("Delete");
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
                                logger.Trace("Movie ID {0} deleted", movie.Id);
                                System.Console.WriteLine("Movie deleted.");
                            }
                            else if (confirm.ToLower() == "n")
                            {
                                break;
                            }
                            else
                            {
                                logger.Error("Invalid input");
                                System.Console.WriteLine("Try again");
                            }
                        }
                    }
                }
                // add user w/occupation
                else if (choice == "6")
                {
                    Console.Clear();
                    logger.Info("Add user");
                    using (var db = new MovieContext())
                    {
                        var newUser = new User();

                        // add age

                        System.Console.WriteLine("Enter age:");

                        var newAge = Console.ReadLine();
                        int newAgeInt;
                        var parseAge = Int32.TryParse(newAge, out newAgeInt);
                        if (parseAge)
                        {
                            logger.Trace("User age added");
                            newUser.Age = newAgeInt;
                        }
                        else
                        {
                            logger.Error("Invalid age");
                            System.Console.WriteLine("Please enter a number.");
                        }
                        // add gender

                        var genderBool = false;

                        while (!genderBool)
                        {
                            System.Console.WriteLine("Enter gender (M/F):");

                            var newGender = Console.ReadLine().ToUpper();

                            if (newGender == "M" || newGender == "F")
                            {
                                logger.Trace("User gender added");
                                newUser.Gender = newGender;
                                genderBool = true;
                            }
                            else
                            {
                                logger.Error("Invalid gender");
                                System.Console.WriteLine("Try again");
                            }
                        }

                        // add zip

                        var zipBool = false;

                        while (!zipBool)
                        {
                            System.Console.WriteLine("Enter 5-digit zip code:");

                            var newZip = Console.ReadLine().Trim();

                            if (newZip.Length == 5)
                            {
                                logger.Trace("User zipcode added");
                                newUser.ZipCode = newZip;
                                zipBool = true;
                            }
                            else
                            {
                                logger.Error("Invalid zip");
                                System.Console.WriteLine("Try again");
                            }
                        }

                        // add occupation

                        var occupations = db.Occupations.ToList();
                        var newOccBool = false;

                        while (!newOccBool)
                        {
                            System.Console.WriteLine("Choose occupation:");

                            foreach (var occ in occupations)
                            {
                                System.Console.WriteLine($"{occ.Id}. {occ.Name}");
                            }

                            var newOcc = Convert.ToInt16(Console.ReadLine());

                            foreach (var occ in occupations)
                            {
                                if (newOcc == occ.Id)
                                {
                                    logger.Trace("User occupation added");
                                    newUser.Occupation = occ;

                                    newOccBool = true;
                                    break;
                                }
                            }
                            if (!newOccBool)
                            {
                                logger.Error("Invalid occupation selection");
                                System.Console.WriteLine("Try again");
                            }
                            else
                            {
                                System.Console.WriteLine($"User age: {newUser.Age}");
                                System.Console.WriteLine($"User gender: {newUser.Gender}");
                                System.Console.WriteLine($"User zipcode: {newUser.ZipCode}");
                                System.Console.WriteLine($"User occupation: {newUser.Occupation.Name}");
                                logger.Trace("New user added to database");
                                System.Console.WriteLine("Press any key to continue");
                                Console.ReadKey();
                                db.Add(newUser);
                                db.SaveChanges();
                            }
                        }
                    }
                }
                // leave
                else if (choice == "0")
                {
                    exit = true;
                }
            }
        }
    }
}