using System.Text.RegularExpressions;
namespace Quorom.Repositories;

public class BannerRepository : IBannerRepository
{
    private readonly string _bannerDirectory;
    private readonly string _imageBasePath = "/assets/banners/"; // Web path
    private readonly Regex _bannerRegex = new Regex(@"^banner\d{3}\.(jpg|png|jpeg)$", RegexOptions.IgnoreCase);
    private readonly ILogger<BannerRepository> _logger;

    public BannerRepository(IWebHostEnvironment webHostEnvironment, ILogger<BannerRepository> logger)
    {
        _bannerDirectory = Path.Combine(webHostEnvironment.WebRootPath, "assets", "banners");
        _logger = logger;
    }

    public string GetRandomBanner()
    {
        string bannerFolder = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "assets", "banners");

        if (!Directory.Exists(bannerFolder))
        {
            Console.WriteLine("ERROR: Banner folder does NOT exist -> " + bannerFolder);
            return "/assets/banners/banner001.jpg"; // Fallback
        }

        string[] files = Directory.GetFiles(bannerFolder, "banner*.jpg");

        if (files.Length == 0)
        {
            Console.WriteLine("ERROR: No banner images found in -> " + bannerFolder);
            return "/assets/banners/banner001.jpg";
        }

        Random rnd = new Random();
        string selectedBanner = files[rnd.Next(files.Length)];

        string relativePath = "/assets/banners/" + Path.GetFileName(selectedBanner);
        Console.WriteLine("Banner Selected: " + relativePath);

        return relativePath;
    }
}
public interface IBannerRepository
{
    string GetRandomBanner();
}


