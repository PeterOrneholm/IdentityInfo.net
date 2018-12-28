using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using ActiveLogin.Identity.Swedish;
using IdentityInfo.Core.Testdata;
using Microsoft.AspNetCore.Mvc;
using IdentityInfo.Web.Models;

namespace IdentityInfo.Web.Controllers
{
    public class SwedishController : Controller
    {
        private static readonly Lazy<Task<IEnumerable<SwedishPersonalIdentityNumber>>> Numbers = new Lazy<Task<IEnumerable<SwedishPersonalIdentityNumber>>>(
            async () =>
            {
                var assembly = typeof(SwedishController).GetTypeInfo().Assembly;
                var csvStream = assembly.GetManifestResourceStream("IdentityInfo.Web.Testdata.SwedishPersonalIdentityNumbers_Testdata_181217.csv");
                return await SwedishPersonalIdentityNumbersTestdataParser.ParseCsvAsync(csvStream);
            });

        public async Task<IActionResult> PersonalIdentityNumberList()
        {
            var numbers = await Numbers.Value;
            var viewModel = new SwedishPersonalIdentityNumberListViewModel(numbers);
            return View(viewModel);
        }
    }
}
