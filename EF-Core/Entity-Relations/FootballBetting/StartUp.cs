using System;
using FootballBetting.Data;
using Microsoft.EntityFrameworkCore;

namespace FootballBetting
{
    public class StartUp
    {
        static void Main(string[] args)
        {
            FootballBettingContext dbContext = new FootballBettingContext();
            dbContext.Database.Migrate();
            Console.WriteLine("db created");

 
        }
    }
}
