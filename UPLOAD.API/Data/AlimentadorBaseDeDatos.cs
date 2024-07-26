
using Microsoft.EntityFrameworkCore;

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
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Argentina" });
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Brasil" });
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Colombia" });
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Chile" });
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Venezuela" });
                _context.Countries.Add(new SHARE.Entities.Country { Name = "Paraguay" });
                await _context.SaveChangesAsync();
            }
        }
    }
}
