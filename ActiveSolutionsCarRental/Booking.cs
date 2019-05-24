using System;
using System.Linq;
using System.Data.Entity;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivesCarRental
{
    public class Booking
    {

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]   ///Automatically generate key
        [Required]
        public int BookingNr { get; set; }
        [Required]
        [Column(TypeName = "datetime2")]
        public DateTime RentalStart { get; set; }
        [Column(TypeName = "datetime2")]
        public DateTime RentalEnd { get; set; }
        public bool ActiveBooking { get; set; }
        [Required]
        public String CustomerID { get; set; }
        [Required]
        public String CarID { get; set; }

        public static void RentCar() ///The method for renting a car
        {
            using (var db =new ApplicationContext()){

                int choice = 0;
                var carChoice = "";
                Boolean condition = false;
                var customerPersonNr = "";
                var customerName = "";
                var customerEmail = "";
                var customerPhone = "";
            do
            {
            Console.WriteLine("What kind of car would you like to rent? \n 1. Small car \n 2. Combi \n 3. Truck");
            choice = Convert.ToInt32(Console.ReadLine());
            } while (choice>3 && choice <1);
                var query = from c in db.Cars   ///The SQL query I started with. Standard and straightforward. The rest of the queries will be written with lambda in mind
                            where c.Busy ==false
                            && c.CarType==choice
                            select c;  
                foreach (var item in query) ///Show every available car
                {
                    Console.WriteLine(item.CarID + " ");
                }
                while (condition==false)    ///Do until the rental registration process has been completed
                {
                    Console.WriteLine("\n Please enter the registration number of the car you would like. See list of availabe vehicles above");
                    carChoice =Console.ReadLine();
                    foreach (var item in query)
                    {
                        if (item.CarID.Equals(carChoice))       ///Checks if chosen car exists in database
                        {
                            condition = true;
                        }
                    }
                    if (condition==false)       ///Chosen car does not exist in database
                    {
                        Console.WriteLine("It seems that the car you've chosen is unavailable. Please choose another.");
                    }
                }
                do      ///Repeat until the customer has entered some type of id, this case a Person number.
                {
                Console.WriteLine("\n Please enter the Personnumber of the customer (required)");
                customerPersonNr = Console.ReadLine();

                } while (customerPersonNr == "");
                Console.WriteLine("\n Please enter the name of the customer");
                customerName = Console.ReadLine();
                Console.WriteLine("\n Please enter the phonenumber of the customer");
                customerPhone=Console.ReadLine();
                Console.WriteLine("\n Please enter the customers email adress");
                customerEmail = Console.ReadLine();
                var previousCustomer = db.Customers.SingleOrDefault(x => x.CustomerID == customerPersonNr);
                if (previousCustomer==null)     ///Create new customer in Database if not already existing
                {
                var customer = new Customer { CustomerID = customerPersonNr, Name = customerName, PhoneNr = customerPhone, Email = customerEmail };
                db.Customers.Add(customer);
                }
                else        ///Update customer info in database
                {
                    previousCustomer.Name = customerName;
                    previousCustomer.PhoneNr = customerPhone;
                    previousCustomer.Email = customerEmail;
                }
                Console.WriteLine("\n Please enter time and date MM/dd/yyyy HH:mm");
                var date=Convert.ToDateTime(Console.ReadLine());    ///Converts the input to datetime format
                var booking = new Booking {RentalStart = date, CustomerID=customerPersonNr, CarID=carChoice, ActiveBooking=true };  ///Add the booking to the database
                db.Bookings.Add(booking);

                var user = db.Cars.Single(x => x.CarID == carChoice);
                user.Busy = true;   ///Set car to busy
                db.SaveChanges();
                var bookingNr = db.Bookings.Single(x => x.CarID == carChoice && x.ActiveBooking==true); ///Find out what the automatically generated bookingNr is
                Console.WriteLine("\n Thank you, your rental period has started, the bookingnumber is: " + bookingNr.BookingNr + ("\n Please press any key to return to menu")); ///Shows bookingNr
                Console.ReadKey();
                Console.Clear();

            }
        }
        public static void ReturnCar() ///The method for returning a car
        {
            int returnBooking=0;
            Boolean condition = false;
            while (condition==false)
            {

            Console.WriteLine("Please enter the booking number of the car you would like to return");
                returnBooking = Convert.ToInt32(Console.ReadLine());
            using (var db = new ApplicationContext())
            {
                    var rentedBooking = db.Bookings.SingleOrDefault(x => x.BookingNr == returnBooking && x.ActiveBooking==true); ///Makes sure that the bookingnumber is active in the database

                    if (returnBooking == 0 || rentedBooking==null)  ///It it isn't, error
                {
                    Console.WriteLine("Error: Wrong Booking number or the car has already been returned \n");
                }
                    else
                    {
                        int currentMileage;
                        int mileageDifference;
                        TimeSpan days;
                        int difference;
                        Double cost;
                        int baseKmCost=0;
                        int baseDayCost;
                        var rentedCar = db.Cars.Single(x => x.CarID == rentedBooking.CarID); ///Gets information about the car
                        do          ///Do this loop until the user enters a mileage higher than whats been previously registered
                        {
                        Console.WriteLine("\n Please enter the mileage of the car");
                        currentMileage =Convert.ToInt32(Console.ReadLine());
                            if (currentMileage<rentedCar.Mileage)        
                            {
                                Console.WriteLine("Error, value too low. Please try again. \n");
                            }
                        } while (currentMileage<rentedCar.Mileage);
                        Console.WriteLine("\n Please enter the base day cost");
                        baseDayCost = Convert.ToInt32(Console.ReadLine());
                        if (rentedCar.CarType == 2 || rentedCar.CarType==3)     ///If combi or truck enter km cost
                        {
                         Console.WriteLine("\n Please enter the base km cost");
                         baseKmCost=Convert.ToInt32( Console.ReadLine());
                        }
                        days = DateTime.Now - rentedBooking.RentalStart;
                        difference=Convert.ToInt32(days.Days);
                        switch (rentedCar.CarType)
                        {
                            case 1:                         ///Calculations of the cost for a small car
                                cost = baseDayCost * difference;
                                condition = true;
                                break;
                            case 2:                         ///Calculations of the cost for a combi
                                mileageDifference = currentMileage - rentedCar.Mileage;
                                cost = (baseDayCost * difference)*1.3;
                                cost += (baseKmCost * mileageDifference);
                                condition = true;
                                Console.WriteLine("\n The cost will be: " + cost + "\n Please press any key to return to menu");
                                Console.ReadKey();
                                break;
                            case 3:                         ///Calculations of the cost for a truck
                                mileageDifference = currentMileage - rentedCar.Mileage;
                                cost = (baseDayCost * difference)*1.5;
                                cost+=(baseKmCost+mileageDifference)*1.5;
                                condition = true;
                                Console.WriteLine("\n The cost will be: " + cost + "\n Please press any key to return to menu");
                                Console.ReadKey();
                                break;
                            default:
                                break;
                        }
                        var car = db.Cars.Single(x => x.CarID == rentedCar.CarID);
                        car.Busy = false;       ///Set the car not to be busy anymore
                        car.Mileage = currentMileage;       ///Update the mileage
                        rentedBooking.ActiveBooking = false;    ///Set the booking to be inactive
                        db.SaveChanges();
                        Console.Clear();
                    }
                }
            }
        }
        public static void showBookings() ///Just a method to show the user which car is booked by whom and what the bookingnumber is.
        {
            using(var db = new ApplicationContext())
                {
                    var bookings = db.Bookings.Where(x=> x.ActiveBooking==true).OrderBy(x => x.BookingNr).ThenBy(x => x.CustomerID); ///Shows only active bookings
                    foreach (var item in bookings)
                    {
                        Console.WriteLine($"{item.BookingNr}\t{item.CustomerID}\t{item.CarID}\t{item.RentalStart}");

                    }
                if (bookings.Count()==0)
                {
                    Console.WriteLine("No active rentals at the moment. Please press any key to return to menu.");
                }
                else
                {
                    Console.WriteLine("Please press any key to return to menu");
                }
                    Console.ReadKey();
                Console.Clear();
            }

        }
        }
    }
