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
    public class PetController : Controller
    {
        //create an instance of the DB
        private PetGroomingContext db = new PetGroomingContext();

        // GET: Pet
        public ActionResult List()
        {
            //this method shows the list of pets
            //we will get the pets from the db via a sql query.
            List<Pet> pets = db.Pets.SqlQuery("Select * from Pets").ToList();
            //now return the pets to the view
            return View(pets);
        }

        // GET: Pet/Details/2
        public ActionResult Show(int? id)
        {   //this method shows one particular pet by it's ID. This is referrenced from the in class example
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            //this is a single pet. Hence not using the list type.
            Pet pet = db.Pets.SqlQuery("select * from pets where petid=@PetID", new SqlParameter("@PetID",id)).FirstOrDefault();
            if (pet == null)
            {
                return HttpNotFound();
            }
            return View(pet);
        }

        //THE [HttpPost] Means that this method will only be activated on a POST form submit to the following URL
        //URL: /Pet/Add
        [HttpPost]
        public ActionResult Add(string PetName, Double PetWeight, String PetColor, int SpeciesID, string PetNotes)
        {
            //this a post method i.e when the user submits the form.
            //the values entered by the user need to be passed to the query.
            string query = "insert into pets (PetName, Weight, color, SpeciesID, Notes) values (@PetName,@PetWeight,@PetColor,@SpeciesID,@PetNotes)";
            SqlParameter[] sqlparams = new SqlParameter[5]; //array for sql parameters
            
            sqlparams[0] = new SqlParameter("@PetName",PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@SpeciesID", SpeciesID);
            sqlparams[4] = new SqlParameter("@PetNotes",PetNotes);

            //now run the query by passing the query and parameters
            db.Database.ExecuteSqlCommand(query, sqlparams);

            
            //return to tle list of pets
            return RedirectToAction("List");
        }


        public ActionResult Add()
        {
            //this is the get method and is used to return the view of the add page. We need the list of species so that the user can select one.
            //Hence, we are getting that list from the DB and returning it to the view.

            List<Species> species = db.Species.SqlQuery("select * from Species").ToList();

            return View(species);
        }

        public ActionResult Update(int id)
        {
            //again this is the get method
            //we just need to show details of the PARTICULAR pet the user selected and return it to the view.
            Pet selectedpet =db.Pets.SqlQuery("select * from pets where petid =@PetID", new SqlParameter("@PetID", id)).FirstOrDefault();

            return View(selectedpet);
        }


        [HttpPost]
        public ActionResult Update(int id, string PetName, string PetColor, string PetNotes, double PetWeight)
        {
            //this a post method i.e when the user submits the form.
            //the values entered by the user need to be passed to the query.
            Debug.WriteLine("I'm pulling data of" + PetName);
            string query = "Update pets set PetName= @PetName, Weight= @PetWeight, color= @PetColor, Notes= @PetNotes where petid=@PetID ";
            SqlParameter[] sqlparams = new SqlParameter[5];//array of parameters
            sqlparams[0] = new SqlParameter("@PetName", PetName);
            sqlparams[1] = new SqlParameter("@PetWeight", PetWeight);
            sqlparams[2] = new SqlParameter("@PetColor", PetColor);
            sqlparams[3] = new SqlParameter("@PetNotes", PetNotes);
            sqlparams[4] = new SqlParameter("@PetID", id);
            db.Database.ExecuteSqlCommand(query, sqlparams);
            
            return RedirectToAction("List");
        }
        
        
        public ActionResult Delete(int? id)
        {
            //this method shows the details of the selected pet
            Pet selectedpet = db.Pets.SqlQuery("select * from pets where petid =@PetID", new SqlParameter("@PetID", id)).FirstOrDefault();

            return View(selectedpet);
        }
        
        [HttpPost]
        public ActionResult Delete(int id)
        {   //this methods deletes the pet from the DB.
            string query = "Delete from pets where petid= @PetID";
            SqlParameter sqlparam = new SqlParameter("@PetID", id);
            db.Database.ExecuteSqlCommand(query,sqlparam);
            return RedirectToAction("List");
        }

        //this again is referrenced form the in class example.
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
