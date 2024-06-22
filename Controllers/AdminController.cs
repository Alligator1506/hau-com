using AutoMapper;
using webtmdt.Data;
using webtmdt.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Text;
using webtmdt.ViewModels;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace webtmdt.Controllers
{
    [Authorize]
    public class AdminController : Controller
    {
        private readonly Hshop2024Context _context;

        public AdminController(Hshop2024Context conetxt)
        {
            _context = conetxt;
        }

        // GET: AdminController/Login
        public IActionResult Login()
        {
            return View();
        }

        // POST: AdminController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginAdminVM model)
        {
            if (ModelState.IsValid)
            {
                string hashedPassword = model.Password.ToSHA256Hash(model.Password);
                var admin = await _context.Admins
                    .FirstOrDefaultAsync(a => a.Username == model.Username && a.PasswordHash == hashedPassword);

                if (admin != null)
                {
                    // Đăng nhập thành công, chuyển hướng đến trang quản lý
                    var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, admin.Username),
                // Thêm các claim khác nếu cần
            };

                    var claimsIdentity = new ClaimsIdentity(
                        claims, CookieAuthenticationDefaults.AuthenticationScheme);

                    var authProperties = new AuthenticationProperties
                    {
                        // Cấu hình các thuộc tính xác thực nếu cần
                    };

                    await HttpContext.SignInAsync(
                        "AdminScheme", // Sửa thành AdminScheme để phù hợp với cấu hình Authentication
                        new ClaimsPrincipal(claimsIdentity),
                        authProperties);

                    return RedirectToAction(nameof(Categories)); // Thay thế "ActionName" bằng tên action bạn muốn chuyển hướng
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Tên đăng nhập hoặc mật khẩu không đúng.");
                }
            }
            return View(model);
        }

        // GET: AdminController
        public async Task<ActionResult> Categories()
        {
            var hshop2023Context = _context.HangHoas.Include(h => h.MaLoaiNavigation).Include(h => h.MaNccNavigation);

            var admins = await _context.Admins.ToListAsync();
            ViewBag.Admins = admins;

            return View(await hshop2023Context.ToListAsync());
        }

        // GET: AdminController/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            var admins = await _context.Admins.ToListAsync();

            ViewBag.Admins = admins;

            return View(hangHoa);
        }

        // GET: AdminController/Create
        [Authorize]
        public IActionResult Create()
        {
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai");
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc");
            return View();
        }

        // POST: AdminController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa)
        {
            //hangHoa.MaLoai = 1002;
            //hangHoa.MaNcc = "NK";
            hangHoa.NgaySx = DateTime.Now;

            if (true)
            {
                _context.Add(hangHoa);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // GET: AdminController/Create1
        [Authorize]
        public IActionResult Create1()
        {
            return View();
        }

        // POST: AdminController/Create1
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create1([Bind("TenLoai,TenLoaiAlias,MoTa,Hinh")] Loai loai)
        {
            if (ModelState.IsValid)
            {
                _context.Add(loai);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(loai);
        }

        // GET: AdminController/Create2
        [Authorize]
        public IActionResult Create2()
        {
            return View();
        }

        // POST: AdminController/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create2([Bind("MaNcc,TenCongTy,Logo,NguoiLienLac,Email,DienThoai,DiaChi,MoTa")] NhaCungCap nhaCC)
        {
            if (ModelState.IsValid)
            {
                _context.Add(nhaCC);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Categories));
            }
            return View(nhaCC);
        }

        // GET: AdminController/Edit/{id}
        [Authorize]
        public async Task<IActionResult> Edit(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // POST: AdminController/Edit/{id}
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("MaHh,TenHh,TenAlias,MaLoai,MoTaDonVi,DonGia,Hinh,NgaySx,GiamGia,SoLanXem,MoTa,MaNcc")] HangHoa hangHoa)
        {

            if (id == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(hangHoa);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Categories));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!HangHoaExists(hangHoa.MaHh))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Categories));
            }

            ViewData["MaLoai"] = new SelectList(_context.Loais, "MaLoai", "MaLoai", hangHoa.MaLoai);
            ViewData["MaNcc"] = new SelectList(_context.NhaCungCaps, "MaNcc", "MaNcc", hangHoa.MaNcc);
            return View(hangHoa);
        }

        // GET: AdminController/Delete/5
        [Authorize]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var hangHoa = await _context.HangHoas
                .Include(h => h.MaLoaiNavigation)
                .Include(h => h.MaNccNavigation)
                .FirstOrDefaultAsync(m => m.MaHh == id);
            if (hangHoa == null)
            {
                return NotFound();
            }

            return View(hangHoa);
        }

        // POST: AdminController/Delete/5
        [Authorize]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var hangHoa = await _context.HangHoas.FindAsync(id);
            if (hangHoa != null)
            {
                _context.HangHoas.Remove(hangHoa);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Categories));
        }

        private bool HangHoaExists(int id)
        {
            return _context.HangHoas.Any(e => e.MaHh == id);
        }
    }
}
