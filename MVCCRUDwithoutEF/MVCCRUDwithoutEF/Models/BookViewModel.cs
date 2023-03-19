using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MVCCRUDwithoutEF.Models
{
    public class BookViewModel
    {

        public BookViewModel() { }

        public BookViewModel(int Id, string Titre, string Auteur, string Info)
        {

            this.Id = Id;
            this.Titre = Titre;
            this.Auteur = Auteur;
            this.Info = Info;

        }
        public int Id { get; set; }
        public string Titre { get; set; }
        public string Auteur { get; set; }
        public string Info { get; set; }


      
    }
}
