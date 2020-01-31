using System;
using System.Collections.Generic;
using System.Data;
//required for SqlParameter class
using System.Data.SqlClient;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PetGrooming.Data;
using PetGrooming.Models;
using System.Diagnostics;

namespace PetGrooming.Controllers
{
    public class SpeciesController : Controller
    {
        private PetGroomingContext db = new PetGroomingContext();
        // GET: Species
        public ActionResult List()
        {   //this methods return the list of species.
            //we can get the list by running a query
            List<Species> species = db.Species.SqlQuery("Select * from Species").ToList();
            return View(species);
            
        }
        public ActionResult Show(int? id)
        {   //this methods shows a particular species.
            //referrenced from the inclass example
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
            Species species = db.Species.SqlQuery("select * from species where speciesid=@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();
            if (species == null)
            {
                return HttpNotFound();
            }
            return View(species);
        }
        public ActionResult Add()
        {
            //this method simply returns a view where a user can add a species

            return View();
        }
        [HttpPost]
        public ActionResult Add(string SpeciesName)
        {   //this method takes the input from the user, pulls that data from the input fields and adds it to the DB
            string query = "insert into species (Name) values (@SpeciesName)";
            SqlParameter param = new SqlParameter("@SpeciesName", SpeciesName);
            db.Database.ExecuteSqlCommand(query, param);
            return RedirectToAction("List");

        }
        public ActionResult Update(int id)
        {
            //this method shows a particular species which the user can update
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid =@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();

            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Update (int id, string SpeciesName)
        {   //this method takes the input from the user, pulls that data from the input fields and adds it to the DB
            string query= "update species set Name=@SpeciesName where SpeciesID=@SpeciesID";
            SqlParameter[] sqlparams = new SqlParameter[2];
            sqlparams[0] = new SqlParameter("@SpeciesName", SpeciesName);
            sqlparams[1] = new SqlParameter("@SpeciesID", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);

            return RedirectToAction("List");

        }
        public ActionResult Delete(int? id)
        {
            //this method shows a particular species which the user can delete
            Species selectedspecies = db.Species.SqlQuery("select * from species where speciesid =@SpeciesID", new SqlParameter("@SpeciesID", id)).FirstOrDefault();

            return View(selectedspecies);
        }
        [HttpPost]
        public ActionResult Delete(int id)
        {   //this method executes the query to delete the species record
            string query = "Delete from species where speciesid= @SpeciesID";
            SqlParameter sqlparam = new SqlParameter("@SpeciesID", id);
            db.Database.ExecuteSqlCommand(query, sqlparam);
            return RedirectToAction("List");
        }


        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

    }
}