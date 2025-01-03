using Microsoft.EntityFrameworkCore;
using System.Runtime.InteropServices;
using UPLOAD.API.Helpers;
using UPLOAD.API.UnitsOfWork.Interfaces;
using UPLOAD.SHARE.Entities;
using UPLOAD.SHARE.Enums;

namespace UPLOAD.API.Data
{
    public class AlimentadorBaseDeDatos
    {
        private readonly DataContext _context;
        private readonly IUsersUnitOfWork _usersUnitOfWork;
        private readonly IFileStorage _fileStorage;

        public AlimentadorBaseDeDatos(DataContext context, IUsersUnitOfWork usersUnitOfWork, IFileStorage fileStorage)
        {
            _context = context;
            _usersUnitOfWork = usersUnitOfWork;
            _fileStorage = fileStorage;
        }

        public async Task SeedAsync()
        {
            //Aseguramen que haya base de datos EnsureCreatedAsync()
            //corre todas las migraciones pendientes...o se update-database
            await _context.Database.EnsureCreatedAsync();
            //await CheckCountriesFullAsync();
            await checkCountriesAsync();
            await checkCateforiesAsync();
            await CheckRolesAsync();
            await CheckUserAsync("1010", "Gabriel", "Lopez", "lopez.gabriel@yopmail.com", "3434564831", "Venezuela 1165", "gabriel.jpg", UserType.Admin);
        }

        private async Task CheckCountriesFullAsync()
        {
            if (!_context.Countries.Any())
            {
                var countriesStatesCitiesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
                await _context.Database.ExecuteSqlRawAsync(countriesStatesCitiesSQLScript);
            }
        }

        private async Task CheckRolesAsync()
        {
            await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
            await _usersUnitOfWork.CheckRoleAsync(UserType.User.ToString());
        }

        private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, string image, UserType userType)
        {
            var user = await _usersUnitOfWork.GetUserAsync(email);
            if (user == null)
            {
                var city = await _context.Cities.FirstOrDefaultAsync(x => x.Name == "Medellín");
                city ??= await _context.Cities.FirstOrDefaultAsync();

                string filePath;
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    filePath = $"{Environment.CurrentDirectory}\\Images\\users\\{image}";
                }
                else
                {
                    filePath = $"{Environment.CurrentDirectory}/Images/users/{image}";
                }
                var fileBytes = File.ReadAllBytes(filePath);
                var imagePath = await _fileStorage.SaveFileAsync(fileBytes, "jpg", "users");

                user = new User
                {
                    FirstName = firstName,
                    LastName = lastName,
                    Email = email,
                    UserName = email,
                    PhoneNumber = phone,
                    Address = address,
                    Document = document,
                    CityId = 1,
                    UserType = userType,
                    Photo = imagePath,
                };

                await _usersUnitOfWork.AddUserAsync(user, "123456");
                await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
            }

            return user;
        }

        private async Task checkCateforiesAsync()
        {
            if (!_context.Categories.Any())
            {
                _context.Categories.Add(new SHARE.Entities.Category { Name = "7" });
                _context.Categories.Add(new SHARE.Entities.Category { Name = "6" });
                _context.Categories.Add(new SHARE.Entities.Category { Name = "5" });
                _context.Categories.Add(new SHARE.Entities.Category { Name = "4" });
                await _context.SaveChangesAsync();
            }
        }

        private async Task checkCountriesAsync()
        {
            if (!_context.Countries.Any())
            {
                _context.Countries.Add(new Country
                {
                    Name = "Colombia",
                    Provincias = new List<Provincia>()
            {
                new Provincia()
                {
                    Name = "Antioquia",
                    Cities = new List<City>() {
                        new City() { Name = "Medellín" },
                        new City() { Name = "Itagüí" },
                        new City() { Name = "Envigado" },
                        new City() { Name = "Bello" },
                        new City() { Name = "Rionegro" },
                    }
                },
                new Provincia()
                {
                    Name = "Bogotá",
                    Cities = new List<City>() {
                        new City() { Name = "Usaquen" },
                        new City() { Name = "Champinero" },
                        new City() { Name = "Santa fe" },
                        new City() { Name = "Useme" },
                        new City() { Name = "Bosa" },
                    }
                },
            }
                });
                _context.Countries.Add(new Country
                {
                    Name = "Estados Unidos",
                    Provincias = new List<Provincia>()
            {
                new Provincia()
                {
                    Name = "Florida",
                    Cities = new List<City>() {
                        new City() { Name = "Orlando" },
                        new City() { Name = "Miami" },
                        new City() { Name = "Tampa" },
                        new City() { Name = "Fort Lauderdale" },
                        new City() { Name = "Key West" },
                    }
                },
                new Provincia()
                {
                    Name = "Texas",
                    Cities = new List<City>() {
                        new City() { Name = "Houston" },
                        new City() { Name = "San Antonio" },
                        new City() { Name = "Dallas" },
                        new City() { Name = "Austin" },
                        new City() { Name = "El Paso" },
                    }
                },
            }
                });

                _context.Countries.Add(new Country
                {
                    Name = "Argentina",
                    Provincias = new List<Provincia>()
                {
                new Provincia()
                    {
                        Name = "Entre Rios",
                        Cities = new List<City>()
                        {
                            new City() { Name = "Paranà" },
                            new City() { Name = "Diamante" },
                            new City() { Name = "Federal" },
                            new City() { Name = "Chajari" },
                            new City() { Name = "Villaguay" },
                        }
                    },
                new Provincia()
                {
                    Name = "Santa Fe",
                    Cities = new List<City>()
                        {
                            new City() { Name = "Santa Fe" },
                            new City() { Name = "Rosario" },
                        }
                },
            }
                });
            }

            //_context.Countries.Add(new SHARE.Entities.Country { Name = "Brasil" });
            //_context.Countries.Add(new SHARE.Entities.Country { Name = "Colombia" });
            //_context.Countries.Add(new SHARE.Entities.Country { Name = "Chile" });
            //_context.Countries.Add(new SHARE.Entities.Country { Name = "Venezuela" });
            //_context.Countries.Add(new SHARE.Entities.Country { Name = "Paraguay" });
            await _context.SaveChangesAsync();
        }
    }
}