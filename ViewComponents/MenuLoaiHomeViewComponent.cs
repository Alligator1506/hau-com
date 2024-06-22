using webtmdt.Data;
using webtmdt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ECommerceMVC.ViewComponents
{
    public class MenuLoaiHomeViewComponent : ViewComponent
    {
        private readonly Hshop2024Context db;

        public MenuLoaiHomeViewComponent(Hshop2024Context context) => db = context;

        public IViewComponentResult Invoke()
        {
            var data = db.Loais.Select(lo => new MenuLoaiHomeVM
            {
                MaLoai = lo.MaLoai,
                TenLoai = lo.TenLoai,
            }).OrderBy(p => p.TenLoai);

            return View(data); // Default.cshtml
                               //return View("Default", data);
        }
    }
}
