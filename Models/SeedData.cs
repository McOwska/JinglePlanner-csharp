using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using JinglePlanner.Data;
using System;
using System.Linq;

namespace JinglePlanner.Models;

public static class SeedData{
    public static void Initialize(IServiceProvider serviceProvider){
        using (var context = new JinglePlannerContext(
            serviceProvider.GetRequiredService<
                DbContextOptions<JinglePlannerContext>>())){
            // Look for any movies.
            if (context.User.Any()){
                return;   // DB has been seeded
            }

            context.User.AddRange(
                new User{
                    Id = 1,
                    UserName = "admin",
                    Password = "admin"
                }
            );
            context.SaveChanges();
        }
    }
}