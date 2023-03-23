using AW_lab10.Data;
using AW_lab10.Models;
using AW_lab10.ViewModels;
using Lista11.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;

namespace AW_lab10.Controllers
{
    [Authorize(Policy ="DenyAdmin")]
    public class ShopController : Controller
    {
        private ArticleDbContext _context;
        private Dictionary<int, CartItemViewModel> _cart;
        private List<CartItemViewModel> _basketList;


        private int TERMIN = 7;

        public ShopController(ArticleDbContext context)
        {
            _context = context;
        }


        // GET: Articles
        //wybieranie
        public IActionResult Index([Bind("Id")] Category myCategory = null)
        {
            var list = new SelectList(_context.Category, "Id", "Name");
            //list.Append(0, new SelectListItem("Wybierz kategorie"));
            ViewData["CategoryId"] = list;

            var myDbContext = _context.Article.Include(a => a.Category).Where(a => a.CategoryId == 1);

            //if (myCategory != null)
            //{
            //    myDbContext = _context.Article.Include(a => a.Category).Where(a => a.CategoryId == myCategory.Id);
            //}


            dynamic mymodel = new ExpandoObject();

            //mymodel.Categories = IEnumerable<AW_lab10.Models.Article>;
            mymodel.Articles = myDbContext;

            return View("CategoryChoose");
            //return View("IndexTest", mymodel);
        }

        //po buttonie
        [HttpPost]
        public IActionResult Articles([Bind("Id")] Category myCategory)
        {

            var myDbContext = _context.Article.Include(a => a.Category).Where(a => a.CategoryId == myCategory.Id);
            return View("Articles", myDbContext);
        }

        //cookies

        public Dictionary<int, CartItemViewModel> GetCart()
        {
            string isCart;

            Request.Cookies.TryGetValue("cart", out isCart);

            if (isCart != null)
            {
                _cart = JsonConvert.DeserializeObject<Dictionary<int, CartItemViewModel>>(Request.Cookies["cart"]);
            }
            else
            {
                _cart = new Dictionary<int, CartItemViewModel>();
            }
            return _cart;
        }

        public void SaveCart(Dictionary<int, CartItemViewModel> cart)
        {
            string cartToString = JsonConvert.SerializeObject(cart);

            CookieOptions options = new CookieOptions();

            options.Expires = DateTime.Now.AddDays(TERMIN); // 7 dni

            Response.Cookies.Append("cart", cartToString, options);
        }

        //

        private bool CartElExists(int id)
        {
            _cart = GetCart();
            if (_cart == null) return false;
            return _cart.ContainsKey(id);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Cart()
        {
            _cart = GetCart();
            ViewData["sum"] = GetSum();
           _basketList = new List<CartItemViewModel>();

            var keys = _cart.Keys.ToList();
            var articles = await _context.Article.Include(a => a.Category).Where(a => keys.Contains(a.Id)).ToListAsync();

            foreach (var article in articles)
            {
                _basketList.Add(new CartItemViewModel
                {
                    ArticleId = article.Id,
                    Article = article,
                    Count = _cart[article.Id].Count
                });
            }

            return View(_basketList);
        }

        public List<CartItemViewModel> getBasketList()
        {
            List<CartItemViewModel> basketList = new List<CartItemViewModel>();
            return basketList;
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Summary()
        {
            _cart = GetCart();
            ViewData["sum"] = GetSum();
            List<CartItemViewModel> basketList = new List<CartItemViewModel>();

            var keys = _cart.Keys.ToList();
            var articles = await _context.Article.Include(a => a.Category).Where(a => keys.Contains(a.Id)).ToListAsync();

            foreach (var article in articles)
            {
                basketList.Add(new CartItemViewModel
                {
                    ArticleId = article.Id,
                    Article = article,
                    Count = _cart[article.Id].Count
                });
            }

            return View(basketList);
        }

        [Authorize(Roles = "Client")]
        public async Task<IActionResult> Confirmation(
            string clientName,
            string clientSurname,
            string clientAddress,
            PaymentMethod paymentMethod)
        {

            ConfirmViewModel model = new ConfirmViewModel();

            model.name = clientName;
            model.surname= clientSurname;
            model.address = clientAddress;
            model.paymentMethod = paymentMethod;

            //////////////////////////////
            _cart = GetCart();
            ViewData["sum"] = GetSum();
            List<CartItemViewModel> basketList = new List<CartItemViewModel>();

            var keys = _cart.Keys.ToList();
            var articles = await _context.Article.Include(a => a.Category).Where(a => keys.Contains(a.Id)).ToListAsync();

            foreach (var article in articles)
            {
                basketList.Add(new CartItemViewModel
                {
                    ArticleId = article.Id,
                    Article = article,
                    Count = _cart[article.Id].Count
                });
            }

            model.basket = basketList;

            //cookies
            Response.Cookies.Delete("cart");

            return View(model);
        }


        public double GetSum()
        {
            _cart = GetCart();

            double sum = 0;

            if (_cart == null)
            {
                return 0;
            }

            var keys = _cart.Keys.ToList();
            var articles = _context.Article.ToList();

            foreach (KeyValuePair<int, CartItemViewModel> item in _cart)
            {
                if (articles.Where(a => a.Id == item.Value.ArticleId).Count() > 0)
                {
                    sum += item.Value.Article.Price * item.Value.Count;
                }
            }
            return (double) sum;
        }

        //CRUD dla koszyka

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddCartEl(int id)
        {
            _cart = GetCart();

            if (CartElExists(id))
            {
                _cart[id].Count++;
            }
            else
            {
                CartItemViewModel cartItemVM = new CartItemViewModel
                {
                    ArticleId = id,
                    Article = _context.Article.SingleOrDefault(a => a.Id == id), Count = 1
                };
                _cart.Add(id, cartItemVM);
            }
            SaveCart(_cart);

            return RedirectToAction("Cart");

        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ReduceCartEl(int id)
        {
            _cart = GetCart();

            if (CartElExists(id))
            {
                if (_cart[id].Count <= 1) _cart.Remove(id);
                else _cart[id].Count--;
            }

            SaveCart(_cart);
            return RedirectToAction("Cart");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteCartEl(int id)
        {
            _cart = GetCart();

            if (CartElExists(id))
            {
                _cart.Remove(id);
                SaveCart(_cart);
            }

            return RedirectToAction("Cart");
        }
    }
}
