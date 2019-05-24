namespace ActiveSolutionsCarRental.Migrations
{
    using System;
    using System.Data.Entity.Migrations;
    
    public partial class Active : DbMigration
    {
        public override void Up()
        {
            AddColumn("dbo.Bookings", "ActiveBooking", c => c.Boolean(nullable: false));
        }
        
        public override void Down()
        {
            DropColumn("dbo.Bookings", "ActiveBooking");
        }
    }
}
