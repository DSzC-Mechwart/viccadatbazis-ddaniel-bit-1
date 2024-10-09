using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Pages
{
    public class DeleteModel : PageModel
    {
        private readonly ViccAdatbazis.Data.ViccDbContext _context;

        public DeleteModel(ViccAdatbazis.Data.ViccDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Vicc Vicc { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vicc = await _context.Viccek.FirstOrDefaultAsync(m => m.Id == id);

            if (vicc == null)
            {
                return NotFound();
            }
            else
            {
                Vicc = vicc;
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var vicc = await _context.Viccek.FindAsync(id);
            if (vicc != null)
            {
                Vicc = vicc;
                _context.Viccek.Remove(Vicc);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("./Index");
        }
    }
}
