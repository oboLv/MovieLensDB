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
                System.Console.WriteLine("5. Add user");
                System.Console.WriteLine("0. Exit");
                var choice = Console.ReadLine();
                // search movies
                if (choice == "1")
                {
                    System.Console.WriteLine("Enter title to search");
                    var searchTerm = Console.ReadLine();
                    using (var db = new MovieContext())
                    {
                        var movies = db.Movies.Where(c => c.Title == searchTerm).ToList();
                        if (movies.Count() > 0)
                        {
                            System.Console.WriteLine("{0, -10}{1, -30}{2, 15}", "ID", "Movie Title", "Release Date");
                            foreach (var movie in movies)
                            {
                                // var movieGenres = db.MovieGenres.Where(c => c.Movie == movie).ToList();
                                
                                // var genreNames = new List<string>();
                                // foreach (var item in movies)
                                // {
                                //     var f = item.MovieGenres.ToList();
                                //     foreach (var q in f)
                                //     {
                                //         genreNames.Add(q.Genre.Name);
                                //     }
                                // }
                                System.Console.WriteLine("{0, -10}{1, -30}{2, 15}", movie.Id, movie.Title, movie.ReleaseDate);
                            }
                        }
                        else
                        {
                            System.Console.WriteLine("No matches found.");
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


                        var movieTitles = db.Movies.Select(m => m.Title).ToList();
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
                // add user w/occupation
                else if (choice == "5")
                {
                    using (var db = new MovieContext())
                    {
                        var newUser = new User();

                        // add age

                        System.Console.WriteLine("Enter age:");

                        var newAge = Convert.ToInt16(Console.ReadLine());
                        newUser.Age = newAge;

                        // add gender

                        var genderBool = false;

                        while (!genderBool)
                        {
                            System.Console.WriteLine("Enter gender (M/F):");

                            var newGender = Console.ReadLine().ToUpper();

                            if (newGender == "M" || newGender == "F")
                            {
                                newUser.Gender = newGender;
                                genderBool = true;
                            }
                            else
                            {
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
                                newUser.ZipCode = newZip;
                                zipBool = true;
                            }
                            else
                            {
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
                                    newUser.Occupation = occ;

                                    newOccBool = true;
                                    break;
                                }
                            }
                            if (!newOccBool)
                            {
                                System.Console.WriteLine("Try again");
                            }
                            else
                            {
                                System.Console.WriteLine($"User age: {newUser.Age}");
                                System.Console.WriteLine($"User gender: {newUser.Gender}");
                                System.Console.WriteLine($"User zipcode: {newUser.ZipCode}");
                                System.Console.WriteLine($"User occupation: {newUser.Occupation.Name}");
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