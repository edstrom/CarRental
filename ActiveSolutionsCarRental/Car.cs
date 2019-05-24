using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivesCarRental
{
    public class Car
    {
        [Key]
        [Required]
        public String CarID { get; set; }
        [Required]
        public int CarType { get; set; }
        [Required]
        public int Mileage { get; set; }
        [Required]
        public Boolean Busy { get; set; }

        public static void AddCar() ///Add new car to the system
        {
            using (var db = new ApplicationContext())
            {
                var regNr = "null";
                int carType = 0;
                var mileage = 0;
                Console.WriteLine("Please enter the Registration number");
                regNr = Console.ReadLine();
                while (carType != 1 && carType != 2 && carType != 3) ///Do until the car has a valid type
                {
                    Console.WriteLine("\n Please enter the number for the type of car \n 1. Small car \n 2. Combi \n 3. Truck");
                    carType = Convert.ToInt32(Console.ReadLine());
                }
                do      ///Do until the car  has a valid mileage
                {
                    Console.WriteLine("\n Please enter the mileage of the car in KM");
                    mileage = Convert.ToInt32(Console.ReadLine());
                } while (mileage <= 0);
                var car = new Car { CarID = regNr, CarType = carType, Mileage = mileage, Busy=false };  
                db.Cars.Add(car);///Added the new car to the database
                db.SaveChanges();
                Console.WriteLine("\n Thank you, the car is being added to the database, press any key to return to the menu");
                Console.ReadKey();
                return;
            }
        }

        public static void CarStatus()      ///Displays the cars. Their ID, their type, if they're busy and the mileage
        {
            using (var db = new ApplicationContext())
            {
                var cars = db.Cars.OrderBy(x => x.Busy).ThenBy(x => x.CarType); ///Get them from database and sort them by busy and type
                foreach (var item in cars)
                {
                    Console.WriteLine($"{item.CarID}\t{item.CarType}\t{item.Busy}\t{item.Mileage}");

                }
                    Console.ReadKey();
            }
            return;
        }
    }
}
