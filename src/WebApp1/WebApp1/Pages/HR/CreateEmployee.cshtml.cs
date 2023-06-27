using DataLibrary;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Text.Encodings.Web;
using System.Drawing;
using QRCoder;
using Image = iText.Layout.Element.Image;

namespace WebApp1.Pages.HR
{
    public class CreateEmployeeModel : PageModel
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UrlEncoder _urlEncoder;

        private readonly IWebHostEnvironment _env;
        public string qrCodeImageBase64 { get; set; }
        public CreateEmployeeModel(UserManager<IdentityUser> userManager, RoleManager<IdentityRole> rolemanager,
            SignInManager<IdentityUser> signInManager, UrlEncoder urlencoder, IWebHostEnvironment env)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = rolemanager;
            _urlEncoder = urlencoder;
            _env = env;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public IList<SelectListItem>? DepartmentOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? DepartmentID { get; set; }//user select

        public byte[]? ImgData { get; set; }
        public Image QRCodeImage { get; set; }
        string qrCodeBase64 = null;
        public string QRCodeImageBase64 { get; set; }



        [BindProperty]
        public bool IsMFAChecked { get; set; }

        public class InputModel
        {
            public string UserName { get; set; }

            public string Password { get; set; }

            public string ConfirmPassword { get; set; }
        }

        public void OnGet()
        {
            DepartmentOptions = _roleManager.Roles
            .Select(r => new SelectListItem
            {
                Text = r.Name,
                Value = r.Id
            }).ToList();

        }

        public async Task<IActionResult> OnPostAsync()
        {
          

            if (ModelState.IsValid)
            {
                var user = new IdentityUser { UserName = Input.UserName };
                var result = await _userManager.CreateAsync(user, Input.Password);


                if (!string.IsNullOrWhiteSpace(DepartmentID))
                {
                    var role = await _roleManager.FindByIdAsync(DepartmentID);
                    string Groupname = role?.NormalizedName ?? "";


                    await _userManager.AddToRoleAsync(user, Groupname);
                }
                if (result.Succeeded)
                {


                    if (IsMFAChecked)
                    {

                        user.TwoFactorEnabled = true;
                        await _userManager.UpdateAsync(user);



                        string AuthenticatorUri = await LoadSharedKeyAndQrCodeUriAsync(user);

                        QRCodeGenerator qrGenerator = new QRCodeGenerator();
                        QRCodeData qrCodeData = qrGenerator.CreateQrCode(AuthenticatorUri, QRCodeGenerator.ECCLevel.Q);
                        QRCode qrCode = new QRCode(qrCodeData);
                        Bitmap qrCodeImage = qrCode.GetGraphic(20);

                        ImageConverter converter = new ImageConverter();
                         
                        byte[] qrCodeImageData = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                       
                        string imagePath = Path.Combine(_env.WebRootPath, "QRImage", "qr.jpg");
                       
                        System.IO.File.WriteAllBytes(imagePath, qrCodeImageData);
                        
                        TempData["QRCode"] = "true";
                       

                    }
                    TempData["Success"] = "true";
                }

                
            }


           
            return RedirectToPage("./CreateEmployee");
        }

       

        private async Task<string> LoadSharedKeyAndQrCodeUriAsync(IdentityUser user)
        {
            // Load the authenticator key & QR code URI to display on the form
            var unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            if (string.IsNullOrEmpty(unformattedKey))
            {
                await _userManager.ResetAuthenticatorKeyAsync(user);
                unformattedKey = await _userManager.GetAuthenticatorKeyAsync(user);
            }

            string AuthenticatorUri = GenerateQrCodeUri(user.UserName, unformattedKey);

            return AuthenticatorUri;
        }

        private string GenerateQrCodeUri(string username, string unformattedKey)
        {
            string keyee = "";
            string AuthenticatorUriFormat = "otpauth://totp/{0}:{1}?secret={2}&issuer={0}&digits=6";
            try
            {
                keyee = string.Format(
              AuthenticatorUriFormat,
                  _urlEncoder.Encode("My App"),
                  _urlEncoder.Encode(username),
                  unformattedKey);
            }
            catch (Exception e)
            {
                return keyee;
            }
            return keyee;
        }
       




    }
}
