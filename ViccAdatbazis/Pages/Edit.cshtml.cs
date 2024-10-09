using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ViccAdatbazis.Data;
using ViccAdatbazis.Models;

namespace ViccAdatbazis.Pages
{
    public class EditModel : PageModel
    {
        private readonly ViccAdatbazis.Data.ViccDbContext _context;

        public EditModel(ViccAdatbazis.Data.ViccDbContext context)
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

            var vicc =  await _context.Viccek.FirstOrDefaultAsync(m => m.Id == id);
            if (vicc == null)
            {
                return NotFound();
            }
            Vicc = vicc;
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(Vicc).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ViccExists(Vicc.Id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool ViccExists(int id)
        {
            return _context.Viccek.Any(e => e.Id == id);
        }
    }
}
