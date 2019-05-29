using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HumaneSociety
{
    public static class Query
    {        
        static HumaneSocietyDataContext db;

        static Query()
        {
            db = new HumaneSocietyDataContext();
        }

        internal static List<USState> GetStates()
        {
            List<USState> allStates = db.USStates.ToList();       

            return allStates;
        }
            
        internal static Client GetClient(string userName, string password)
        {
            Client client = db.Clients.Where(c => c.UserName == userName && c.Password == password).Single();

            return client;
        }

        internal static List<Client> GetClients()
        {
            List<Client> allClients = db.Clients.ToList();

            return allClients;
        }

        internal static void AddNewClient(string firstName, string lastName, string username, string password, string email, string streetAddress, int zipCode, int stateId)
        {
            Client newClient = new Client();

            newClient.FirstName = firstName;
            newClient.LastName = lastName;
            newClient.UserName = username;
            newClient.Password = password;
            newClient.Email = email;

            Address addressFromDb = db.Addresses.Where(a => a.AddressLine1 == streetAddress && a.Zipcode == zipCode && a.USStateId == stateId).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if (addressFromDb == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = streetAddress;
                newAddress.City = null;
                newAddress.USStateId = stateId;
                newAddress.Zipcode = zipCode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                addressFromDb = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            newClient.AddressId = addressFromDb.AddressId;

            db.Clients.InsertOnSubmit(newClient);

            db.SubmitChanges();
        }

        internal static void UpdateClient(Client clientWithUpdates)
        {
            // find corresponding Client from Db
            Client clientFromDb = db.Clients.Where(c => c.ClientId == clientWithUpdates.ClientId).Single();

            // update clientFromDb information with the values on clientWithUpdates (aside from address)
            clientFromDb.FirstName = clientWithUpdates.FirstName;
            clientFromDb.LastName = clientWithUpdates.LastName;
            clientFromDb.UserName = clientWithUpdates.UserName;
            clientFromDb.Password = clientWithUpdates.Password;
            clientFromDb.Email = clientWithUpdates.Email;

            // get address object from clientWithUpdates
            Address clientAddress = clientWithUpdates.Address;

            // look for existing Address in Db (null will be returned if the address isn't already in the Db
            Address updatedAddress = db.Addresses.Where(a => a.AddressLine1 == clientAddress.AddressLine1 && a.USStateId == clientAddress.USStateId && a.Zipcode == clientAddress.Zipcode).FirstOrDefault();

            // if the address isn't found in the Db, create and insert it
            if(updatedAddress == null)
            {
                Address newAddress = new Address();
                newAddress.AddressLine1 = clientAddress.AddressLine1;
                newAddress.City = null;
                newAddress.USStateId = clientAddress.USStateId;
                newAddress.Zipcode = clientAddress.Zipcode;                

                db.Addresses.InsertOnSubmit(newAddress);
                db.SubmitChanges();

                updatedAddress = newAddress;
            }

            // attach AddressId to clientFromDb.AddressId
            clientFromDb.AddressId = updatedAddress.AddressId;
            
            // submit changes
            db.SubmitChanges();
        }
        
        internal static void AddUsernameAndPassword(Employee employee)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.EmployeeId == employee.EmployeeId).FirstOrDefault();

            employeeFromDb.UserName = employee.UserName;
            employeeFromDb.Password = employee.Password;

            db.SubmitChanges();
        }

        internal static Employee RetrieveEmployeeUser(string email, int employeeNumber)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.Email == email && e.EmployeeNumber == employeeNumber).FirstOrDefault();

            if (employeeFromDb == null)
            {
                throw new NullReferenceException();
            }
            else
            {
                return employeeFromDb;
            }
        }

        internal static Employee EmployeeLogin(string userName, string password)
        {
            Employee employeeFromDb = db.Employees.Where(e => e.UserName == userName && e.Password == password).FirstOrDefault();

            return employeeFromDb;
        }

        internal static bool CheckEmployeeUserNameExist(string userName)
        {
            Employee employeeWithUserName = db.Employees.Where(e => e.UserName == userName).FirstOrDefault();

            return employeeWithUserName == null;
        }


        //// TODO Items: ////
        
        // TODO: Allow any of the CRUD operations to occur here
        internal static void RunEmployeeQueries(Employee employee, string crudOperation)
        {
            switch(crudOperation)
            {
                case "create":
                    CreateNewEmployee(employee);
                    break;
                case "delete":
                    DeleteEmployee(employee);
                    break;
                case "read":
                    DisplayEmployee(employee);
                    break;
                case "update":
                    UpdateEmployee(employee);
                    break;
                default:
                    break;
            }
        }
        internal static void CreateNewEmployee(Employee employee)
        {
            Employee employeeToCreate = db.Employees.Where(e => e.FirstName == employee.FirstName && e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber && e.Email == employee.Email).FirstOrDefault();
            if (employeeToCreate == null)
            {
                db.Employees.InsertOnSubmit(employee);
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void DeleteEmployee(Employee employee)
        {
            Employee employeeToDelete = db.Employees.Where(e => e.LastName == employee.LastName && e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToDelete != null)
            {
                db.Employees.DeleteOnSubmit(employeeToDelete);
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void DisplayEmployee(Employee employee)
        {
            Employee employeeToRead = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToRead != null)
            {
                UserInterface.DisplayEmployeeInfo(employeeToRead);
            }
            else
            {
                throw new Exception();
            }
        }
        internal static void UpdateEmployee(Employee employee)
        {
            Employee employeeToUpdate = db.Employees.Where(e => e.EmployeeNumber == employee.EmployeeNumber).FirstOrDefault();
            if (employeeToUpdate != null)
            {
                employeeToUpdate.FirstName = employee.FirstName;
                employeeToUpdate.LastName = employee.LastName;
                employeeToUpdate.Email = employee.Email;
                db.SubmitChanges();
            }
            else
            {
                throw new Exception();
            }
        }

        // TODO: Animal CRUD Operations
        internal static void AddAnimal(Animal animal)
        {
            db.Animals.InsertOnSubmit(animal);
            db.SubmitChanges();
        }    
        internal static Animal GetAnimalByID(int id)
        {
            Animal AnimalToRead = db.Animals.Where(e => e.AnimalId == id).FirstOrDefault();
            return AnimalToRead;
        }       

        internal static void UpdateAnimal(Animal animal, Dictionary<int, string> updates)
        {
            Animal animalToUpdate = db.Animals.Where(e => e.AnimalId == animal.AnimalId).FirstOrDefault();
            //dictionary(1, fluffy)
            
            foreach ( int key in updates.Keys)
            {
                var value = updates[key];

                switch (key)
                {
                    case 1:
                        animalToUpdate.CategoryId = 1;
                            //db.Categories.Select(CategoryId).Where(e => e.Name == value).FirstOrDefault();
                        break;
                    case 2:
                        animalToUpdate.Name = value;
                        break;
                    case 3:
                        animalToUpdate.Age = Convert.ToInt32(value);
                        break;
                    case 4:
                        animalToUpdate.Demeanor = value;
                        break;
                    case 5:
                        animalToUpdate.KidFriendly = Convert.ToBoolean(value);
                        break;
                    case 6:
                        animalToUpdate.PetFriendly = Convert.ToBoolean(value);
                        break;
                    case 7:
                        animalToUpdate.Weight = Convert.ToInt32(value);
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }
            db.SubmitChanges();
        }

        internal static void RemoveAnimal(Animal animal)
        {
            Animal AnimalToDelete = db.Animals.Where(e => e.AnimalId == animal.AnimalId).FirstOrDefault();
            db.Animals.DeleteOnSubmit(AnimalToDelete);
            db.SubmitChanges();
        }

        // TODO: Animal Multi-Trait Search
        internal static IQueryable<Animal> SearchForAnimalByMultipleTraits(Dictionary<int, string> updates) // parameter(s)?
        {
            throw new NotImplementedException();
        }

        // TODO: Misc Animal Things
        internal static int GetCategoryId(string categoryName)
        {

            switch (categoryName)
            {
                case "Cat":
                    Category animalCategory1 = db.Categories.Where(e => e.Name == "Cat").FirstOrDefault();
                    return animalCategory1.CategoryId;
                case "Dog":
                    Category animalCategory2 = db.Categories.Where(e => e.Name == "Dog").FirstOrDefault();
                    return animalCategory2.CategoryId;
                case "Bird":
                    Category animalCategory3 = db.Categories.Where(e => e.Name == "Bird").FirstOrDefault();
                    return animalCategory3.CategoryId;
                case "Micro Pig":
                    Category animalCategory4 = db.Categories.Where(e => e.Name == "Micro Pig").FirstOrDefault();
                    return animalCategory4.CategoryId;
                case "Rabbit":
                    Category animalCategory5 = db.Categories.Where(e => e.Name == "Rabbit").FirstOrDefault();
                    return animalCategory5.CategoryId;
                default:
                    throw new NotImplementedException();
            }            
        }
        
        internal static Room GetRoom(int animalId)
        {
            switch (animalId)
            {
                case 1:
                    Room animalRoom1 = db.Rooms.Where(e => e.AnimalId == 1).FirstOrDefault();
                    return animalRoom1;
                case 2:
                    Room animalRoom2 = db.Rooms.Where(e => e.AnimalId == 2).FirstOrDefault();
                    return animalRoom2;
                case 3:
                    Room animalRoom3 = db.Rooms.Where(e => e.AnimalId == 3).FirstOrDefault();
                    return animalRoom3;
                case 4:
                    Room animalRoom4 = db.Rooms.Where(e => e.AnimalId == 4).FirstOrDefault();
                    return animalRoom4;
                case 5:
                    Room animalRoom5 = db.Rooms.Where(e => e.AnimalId == 5).FirstOrDefault();
                    return animalRoom5;
               //necessary?
                case 6:
                    Room animalRoom6 = db.Rooms.Where(e => e.AnimalId == 6).FirstOrDefault();
                    return animalRoom6;
                case 7:
                    Room animalRoom7 = db.Rooms.Where(e => e.AnimalId == 7).FirstOrDefault();
                    return animalRoom7;
                case 8:
                    Room animalRoom8 = db.Rooms.Where(e => e.AnimalId == 8).FirstOrDefault();
                    return animalRoom8;
                case 9:
                    Room animalRoom9 = db.Rooms.Where(e => e.AnimalId == 9).FirstOrDefault();
                    return animalRoom9;
                case 10:
                    Room animalRoom10 = db.Rooms.Where(e => e.AnimalId == 10).FirstOrDefault();
                    return animalRoom10;
                default:
                    throw new NotImplementedException(); ;
            }
        }
        
        internal static int GetDietPlanId(string dietPlanName)
        {
            switch (dietPlanName)
            {
                case "Cat Food":
                    DietPlan animalDietPlan1 = db.DietPlans.Where(e => e.Name == "Cat Food").FirstOrDefault();
                    return animalDietPlan1.DietPlanId;
                case "Dog Food":
                    DietPlan animalDietPlan2 = db.DietPlans.Where(e => e.Name == "Dog Food").FirstOrDefault();
                    return animalDietPlan2.DietPlanId;
                case "Bird Food":
                    DietPlan animalDietPlan3 = db.DietPlans.Where(e => e.Name == "Bird Food").FirstOrDefault();
                    return animalDietPlan3.DietPlanId;
                case "Pig Food":
                    DietPlan animalDietPlan4 = db.DietPlans.Where(e => e.Name == "Pig Food").FirstOrDefault();
                    return animalDietPlan4.DietPlanId;
                case "Rabbit Food":
                    DietPlan animalDietPlan5 = db.DietPlans.Where(e => e.Name == "Rabbit Food").FirstOrDefault();
                    return animalDietPlan5.DietPlanId;
                default:
                    throw new NotImplementedException(); 
            }
        }

        // TODO: Adoption CRUD Operations
        internal static void Adopt(Animal animal, Client client)
        {
            Adoption adoption = new Adoption();
            adoption.ClientId = client.ClientId;
            adoption.AnimalId = animal.AnimalId;
            adoption.ApprovalStatus = "awaiting";
            adoption.AdoptionFee = 75;
            adoption.PaymentCollected = true;
            db.Adoptions.InsertOnSubmit(adoption);
            db.SubmitChanges();
        }

        internal static IQueryable<Adoption> GetPendingAdoptions()
        {
            var adoptions = db.Adoptions.Where(a => a.ApprovalStatus == "awaiting");
            return adoptions;
        }

        internal static void UpdateAdoption(bool isAdopted, Adoption adoption)
        {
            switch(isAdopted)
            {
                case true:
                    adoption.ApprovalStatus = "approved";
                    break;
                case false:
                    adoption.ApprovalStatus = "denied";
                    RemoveAdoption(adoption.AnimalId, adoption.ClientId);
                    break;
            }
            db.SubmitChanges();
        }

        internal static void RemoveAdoption(int animalId, int clientId)
        {
            throw new NotImplementedException();
        }

        // TODO: Shots Stuff
        internal static IQueryable<AnimalShot> GetShots(Animal animal)
        {
            throw new NotImplementedException();
        }

        internal static void UpdateShot(string shotName, Animal animal)
        {
            throw new NotImplementedException();
        }
    }
}