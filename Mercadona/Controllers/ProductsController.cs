using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Mercadona.Context;
using Mercadona.Models;
using Microsoft.AspNetCore.Authorization;

namespace Mercadona.Controllers
{
    [Authorize(Roles = "Administrator")]
    public class ProductsController : Controller
    {
        private readonly MercadonaDbContext _context;

        public ProductsController(MercadonaDbContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var mercadonaDbContext = _context.Products.Include(p => p.Category).Include(p => p.Offer);
            return View(await mercadonaDbContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            ViewBag.AllOffers = new SelectList(
                _context.Offers.Select(o => new SelectListItem
                {
                    Value = o.Id.ToString(),
                    Text = $"{o.Discount}% du {o.StartDate.ToString("dd/MM/yyyy")} au {o.EndDate.ToString("dd/MM/yyyy")}"
                }),
                "Value",
                "Text"
            );

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,Price,Picture,CategoryId,OfferId")] Product product)
        {
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", product.OfferId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Description,Price,Picture,CategoryId,OfferId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(product);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            var modelErrors = ModelState.Values.SelectMany(v => v.Errors);

            foreach (var error in modelErrors)
            {
                var errorMessage = error.ErrorMessage;
                var exception = error.Exception;
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "Name", product.CategoryId);
            ViewData["OfferId"] = new SelectList(_context.Offers, "Id", "Id", product.OfferId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Products == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Offer)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Products == null)
            {
                return Problem("Entity set 'MercadonaDbContext.Products'  is null.");
            }
            var product = await _context.Products.FindAsync(id);
            if (product != null)
            {
                _context.Products.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Products?.Any(e => e.Id == id)).GetValueOrDefault();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOffer(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model.OfferId.HasValue)
                {
                    var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);
                    if (product != null)
                    {
                        product.OfferId = model.OfferId.Value;
                        _context.SaveChanges(); 
                    }

                    return RedirectToAction("Details", new { id = model.Id });
                }
            }

            return View("Details", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult CreateOffer(Product model)
        {
            if (ModelState.IsValid)
            {
                if (model.Offer != null) 
                {
                    var newOffer = new Offer
                    {
                        StartDate = model.Offer.StartDate,
                        EndDate = model.Offer.EndDate,
                        Discount = model.Offer.Discount
                    };

                    _context.Offers.Add(newOffer);
                    _context.SaveChanges();

                    var product = _context.Products.FirstOrDefault(p => p.Id == model.Id);
                    if (product != null)
                    {
                        product.OfferId = newOffer.Id; 
                        _context.SaveChanges(); 
                    }

                    return RedirectToAction("Details", new { id = model.Id });
                }
            }
            else
            {
                // Le modèle a des erreurs de validation.
                // Vous pouvez accéder aux erreurs individuelles comme ceci :


                // Vous pouvez également obtenir une liste de toutes les erreurs de modèle.
                var modelErrors = ModelState.Values.SelectMany(v => v.Errors).ToList();

                // Vous pouvez effectuer des actions appropriées en fonction des erreurs ici.
            }

            return View("Details", model);
        }

    }
}
