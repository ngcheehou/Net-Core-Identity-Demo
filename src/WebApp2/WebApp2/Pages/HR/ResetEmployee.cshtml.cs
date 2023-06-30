
using iText.IO.Image;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using QRCoder;
using System.Drawing;
using System.Security.Cryptography;
using System.Text.Encodings.Web;

namespace WebApp2.Pages.HR
{
    public class ResetEmployeeModel : PageModel
    {
        private readonly UserManager<MyEmployee> _userManager;

        private readonly IWebHostEnvironment _env;

        private readonly UrlEncoder _urlEncoder;

        public string ImageName { get; set; }

        public byte[]? ImgData { get; set; }
        public IList<SelectListItem>? EmployeeOptions { get; set; }

        [BindProperty(SupportsGet = true)]
        public string? SelectedEmployeeID { get; set; }//user select
        public ResetEmployeeModel(UserManager<MyEmployee> usermanager, IWebHostEnvironment env, UrlEncoder urlencoder)
        {
            _userManager = usermanager;
            _urlEncoder = urlencoder;
            _env = env;
        }
        public void OnGet()
        {
            EmployeeOptions = _userManager.Users
                .Select(r => new SelectListItem
                {
                    Text = r.UserName,
                    Value = r.Id
                }).ToList();
        }


        public async Task<IActionResult> OnPost()
        {

          
            var user = await _userManager.FindByIdAsync(SelectedEmployeeID);



            if (user != null)
            {
                // Generate a new password reset token
                var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                var password = GeneratePassword(); 


                var resetResult = await _userManager.ResetPasswordAsync(user, token, password);
                if (resetResult.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    TempData["Success"] = "true";
                    TempData["Employee"] = user.UserName;
                    TempData["NewPassword"] = password;
                }


                
                if (user.TwoFactorEnabled)//check if this user is TWO FA enabled 
                {
                    //regenerate need 2D QR Code
                    await _userManager.SetAuthenticationTokenAsync(user, "Google", "secret", null);

                    await _userManager.GenerateNewTwoFactorRecoveryCodesAsync(user, 1);

                    string AuthenticatorUri = await LoadSharedKeyAndQrCodeUriAsync(user);

                    QRCodeGenerator qrGenerator = new QRCodeGenerator();
                    QRCodeData qrCodeData = qrGenerator.CreateQrCode(AuthenticatorUri, QRCodeGenerator.ECCLevel.Q);
                    QRCode qrCode = new QRCode(qrCodeData);
                    Bitmap qrCodeImage = qrCode.GetGraphic(20);

                    ImageConverter converter = new ImageConverter();

                    byte[] qrCodeImageData = (byte[])converter.ConvertTo(qrCodeImage, typeof(byte[]));

                    ImageName = "QR_" + DateTime.Now.ToString("yyyyMMddHHmmssfff") + ".jpg";
                    TempData["ImageName"] = ImageName;
                    string imagePath = Path.Combine(_env.WebRootPath, "Image", ImageName);

                    System.IO.File.WriteAllBytes(imagePath, qrCodeImageData);
                    // Save the image to the specified path
                    TempData["QRCode"] = "true";

                }
            }
           

           // ViewData to trigger the update successful modal.
            return RedirectToPage("./ResetEmployee");


        }



        private async Task<string> LoadSharedKeyAndQrCodeUriAsync(MyEmployee user)
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

        public  string GeneratePassword()
        {
            string LowercaseLetters = "abcdefghijklmnopqrstuvwxyz";
            string UppercaseLetters = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string Digits = "0123456789";
            string SpecialCharacters = "!@#$%^&*()-=_+[]{}|;:,.<>?";
            int length = 12;

            string validCharacters = "";
             
                validCharacters += LowercaseLetters; 
                validCharacters += UppercaseLetters; 
                validCharacters += Digits; 
                validCharacters += SpecialCharacters;

            using (var rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                var passwordChars = randomBytes.Select(b => validCharacters[b % validCharacters.Length]);
                return new string(passwordChars.ToArray());
            }
        }
    }
}
