using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics;
using System.Text.RegularExpressions;
using Tournament.Data.Context;
using Tournament.Data.Entities;
using Tournament.Extensions;
using Tournament.Models;

namespace Tournament.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly ApplicationContext _applicationContext;
        private readonly EmailService _emailService;

        public HomeController(ILogger<HomeController> logger, ApplicationContext applicationContext, EmailService emailService)
        {
            _logger = logger;
            _applicationContext = applicationContext;
            _emailService = emailService;
        }

        [Route("/")]
        // Ana sayfa
        public IActionResult Index()
        {
            return View();
        }
        [Route("/kurallar")]
        public IActionResult Rules()
        {
            return View();
        }

        [Route("/takýmbasvuru")]
        [HttpGet]
        public IActionResult Contact()
        {
            return View();
        }
        [Route("/bireyselbasvuru")]
        [HttpGet]
        public IActionResult application()
        {
            return View();
        }
        [Route("/sýkayetet")]
        [HttpGet]
        public IActionResult Complaint()
        {
            return View();
        }
        [Route("/sýkayettakýp")]
        [HttpGet]
        public IActionResult Complainttracking()
        {
            return View();
        }
        [Route("/tertipkurulu")]
        [HttpGet]
        public IActionResult Tkkurulu()
        {
            return View();
        }

        [Route("/iletisim")]
        public IActionResult Communication()
        {
            return View();
        }



        [Route("/dýsýplýnkurulu")]
        [HttpGet]
        public IActionResult Dkurulu()
        {
            return View();
        }
        [HttpPost]
        [Route("/")]
        public async Task<IActionResult> Contact([FromForm] TeamApplicationForm form, [Required(ErrorMessage = "Kurum belgesi zorunludur.")] IFormFile? file)
        {
            Regex regex = new Regex(@"[^\d]");
            var phoneNumber = "";
            var isMailSuccess = false;

            if (form.PhoneNumber != null)
            {
                phoneNumber = regex.Replace(form?.PhoneNumber, "");
                form.PhoneNumber = phoneNumber;
                phoneNumber = "+90" + phoneNumber;
            }


            // Doðrulama kontrolleri
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage);
                var errorResult = new { success = false, message = "Doðrulama hatasý", errors = errors };
                return Json(errorResult);
            }

            // Takým adý kontrolü
            var isTeamExist = await _applicationContext.TeamApplicationForms.AnyAsync(x => x.Teamname == form.Teamname);
            if (isTeamExist)
            {
                var errorResult = new { success = false, message = "Ayný Ýsimde Takým Mevcut! Baþka bir isim deneyin." };
                return Json(errorResult);
            }



            // Diðer iþlemler
            try
            {

                var fileId = 0;
                var fileDownloadLink = "";
                if (file != null && file.Length > 0)
                {
                    var uniqueFileName = form.Teamname.Replace(" ", ".") + "_" + file.FileName.Replace(" ", "");

                    var filePath = Path.Combine("wwwroot/uploads", uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    var fileDoc = new FileDocument
                    {
                        FileName = uniqueFileName,
                        FilePath = filePath
                    };
                    _applicationContext.FileDocuments.Add(fileDoc);
                    _applicationContext.SaveChanges();
                    fileDownloadLink = $"https://maraskamuturnuva.com/uploads/{fileDoc.FileName}";
                    fileId = fileDoc.Id;
                }
				#region sendMail

                try
                {
                    var teamApplicationForm = new TeamApplicationForm()
                    {
                        Company = form.Company,
                        Teamname = form.Teamname,
                        TeamSize = form.TeamSize,
                        Email = form.Email,
                        Disclaimer = form.Disclaimer,
                        FileId = fileId,
                        PhoneNumber = phoneNumber,
                        isMailSendSuccess = isMailSuccess
                    };

                    _applicationContext.TeamApplicationForms.Add(teamApplicationForm);
                    _applicationContext.SaveChanges();




                    foreach (var i in form.TeamMembers)
                    {
                        i.TeamApplicationFormId = teamApplicationForm.Id;
                        i.isCaptain = i.Captain == "on";
                        if (i.Iletisim != null)
                        {
                            i.Iletisim = regex.Replace(i.Iletisim, "");
                            i.Iletisim = "+90" + i.Iletisim;
                        }


                    }
                    string emailBody = $@"<!DOCTYPE html>
<html lang=""tr"">
<head>
    <meta charset=""UTF-8"">
    <meta name=""viewport"" content=""width=device-width, initial-scale=1.0"">
    <style>
        body {{
            font-family: 'Arial', sans-serif;
            line-height: 1.6;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}

        .container {{
            max-width: 600px;
            margin: 0 auto;
            padding: 20px;
            background-color: #fff;
        }}

        h1 {{
            color: #333;
        }}

        p {{
            color: #555;
        }}

        table {{
            width: 100%;
            margin-top: 20px;
            border-collapse: collapse;
        }}

        th, td {{
            padding: 10px;
            text-align: left;
            border-bottom: 1px solid #ddd;
        }}

        th {{
            background-color: #f2f2f2;
        }}
    </style>
</head>
<body>
    <div class=""container"">
        <h1>Baþvuru Detaylarý</h1>
        <p><strong>Kurum:</strong> {form.Company}</p>
        <p><strong>Takým Adý:</strong> {form.Teamname}</p>
        <p><strong>Oyuncu Sayýsý:</strong> {form.TeamSize}</p>
        <p><strong>Email:</strong> {form.Email}</p>
        <p><strong>Kurum Ýrtibat Tel no:</strong> {form.PhoneNumber}</p>
        <p><strong>Kurum Belgesi:</strong> <a download href=""{fileDownloadLink}"">Belgeyi Indir</a></p>
         

        <h2>Takým Üyeleri</h2>
        <table>
            <thead>
                <tr>
                    <th>Ýsim</th>
                    <th>Soyisim</th>
                    <th>Tel No</th>
                    <th>TC</th>
                    <th>Yönetici/Kaptan</th>
                </tr>
            </thead>
            <tbody>";

                    foreach (var teamMember in form.TeamMembers)
                    {
                        emailBody += $@"
                <tr>
                    <td>{teamMember?.Name}</td>
                    <td>{teamMember?.Surname}</td>
                    <td>{teamMember?.Iletisim}</td>
                    <td>{teamMember?.TC}</td>
                    <td>{((bool)teamMember?.isCaptain ? "Evet" : "Hayýr")}</td>
                </tr>";
                    }

                    emailBody += @"
            </tbody>
        </table>
       
    </div>
</body>
</html>";
                    var sendMail = await _emailService.SendEmailAsync("ahmtblc1986@gmail.com", "Baþvuru Formu", emailBody);
					var sendMail2 = await _emailService.SendEmailAsync(form.Email, "Baþvuru Formunuz Oluþturuldu", emailBody);
                    isMailSuccess = true;
					TempData["MailInfo"] = "Mail Gönderme iþlemi baþarýlý.";

				}
				catch (Exception ex)
                {
                    TempData["MailInfo"] = ex.Message + "Mail Gönderilirken Hata oluþtu.";

				}


				#endregion



                _applicationContext.TeamMembers.AddRange(form.TeamMembers);
                _applicationContext.SaveChanges();
                var mailMessage = TempData["MailInfo"];

                return Json(new { success = true, message = "Baþvuru baþarýyla alýndý. " + mailMessage }) ;

			}
			catch (Exception ex)
            {
                var errorResult = new { success = false, message = "Bir hata oluþtu. Lütfen tekrar deneyin. " + ex.Message, error = ex.Message };
                return Json(errorResult);
            }
        }


    }
}
