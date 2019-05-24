using System;
using System.Data.Entity;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ActivesCarRental
{
    public class Customer
    {
        [Key][Required]
        public String CustomerID { get; set; }
        public String Name { get; set; }
        public String PhoneNr { get; set; }
        public String Email { get; set; }
    }

}
