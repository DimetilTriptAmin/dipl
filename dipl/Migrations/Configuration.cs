namespace dipl.Migrations
{
    using dipl.Models;
    using dipl.Models.Data;
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Migrations;
    using System.Linq;
    using System.Security;
    using System.Text;

    internal sealed class Configuration : DbMigrationsConfiguration<dipl.Models.Data.PlayerContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
        }

        protected override void Seed(dipl.Models.Data.PlayerContext context)
        {
        }
    }
}
