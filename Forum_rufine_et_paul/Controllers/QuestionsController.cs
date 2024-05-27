using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using Forum_rufine_et_paul.Models;
using Forum_rufine_et_paul.ViewModels;
using Forum_rufine_et_paul.Tools;
using Microsoft.AspNetCore.Authorization;

namespace Forum_rufine_et_paul.Controllers
{
    public class QuestionsController : Controller
    {
        private readonly Forum23105Context _context;

        public QuestionsController(Forum23105Context context)
        {
            _context = context;
        }

        // GET: Questions
        public async Task<IActionResult> Index(int? catid, int? pageNumber, int? custPageSize)
        {
            var currentPage = pageNumber ?? 1;
            var pageSize = custPageSize ?? 5;

            ViewData["custPageSize"] = pageSize;
            ViewData["catfk"] = catid;
            ViewData["ConnectedUser"] = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var questionsQuery = _context.Questions
                                        .Include(r => r.Responses)
                                        .Include(u => u.UserFkNavigation)
                                        .Where(q => catid == null || q.CatFk == catid)
                                        .OrderByDescending(q => q.QuestDate)
                                        .Select(q => new custquest
                                        {
                                                  QuestPk=q.QuestPk,
                                                  CatFk=q.CatFk,
                                                  UserFk=q.UserFk,  
                                                  QuestTitle=q.QuestTitle,
                                                  UserName=q.UserFkNavigation.UserName,
                                                  QuestDate=q.QuestDate,
                                                  TotalReponses=q.Responses.Count,
                                                  QuestViews=q.QuestViews,
                                                  DateDerniereReponse=q.Responses.OrderByDescending(r => r.RespDate).Select(r => r.RespDate).FirstOrDefault()??DateTime.Now,
                                                  DernierPosteur=q.Responses.OrderByDescending(r => r.RespDate).FirstOrDefault().UserFkNavigation.UserName??q.UserFkNavigation.UserName

                                              });

            return View(await PaginatedList<custquest>.CreateAsync(questionsQuery, currentPage, pageSize));
        }

        // GET: Questions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.QuestPk == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // GET: Questions/Create
        [Authorize]
        public IActionResult Create(int? catid)
        {
            ViewData["CatFk"] = catid;
            ViewData["QuestDate"] = DateTime.Now;

            return View();
        }

        // POST: Questions/Create
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("QuestPk,CatFk,UserFk,QuestTitle,QuestText,QuestDate,QuestViews,QuestActif")] Question question)
        {
            if (ModelState.IsValid)
            {
                question.UserFk = User.FindFirstValue(ClaimTypes.NameIdentifier);

                _context.Add(question);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index), new { catid = question.CatFk });
            }

            return View(question);
        }

        // GET: Questions/Edit/5
        [Authorize]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions.FindAsync(id);
            if (question == null)
            {
                return NotFound();
            }

            ViewData["QuestDate"] = DateTime.Now;

            return View(question);
        }

        // POST: Questions/Edit/5
        [HttpPost]
        [Authorize]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("QuestPk,CatFk,UserFk,QuestTitle,QuestText,QuestDate,QuestViews,QuestActif")] Question question)
        {
            if (id != question.QuestPk)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    question.UserFk = User.FindFirstValue(ClaimTypes.NameIdentifier);

                    _context.Update(question);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!QuestionExists(question.QuestPk))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index), new { catid = question.CatFk });
            }

            return View(question);
        }

        // GET: Questions/Delete/5
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var question = await _context.Questions
                .Include(q => q.CatFkNavigation)
                .FirstOrDefaultAsync(m => m.QuestPk == id);
            if (question == null)
            {
                return NotFound();
            }

            return View(question);
        }

        // POST: Questions/Delete/5
        [HttpPost, ActionName("Delete")]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var question = await _context.Questions.FindAsync(id);
            if (question != null)
            {
                question.QuestActif=false;
                //_context.Questions.Remove(question);
                
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool QuestionExists(int id)
        {
            return _context.Questions.Any(e => e.QuestPk == id);
        }
    }
}
