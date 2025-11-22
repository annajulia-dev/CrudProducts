using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ProductsCrud.Data;
using ProductsCrud.Models;

namespace ProductsCrud.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;

        public ProductController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var products = await _context.Products.
                                          Include(p => p.Supplier).
                                          ToListAsync();
            return View( products );
        }

        [HttpGet]
        public IActionResult Create()
        {

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name"); // pega os suppliers _context.Suppliers, Envia o "Id" para o bd, Mostra o "Name"

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(Product newProduct)
        {
            if (ModelState.IsValid)
            {
                _context.Products.Add(newProduct);

                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name"); // caso de erro recarregar o dropdown

            return View(newProduct);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
                return NotFound();

            var productToEdit = await _context.Products.FirstOrDefaultAsync(p => p.Id == id);

            if (productToEdit == null)
                return NotFound();

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", productToEdit.Id);
    
            return View(productToEdit);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product productEdited)
        {
            if (ModelState.IsValid)
            {
                _context.Update(productEdited);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index");
            }

            ViewData["SupplierId"] = new SelectList(_context.Suppliers, "Id", "Name", productEdited.Id);

            return View(productEdited);
        }
        [HttpGet]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
                return NotFound();

            var product = _context.Products.Find(id);

            if (product == null)
                return NotFound();

            return View(product);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeleteConfirmed(int? id)
        {
            var productToDelete = _context.Products.Find(id);
            
            if (productToDelete != null)
            {
                _context.Products.Remove(productToDelete);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction("Index");
        }
    }
}
