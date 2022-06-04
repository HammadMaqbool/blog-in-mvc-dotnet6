using BlogTutorial.Data;
using BlogTutorial.Models;
using BlogTutorial.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BlogTutorial.Controllers
{
    public class AdminController : Controller
    {
        AppDbContext db;
        IWebHostEnvironment env;

        public AdminController(AppDbContext _db, IWebHostEnvironment environment)
        {
            db = _db;
            env = environment;
        }
        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                ViewBag.NumberOfPosts = db.Tbl_Post.Count();
                ViewBag.NumberOfusers = db.Tbl_Profile.Count();
                DisplayData();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        public IActionResult AddPost()
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                DisplayData();
                return View();
            }
            else
            {
                //return RedirectToAction("Login", "Admin");
                return Redirect("/Admin/Login?ReturnUrl=/Admin/AddPost");
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddPost(PostVM myPost)
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                DisplayData();
                if (ModelState.IsValid)
                {
                    string ImageName = myPost.Image.FileName.ToString();
                    var FolderPath = Path.Combine(env.WebRootPath, "images");
                    var CompletePicPath = Path.Combine(FolderPath, ImageName);
                    myPost.Image.CopyTo(new FileStream(CompletePicPath, FileMode.Create));

                    Post post = new Post();
                    post.Title = myPost.Title;
                    post.SubTitle = myPost.SubTitle;
                    post.Date = myPost.Date;
                    post.Slug = myPost.Slug;
                    post.Content = myPost.Content;
                    post.Image = ImageName;
                    db.Tbl_Post.Add(post);
                    db.SaveChanges();

                    return RedirectToAction("Index", "Home");
                }

                return View();
            }
                return RedirectToAction("Login", "Admin");
        }

        public IActionResult AllPosts()
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                DisplayData();
                var myAllPosts = db.Tbl_Post;
                return View(myAllPosts);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }

        }

        public IActionResult DeletePost(int Id)
        {
            var PostToDelete = db.Tbl_Post.Find(Id);
            if(PostToDelete!=null)
            {
                db.Remove(PostToDelete);
                db.SaveChanges();
            }
            return RedirectToAction("AllPosts","Admin");
        }

        public  IActionResult UpdatePost(int Id)
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                DisplayData();
                var PosttoUpdate = db.Tbl_Post.Find(Id);
                return View(PosttoUpdate);
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }

        }

        [HttpPost]
        public IActionResult UpdatePost(Post post)
        {
            db.Tbl_Post.Update(post);
            db.SaveChanges();
            return RedirectToAction("AllPosts", "Admin");
        }

        public IActionResult CreateProfile()
        {
            if (HttpContext.Session.GetString("LoginFlag") != null)
            {
                DisplayData();
                return View();
            }
            else
            {
                return RedirectToAction("Login", "Admin");
            }
        }

        [HttpPost]
        public IActionResult CreateProfile(ProfileVM profileVM)
        {
            DisplayData();
            if (ModelState.IsValid)
            {
                string ImageName = profileVM.Image.FileName.ToString();
                var FolderPath = Path.Combine(env.WebRootPath, "images");
                var CompleteImagePath = Path.Combine(FolderPath,ImageName);
                profileVM.Image.CopyTo(new FileStream(CompleteImagePath, FileMode.Create));

                Profile profile = new Profile();
                profile.Name = profileVM.Name;
                profile.FatherName = profileVM.FatherName;
                profile.Bio = profileVM.Bio;
                profile.Image = ImageName;
                profile.username = profileVM.username;
                profile.Password = profileVM.Password;

                db.Tbl_Profile.Add(profile);
                db.SaveChanges();
                return RedirectToAction("Index", "Home");
            }
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(LoginVM loginVM, string? ReturnUrl)
        {
            if(ModelState.IsValid)
            {
                var result = db.Tbl_Profile.Where(opt => opt.username.Equals(loginVM.Username) && opt.Password.Equals(loginVM.Password)).FirstOrDefault();
                if(result!=null)
                {
                    HttpContext.Session.SetInt32("ProfileId",result.Id);
                    HttpContext.Session.SetString("LoginFlag","true");
                    if (ReturnUrl == null)
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                    else
                    {
                        return Redirect(ReturnUrl);
                    }
                    
                }
                ViewData["LoginFlag"] = "Invalid username or Password!";
                return View();

            }
            return View();
        }

        public void DisplayData()
        {
            ViewBag.Profile = db.Tbl_Profile.Where(x => x.Id.Equals(HttpContext.Session.GetInt32("ProfileId"))).AsNoTracking().FirstOrDefault();
        }


        public IActionResult UpdateProfile(int id)
        {
            DisplayData();
            var myProfile = db.Tbl_Profile.Find(id);
            
            ProfileVM pVM = new ProfileVM();
            pVM.Id = id;
            pVM.Name = myProfile.Name;
            pVM.FatherName = myProfile.FatherName;
            pVM.Bio = myProfile.Bio;
            pVM.username = myProfile.username;
            pVM.Password = myProfile.Password;
            pVM.ConfirmPassword = myProfile.Password;
            ViewData["ImageName"] = myProfile.Image;

            return View(pVM);
        }

        [HttpPost]
        public IActionResult UpdateProfile(ProfileVM Profilevm, string? oldPic)
        {
            DisplayData();
            string imageName = null;
            if(ModelState.IsValid)
            {
                if(Profilevm.Image!=null)
                {
                    imageName = Profilevm.Image.FileName.ToString();
                    var FolderPath = Path.Combine(env.WebRootPath, "images");
                    var ImagePath = Path.Combine(FolderPath, imageName);
                    Profilevm.Image.CopyTo(new FileStream(ImagePath, FileMode.Create));
                }

                Profile originalProfile = new Profile();
                originalProfile.username = Profilevm.username;
                originalProfile.Id = Profilevm.Id;
                originalProfile.Name = Profilevm.Name;
                originalProfile.FatherName = Profilevm.FatherName;
                originalProfile.Bio = Profilevm.Bio;
                originalProfile.Password = Profilevm.Password;

                if(!string.IsNullOrEmpty(imageName))
                {
                    originalProfile.Image = imageName;
                }

                else
                {
                    originalProfile.Image = oldPic;
                }

                db.Tbl_Profile.Update(originalProfile);
                db.SaveChanges();
                return RedirectToAction("Index", "Admin");
            }
            if (!string.IsNullOrEmpty(imageName))
            {
                ViewData["ImageName"] = imageName;
            }
            else
            {
                ViewData["ImageName"] = oldPic;
            }

            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("index", "home");
        }



    }
}
