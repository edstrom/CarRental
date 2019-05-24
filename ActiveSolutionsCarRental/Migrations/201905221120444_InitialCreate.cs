namespace ActiveSolutionsCarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class InitialCreate : DbMigration
    {
        public override void Up()
        {
            CreateTable(
                "dbo.Bookings",
                c => new
                    {
                        BookingNr = c.Int(nullable: false, identity: true),
                        RentalStart = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        RentalEnd = c.DateTime(nullable: false, precision: 7, storeType: "datetime2"),
                        CustomerID = c.String(nullable: false),
                        CarID = c.String(nullable: false),
                    })
                .PrimaryKey(t => t.BookingNr);
            
            CreateTable(
                "dbo.Cars",
                c => new
                    {
                        CarID = c.String(nullable: false, maxLength: 128),
                        CarType = c.Int(nullable: false),
                        Mileage = c.Int(nullable: false),
                        Busy = c.Boolean(nullable: false),
                    })
                .PrimaryKey(t => t.CarID);
            
            CreateTable(
                "dbo.Customers",
                c => new
                    {
                        CustomerID = c.String(nullable: false, maxLength: 128),
                        Name = c.String(),
                        PhoneNr = c.String(),
                        Email = c.String(),
                    })
                .PrimaryKey(t => t.CustomerID);
            
        }
        
        public override void Down()
        {
            DropTable("dbo.Customers");
            DropTable("dbo.Cars");
            DropTable("dbo.Bookings");
        }
    }
}
