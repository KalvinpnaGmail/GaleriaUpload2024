using UPLOAD.SHARE.Entities;

namespace UPLOAD.API.Data
{
    public class AlimentadorBaseDeDatos
    {
        private readonly DataContext _context;

        public AlimentadorBaseDeDatos(DataContext context)
        {
            _context = context;
        }

        public async Task SeedAsync()
        {
            //Aseguramen que haya base de datos EnsureCreatedAsync() 
            //corre todas las migraciones pendientes...o se update-database
            await _context.Database.EnsureCreatedAsync();
            await checkCountriesAsync();
            await checkCateforiesAsync();

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
