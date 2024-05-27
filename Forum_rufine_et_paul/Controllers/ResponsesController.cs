using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Forum_rufine_et_paul.Models;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Forum_rufine_et_paul.Tools;

namespace Forum_rufine_et_paul.Controllers
{
    public class ResponsesController : Controller
    {
        private readonly Forum23105Context _context;

        public ResponsesController(Forum23105Context context)
        {
            _context = context;
        }

        // GET: Responses
        public async Task<IActionResult> Index(int? Questid, int? pageNumber)
        {
            var pageSize = 5; // Set the page size to a constant or retrieve it from configuration or user preference

            ViewData["catfk"] = _context.Questions.Where(q => q.QuestPk == Questid).Select(q => q.CatFk).FirstOrDefault();
            ViewData["Questfk"] = Questid;

            var questtexte = _context.Questions.Where(q => q.QuestPk == Questid).Select(q => q.QuestText).FirstOrDefault();
            ViewData["QuestText"] = questtexte;


            //Gestion decompte des vues
            var question = await _context.Questions
                .Include(q => q.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.QuestPk == Questid);
            if (question == null)
            {
                return NotFound();
            }

            // Augmenter le nombre de vues
            question.QuestViews = (question.QuestViews ?? 0) + 1;
            await _context.SaveChangesAsync();

            var responses = _context.Responses.Include(r => r.QuestFkNavigation)
                                              .Include(u => u.UserFkNavigation)
                                              .OrderByDescending(r => r.RespDate)
                                              .Where(r => r.QuestFkNavigation.QuestActif == true && r.RespActif == true && r.QuestFk == Questid);

            return View(await PaginatedList<Response>.CreateAsync(responses.AsNoTracking(), pageNumber ?? 1, pageSize));

        }

        // GET: Responses/Details/5
        [Authorize]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Responses == null)
            {
                return NotFound();
            }

            var response = await _context.Responses
                .Include(r => r.QuestFkNavigation)
                .FirstOrDefaultAsync(m => m.RespPk == id);
            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        // GET: Responses/Create
        [Authorize]
        public IActionResult Create(int? questfk)
        {
            var questtexte= _context.Questions.Where(q => q.QuestPk==questfk).Select(q=>q.QuestText).FirstOrDefault();
            ViewData["QuestText"] = questtexte;
            ViewData["QuestFk"] = questfk;


            return View();
        }

        // POST: Responses/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("RespPk,QuestFk,UserFk,RespText,RespDate,RespActif")] Response response)
        {
            if (ModelState.IsValid)
            {
                response.UserFk=User.FindFirstValue(ClaimTypes.NameIdentifier);
                response.RespDate=DateTime.Now;

                _context.Add(response);
                await _context.SaveChangesAsync();
                return RedirectToAction("index", new { Questid = response.QuestFk });
            }
            //ViewData["QuestFk"] = new SelectList(_context.Questions, "QuestPk", "QuestPk", response.QuestFk);
            return View(response);
        }

        // GET: Responses/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
           
            if (id == null || _context.Responses == null)
            {
                return NotFound();
            }

            var response = await _context.Responses.FindAsync(id);
            if (response == null)
            {
                return NotFound();
            }
           // ViewData["QuestFk"] = new SelectList(_context.Questions, "QuestPk", "QuestPk", response.QuestFk);
            return View(response);
        }

        // POST: Responses/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("RespPk,QuestFk,UserFk,RespText,RespDate,RespActif")] Response response)
        {
            if (id != response.RespPk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    response.UserFk=User.FindFirstValue(ClaimTypes.NameIdentifier);
                    response.RespDate=DateTime.Now;

                    _context.Update(response);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ResponseExists(response.RespPk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("index", new { Questid = response.QuestFk });
            }
            //ViewData["QuestFk"] = new SelectList(_context.Questions, "QuestPk", "QuestPk", response.QuestFk);
            return View(response);
        }

        // GET: Responses/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var response = await _context.Responses
                .Include(r => r.UserFkNavigation) 
                .FirstOrDefaultAsync(m => m.RespPk == id);

            if (response == null)
            {
                return NotFound();
            }

            return View(response);
        }

        // POST: Responses/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Responses == null)
            {
                return Problem("Entity set 'Forum23105Context.Responses'  is null.");
            }
            var response = await _context.Responses.FindAsync(id);
            if (response != null)
            {
                response.RespActif=false;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ResponseExists(int id)
        {
          return (_context.Responses?.Any(e => e.RespPk == id)).GetValueOrDefault();
        }
    }
}
