using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Mission13.Models;

namespace Mission13.Controllers
{
    public class HomeController : Controller
    {
        private IBowlersRepository _repo { get; set; }
        private ITeamsRepository _repoT { get; set; }

        public HomeController(IBowlersRepository temp, ITeamsRepository tempT)
        {
            _repo = temp;
            _repoT = tempT;
        }

        public IActionResult Index(string teamName)
        {
            var bowlers = _repo.Bowlers
                .Include("Team")
                .Where(x => x.Team.TeamName == teamName || teamName == null)
                .ToList();

            return View(bowlers);
        }

        [HttpGet]
        public IActionResult AddBowler()
        {
            ViewBag.Teams = _repoT.Teams.ToList();

            var bowler = _repo.Bowlers.Select(x => x.BowlerID);

            return View("AddEditBowler");
        }

        [HttpPost]
        public IActionResult AddBowler(Bowler b)
        {
            if (ModelState.IsValid)
            {
                _repo.CreateBowler(b);

                return View("Confirmation", b);
            }

            else //if invalid
            {
                ViewBag.Bowlers = _repo.Bowlers.ToList();

                return View(b);
            }

        }

        [HttpGet]
        public IActionResult Edit(int bowlerid)
        {
            ViewBag.Teams = _repoT.Teams.ToList();

            var bowler = _repo.Bowlers.Include("Team")
                .Where(x => x.BowlerID == bowlerid)
                .FirstOrDefault();

            return View("AddEditBowler", bowler);
        }

        [HttpPost]
        public IActionResult Edit(Bowler b)
        {
            _repo.SaveBowler(b);

            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Delete(int bowlerid)
        {
            var bowler = _repo.Bowlers.Single(x => x.BowlerID == bowlerid);

            return View(bowler);
        }

        [HttpPost]
        public IActionResult Delete(Bowler b)
        {
            _repo.DeleteBowler(b);

            return RedirectToAction("Index");
        }

    }
}
