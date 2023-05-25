using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;

namespace Controllers
{
    public class HomeController : Controller
    {
        private static List<int> hoursStudiedBuffer = new List<int>();
        private static List<double> chanceBuffer = new List<double>();

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CalculateChance(int hoursStudied)
        {
            double maxHours = 100; // Cantidad máxima de horas para aprobar
            double successRate = 0.8; // Tasa de éxito esperada (80%)

            // Calcula el chance basado en la cantidad de horas estudiadas y la tasa de éxito
            double chance = hoursStudied / maxHours * successRate * 100;
            chance = Math.Min(chance, 100); // Limit chance to maximum 100%

            hoursStudiedBuffer.Add(hoursStudied);
            chanceBuffer.Add(chance);

            TempData["Chance"] = chance.ToString();
            TempData["HoursStudied"] = hoursStudied;

            return RedirectToAction("Index");
        }

        [HttpGet]
        public JsonResult GetMeans()
        {
            double meanHoursStudied = CalculateMean(hoursStudiedBuffer);
            double meanChance = CalculateMean(chanceBuffer);

            return Json(new { MeanHoursStudied = meanHoursStudied, MeanChance = meanChance });
        }

        private double CalculateMean(List<int> values)
        {
            if (values.Count == 0)
                return 0;

            int sum = 0;
            foreach (int value in values)
            {
                sum += value;
            }

            return (double)sum / values.Count;
        }

        private double CalculateMean(List<double> values)
        {
            if (values.Count == 0)
                return 0;

            double sum = 0;
            foreach (double value in values)
            {
                sum += value;
            }

            return sum / values.Count;
        }
    }
}