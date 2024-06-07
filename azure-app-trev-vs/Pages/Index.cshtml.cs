using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using azure_app_trev_vs.Data;
using azure_app_trev_vs.Models;

namespace azure_app_trev_vs.Pages
{
    public class IndexModel : PageModel
    {
        private readonly azure_app_trev_vs.Data.ApplicationDbContext _context;
        private readonly ILogger<IndexModel> logger;

        public IndexModel(azure_app_trev_vs.Data.ApplicationDbContext context , ILogger<IndexModel> logger)
        {
            _context = context;
            this.logger = logger;
        }

        public IList<Person> Person { get;set; } = default!;

        public async Task OnGetAsync()
        {
            logger.LogInformation("getting all person details");
            if (_context.Persons != null)
            {
                Person = await _context.Persons.ToListAsync();
                logger.LogInformation("getting all person details: {person}",Person);

            }
        }
    }
}
