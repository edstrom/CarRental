using System;
using System.Data.Entity;

namespace ActivesCarRental
{
    public class ApplicationContext : DbContext
    {
        public ApplicationContext()
            : base("ActivesCarRental")
        {

        }
        public DbSet<Customer> Customers { get; set; }
        public DbSet<Car> Cars { get; set; }
        public DbSet<Booking> Bookings { get; set; }
    }
    class Program
    {
        static void Main(string[] args)
        {
            bool exit=false;
            while (exit==false)
            {
            String choice;
            Console.WriteLine("Hello and Welcome to CarRental! \n Please choose one of the following options \n 1. Rent a Car \n 2. Return a Car \n 3. Carstatus \n 4. Bookings \n 5. Add a Car \n 6. Exit");
            choice=Console.ReadLine();
               switch (choice)
               {
                    case "1":
                        Console.Clear();
                        Booking.RentCar();
                        break;

                    case "2":
                        Console.Clear();
                        Booking.ReturnCar();
                        break;

                    case "3":
                        Console.Clear();
                        Car.CarStatus();
                        break;
                    case "4":
                        Console.Clear();
                        Booking.showBookings();
                        break;
                    case "5":
                        Console.Clear();
                        Car.AddCar();
                        break;
                    case "6":
                      exit = true;
                        break;

                    default:
                        Console.WriteLine("Error, please enter one of the options mentioned above");
                        break;
               }
            }
        }
         
    }
}
