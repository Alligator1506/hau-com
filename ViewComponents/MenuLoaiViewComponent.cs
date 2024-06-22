using webtmdt.Data;
using webtmdt.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace webtmdt.ViewComponents
{
	public class MenuLoaiViewComponent : ViewComponent
	{
		private readonly Hshop2024Context db;

		public MenuLoaiViewComponent(Hshop2024Context context) => db = context;

		public IViewComponentResult Invoke()
		{
			var data = db.Loais.Select(lo => new MenuLoaiVM
			{
				MaLoai = lo.MaLoai,
				TenLoai = lo.TenLoai,
				SoLuong = lo.HangHoas.Count
			}).OrderBy(p => p.TenLoai);
			return View("Default", data);
		}
	}
}
