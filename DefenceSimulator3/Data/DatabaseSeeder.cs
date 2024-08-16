using DefenceSimulator3.Data;
using DefenceSimulator3.Models;
using Microsoft.EntityFrameworkCore;
namespace DefenceSimulator3.Data;
public static class DatabaseSeeder
{
    public static void Seed(DefenceSimulator3Context context)
    {
        // Check if the database is empty
        if (!context.Origin.Any() && !context.WeaponDefence.Any() && !context.Weapon.Any())
        {
            //context.Database.ExecuteSqlRaw("SET IDENTITY_INSERT dbo.WeaponDefence ON");

            // Seed data for Origins
            context.Origin.AddRange(
                new Origin
                {
                    Name = "לבנון",
                    Distance = 100
                },
                new Origin
                {
                    Name = "איראן",
                    Distance = 1600
                },
                new Origin
                {
                    Name = "תימן",
                    Distance = 2377
                },
                new Origin
                {
                    Name = "חמאס",
                    Distance = 70
                }
            );
            context.SaveChanges();


            // Seed data for WeaponDefences
            context.WeaponDefence.Add(
                new WeaponDefence
                {
                    Name = "כיפת ברזל",
                    Speed = 1000,
                    Amount = 0
                }
            );
            context.SaveChanges();


            // Seed data for Weapons
            context.Weapon.AddRange(
                new Weapon
                {
                    Name = "כטבם",
                    Speed = 300,
                    WeaponDefenceId = 2
                },
                new Weapon
                {
                    Name = "טיל",
                    Speed = 880,
                    WeaponDefenceId = 2
                },
                new Weapon
                {
                    Name = "טיל בליסטי",
                    Speed = 18000,
                    WeaponDefenceId = 2
                }
            );

            // Save changes to the database
            context.SaveChanges();
        }
        


    }
}

