using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using MVCCRUDwithoutEF.Data;
using MVCCRUDwithoutEF.Models;

namespace MVCCRUDwithoutEF.Controllers
{
    public class BookController : Controller
    {
        private readonly IConfiguration _configuration;
        public string chaineConnexion = @"Data Source=localhost; Initial Catalog=bdgplcc; Integrated Security=true";

        public BookController(IConfiguration configuration)
        {
            this._configuration = configuration;
        }


        // GET: Book
        public IActionResult Index()
        {
            DataTable dtbl = new DataTable();
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requette = "SELECT * FROM Livre";
                SqlDataAdapter sqlDa = new SqlDataAdapter(requette, connexion);
                sqlDa.Fill(dtbl);
            }
            return View(dtbl);
        }

       

        // GET: Book/AddOrEdit/
        public IActionResult AddOrEdit(int? id)
        {
           BookViewModel bookViewModel = new BookViewModel();
            if (id > 0)
                bookViewModel = FetchBookByID(id);
           return View(bookViewModel);
        }

        // POST: Book/AddOrEdit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult AddOrEdit(int id, [Bind("Id,Titre,Auteur,Info")] BookViewModel bookViewModel)
        {
            if (ModelState.IsValid)
            {
                using (SqlConnection connexion = new SqlConnection(chaineConnexion))
                {
                    connexion.Open();
                    string requette = "IF (@Id = 0) BEGIN INSERT INTO Livre(Titre, Auteur, Info) VALUES(@Titre, @Auteur, @Info) END ELSE BEGIN UPDATE Livre SET Titre = @Titre, Auteur = @Auteur, Info = @Info WHERE Id = @Id END";
                    SqlCommand sqlCmd = new SqlCommand(requette, connexion);
                    sqlCmd.Parameters.AddWithValue("Id", bookViewModel.Id);
                    sqlCmd.Parameters.AddWithValue("Titre", bookViewModel.Titre);
                    sqlCmd.Parameters.AddWithValue("Auteur", bookViewModel.Auteur);
                    sqlCmd.Parameters.AddWithValue("Info", bookViewModel.Info);
                    sqlCmd.ExecuteNonQuery();
                }
                return RedirectToAction(nameof(Index));
            }
            return View(bookViewModel);
        }

        // GET: Book/Delete/5
        public IActionResult Delete(int? id)
        {
            BookViewModel bookViewModel = FetchBookByID(id);
            return View(bookViewModel);
        }

        // POST: Book/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                connexion.Open();
                string requette = "DELETE Livre WHERE Id=@Id";
                SqlCommand sqlCmd = new SqlCommand(requette, connexion);
                sqlCmd.Parameters.AddWithValue("Id",id);
                sqlCmd.ExecuteNonQuery();
            }
            return RedirectToAction(nameof(Index));
        }

        [NonAction]
        public BookViewModel FetchBookByID(int? id) 
        {
            BookViewModel bookViewModel = new BookViewModel();
            using (SqlConnection connexion = new SqlConnection(chaineConnexion))
            {
                DataTable dtbl = new DataTable();
                connexion.Open();
                string requette = "SELECT * FROM Livre WHERE Id=@Id";
                SqlDataAdapter sqlDa = new SqlDataAdapter(requette, connexion);
                sqlDa.SelectCommand.Parameters.AddWithValue("Id", id);
                sqlDa.Fill(dtbl);
                if(dtbl.Rows.Count == 1)
                {
                    bookViewModel.Id =Convert.ToInt32(dtbl.Rows[0]["Id"].ToString());
                    bookViewModel.Titre = dtbl.Rows[0]["Titre"].ToString();
                    bookViewModel.Auteur = dtbl.Rows[0]["Auteur"].ToString();
                    bookViewModel.Info= dtbl.Rows[0]["Info"].ToString();
                }
                return bookViewModel;
            }
        }
    }
}
