using System.Linq;
using System.Threading.Tasks;
using Core.Entities.Identity;
using Microsoft.AspNetCore.Identity;
namespace Infrastructure.Identity
{
    public class AppIdentityDbContextSeed
    {
         public static async Task SeedUserAsync(UserManager<AppUser> userManager)
        {
            if(!userManager.Users.Any())
            {
                var user = new AppUser
                {
                    DisplayName = "Shivam",
                    Email = "shivam@test.com",
                    UserName = "shivam@test.com",
                    Address = new Address
                    {
                        FirstName = "Shivam",
                        LastName = "Dubey",
                        Street = "Azad Nagar",
                        City = "Gorakhpur",
                        State = "UP",
                        ZipCode = "273001"
                    }
                };

            await userManager.CreateAsync(user, "Pa$$w0rd");
            
            }
        }        
    }
}